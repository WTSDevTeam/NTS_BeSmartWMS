
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BeSmartMRP.Report;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Component;
using AppUtil;

namespace BeSmartMRP.Report
{
    public partial class frmXRVATSale : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = new DataSet();
        private static frmXRVATSale mInstanse = null;

        private DialogForms.dlgGetBranch pofrmGetBranch = null;
        private DialogForms.dlgGetBook pofrmGetBook = null;
        private DialogForms.dlgGetVatType pofrmGetVatType = null;

        private ArrayList pATagCode = new ArrayList();
        private string mstrOrderBy = "fcCode";
        private string mstrBranch = "";
        private string mstrCrZone = "";

        private string mstrRefType = "SI";
        private string mstrForm = "FORM1";

        private string mstrBrType = "";
        private string mstrBookPrefix = "";
        private string mstrQcBranch = "0001";
        private string mstrAddr1 = "";
        private string mstrAddr2 = "";

        private string mstrTaskName = "";

        //private Thread thread;
        //private bool stop;


        public frmXRVATSale()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public void SetTitle(string inText, string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
            this.Text = inText;
            this.mstrTaskName = inTaskName;

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

            this.txtBegVat.Text = "";
            this.txtEndVat.Text = "";

            this.pmDefaultOption();

        }

        private void pmDefaultOption()
        {
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { App.gcCorp });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select * from " + MapTable.Table.Branch + " where fcCorp = ? order by fcCode", ref strErrorMsg))
            {
                this.mstrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcSkid"].ToString();
                this.txtQcBranch.Tag = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcSkid"].ToString();
                this.txtQcBranch.Text = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcCode"].ToString().TrimEnd();
                this.txtQnBranch.Text = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcName"].ToString().TrimEnd();
                this.mstrAddr1 = this.dtsDataEnv.Tables["QBranch"].Rows[0]["FCADDR1"].ToString().TrimEnd();
                this.mstrAddr2 = this.dtsDataEnv.Tables["QBranch"].Rows[0]["FCADDR2"].ToString().TrimEnd();
            }

            objSQLHelper.SQLExec(ref this.dtsDataEnv, "QVATType_Option", "VATTYPE", "select * from VATTYPE order by fcCode", ref strErrorMsg);
            //this.cmbBegVATType.DataBindings.Add
            //this.cmbBegVATType.DataSource = this.dtsDataEnv.Tables["QVATType_Option"];
            //this.cmbBegVATType.DisplayMember = "fcCode";

            //this.cmbEndVATType.DataSource = this.dtsDataEnv.Tables["QVATType_Option"];
            //this.cmbEndVATType.DisplayMember = "fcCode";

            //if (this.dtsDataEnv.Tables["QVATType_Option"].Rows.Count > 0)
            //{
            //    this.cmbBegVATType.SelectedIndex = 0;
            //    this.cmbEndVATType.SelectedIndex = this.dtsDataEnv.Tables["QVATType_Option"].Rows.Count - 1;
            //}

            switch (this.mstrBrType)
            {
                case "0":
                    //this.mstrBookPrefix = " and left(Book.fcCode, 2) in ('F1', 'F2','F3', 'FX') ";
                    //this.mstrBookPrefix = " and left(Book.fcCode, 2) in ('F1', 'F2','F3', 'FX', 'FF') ";
                    this.mstrBookPrefix = " and left(Book.fcCode, 3) in ('F1B', 'F2B', 'F3B', 'F3E', 'F5C', 'F5D', 'F5E','FF', 'FXA', 'FXB','01') ";
                    this.mstrQcBranch = "0001";
                    break;
                case "3":
                    //this.mstrBookPrefix = " and left(Book.fcCode, 2) = 'FO' ";
                    //this.mstrBookPrefix = " and left(Book.fcCode, 3) in ('FO', 'FFO', 'FOTH') ";
                    this.mstrBookPrefix = " and left(Book.fcCode, 4) in ('FO', 'FFO', 'FOTH') ";
                    this.mstrQcBranch = "0003";
                    break;
                case "4":
                    this.mstrBookPrefix = " and left(Book.fcCode, 1) = '4' ";
                    this.mstrQcBranch = "0004";
                    break;
                case "5":
                    this.mstrBookPrefix = " and left(Book.fcCode, 1) = '5' ";
                    this.mstrQcBranch = "0005";
                    break;
            }

            this.txtBegDate.DateTime = DateTime.Now;
            this.txtEndDate.DateTime = DateTime.Now;
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "BRANCH":
                    if (this.pofrmGetBranch == null)
                    {
                        this.pofrmGetBranch = new DialogForms.dlgGetBranch();
                        this.pofrmGetBranch.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBranch.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "BOOK":
                    if (this.pofrmGetBook == null)
                    {
                        this.pofrmGetBook = new DialogForms.dlgGetBook();
                        this.pofrmGetBook.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBook.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "VAT":
                    if (this.pofrmGetVatType == null)
                    {
                        this.pofrmGetVatType = new DialogForms.dlgGetVatType();
                        this.pofrmGetVatType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetVatType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
            }
        }

        private void txtPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            this.pmPopUpButtonClick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
        }

        private void txtPopUp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
                    this.pmPopUpButtonClick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
                    break;
            }

        }

        private void pmPopUpButtonClick(string inTextbox, string inPara1)
        {
            switch (inTextbox)
            {
                case "TXTQCBRANCH":
                case "TXTQNBRANCH":
                    this.pmInitPopUpDialog("BRANCH");
                    this.pofrmGetBranch.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCBRANCH" ? "FCCODE" : "FCNAME"), true);
                    if (this.pofrmGetBranch.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTBEGVAT":
                case "TXTENDVAT":
                    this.pmInitPopUpDialog("VAT");
                    this.pofrmGetVatType.ValidateField(inPara1, "FCCODE", true);
                    if (this.pofrmGetVatType.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTQCBRANCH":
                case "TXTQNBRANCH":
                    if (this.pofrmGetBranch != null)
                    {
                        DataRow dtrCrZone = this.pofrmGetBranch.RetrieveValue();

                        if (dtrCrZone != null)
                        {
                            if (this.mstrBranch != dtrCrZone["fcSkid"].ToString())
                            {
                                this.mstrBranch = dtrCrZone["fcSkid"].ToString();
                            }

                            this.txtQcBranch.Tag = dtrCrZone["fcSkid"].ToString();
                            this.txtQcBranch.Text = dtrCrZone["fcCode"].ToString().TrimEnd();
                            this.txtQnBranch.Text = dtrCrZone["fcName"].ToString().TrimEnd();
                            this.mstrAddr1 = dtrCrZone["FCADDR1"].ToString().TrimEnd();
                            this.mstrAddr2 = dtrCrZone["FCADDR2"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcBranch.Tag = "";
                            this.txtQcBranch.Text = "";
                            this.txtQnBranch.Text = "";
                        }
                    }
                    break;
                case "TXTBEGVAT":
                case "TXTENDVAT":
                    if (this.pofrmGetVatType != null)
                    {
                        DataRow dtrVatType = this.pofrmGetVatType.RetrieveValue();
                        if (dtrVatType != null)
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGVAT")
                            {
                                this.txtBegVat.Text = dtrVatType["fcCode"].ToString().TrimEnd();
                            }
                            else
                            {
                                this.txtEndVat.Text = dtrVatType["fcCode"].ToString().TrimEnd();
                            }
                        }
                        else
                        {
                            if (inPopupForm.TrimEnd().ToUpper() == "TXTBEGVAT")
                            {
                                this.txtBegVat.Text = "";
                            }
                            else
                            {
                                this.txtEndVat.Text = "";
                            }
                        }
                    }
                    break;

            }
        }

        private void txtQcBranch_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "";

            if (txtPopUp.Name.ToUpper() == "TXTQCBRANCH")
            {
                strOrderBy = "FCCODE";
            }
            else
            {
                strOrderBy = "FCNAME";
            }

            if (txtPopUp.Text == "")
            {
                this.txtQcBranch.Tag = "";
                this.txtQcBranch.Text = "";
                this.txtQnBranch.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("BRANCH");
                e.Cancel = !this.pofrmGetBranch.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetBranch.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtVat_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "";
            strOrderBy = "FCCODE";

            if (txtPopUp.Text == "")
            {
            }
            else
            {
                this.pmInitPopUpDialog("VAT");
                e.Cancel = !this.pofrmGetVatType.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetVatType.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            this.pmPrintData();

            //this.EndReport();

        }

        private void pmPrintData()
        {

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            string strSQLStr = "select TRADETRM.FCCODE as QcTradeTrm,TRADETRM.FCNAME as QnTradeTrm";
            strSQLStr = strSQLStr + " ,Coor.fcCode as QcCoor,Coor.fcName as QnCoor,Coor.fcPersonTy,COOR.FMMEMDATA as FMCMMEMDATA,COOR.FMMEMDATA2 as FMCMMEMDATA2,COOR.FMMEMDATA3 as FMCMMEMDATA3,COOR.FMMEMDATA4 as FMCMMEMDATA4,COOR.FMMEMDATA5 as FMCMMEMDATA5";
            strSQLStr = strSQLStr + " ,GLRef.* from GLRef ";
            strSQLStr = strSQLStr + " left join BOOK on Book.fcSkid = GLRef.fcBook ";
            strSQLStr = strSQLStr + " left join COOR on COOR.FCSKID = GLREF.FCCOOR ";
            strSQLStr = strSQLStr + " left join TRADETRM on GLREF.FCTRADETRM = TRADETRM.FCSKID ";
            strSQLStr = strSQLStr + " where GLRef.fcCorp = ? ";
            strSQLStr = strSQLStr + " and GLRef.fcBranch = ? ";
            strSQLStr = strSQLStr + " and GLRef.fcVatType in ( select VatType.fcCode from VatType where VatType.fcCode between ? and ?) ";
            strSQLStr = strSQLStr + " and GLRef.fdReceDate between ? and ? ";
            strSQLStr = strSQLStr + " and GLRef.fcRfType in ('F', 'S', 'E', 'K')";
            strSQLStr = strSQLStr + " and GLRef.fcVatDue = 'Y' ";
            strSQLStr = strSQLStr + " and GLRef.FCISHOLD = '' ";
            strSQLStr = strSQLStr + this.mstrBookPrefix;
            strSQLStr = strSQLStr + " order by GLRef.fdDate, GLRef.fcRefNo";

            Report.LocalDataSet.DTSPVAT dtsPrintPreview = new Report.LocalDataSet.DTSPVAT();

            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DateTime dttBegDate = new DateTime(this.txtBegDate.DateTime.Year, this.txtBegDate.DateTime.Month, 1);
            //DateTime dttEndDate = new DateTime(this.txtEndDate.DateTime.Year, this.txtEndDate.DateTime.Month + 1, 1).AddDays(-1);
            DateTime dttEndDate = new DateTime(this.txtEndDate.DateTime.Year, this.txtEndDate.DateTime.Month, 1).AddMonths(1).AddDays(-1);
            //MessageBox.Show(dttEndDate.ToString("dd/MM/yyyy"));

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag.ToString(), this.txtBegVat.Text, this.txtEndVat.Text, dttBegDate.Date, dttEndDate.Date });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    DataRow dtrPreview = dtsPrintPreview.XRPVAT.NewRow();

                    string strDetail = "";
                    string strRemark = (Convert.IsDBNull(dtrGLRef["fmMemData"]) ? "" : dtrGLRef["fmMemData"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrGLRef["fmMemData2"]) ? "" : dtrGLRef["fmMemData2"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrGLRef["fmMemData3"]) ? "" : dtrGLRef["fmMemData3"].ToString().TrimEnd());
                    strRemark += (Convert.IsDBNull(dtrGLRef["fmMemData4"]) ? "" : dtrGLRef["fmMemData4"].ToString().TrimEnd());
                    if (dtrGLRef["fmMemData5"] != null)
                        strRemark += (Convert.IsDBNull(dtrGLRef["fmMemData5"]) ? "" : dtrGLRef["fmMemData5"].ToString().TrimEnd());

                    //strDetail = BizRule.GetMemData(strRemark, "Det");
                    strDetail = dtrGLRef["QnCoor"].ToString().Trim();
                    string gcTemStr01 = (Convert.IsDBNull(dtrGLRef["fmCmMemData"]) ? "" : dtrGLRef["fmCmMemData"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrGLRef["fmCmMemData2"]) ? "" : dtrGLRef["fmCmMemData2"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrGLRef["fmCmMemData3"]) ? "" : dtrGLRef["fmCmMemData3"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrGLRef["fmCmMemData4"]) ? "" : dtrGLRef["fmCmMemData4"].ToString().TrimEnd());
                    gcTemStr01 += (Convert.IsDBNull(dtrGLRef["fmCmMemData5"]) ? "" : dtrGLRef["fmCmMemData5"].ToString().TrimEnd());

                    string strMTaxID = BizRule.GetMemData(gcTemStr01, "MId");
                    string strTaxID = BizRule.GetMemData(gcTemStr01, "Tax");
                    string strBizType = BizRule.GetMemData(gcTemStr01, "TBu");
                    string strTax = "";

                    if (dtrGLRef["QcCoor"].ToString().Trim() == "-")
                    {
                        strTax = dtrGLRef["fcRecvMan"].ToString().Trim();
                    }
                    else
                    {
                        if (dtrGLRef["fcPersonTy"].ToString() == "Y")
                        {
                            strTax = strTaxID;
                        }
                        else
                        {
                            strTax = strMTaxID;
                        }
                    }
                    //App.WriteEventsLog2(dtrGLRef["fcRefNo"].ToString());
                    //decimal decAmt = Convert.ToDecimal(dtrGLRef["fnAmtKe"]) - Convert.ToDecimal(dtrGLRef["nCostAdj1"]) - Convert.ToDecimal(dtrGLRef["nCostAdj2"]);
                    decimal decAmt = Convert.ToDecimal(dtrGLRef["fnAmtKe"]);
                    //decimal decAmt = Convert.ToDecimal(dtrGLRef["fnAmtKe"]);
                    decimal decVATAmt = Convert.ToDecimal(dtrGLRef["fnVatAmt"]);
                    decAmt = decAmt * Convert.ToDecimal(dtrGLRef["fnXRate"]) * (dtrGLRef["fcRfType"].ToString() == "F" ? -1 : 1);
                    decVATAmt = decVATAmt * (dtrGLRef["fcRfType"].ToString() == "F" ? -1 : 1);

                    if (dtrGLRef["fcStat"].ToString() == "C")
                    {
                        decAmt = 0;
                        decVATAmt = 0;
                    }

                    dtrPreview["CSTAT"] = dtrGLRef["fcStat"].ToString();
                    dtrPreview["CVATTYPE"] = dtrGLRef["fcVatType"].ToString();
                    dtrPreview["DDATE"] = Convert.ToDateTime(dtrGLRef["fdDate"]);
                    dtrPreview["CREFTYPE"] = dtrGLRef["fcRefType"].ToString();
                    dtrPreview["CREFNO"] = dtrGLRef["fcRefNo"].ToString();
                    dtrPreview["CDETAIL"] = strDetail;
                    dtrPreview["NAMT"] = decAmt;
                    dtrPreview["NVATAMT"] = decVATAmt;

                    //fcRecvMan
                    dtrPreview["TaxID"] = strTax;
                    if (!Convert.IsDBNull(dtrGLRef["QCTradeTrm"]))
                    {
                        dtrPreview["QCTRADETRM"] = dtrGLRef["QcTradeTrm"].ToString().TrimEnd();
                        dtrPreview["QNTRADETRM"] = dtrGLRef["QnTradeTrm"].ToString().TrimEnd();
                    }
                    dtrPreview["BIZTYPE"] = strBizType;
                    dtsPrintPreview.XRPVAT.Rows.Add(dtrPreview);
                }
                this.pmPreviewReport(dtsPrintPreview);
            }
            else
            {
                //this.EndReport();
                MessageBox.Show("ไม่มีข้อมูล");
            }


        }

        private decimal pmToDecimal(DataRow inSource, string inFld)
        {
            return (Convert.IsDBNull(inSource[inFld]) ? 0 : Convert.ToDecimal(inSource[inFld]));
        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            App.AppMessage = "Generate preview Report";

            //string strRPTFileName = Application.StartupPath + @"\RPT\XRPVATSALE.rpt";

            //ReportDocument rptPreviewReport = new ReportDocument();
            //if (!System.IO.File.Exists(strRPTFileName))
            //{
            //    //this.EndReport();
            //    MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //rptPreviewReport.Load(strRPTFileName);
            //rptPreviewReport.SetDataSource(inData);

            ////this.EndReport();

            //this.pACrPara.Clear();

            //int intYear = this.txtBegDate.DateTime.Year + (this.txtBegDate.DateTime.ToString("gg") == "พ.ศ." ? 543 : 0);

            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "นำยื่นในเดือน " + this.pmGetThaiMonthName(this.txtBegDate.DateTime.Month) + " ปี " + intYear.ToString());
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.Name);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.txtQnBranch.Text);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.TaxID);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.txtQcBranch.Text);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrAddr1);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrAddr2);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, (this.mstrBrType == "0" ? "Y" : "N"));
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.mstrBrType);
            ////

            //int i = 0;

            //ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            //prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE3"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE4"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE5"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE6"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFREPORTTITLE7"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFISHEADBRANCH"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);
            //prmCRPara["PFBRANCHCODE"].ApplyCurrentValues((ParameterValues)this.pACrPara[i++]);

            //App.PreviewReport(this, false, rptPreviewReport);

        }

        private string pmGetThaiMonthName(int inMonth)
        {
            string strResult = "";
            switch (inMonth)
            {
                case 1:
                    strResult = "มกราคม";
                    break;
                case 2:
                    strResult = "กุมภาพันธ์";
                    break;
                case 3:
                    strResult = "มีนาคม";
                    break;
                case 4:
                    strResult = "เมษายน";
                    break;
                case 5:
                    strResult = "พฤษภาคม";
                    break;
                case 6:
                    strResult = "มิถุนายน";
                    break;
                case 7:
                    strResult = "กรกฎาคม";
                    break;
                case 8:
                    strResult = "สิงหาคม";
                    break;
                case 9:
                    strResult = "กันยายน";
                    break;
                case 10:
                    strResult = "ตุลาคม";
                    break;
                case 11:
                    strResult = "พฤศจิกายน";
                    break;
                case 12:
                    strResult = "ธันวาคม";
                    break;
            }
            return strResult;
        }


    }
}
