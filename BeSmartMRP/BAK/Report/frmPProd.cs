
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
    public partial class frmPProd : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = new DataSet();
        private static frmPProd mInstanse = null;

        private frmProd pofrmGetProd = null;

        private ArrayList pATagPdType = new ArrayList();
        private ArrayList pATagPdGrp = new ArrayList();

        private ArrayList pATagCode = new ArrayList();
        private string mstrOrderBy = "cCode";

        public static frmPProd GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmPProd();
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
        
        public frmPProd()
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

            this.cmbPOrderBy.Properties.Items.AddRange(new object[] { "กลุ่มสินค้า, ชนิดสินค้า, รหัสสินค้า", "กลุ่มสินค้า, ชนิดสินค้า, ชื่อสินค้า, รหัสสินค้า", "รหัสสินค้า", "ชื่อสินค้า" });
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
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "Prod", "select top 1 cCode, cName from " + MapTable.Table.MasterProd + " order by " + this.mstrOrderBy, ref strErrorMsg))
            {
                this.txtBegCode.Text = this.dtsDataEnv.Tables["QProd"].Rows[0][this.mstrOrderBy].ToString();
            }
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "Prod", "select top 1 cCode , cName from " + MapTable.Table.MasterProd + " order by " + this.mstrOrderBy + " desc", ref strErrorMsg))
            {
                this.txtEndCode.Text = this.dtsDataEnv.Tables["QProd"].Rows[0][this.mstrOrderBy].ToString();
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
            switch (this.cmbPOrderBy.SelectedIndex)
            {
                case 0:
                case 2:
                    this.mstrOrderBy = "cCode";
                    this.txtBegCode.Properties.MaxLength = frmProd.MAXLENGTH_CODE;
                    this.txtEndCode.Properties.MaxLength = frmProd.MAXLENGTH_CODE;
                    break;
                case 1:
                case 3:
                    this.mstrOrderBy = "cName";
                    this.txtBegCode.Properties.MaxLength = frmProd.MAXLENGTH_NAME;
                    this.txtEndCode.Properties.MaxLength = frmProd.MAXLENGTH_NAME;
                    break;
            }

            this.pmDefaultOption();
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "PROD":
                    if (this.pofrmGetProd == null)
                    {
                        this.pofrmGetProd = new DatabaseForms.frmProd(FormActiveMode.Report);
                        this.pofrmGetProd.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetProd.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "TAG_PDTYPE":
                    using (DialogForms.dlgTagItems2 dlg = new mBudget.DialogForms.dlgTagItems2())
                    {

                        dlg.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlg.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);

                        string strSQLExec = "select {0}.FCSKID, {0}.FCCODE, {0}.FCNAME from {0} ";

                        strSQLExec = string.Format(strSQLExec, new string[] { "PRODTYPE" });

                        dlg.SetBrowView(strSQLExec, null, MapTable.Table.MasterPdGrp, 1, 20);
                        dlg.ShowDialog();
                        if (dlg.PopUpResult)
                        {
                            dlg.LoadTagValue(ref this.pATagPdType);
                            this.txtTagPdType.Text = this.pmGetRngCode2();
                        }
                    }
                    break;
                case "TAG_PDGRP":
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
                            dlg.LoadTagValue(ref this.pATagPdGrp);
                            this.txtTagPdGrp.Text = this.pmGetRngCode3();
                        }
                    }
                    break;
                case "TAG_PROD":
                    using (DialogForms.dlgTagItems dlg = new mBudget.DialogForms.dlgTagItems())
                    {

                        dlg.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - dlg.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);

                        string strSQLExec = "select {0}.CROWID, {0}.CCODE, {0}.CNAME, {0}.DCREATE, {0}.DLASTUPD, EM1.FCLOGIN as CLOGIN_ADD, EM2.FCLOGIN as CLOGIN_UPD from {0} ";
                        strSQLExec += " left join {1} EM1 ON EM1.FCSKID = {0}.CCREATEBY ";
                        strSQLExec += " left join {1} EM2 ON EM2.FCSKID = {0}.CLASTUPDBY ";

                        string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
                        string strEmplRTab = strFMDBName + ".dbo.EMPLR";
                        strSQLExec = string.Format(strSQLExec, new string[] { MapTable.Table.MasterProd, strEmplRTab });

                        dlg.SetBrowView(strSQLExec, null, MapTable.Table.MasterProd, frmProd.MAXLENGTH_CODE, frmProd.MAXLENGTH_NAME);
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
                    this.pmInitPopUpDialog("Prod");
                    this.pofrmGetProd.ValidateField(inPara1, this.mstrOrderBy, true);
                    if (this.pofrmGetProd.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTTAGPDTYPE":
                    this.pmInitPopUpDialog("TAG_PDTYPE");
                    break;
                case "TXTTAGPDGRP":
                    this.pmInitPopUpDialog("TAG_PDGRP");
                    break;
                case "TXTTAGCODE":
                    this.pmInitPopUpDialog("TAG_PROD");
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTBEGCODE":
                case "TXTENDCODE":
                    if (this.pofrmGetProd != null)
                    {
                        DataRow dtrAcChart = this.pofrmGetProd.RetrieveValue();

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
                this.pmInitPopUpDialog("Prod");
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
            for (int intCnt= 0;intCnt<this.pATagCode.Count;intCnt++)
            {
                strTagStr += "'" + this.pATagCode[intCnt].ToString() + "', ";
            }
            strTagStr = (strTagStr.Length > 2 ? AppUtil.StringHelper.Left(strTagStr, strTagStr.Length-2) : "");
            return strTagStr;
        }

        private string pmGetRngCode2()
        {
            string strTagStr = "";
            for (int intCnt = 0; intCnt < this.pATagPdType.Count; intCnt++)
            {
                strTagStr += "'" + this.pATagPdType[intCnt].ToString() + "', ";
            }
            strTagStr = (strTagStr.Length > 2 ? AppUtil.StringHelper.Left(strTagStr, strTagStr.Length - 2) : "");
            return strTagStr;
        }

        private string pmGetRngCode3()
        {
            string strTagStr = "";
            for (int intCnt = 0; intCnt < this.pATagPdGrp.Count; intCnt++)
            {
                strTagStr += "'" + this.pATagPdGrp[intCnt].ToString() + "', ";
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
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strCriteria = "";
            string strOrderBy = "";
            string strBegValue = ""; string strEndValue = "";
            strOrderBy = "Master_Prod." + this.mstrOrderBy;

            string strOrderBy2 = "";
            switch (this.cmbPOrderBy.SelectedIndex)
            {
                case 0:
                    strOrderBy2 = "Master_PdGrp.cCode, Master_Prod.cType, Master_Prod.cCode";
                    break;
                case 1:
                    strOrderBy2 = "Master_PdGrp.cCode, Master_Prod.cType, Master_Prod.cName, Master_Prod.cCode";
                    break;
                case 2:
                    strOrderBy2 = "Master_Prod.cCode";
                    break;
                case 3:
                    strOrderBy2 = "Master_Prod.cName";
                    break;
            }

            strOrderBy = "Master_Prod." + this.mstrOrderBy;

            
            if (this.cmbWRng.SelectedIndex == 0)
            {
                strCriteria = strOrderBy + " between ? and ?";
                objSQLHelper.SetPara(new object[] { this.txtBegCode.Text.TrimEnd(), this.txtEndCode.Text.TrimEnd() });
            }
            else
            {
                strCriteria = " Master_Prod.cCode in (" + this.pmGetRngCode() + ")";
            }

            if (this.pATagPdType.Count > 0)
            {
                strCriteria += " and Master_Prod.cType in (" + this.pmGetRngCode2() + ")";
            }

            if (this.pATagPdGrp.Count > 0)
            {
                strCriteria += " and Master_PdGrp.cCode in (" + this.pmGetRngCode3() + ")";
            }

            Report.LocalDataSet.DTSPPROD dtsPrintPreview = new Report.LocalDataSet.DTSPPROD();

            string strAcChartFld = " , AcChart1.cCode as QcAcChart, AcChart1.cName as QnAcChart ";
            strAcChartFld += " , AcChart2.cCode as QcAcChart2, AcChart2.cName as QnAcChart2 ";
            strAcChartFld += " , Master_PdGrp.cCode as QcPdGrp, Master_PdGrp.cName as QnPdGrp ";
            strAcChartFld += " , Master_UM.cCode as QcUM, Master_UM.cName as QnUM ";

            string strSQLStr = "select Master_Prod.* " + strAcChartFld + " from " + MapTable.Table.MasterProd;
            strSQLStr += " left join Master_PdGrp on Master_PdGrp.cRowID = Master_Prod.cPdGrp ";
            strSQLStr += " left join Master_UM on Master_UM.cRowID = Master_Prod.cUM ";
            strSQLStr += " left join Master_AcChart AcChart1 on AcChart1.cRowID = Master_Prod.cAccBCash ";
            strSQLStr += " left join Master_AcChart AcChart2 on AcChart2.cRowID = Master_Prod.cAccBCred ";
            strSQLStr += " where ";
            strSQLStr += strCriteria + " order by " + strOrderBy2;
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "Prod", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrPData in this.dtsDataEnv.Tables["QProd"].Rows)
                {
                    DataRow dtrPreview = dtsPrintPreview.XRPPROD.NewRow();
                    dtrPreview["cCode"] = dtrPData["cCode"].ToString();
                    dtrPreview["cName"] = dtrPData["cName"].ToString();
                    dtrPreview["cSName"] = dtrPData["cSName"].ToString();
                    dtrPreview["cName2"] = dtrPData["cName2"].ToString();

                    dtrPreview["cQcPdGrp"] = dtrPData["QcPdGrp"].ToString();
                    dtrPreview["cQnPdGrp"] = dtrPData["QnPdGrp"].ToString();

                    dtrPreview["cQcUM"] = dtrPData["QcUM"].ToString();
                    dtrPreview["cQnUM"] = dtrPData["QnUM"].ToString();

                    dtrPreview["cQcAcChart"] = dtrPData["QcAcChart"].ToString();
                    dtrPreview["cQnAcChart"] = dtrPData["QnAcChart"].ToString();

                    dtrPreview["cQcAcChart2"] = dtrPData["QcAcChart2"].ToString();
                    dtrPreview["cQnAcChart2"] = dtrPData["QnAcChart2"].ToString();

                    pobjSQLUtil2.SetPara(new object[] { dtrPData["cType"].ToString() });
                    if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPdType", "PDTYPE", "select * from PRODTYPE where FCCODE = ?", ref strErrorMsg))
                    {
                        DataRow dtrPdType = this.dtsDataEnv.Tables["QPdType"].Rows[0];
                        dtrPreview["cQcPdType"] = dtrPdType["fcCode"].ToString();
                        dtrPreview["cQnPdType"] = dtrPdType["fcName"].ToString();
                    }

                    dtrPreview["cVatIsOut"] = (dtrPData["cVatIsOut"].ToString() == "Y" ? "N" : "Y");
                    dtrPreview["cCtrlStock"] = dtrPData["cCtrlStoc"].ToString();

                    string gcTemStr01 = (Convert.IsDBNull(dtrPData["cMemData"]) ? "" : dtrPData["cMemData"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrPData["cMemData2"]) ? "" : dtrPData["cMemData2"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrPData["cMemData3"]) ? "" : dtrPData["cMemData3"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrPData["cMemData4"]) ? "" : dtrPData["cMemData4"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrPData["cMemData5"]) ? "" : dtrPData["cMemData5"].ToString().TrimEnd());

                    string strRemark1 = BizRule.GetMemData(gcTemStr01, frmProd.xdCMRem);
                    strRemark1 += " " + BizRule.GetMemData(gcTemStr01, frmProd.xdCMRem2);
                    strRemark1 += " " + BizRule.GetMemData(gcTemStr01, frmProd.xdCMRem3);
                    strRemark1 += " " + BizRule.GetMemData(gcTemStr01, frmProd.xdCMRem4);
                    strRemark1 += " " + BizRule.GetMemData(gcTemStr01, frmProd.xdCMRem5);
                    strRemark1 += " " + BizRule.GetMemData(gcTemStr01, frmProd.xdCMRem6);
                    strRemark1 += " " + BizRule.GetMemData(gcTemStr01, frmProd.xdCMRem7);
                    strRemark1 += " " + BizRule.GetMemData(gcTemStr01, frmProd.xdCMRem8);
                    strRemark1 += " " + BizRule.GetMemData(gcTemStr01, frmProd.xdCMRem9);
                    strRemark1 += " " + BizRule.GetMemData(gcTemStr01, frmProd.xdCMRem10);

                    dtrPreview["cRemark"] = strRemark1;

                    dtsPrintPreview.XRPPROD.Rows.Add(dtrPreview);
                }
                if (dtsPrintPreview.XRPPROD.Rows.Count > 0)
                    this.pmPreviewReport(dtsPrintPreview);
            }

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            string strRPTFileName = Application.StartupPath + @"\RPT\XRPProd.rpt";

            switch (this.cmbPOrderBy.SelectedIndex)
            {
                case 0:
                case 1:
                    strRPTFileName = Application.StartupPath + @"\RPT\XRPProd_F2.rpt";
                    break;
                case 2:
                case 3:
                    strRPTFileName = Application.StartupPath + @"\RPT\XRPProd.rpt";
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

            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, (this.mstrOrderBy == "cCode" ? "รหัส" : "ชื่อ") + "สินค้า " + this.txtBegCode.Text.TrimEnd() + " ถึง " + this.txtEndCode.Text.TrimEnd());
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrOrderBy == "cCode" ? "CODE" : "NAME");

            ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            prmCRPara["PFORDERBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);

            App.PreviewReport(this, false, rptPreviewReport);
        }



    }
}
