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
    public partial class XRREPORT02 : UIHelper.frmBase
    {

        private DataSet dtsDataEnv = new DataSet();
        private static XRREPORT02 mInstanse = null;

        private DialogForms.dlgGetPdGrp pofrmGetPdGrp = null;

        private ArrayList pATagCode = new ArrayList();
        private string mstrOrderBy = "fcCode";
        private string mstrBranch = "";

        public static XRREPORT02 GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new XRREPORT02();
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

        public XRREPORT02()
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
            this.pmPrintData();
            //this.pmPrintData2();
        }

        private void pmPrintData()
        {

            string strSQLStr = "select * from STOCK01 where cCorp = ? and QcPdGrp between ? and ? and dDate between ? and ? order by QcPdGrp, dDate";

            Report.LocalDataSet.DTSPSTMOVE dtsPrintPreview = new Report.LocalDataSet.DTSPSTMOVE();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            DateTime dttBegDate = DateTime.Now;
            DateTime dttEndDate = DateTime.Now;

            dttBegDate = this.txtBegDate.DateTime;// new DateTime(this.txtBegDate.DateTime.Year, 1, 1);
            dttEndDate = this.txtEndDate.DateTime;   // new DateTime(this.txtEndDate.DateTime.Year, 12, 13);

            objSQLHelper.SetPara(new object[] { App.gcCorp, this.txtBegQcPdGrp.Text.TrimEnd(), this.txtEndQcPdGrp.Text.TrimEnd(), dttBegDate.Date, dttEndDate.Date });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {

                DataRow dtrPreview = null;
                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    //จำนวน
                    dtrPreview = dtsPrintPreview.XRPSTMOVE.NewRow();

                    decimal decBfQty = 0;
                    decimal decBfAmt = 0;
                    this.pmCreateBFStock01(dtrGLRef["cPdGrp"].ToString(),  ref decBfQty, ref decBfAmt);

                    dtrPreview["cQcPdType"] = "1";
                    dtrPreview["cQcPdGrp"] = dtrGLRef["QcPdGrp"].ToString().TrimEnd();
                    dtrPreview["cQnPdGrp"] = dtrGLRef["QnPdGrp"].ToString().TrimEnd();

                    dtrPreview["nBfQty"] = decBfQty;
                    dtrPreview["nQty_IN"] = Convert.ToDecimal(dtrGLRef["Qty_IN"]);
                    dtrPreview["nQty_OUT"] = Convert.ToDecimal(dtrGLRef["Qty_OUT"]);
                    dtrPreview["nQty_RET"] = Convert.ToDecimal(dtrGLRef["Qty_RET"]);

                    dtsPrintPreview.XRPSTMOVE.Rows.Add(dtrPreview);

                    dtrPreview = dtsPrintPreview.XRPSTMOVE.NewRow();

                    //มูลค่า
                    dtrPreview["cQcPdType"] = "2";
                    dtrPreview["cQcPdGrp"] = dtrGLRef["QcPdGrp"].ToString().TrimEnd();
                    dtrPreview["cQnPdGrp"] = dtrGLRef["QnPdGrp"].ToString().TrimEnd();

                    dtrPreview["nBfAmt"] = decBfAmt;
                    dtrPreview["nAmt_IN"] = Convert.ToDecimal(dtrGLRef["Amt_IN"]);
                    dtrPreview["nAmt_OUT"] = Convert.ToDecimal(dtrGLRef["Amt_OUT"]);
                    dtrPreview["nAmt_RET"] = Convert.ToDecimal(dtrGLRef["Amt_RET"]);

                    dtsPrintPreview.XRPSTMOVE.Rows.Add(dtrPreview);

                }

                this.pmPreviewReport(dtsPrintPreview);
            }

        }

        private void pmCreateBFStock01(string inPdGrp, ref decimal ioBfQty, ref decimal ioBfAmt)
        {

            string strFld = " sum(QTY_IN) as SumQTY_In, sum(QTY_OUT) as SumQTY_OUT, sum(QTY_RET) as SumQty_Ret, sum(AMT_IN) as SumAMT_IN, sum(AMT_OUT) as SumAMT_OUT, sum(AMT_RET) as SumAmt_Ret";
            string strSQLStr = "select " + strFld + " from STOCK01 ";
            strSQLStr = strSQLStr + " where STOCK01.CCORP = ? ";
            strSQLStr = strSQLStr + " and STOCK01.CPDGRP = ? ";
            strSQLStr = strSQLStr + " and STOCK01.DDATE < ? ";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //objSQLHelper.SetPara(new object[] {App.gcCorp, this.txtBegDate.DateTime.Date, this.txtBegQcPdType.Text.TrimEnd() , this.txtEndQcPdType.Text.TrimEnd() , this.txtBegQcProd.Text.TrimEnd() , this.txtEndQcProd.Text.TrimEnd() , this.txtBegQcDept.Text.TrimEnd(), this.txtEndQcDept.Text.TrimEnd() });

            objSQLHelper.SetPara(new object[] { App.gcCorp, inPdGrp, this.txtBegDate.DateTime.Date });

            ioBfQty = 0; ioBfAmt = 0;
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBFStock", "RefProd", strSQLStr, ref strErrorMsg))
            {
                
                DataRow dtrSumBF = this.dtsDataEnv.Tables["QBFStock"].Rows[0];
                decimal decQty_IN = pmToDecimal(dtrSumBF, "SumQTY_In");
                decimal decQty_Out = pmToDecimal(dtrSumBF, "SumQTY_Out");
                decimal decQty_Ret = pmToDecimal(dtrSumBF, "SumQTY_Ret");

                decimal decAmt_IN = pmToDecimal(dtrSumBF, "SumQTY_In");
                decimal decAmt_Out = pmToDecimal(dtrSumBF, "SumQTY_Out");
                decimal decAmt_Ret = pmToDecimal(dtrSumBF, "SumQTY_Ret");

                ioBfQty = decQty_IN + decQty_Ret - decQty_Out;
                ioBfAmt = decAmt_IN + decAmt_Ret - decAmt_Out;
            
            }

        }

        private decimal pmToDecimal(DataRow inSource, string inFld)
        {
            return (Convert.IsDBNull(inSource[inFld]) ? 0 : Convert.ToDecimal(inSource[inFld]));
        }

        private void pmPrintData2()
        {

            string strFld = " REFPROD.FCPROD , REFPROD.FCREFTYPE,FCIOTYPE, sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as FNSUMQTY , sum(REFPROD.FNCOSTAMT) as FNSUMCOST";
            string strSQLStr = "select " + strFld + " from REFPROD ";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " left join PDGRP on PDGRP.FCSKID = PROD.FCPDGRP ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? and REFPROD.FCBRANCH = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE between ? and ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' ";
            strSQLStr = strSQLStr + " and PDGRP.FCCORP = ? and PDGRP.FCTYPE = 'G' and PDGRP.FCCODE between ? and ? ";
            strSQLStr = strSQLStr + " group by REFPROD.FCPROD , REFPROD.FCREFTYPE, REFPROD.FCIOTYPE";

            Report.LocalDataSet.DTSPSTMOVE dtsPrintPreview = new Report.LocalDataSet.DTSPSTMOVE();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //stop = false;
            //thread = new Thread(new ThreadStart(StartPrint));
            //thread.Start();

            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            App.MoreProcess = true;
            App.AppMessage = "Start Extract Data...";

            //this.pmCreateBFStock();

            objSQLHelper.SetPara(new object[] { App.gcCorp, this.mstrBranch, this.txtBegDate.DateTime.Date, this.txtEndDate.DateTime.Date, App.gcCorp, this.txtBegQcPdGrp.Text.TrimEnd(), this.txtEndQcPdGrp.Text.TrimEnd() });

            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QGLRef", "GLRef", strSQLStr, ref strErrorMsg))
            {
                string strPrefix = "";
                string strRefType = "";

                DataRow dtrPreview = null;
                foreach (DataRow dtrGLRef in this.dtsDataEnv.Tables["QGLRef"].Rows)
                {

                    if (App.MoreProcess == false) break;

                    if (strPrefix != dtrGLRef["fcProd"].ToString())
                    {
                        strPrefix = dtrGLRef["fcProd"].ToString();

                        dtrPreview = dtsPrintPreview.XRPSTMOVE.NewRow();

                        string strQcProd = "";
                        string strQnProd = "";
                        string strQcSect = "";
                        objSQLHelper.SetPara(new object[] { dtrGLRef["fcProd"].ToString() });
                        string strSQLExec_Prod = "select Prod.fcCode, Prod.fcName, Prod.fcSName, UM.fcName as cQnUM, PdGrp.fcCode as cQcPdGrp, PdGrp.fcName as cQnPdGrp from Prod  ";
                        strSQLExec_Prod += " left join UM on UM.fcSkid = Prod.fcUm ";
                        strSQLExec_Prod += " left join PDGRP on PDGRP.fcSkid = Prod.fcPdGrp ";
                        strSQLExec_Prod += " where Prod.fcSkid = ? ";

                        if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", strSQLExec_Prod, ref strErrorMsg))
                        {

                            DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];

                            strQcProd = dtrProd["fcCode"].ToString();

                            dtrPreview["cQnUm"] = dtrProd["cQnUm"].ToString().TrimEnd();

                            if (Convert.IsDBNull(dtrProd["cQcPdGrp"]))
                            {
                                dtrPreview["cQcPdGrp"] = "9999999";
                                dtrPreview["cQnPdGrp"] = "Not Specific";
                            }
                            else
                            {
                                dtrPreview["cQcPdGrp"] = dtrProd["cQcPdGrp"].ToString().TrimEnd();
                                dtrPreview["cQnPdGrp"] = dtrProd["cQnPdGrp"].ToString().TrimEnd();
                            }
                        }

                        //App.AppMessage = "กำลังพิมพ์ข้อมูล สินค้า : \r\n" + "(" + strQcProd.TrimEnd() + ")" + strQnProd.TrimEnd();

                        decimal decBFQty = 0; decimal decBFAmt = 0;
                        this.pmCreateBFStock2(dtrGLRef["fcProd"].ToString());
                        this.pmGetBFStock(strQcProd, strQcSect, ref decBFQty, ref decBFAmt);

                        dtrPreview["nBfQty"] = decBFQty;
                        dtrPreview["nBfAmt"] = decBFAmt;

                        dtrPreview["nQty_IN"] = 0;
                        dtrPreview["nQty_Out"] = 0;
                        dtrPreview["nQty_Ret"] = 0;

                        dtrPreview["nAmt_In"] = 0;
                        dtrPreview["nAmt_Out"] = 0;
                        dtrPreview["nAmt_Ret"] = 0;

                        bool bllIsPrn = true;
                        dtsPrintPreview.XRPSTMOVE.Rows.Add(dtrPreview);

                    }
                    strRefType = dtrGLRef["fcRefType"].ToString();
                    switch (strRefType)
                    {
                        case "BI":
                        case "BR":
                            dtrPreview["nQty_IN"] = Convert.ToDecimal(dtrPreview["nQty_IN"]) + Convert.ToDecimal(dtrGLRef["fnSumQty"]);
                            dtrPreview["nAmt_IN"] = Convert.ToDecimal(dtrPreview["nAmt_IN"]) + Convert.ToDecimal(dtrGLRef["fnSumCost"]);
                            break;
                        case "BC":
                        case "BM":
                            dtrPreview["nQty_Ret"] = Convert.ToDecimal(dtrPreview["nQty_Ret"]) + Convert.ToDecimal(dtrGLRef["fnSumQty"]);
                            dtrPreview["nAmt_Ret"] = Convert.ToDecimal(dtrPreview["nAmt_Ret"]) + Convert.ToDecimal(dtrGLRef["fnSumCost"]);
                            break;
                        case "RW":
                        case "RX":
                            dtrPreview["nQty_IN"] = Convert.ToDecimal(dtrPreview["nQty_IN"]) + Convert.ToDecimal(dtrGLRef["fnSumQty"]);
                            dtrPreview["nAmt_IN"] = Convert.ToDecimal(dtrPreview["nQty_IN"]) + Convert.ToDecimal(dtrGLRef["fnSumCost"]);
                            break;
                        case "WR":
                        case "WX":
                        case "SI":
                        case "SR":
                            dtrPreview["nQty_Out"] = Convert.ToDecimal(dtrPreview["nQty_Out"]) + Convert.ToDecimal(dtrGLRef["fnSumQty"]);
                            dtrPreview["nAmt_Out"] = Convert.ToDecimal(dtrPreview["nAmt_Out"]) + Convert.ToDecimal(dtrGLRef["fnSumCost"]);
                            break;
                    }

                }
                this.dataGridView1.DataSource = dtsPrintPreview.XRPSTMOVE;
                this.pmPreviewReport(dtsPrintPreview);
            }

        }
        private void pmGetBFStock(string inQcProd, string inQcSect, ref decimal ioQty, ref decimal ioAmt)
        {
            decimal decQty = 0;
            decimal decAmt = 0;
            //DataRow[] dtaSel = this.dtsDataEnv.Tables["QBFStock"].Select("cQcProd = '" +inQcProd+"' and cQcSect = '" +inQcSect+"' ");
            DataRow[] dtaSel = this.dtsDataEnv.Tables["QBFStock"].Select("cQcProd = '" + inQcProd + "'");
            if (dtaSel.Length > 0)
            {
                for (int intCnt = 0; intCnt < dtaSel.Length; intCnt++)
                {
                    decQty += (Convert.ToDecimal(dtaSel[intCnt]["fnSumQty"]) * (dtaSel[intCnt]["fcIOType"].ToString() == "I" ? 1 : -1));
                    decAmt += (Convert.ToDecimal(dtaSel[intCnt]["fnSumCost"]) * (dtaSel[intCnt]["fcIOType"].ToString() == "I" ? 1 : -1));
                }
            }
            ioQty = decQty;
            ioAmt = decAmt;
        }
        private void pmCreateBFStock()
        {
            string strFld = " PROD.FCCODE as cQcProd , REFPROD.FCIOTYPE, sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as FNSUMQTY , sum(REFPROD.FNCOSTAMT) as FNSUMCOST";
            string strSQLStr = "select " + strFld + " from REFPROD ";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " left join PDGRP on PDGRP.FCSKID = PROD.FCPDGRP ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = ? and REFPROD.FCBRANCH = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE < ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' ";
            strSQLStr = strSQLStr + " and PDGRP.FCCODE between ? and ? ";
            strSQLStr = strSQLStr + " group by PROD.FCCODE , REFPROD.FCIOTYPE";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //objSQLHelper.SetPara(new object[] {App.gcCorp, this.txtBegDate.DateTime.Date, this.txtBegQcPdType.Text.TrimEnd() , this.txtEndQcPdType.Text.TrimEnd() , this.txtBegQcProd.Text.TrimEnd() , this.txtEndQcProd.Text.TrimEnd() , this.txtBegQcDept.Text.TrimEnd(), this.txtEndQcDept.Text.TrimEnd() });

            objSQLHelper.SetPara(new object[] { App.gcCorp, this.mstrBranch, this.txtBegDate.DateTime.Date, this.txtBegQcPdGrp.Text.TrimEnd(), this.txtEndQcPdGrp.Text.TrimEnd() });

            objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBFStock", "RefProd", strSQLStr, ref strErrorMsg);

        }
        private void pmCreateBFStock2(string inProd)
        {
            string strFld = " PROD.FCCODE as cQcProd , REFPROD.FCIOTYPE, sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as FNSUMQTY , sum(REFPROD.FNCOSTAMT) as FNSUMCOST";
            string strSQLStr = "select " + strFld + " from REFPROD ";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " left join PDGRP on PDGRP.FCSKID = PROD.FCPDGRP ";
            strSQLStr = strSQLStr + " where REFPROD.FCPROD = ? ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE < ? ";
            strSQLStr = strSQLStr + " and REFPROD.FCSTAT <> 'C' ";
            strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' ";
            strSQLStr = strSQLStr + " group by PROD.FCCODE , REFPROD.FCIOTYPE";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //objSQLHelper.SetPara(new object[] {App.gcCorp, this.txtBegDate.DateTime.Date, this.txtBegQcPdType.Text.TrimEnd() , this.txtEndQcPdType.Text.TrimEnd() , this.txtBegQcProd.Text.TrimEnd() , this.txtEndQcProd.Text.TrimEnd() , this.txtBegQcDept.Text.TrimEnd(), this.txtEndQcDept.Text.TrimEnd() });

            objSQLHelper.SetPara(new object[] { inProd, this.txtBegDate.DateTime.Date });

            objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBFStock", "RefProd", strSQLStr, ref strErrorMsg);

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            string strRPTFileName = Application.StartupPath + @"\RPT\XRSALESUM02.rpt";

            ReportDocument rptPreviewReport = new ReportDocument();
            if (!System.IO.File.Exists(strRPTFileName))
            {
                MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rptPreviewReport.Load(strRPTFileName);
            rptPreviewReport.SetDataSource(inData);

            this.pACrPara.Clear();

            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "สรุปรายงานสินค้าคงเหลือ ณ วันที่ " + this.txtEndDate.DateTime.ToString("dd MMMM yyyy"));
            AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "MAC_XRREPORT02");
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, (this.cmbPOrderBy.SelectedIndex == 0 ? "รหัส" : "ชื่อ") + "ผังกลุ่มสินค้า " + this.txtBegPdGrp.Text.TrimEnd() + " ถึง " + this.txtEndPdGrp.Text.TrimEnd());
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.cmbPOrderBy.SelectedIndex == 0 ? "CODE" : "NAME");

            ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            //prmCRPara["PFORDERBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);

            App.PreviewReport(this, false, rptPreviewReport);

        }

    }
}
