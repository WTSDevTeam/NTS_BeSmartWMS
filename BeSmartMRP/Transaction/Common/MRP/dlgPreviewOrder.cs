
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Report;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Component;

namespace BeSmartMRP.Transaction.Common.MRP
{

    public partial class dlgPreviewOrder : UIHelper.frmBase
    {

        public dlgPreviewOrder()
        {
            InitializeComponent();

            this.pmInitForm();
        }

        private DataSet dtsDataEnv = null;
        private string mstrAlias = "";
        private string mstrSortKey = "";

        private string mstrRemark1 = "";
        private string mstrRemark2 = "";
        private string mstrRemark3 = "";
        private string mstrRemark4 = "";
        private string mstrRemark5 = "";
        private string mstrRemark6 = "";
        private string mstrRemark7 = "";
        private string mstrRemark8 = "";
        private string mstrRemark9 = "";
        private string mstrRemark10 = "";

        private void pmInitForm()
        {
        }

        public bool IsSave
        {
            set 
            {
                if ((bool)value)
                {
                    this.barManager1.Items["barSave"].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
                else
                {
                    this.barManager1.Items["barSave"].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
        }

        public void BindData(DataSet inDataEnv, string inAlias)
        {
            this.mstrAlias = inAlias;
            this.dtsDataEnv = inDataEnv;
            this.pmBindData();
            //this.dataGridView1.DataSource = this.dtsDataEnv.Tables["TemPd_GenPR1"];
        }

        private void pmBindData()
        {
            this.pmInitGridProp();
        }

        private void pmInitGridProp()
        {
            //this.grdBrowView.SetDataBinding(this.dtsDataEnv.Tables[this.mstrAlias], "");
            //this.grdBrowView.RetrieveStructure();

            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrAlias].DefaultView;
            //dvBrow.Sort = "dDate, cRefNo";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView1.Columns["cCode"].Group();

            this.gridView1.Columns["cCode"].VisibleIndex = i++;
            this.gridView1.Columns["cRefNo"].VisibleIndex = i++;
            this.gridView1.Columns["cRefNo_MO"].VisibleIndex = i++;
            this.gridView1.Columns["cQcCoor"].VisibleIndex = i++;
            //this.gridView1.Columns["cQnCoor"].VisibleIndex = i++;
            this.gridView1.Columns["cQcProd"].VisibleIndex = i++;
            //this.gridView1.Columns["cQnProd"].VisibleIndex = i++;
            this.gridView1.Columns["nBakQty_MO"].VisibleIndex = i++;
            this.gridView1.Columns["nBakQty_PR"].VisibleIndex = i++;
            this.gridView1.Columns["nBakQty_PO"].VisibleIndex = i++;
            this.gridView1.Columns["nStkQty"].VisibleIndex = i++;
            this.gridView1.Columns["nIssueQty"].VisibleIndex = i++;
            this.gridView1.Columns["nRefQty"].VisibleIndex = i++;
            this.gridView1.Columns["nQty"].VisibleIndex = i++;
            this.gridView1.Columns["nPrice"].VisibleIndex = i++;

            this.gridView1.Columns["cCode"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่ภายใน", "Document Code" });
            this.gridView1.Columns["cRefNo"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "#Ref. No." });
            this.gridView1.Columns["cRefNo_MO"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง MO", "#MO Ref. No." });
            this.gridView1.Columns["cQcCoor"].Caption = UIBase.GetAppUIText(new string[] { "รหัสผู้จำหน่าย", "Supplier Code" });
            this.gridView1.Columns["cQnCoor"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อผู้จำหน่าย", "Supplier Name" });
            this.gridView1.Columns["cQcProd"].Caption = UIBase.GetAppUIText(new string[] { "รหัสสินค้า", "Product Code" });
            this.gridView1.Columns["cQnProd"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อสินค้า", "Product Name" });
            this.gridView1.Columns["nBakQty_MO"].Caption = UIBase.GetAppUIText(new string[] { "ยอดจอง MO", "Booking\r\nM/O. Qty." });
            this.gridView1.Columns["nBakQty_PR"].Caption = UIBase.GetAppUIText(new string[] { "PR ค้างสั่ง", "Outstanding\r\nP/R. Qty." });
            this.gridView1.Columns["nBakQty_PO"].Caption = UIBase.GetAppUIText(new string[] { "PO ค้างรับ", "Outstanding\r\nP/O. Qty" });
            this.gridView1.Columns["nStkQty"].Caption = UIBase.GetAppUIText(new string[] { "สินค้าคงหลือ", "Stock\r\nQty." });
            this.gridView1.Columns["nIssueQty"].Caption = UIBase.GetAppUIText(new string[] { "เบิก/รับคืน", "Issue\r\nQty." });
            this.gridView1.Columns["nRefQty"].Caption = UIBase.GetAppUIText(new string[] { "จำนวน", "M/O\r\nQty." });
            this.gridView1.Columns["nQty"].Caption = UIBase.GetAppUIText(new string[] { "จำนวน ญฑ", "Gen P/R\r\nQty." });
            this.gridView1.Columns["nPrice"].Caption = UIBase.GetAppUIText(new string[] { "ราคา", "Price" });


            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.Columns["cRefNo"].Width = 120;
            this.gridView1.Columns["cRefNo_MO"].Width = 120;
            this.gridView1.Columns["cQcCoor"].Width = 90;
            //this.gridView1.Columns["cQnCoor"].Width = 40;
            this.gridView1.Columns["cQcProd"].Width = 100;
            this.gridView1.Columns["nQty"].Width = 80;
            this.gridView1.Columns["nBakQty_MO"].Width = 80;
            this.gridView1.Columns["nBakQty_PR"].Width = 80;
            this.gridView1.Columns["nStkQty"].Width = 80;
            this.gridView1.Columns["nIssueQty"].Width = 80;
            this.gridView1.Columns["nQty"].Width = 80;
            this.gridView1.Columns["nRefQty"].Width = 80;
            this.gridView1.Columns["nPrice"].Width = 80;

            string strQtyFormat = "#,###,###.00;(#,###,###.00);-";
            this.gridView1.Columns["nQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nQty"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nRefQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nRefQty"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nPrice"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nPrice"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nStkQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nStkQty"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nIssueQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nIssueQty"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nBakQty_MO"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nBakQty_MO"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nBakQty_PR"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nBakQty_PR"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nBakQty_PO"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nBakQty_PO"].DisplayFormat.FormatString = strQtyFormat;

            this.gridView1.Columns["nQty"].AppearanceCell.BackColor = Color.LemonChiffon;
            this.gridView1.Columns["nPrice"].AppearanceCell.BackColor = Color.LemonChiffon;
            this.gridView1.Columns["nStkQty"].AppearanceCell.BackColor = Color.LemonChiffon;
            this.gridView1.Columns["nBakQty_PR"].AppearanceCell.BackColor = Color.LemonChiffon;
            this.gridView1.Columns["nBakQty_PO"].AppearanceCell.BackColor = Color.LemonChiffon;
            this.gridView1.Columns["nRefQty"].AppearanceCell.BackColor = Color.LemonChiffon;
            this.gridView1.Columns["nIssueQty"].AppearanceCell.BackColor = Color.LemonChiffon;

            //this.gridView1.Columns["dDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            //this.gridView1.Columns["dDate"].DisplayFormat.FormatString = "dd/MM/yy";

            //this.gridView1.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView1.Columns["dDate"], Janus.Windows.GridEX.SortOrder.Ascending));
            //this.gridView1.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.gridView1.Columns["cRefNo"], Janus.Windows.GridEX.SortOrder.Ascending));
            ////this.gridView1.Columns[this.mstrSortKey].CellStyle.BackColor = Color.WhiteSmoke;

            //this.gridView1.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "nAmt", this.gridView1.Columns["nAmt"], "{0:n2}");

            this.grdBrowView.Focus();

            this.gridView1.ExpandAllGroups();

        }

        private void barManager1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (e.Item.Tag.ToString())
            {
                case "PRINT":

                    using (Transaction.Common.dlgPRange dlg = new Transaction.Common.dlgPRange())
                    {

                        dlg.BeginCode = "";
                        dlg.EndCode = "";

                        string[] strADir = System.IO.Directory.GetFiles(Application.StartupPath + "\\RPT\\FORM_GENPR\\");
                        dlg.LoadRPT(Application.StartupPath + "\\RPT\\FORM_GENPR\\");

                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            //App.mSave_hWnd = App.GetForegroundWindow();
                            string strRPTFileName = "";
                            //this.pmPrintRangeData(dlg.BeginCode, dlg.EndCode, strADir[dlg.cmbPForm.SelectedIndex]);
                            this.pmPrintData(strADir[dlg.cmbPForm.SelectedIndex]);
                        }
                    }

                    break;
                case "SAVE":
                    this.DialogResult = DialogResult.OK;
                    break;
                case "CANCEL":
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }

        }

        private void dlgPreviewOrder_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }
        }

        private void pmPrintData(string inRPTFileName)
        {
            //BeSmartMRP.Report.LocalDataSet.DTSPGENPR dtsPreview

            string strErrorMsg = "";
            Report.LocalDataSet.DTSPGENPR dtsPrintPreview = new Report.LocalDataSet.DTSPGENPR();

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strQcMfgProd = "";
            string strQnMfgProd = "";
            string strQcBOM = "";
            string strQnBOM = "";
            decimal decMfgQty = 0;
            decimal decMOQty = 0;
            DateTime dttDate = DateTime.Now;
            DateTime dttStartDate = DateTime.Now;
            DateTime dttDueDate = DateTime.Now;
            string strQcSect = "";
            string strQnSect = "";
            string strQcJob = "";
            string strQnJob = "";
            string strWHouse_List1 = "";
            string strQcCorp = "";
            string strQnCorp = "";

            string strRowID = this.dtsDataEnv.Tables[this.mstrAlias].Rows[0]["cWOrderH"].ToString();
            objSQLHelper2.SetPara(new object[] { strRowID });
            if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QWOrderH", QMFWOrderHDInfo.TableName, "select * from " + QMFWOrderHDInfo.TableName + " where cRowID = ? ", ref strErrorMsg))
            {

                DataRow dtrWOrderH = this.dtsDataEnv.Tables["QWOrderH"].Rows[0];
                DataRow dtrTemVal = null;

                objSQLHelper.SetPara(new object[] { dtrWOrderH[QMFWOrderHDInfo.Field.MfgProdID].ToString() });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcSkid,fcCode,fcName,fcSupp,fnStdCost from PROD where fcSKid = ?", ref strErrorMsg))
                {
                    dtrTemVal = this.dtsDataEnv.Tables["QProd"].Rows[0];
                    strQcMfgProd = dtrTemVal["fcCode"].ToString();
                    strQnMfgProd = dtrTemVal["fcName"].ToString();
                }

                objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCorp", "CORP", "select fcSkid,fcCode,fcName from CORP where fcSKid = ?", ref strErrorMsg))
                {
                    dtrTemVal = this.dtsDataEnv.Tables["QCorp"].Rows[0];
                    strQcCorp = dtrTemVal["fcCode"].ToString();
                    strQnCorp = dtrTemVal["fcName"].ToString();
                }

                objSQLHelper.SetPara(new object[] { dtrWOrderH[QMFWOrderHDInfo.Field.SectID].ToString() });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select fcSkid,fcCode,fcName from SECT where fcSKid = ?", ref strErrorMsg))
                {
                    dtrTemVal = this.dtsDataEnv.Tables["QSect"].Rows[0];
                    strQcSect = dtrTemVal["fcCode"].ToString();
                    strQnSect = dtrTemVal["fcName"].ToString();
                }

                objSQLHelper.SetPara(new object[] { dtrWOrderH[QMFWOrderHDInfo.Field.JobID].ToString() });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QJob", "JOB", "select fcSkid,fcCode,fcName from JOB where fcSKid = ?", ref strErrorMsg))
                {
                    dtrTemVal = this.dtsDataEnv.Tables["QJob"].Rows[0];
                    strQcJob = dtrTemVal["fcCode"].ToString();
                    strQnJob = dtrTemVal["fcName"].ToString();
                }

                objSQLHelper2.SetPara(new object[] { dtrWOrderH[QMFWOrderHDInfo.Field.BOMID].ToString() });
                if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QBOM", QMFBOMInfo.TableName, "select cCode,cName from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                {
                    dtrTemVal = this.dtsDataEnv.Tables["QBOM"].Rows[0];
                    strQcBOM = dtrTemVal["cCode"].ToString();
                    strQnBOM = dtrTemVal["cName"].ToString();
                }

                objSQLHelper2.SetPara(new object[] { dtrWOrderH[QMFWOrderHDInfo.Field.MfgBookID].ToString() });
                if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QMfgBook", QMFBOMInfo.TableName, "select cCode,cName," + QMfgBookInfo.Field.WHouse_RM + " from " + QMfgBookInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                {
                    dtrTemVal = this.dtsDataEnv.Tables["QMfgBook"].Rows[0];
                    strWHouse_List1 = dtrTemVal[QMfgBookInfo.Field.WHouse_RM].ToString();
                    strWHouse_List1 = strWHouse_List1.Replace("'", "");
                }

                decMfgQty = Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.Qty]);
                decMOQty = Convert.ToDecimal(dtrWOrderH[QMFWOrderHDInfo.Field.MfgQty]);
                dttDate = Convert.ToDateTime(dtrWOrderH[QMFWOrderHDInfo.Field.Date]);
                if (!Convert.IsDBNull(dtrWOrderH[QMFWOrderHDInfo.Field.StartDate]))
                {
                    dttStartDate = Convert.ToDateTime(dtrWOrderH[QMFWOrderHDInfo.Field.StartDate]);
                }
                else 
                {
                    dttStartDate = dttDate;
                }
                dttDueDate = Convert.ToDateTime(dtrWOrderH[QMFWOrderHDInfo.Field.DueDate]);

            }

            foreach (DataRow dtrSource in this.dtsDataEnv.Tables[this.mstrAlias].Rows)
            {
                DataRow dtrPrnData = dtsPrintPreview.XRPGENPR.NewRow();

                dtrPrnData["cQcCorp"] = strQcCorp;
                dtrPrnData["cQnCorp"] = strQnCorp;

                dtrPrnData["cQcMfgProd"] = strQcMfgProd;
                dtrPrnData["cQnMfgProd"] = strQnMfgProd;
                dtrPrnData["cQcBOM"] = strQcBOM;
                dtrPrnData["cQnBOM"] = strQnBOM;
                dtrPrnData["nMfgQty"] = decMfgQty;
                dtrPrnData["nMOQty"] = decMOQty;
                dtrPrnData["dDate"] = dttDate;
                dtrPrnData["dStartDate"] = dttStartDate;
                dtrPrnData["dDueDate"] = dttDueDate;
                dtrPrnData["cQcSect"] = strQcSect;
                dtrPrnData["cQnSect"] = strQnSect;
                dtrPrnData["cQcJob"] = strQcJob;
                dtrPrnData["cQnJob"] = strQnJob;

                dtrPrnData["cRefNo_MO"] = dtrSource["cRefNo_MO"].ToString();
                dtrPrnData["cCode"] = dtrSource["cCode"].ToString();
                dtrPrnData["cRefNo"] = dtrSource["cRefNo"].ToString();
                dtrPrnData["cQcCoor"] = dtrSource["cQcCoor"].ToString();
                dtrPrnData["cQnCoor"] = dtrSource["cQnCoor"].ToString();
                dtrPrnData["cQcProd"] = dtrSource["cQcProd"].ToString();
                dtrPrnData["cQnProd"] = dtrSource["cQnProd"].ToString();

                //this.pmLoadRemark(dtrSource["cProd"].ToString());
                this.pmLoadRemark2(dtrSource["cWOrderI"].ToString());

                dtrPrnData["nBakQty_MO"] = Convert.ToDecimal(dtrSource["nBakQty_MO"]);
                dtrPrnData["nBakQty_PR"] = Convert.ToDecimal(dtrSource["nBakQty_PR"].ToString());
                dtrPrnData["nBakQty_PO"] = Convert.ToDecimal(dtrSource["nBakQty_PO"].ToString());
                dtrPrnData["nStkQty"] = Convert.ToDecimal(dtrSource["nStkQty"].ToString());
                dtrPrnData["nRefQty"] = Convert.ToDecimal(dtrSource["nRefQty"].ToString());
                dtrPrnData["nIssueQty"] = Convert.ToDecimal(dtrSource["nIssueQty"].ToString());
                dtrPrnData["nQty"] = Convert.ToDecimal(dtrSource["nQty"].ToString());
                dtrPrnData["nPrice"] = Convert.ToDecimal(dtrSource["nPrice"].ToString());

                dtrPrnData["cWHouse_List1"] = strWHouse_List1;

                dtrPrnData["cProd_Remark1"] = this.mstrRemark1;
                dtrPrnData["cProd_Remark2"] = this.mstrRemark2;
                dtrPrnData["cProd_Remark3"] = this.mstrRemark3;
                dtrPrnData["cProd_Remark4"] = this.mstrRemark4;
                dtrPrnData["cProd_Remark5"] = this.mstrRemark5;
                dtrPrnData["cProd_Remark6"] = this.mstrRemark6;
                dtrPrnData["cProd_Remark7"] = this.mstrRemark7;
                dtrPrnData["cProd_Remark8"] = this.mstrRemark8;
                dtrPrnData["cProd_Remark9"] = this.mstrRemark9;
                dtrPrnData["cProd_Remark10"] = this.mstrRemark10;

                dtsPrintPreview.XRPGENPR.Rows.Add(dtrPrnData);
            }
            this.pmPreviewReport(dtsPrintPreview, inRPTFileName);
        }

        private void pmLoadRemark2(string inWOrderI)
        {

            this.mstrRemark1 = "";
            this.mstrRemark2 = "";
            this.mstrRemark3 = "";
            this.mstrRemark4 = "";
            this.mstrRemark5 = "";
            this.mstrRemark6 = "";
            this.mstrRemark7 = "";
            this.mstrRemark8 = "";
            this.mstrRemark9 = "";
            this.mstrRemark10 = "";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { inWOrderI });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWOrderI4", QMFWOrderIT_PdInfo.TableName, "select * from " + QMFWOrderIT_PdInfo.TableName + " where cRowID = ?", ref strErrorMsg))
            {
                DataRow dtrWOrderI = this.dtsDataEnv.Tables["QWOrderI4"].Rows[0];

                this.mstrRemark1 = (Convert.IsDBNull(dtrWOrderI["cReMark1"]) ? "" : dtrWOrderI["cReMark1"].ToString().TrimEnd());
                this.mstrRemark2 = (Convert.IsDBNull(dtrWOrderI["cReMark2"]) ? "" : dtrWOrderI["cReMark2"].ToString().TrimEnd());
                this.mstrRemark3 = (Convert.IsDBNull(dtrWOrderI["cReMark3"]) ? "" : dtrWOrderI["cReMark3"].ToString().TrimEnd());
                this.mstrRemark4 = (Convert.IsDBNull(dtrWOrderI["cReMark4"]) ? "" : dtrWOrderI["cReMark4"].ToString().TrimEnd());
                this.mstrRemark5 = (Convert.IsDBNull(dtrWOrderI["cReMark5"]) ? "" : dtrWOrderI["cReMark5"].ToString().TrimEnd());
                this.mstrRemark6 = (Convert.IsDBNull(dtrWOrderI["cReMark6"]) ? "" : dtrWOrderI["cReMark6"].ToString().TrimEnd());
                this.mstrRemark7 = (Convert.IsDBNull(dtrWOrderI["cReMark7"]) ? "" : dtrWOrderI["cReMark7"].ToString().TrimEnd());
                this.mstrRemark8 = (Convert.IsDBNull(dtrWOrderI["cReMark8"]) ? "" : dtrWOrderI["cReMark8"].ToString().TrimEnd());
                this.mstrRemark9 = (Convert.IsDBNull(dtrWOrderI["cReMark9"]) ? "" : dtrWOrderI["cReMark9"].ToString().TrimEnd());
                this.mstrRemark10 = (Convert.IsDBNull(dtrWOrderI["cReMark10"]) ? "" : dtrWOrderI["cReMark10"].ToString().TrimEnd());

            }

        }

        private void pmLoadRemark(string inProd)
        {

            this.mstrRemark1 = "";
            this.mstrRemark2 = "";
            this.mstrRemark3 = "";
            this.mstrRemark4 = "";
            this.mstrRemark5 = "";
            this.mstrRemark6 = "";
            this.mstrRemark7 = "";
            this.mstrRemark8 = "";
            this.mstrRemark9 = "";
            this.mstrRemark10 = "";

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            pobjSQLUtil.SetPara(new object[] { inProd });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProdX4", "PRODX4", "select * from ProdX4 where fcProd = ?", ref strErrorMsg))
            {
                DataRow dtrProdX4 = this.dtsDataEnv.Tables["QProdX4"].Rows[0];

                string gcTemStr01 = (Convert.IsDBNull(dtrProdX4["fmMemData"]) ? "" : dtrProdX4["fmMemData"].ToString().TrimEnd());
                string gcTemStr02 = (Convert.IsDBNull(dtrProdX4["fmMemData2"]) ? "" : dtrProdX4["fmMemData2"].ToString().TrimEnd());
                string gcTemStr03 = (Convert.IsDBNull(dtrProdX4["fmMemData3"]) ? "" : dtrProdX4["fmMemData3"].ToString().TrimEnd());
                string gcTemStr04 = (Convert.IsDBNull(dtrProdX4["fmMemData4"]) ? "" : dtrProdX4["fmMemData4"].ToString().TrimEnd());
                string gcTemStr05 = (Convert.IsDBNull(dtrProdX4["fmMemData5"]) ? "" : dtrProdX4["fmMemData5"].ToString().TrimEnd());

                this.mstrRemark1 = BizRule.GetMemData(gcTemStr01, frmProd.xdCMRem);
                this.mstrRemark2 = BizRule.GetMemData(gcTemStr01, frmProd.xdCMRem2);
                this.mstrRemark3 = BizRule.GetMemData(gcTemStr02, frmProd.xdCMRem3);
                this.mstrRemark4 = BizRule.GetMemData(gcTemStr02, frmProd.xdCMRem4);
                this.mstrRemark5 = BizRule.GetMemData(gcTemStr03, frmProd.xdCMRem5);
                this.mstrRemark6 = BizRule.GetMemData(gcTemStr03, frmProd.xdCMRem6);
                this.mstrRemark7 = BizRule.GetMemData(gcTemStr04, frmProd.xdCMRem7);
                this.mstrRemark8 = BizRule.GetMemData(gcTemStr04, frmProd.xdCMRem8);
                this.mstrRemark9 = BizRule.GetMemData(gcTemStr05, frmProd.xdCMRem9);
                this.mstrRemark10 = BizRule.GetMemData(gcTemStr05, frmProd.xdCMRem10);

            }

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData, string inRPTFileName)
        {
            //ReportDocument rptPreviewReport = new ReportDocument();

            //string strRPTFileName = inRPTFileName; // Application.StartupPath + @"\RPT\XRPGENPR.rpt";

            //if (!System.IO.File.Exists(strRPTFileName))
            //{
            //    MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //rptPreviewReport.Load(strRPTFileName);
            //rptPreviewReport.SetDataSource(inData);

            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "Print Generate PR from MO");
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "");
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "XRPGENPR");
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.AppUserName);
            
            //ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            //prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            //prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            //prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            //prmCRPara["PFPRINTBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[3]);
            
            //App.PreviewReport(this, false, rptPreviewReport);
        }
    
    }
}
