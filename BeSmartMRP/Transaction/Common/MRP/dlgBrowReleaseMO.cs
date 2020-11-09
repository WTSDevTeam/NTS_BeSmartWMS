
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
using DevExpress.XtraGrid.Views.Base;


namespace BeSmartMRP.Transaction.Common.MRP
{
    public partial class dlgBrowReleaseMO : UIHelper.frmBase
    {

        public dlgBrowReleaseMO()
        {
            InitializeComponent();
            this.pmInitForm();
        }


        private DataSet dtsDataEnv = null;
        private string mstrAlias = "";
        private string mstrSortKey = "";
        private string mstrTemPd = "TemPd";
        private string mstrBranch = "";
        private string mstrFrWhType = " ";

        private DialogForms.dlgGetWHouse pofrmGetWareHouse = null;

        public string BranchID
        {
            set { this.mstrBranch = value; }
        }

        private void pmInitForm()
        {
            this.pmMapEvent();
            this.pmSetFormUI();
            UIBase.SetDefaultChildAppreance(this);
        }

        private void pmSetFormUI()
        {

            this.lblFrQcWHouse.Text = UIBase.GetAppUIText(new string[] { "คลังสินค้า :", "Ware House :" });

        }

        private void pmMapEvent()
        {
            this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
            this.grcQcWHouse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcWHouse.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);
        }

        public void BindData(DataSet inDataEnv, string inAlias)
        {
            this.mstrAlias = inAlias;
            this.dtsDataEnv = inDataEnv;
            this.pmBindData();
        }

        private void pmBindData()
        {
            this.pmInitGridProp_TemPd();
        }

        private void pmInitGridProp_TemPd()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemPd].DefaultView;

            this.grdTemPd.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
                this.gridView1.Columns[intCnt].OptionsColumn.AllowEdit = false;
                this.gridView1.Columns[intCnt].OptionsColumn.AllowFocus = false;
            }

            int i = 0;
            this.gridView1.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView1.Columns["cOPSeq"].VisibleIndex = i++;
            this.gridView1.Columns["cPdType"].VisibleIndex = i++;
            this.gridView1.Columns["cQcProd"].VisibleIndex = i++;
            //this.gridView1.Columns["cQnProd"].VisibleIndex = i++;
            //this.gridView1.Columns["cRemark1"].VisibleIndex = i++;
            this.gridView1.Columns["cQcWHouse"].VisibleIndex = i++;
            this.gridView1.Columns["cLot"].VisibleIndex = i++;
            this.gridView1.Columns["nQty"].VisibleIndex = i++;
            this.gridView1.Columns["cQnUOM"].VisibleIndex = i++;
            this.gridView1.Columns["cProcure"].VisibleIndex = i++;
            this.gridView1.Columns["cScrap"].VisibleIndex = i++;
            this.gridView1.Columns["cRoundCtrl"].VisibleIndex = i++;
            this.gridView1.Columns["cQcBOM"].VisibleIndex = i++;
            //this.gridView1.Columns["cSubSti"].VisibleIndex = i++;

            this.gridView1.Columns["nRecNo"].Caption = "No.";
            this.gridView1.Columns["cOPSeq"].Caption = "OP. Seq.";
            this.gridView1.Columns["cPdType"].Caption = "T";
            this.gridView1.Columns["cQcProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "รหัสสินค้า", "RM / Product Code" });
            this.gridView1.Columns["cQnProd"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ชื่อสินค้า", "RM / Product Description" });
            this.gridView1.Columns["cRemark1"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หมายเหตุ", "Remark" });
            this.gridView1.Columns["nRefQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน BOM", "BOM Qty." });
            this.gridView1.Columns["nQty"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "จำนวน", "Qty." });
            this.gridView1.Columns["cQnUOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "หน่วย", "Unit" });
            this.gridView1.Columns["cQcWHouse"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "คลังสินค้า", "WareHouse" });
            this.gridView1.Columns["cLot"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ล๊อต", "Lot" });
            this.gridView1.Columns["cScrap"].Caption = "Scrap";
            this.gridView1.Columns["cProcure"].Caption = "Procure";
            this.gridView1.Columns["cSubSti"].Caption = "S";
            this.gridView1.Columns["cQcBOM"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "BOM", "BOM" });
            this.gridView1.Columns["cRoundCtrl"].Caption = "Round";

            this.gridView1.Columns["nRecNo"].Width = 50;
            this.gridView1.Columns["cOPSeq"].Width = 80;
            this.gridView1.Columns["cPdType"].Width = 50;
            this.gridView1.Columns["cQcProd"].Width = 130;
            this.gridView1.Columns["cRemark1"].Width = 80;
            this.gridView1.Columns["nRefQty"].Width = 80;
            this.gridView1.Columns["nQty"].Width = 80;
            this.gridView1.Columns["cQnUOM"].Width = 80;
            this.gridView1.Columns["cScrap"].Width = 80;
            this.gridView1.Columns["cProcure"].Width = 60;
            this.gridView1.Columns["cQcBOM"].Width = 130;
            //this.gridView1.Columns["cSubSti"].Width = 5;
            this.gridView1.Columns["cRoundCtrl"].Width = 60;

            //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.grcQcProd.Buttons[0].Tag = "GRDVIEW1_CQCPROD";
            this.grcPdType.Buttons[0].Tag = "GRDVIEW1_CPDTYPE";
            //this.grcRemark2.Buttons[0].Tag = "GRDVIEW1_CREMARK1";
            this.grcQcBOM.Buttons[0].Tag = "GRDVIEW1_CQCBOM";
            this.grcQcWHouse.Buttons[0].Tag = "GRDVIEW1_CQCWHOUSE";
            this.grcLot.Buttons[0].Tag = "GRDVIEW1_CQCWHOUSE";

            this.grcQcProd.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, "PROD", "FCCODE");
            this.grcQcWHouse.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, "WHOUSE", "FCCODE");
            //this.grcRemark.MaxLength = 150;
            //this.grcRemark2.MaxLength = 150;
            this.grcScrap.MaxLength = 20;

            this.gridView1.Columns["cQcWHouse"].OptionsColumn.AllowEdit = true;
            this.gridView1.Columns["cQcWHouse"].OptionsColumn.AllowFocus = true;

            this.gridView1.Columns["cLot"].OptionsColumn.AllowEdit = true;
            this.gridView1.Columns["cLot"].OptionsColumn.AllowFocus = true;

            //this.gridView1.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView1.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView1.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView1.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView1.Columns["nRefQty"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView1.Columns["nRefQty"].OptionsColumn.AllowFocus = false;
            this.gridView1.Columns["nRefQty"].OptionsColumn.ReadOnly = true;

            this.gridView1.Columns["nRefQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nRefQty"].DisplayFormat.FormatString = "#,###,###,##0.0000";

            this.gridView1.Columns["cQnProd"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView1.Columns["cQnProd"].OptionsColumn.AllowFocus = false;
            this.gridView1.Columns["cQnProd"].OptionsColumn.ReadOnly = true;

            this.gridView1.Columns["cQnUOM"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView1.Columns["cQnUOM"].OptionsColumn.AllowFocus = false;
            this.gridView1.Columns["cQnUOM"].OptionsColumn.ReadOnly = true;

            this.gridView1.Columns["nQty"].ColumnEdit = this.grcQty;
            this.gridView1.Columns["cPdType"].ColumnEdit = this.grcPdType;
            this.gridView1.Columns["cQcProd"].ColumnEdit = this.grcQcProd;
            //this.gridView1.Columns["cRemark1"].ColumnEdit = this.grcRemark2;
            this.gridView1.Columns["cProcure"].ColumnEdit = this.grcProcure;
            this.gridView1.Columns["cScrap"].ColumnEdit = this.grcScrap;
            this.gridView1.Columns["cQcBOM"].ColumnEdit = this.grcQcBOM;
            this.gridView1.Columns["cRoundCtrl"].ColumnEdit = this.grcRoundCtrl;
            this.gridView1.Columns["cQcWHouse"].ColumnEdit = this.grcQcWHouse;
            this.gridView1.Columns["cLot"].ColumnEdit = this.grcLot;

            this.pmCalcColWidth2();
        }

        private void pmCalcColWidth2()
        {

            int intColWidth = this.gridView1.Columns["nRecNo"].Width
                                    + this.gridView1.Columns["cOPSeq"].Width
                                    + this.gridView1.Columns["cPdType"].Width
                                    + this.gridView1.Columns["cQcProd"].Width
                                    + this.gridView1.Columns["cRemark1"].Width
                                    + this.gridView1.Columns["cScrap"].Width
                                    + this.gridView1.Columns["nQty"].Width
                                    + this.gridView1.Columns["cQnUOM"].Width
                                    + this.gridView1.Columns["cProcure"].Width
                                    + this.gridView1.Columns["cRoundCtrl"].Width
                                    + this.gridView1.Columns["cQcBOM"].Width;

            int intNewWidth = this.Width - intColWidth - 60;
            this.gridView1.Columns["cQnProd"].Width = (intNewWidth > 70 ? intNewWidth : 70);
        }

        private bool pmInitPopUpDialog(string inDialogName)
        {

            string strErrorMsg = "";
            DataRow dtrGetVal = null;
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "WHOUSE":
                    if (this.pofrmGetWareHouse == null)
                    {
                        this.pofrmGetWareHouse = new DialogForms.dlgGetWHouse();
                        this.pofrmGetWareHouse.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetWareHouse.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
            }
            return true;
        }

        private void pmPopUpButtonClick(string inTextbox, string inTag, string inPara1)
        {
            string strPrefix = "";
            switch (inTextbox)
            {
                case "TXTFRQCWHOUSE":
                case "TXTFRQNWHOUSE":
                    this.pmInitPopUpDialog("WHOUSE");
                    strPrefix = ("TXTFRQCWHOUSE".IndexOf(inTextbox) > -1 ? "FCCODE" : "FCNAME");
                    this.pofrmGetWareHouse.ValidateField(this.mstrBranch, "(" + this.pmSplitToSQLStr(this.mstrFrWhType) + ")", "", strPrefix, true);
                    if (this.pofrmGetWareHouse.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            //App.ActivateMainScreen();
            //App.SetForegroundWindow(this.Handle);

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DataRow dtrGetVal = null;
            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "TXTFRQCWHOUSE":
                case "TXTFRQNWHOUSE":
                    if (this.pofrmGetWareHouse != null)
                    {
                        dtrGetVal = this.pofrmGetWareHouse.RetrieveValue();
                        if (dtrGetVal != null)
                        {
                            if (this.txtFrQcWHouse.Tag.ToString() != dtrGetVal["fcSkid"].ToString())
                            {
                            }
                            this.txtFrQcWHouse.Tag = dtrGetVal["fcSkid"].ToString();
                            this.txtFrQcWHouse.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                            this.txtFrQnWHouse.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtFrQcWHouse.Tag = "";
                            this.txtFrQcWHouse.Text = "";
                            this.txtFrQnWHouse.Text = "";
                        }
                    }
                    break;
                case "GRDVIEW1_CQCWHOUSE":
                case "GRDVIEW1_CQNWHOUSE":

                    dtrGetVal = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrGetWHouse = this.pofrmGetWareHouse.RetrieveValue();
                    if (dtrGetWHouse != null)
                    {
                        dtrGetVal["cWHouse"] = dtrGetWHouse["fcSkid"].ToString();
                        dtrGetVal["cQcWHouse"] = dtrGetWHouse["fcCode"].ToString().TrimEnd();
                        dtrGetVal["cQnWHouse"] = dtrGetWHouse["fcName"].ToString().TrimEnd();
                    }
                    else
                    {
                        dtrGetVal["cWHouse"] = "";
                        dtrGetVal["cQcWHouse"] = "";
                        dtrGetVal["cQnWHouse"] = "";
                    }

                    this.gridView1.UpdateCurrentRow();

                    break;

            }
        }

        private void barManager1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (e.Item.Tag.ToString())
            {
                case "ENTER":
                    this.DialogResult = DialogResult.OK;
                    break;
                case "EXIT":
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }

        }

        private void dlgBOMViewCost_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

        private void grdTemPd_SortKeysChanged(object sender, System.EventArgs e)
        {
        }

        private void pmGridPopUpButtonClick(string inKeyField)
        {
            string strOrderBy = "";
            switch (inKeyField.ToUpper())
            {
                case "GRDVIEW1_CQCWHOUSE":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW1_CQCWHOUSE" ? "FCCODE" : "FCNAME");
                    this.pmInitPopUpDialog("WHOUSE");
                    this.pofrmGetWareHouse.ValidateField(this.mstrBranch, "", strOrderBy, true);
                    if (this.pofrmGetWareHouse.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inKeyField);
                    }
                    break;

            }
        }

        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
                return;

            ColumnView view = this.grdTemPd.MainView as ColumnView;

            string strValue = "";
            string strCol = gridView1.FocusedColumn.FieldName;
            DataRow dtrTemPd = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);


            switch (strCol.ToUpper())
            {
                case "CQCWHOUSE":
                case "CQNWHOUSE":

                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cWHouse"] = "";
                        dtrTemPd["cQcWHouse"] = "";
                        dtrTemPd["cQnWHouse"] = "";
                    }
                    else
                    {

                        this.pmInitPopUpDialog("WHOUSE");
                        string strOrderBy = (strCol.ToUpper() == "CQCWHOUSE" ? "FCCODE" : "FCNAME");
                        e.Valid = !this.pofrmGetWareHouse.ValidateField(this.mstrBranch, "(" + this.pmSplitToSQLStr(this.mstrFrWhType) + ")", strValue, strOrderBy, false);

                        if (this.pofrmGetWareHouse.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView1.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW1_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCWHOUSE" ? dtrTemPd["cQcWHouse"].ToString().TrimEnd() : dtrTemPd["cQnWHouse"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cWHouse"] = "";
                            dtrTemPd["cQcWHouse"] = "";
                            dtrTemPd["cQnWHouse"] = "";
                        }
                    }
                    break;
            }

        }

        private void grcGridColumn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Tag != null)
                this.pmGridPopUpButtonClick(e.Button.Tag.ToString());
        }

        private void grcGridColumn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    string strPrefix = "GRDVIEW1_";
                    this.pmGridPopUpButtonClick(strPrefix + this.gridView1.FocusedColumn.FieldName);
                    //this.pmGridPopUpButtonClick(gridView2.FocusedColumn.FieldName);
                    break;
            }
        }

        private string pmSplitToSQLStr(string inStr)
        {
            string strResult = "";
            string[] staSQL = inStr.Split(",".ToCharArray());
            if (staSQL.Length > 0)
            {
                for (int intCnt = 0; intCnt < staSQL.Length; intCnt++)
                {
                    strResult += "'" + staSQL[intCnt] + (intCnt == staSQL.Length - 1 ? "'" : "',");
                }
            }
            return strResult;
        }

    }
}