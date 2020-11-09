
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using DevExpress.XtraGrid.Views.Base;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;

namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgShowPack : UIHelper.frmBase
    {

        public dlgShowPack()
        {
            InitializeComponent();

            this.pmInitForm();
            this.barMainEdit.Items["btnDelSO"].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        private string mstrRefType = "SO";
        private string mstrBranch = "";
        private string mstrCoor = "";
        private string mstrProd = "";
        private string mstrReferKey = "";
        private string mstrTemPd = "dlgShowPack_TemPd";

        private bool mbllRecalTotPd = false;
        private decimal mdecCurrQty = 0;

        private UIHelper.IfrmDBBase pofrmGetWHPack = null;
        
        private DataSet dtsDataEnv = new DataSet();

        private string mstrQcProd = "";

        public void BindData(string inQcProd, DataSet inDataSet, DataTable inSource)
        {
            this.mstrQcProd = inQcProd;
            dtsDataEnv = inDataSet;

            DataRow[] da1 = this.dtsDataEnv.Tables["TemPd"].Select("cQcProd = '" + this.mstrQcProd + "'");
            if (da1.Length > 0)
            {
                this.txtQcProd.Text = da1[0]["cQcProd"].ToString();
                this.txtQnProd.Text = da1[0]["cQnProd"].ToString();
                decimal decSumQty = 0;
                for (int i = 0; i < da1.Length; i++)
                {
                    decSumQty += Convert.ToDecimal(da1[i]["nQty"]);
                }
                this.txtSumQty.Text = decSumQty.ToString("#,###,###.00");
            }

            foreach (DataRow dtrTemSO in inSource.Rows)
            {
                DataRow[] da = this.dtsDataEnv.Tables[this.mstrTemPd].Select("cParent_GUID = '" + dtrTemSO["cGUID"].ToString() + "'");
                if (da.Length == 0)
                {
                    //DataRow dtrNewSO = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                    //dtrNewSO["cParent_GUID"] = dtrTemSO["cGUID"].ToString();
                    //this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrNewSO);
                }

            }
        }

        private void pmInitForm()
        {

            this.pmMapEvent();
            this.pmSetFormUI();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            UIBase.SetDefaultChildAppreance(this);
            this.pmCreateTem();
            this.pmInitGridProp();
        }

        private void pmMapEvent()
        {

            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);

            this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
            //this.gridView1.GotFocus += new EventHandler(gridView1_GotFocus);
            //this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridView1.CellValueChanged += new CellValueChangedEventHandler(this.gridView1_CellValueChanged);

            this.grcQcPack.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grcGridColumn_KeyDown);
            this.grcQcPack.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.grcGridColumn_ButtonClick);

        }

        private void pmSetFormUI()
        {

            //this.Text = UIBase.GetAppUIText(new string[] { "เพิ่ม/แก้ไข" + this.mstrFormMenuName, this.mstrFormMenuName + " ENTRY" });

            //this.lblBook.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "ระบุเล่มเอกสาร", "Book Code" }) });
            this.lblCoor.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "สินค้า", "Product" }) });
            //this.lblTitle1.Text = UIBase.GetAppUIText(new string[] { "รายการสินค้าตาม P/O", "Product Items from P/O" });
        }

        public DataTable RefTable
        {
            get { return this.dtsDataEnv.Tables[this.mstrTemPd]; }
            //set { this.dtsDataEnv.Tables[this.mstrTemPd] = value; }
        }

        private void pmCreateTem()
        {

            this.dtsDataEnv.CaseSensitive = true;

            DataTable dtbTemRef2Pack = new DataTable(this.mstrTemPd);
            dtbTemRef2Pack.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemRef2Pack.Columns.Add("cParent_GUID", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cGUID", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cQcProd", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cQnProd", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cLot", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cWHLoca", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cQcWHLoca", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
            dtbTemRef2Pack.Columns.Add("nLastQty", System.Type.GetType("System.Decimal"));
            dtbTemRef2Pack.Columns.Add("cQnUOM", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cShowPack", System.Type.GetType("System.String"));

            dtbTemRef2Pack.Columns.Add("nPackSize", System.Type.GetType("System.Decimal"));
            dtbTemRef2Pack.Columns.Add("cPackStyle", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cQcPackStyle", System.Type.GetType("System.String"));
            dtbTemRef2Pack.Columns.Add("cQnPackStyle", System.Type.GetType("System.String"));

            dtbTemRef2Pack.Columns["cRowID"].DefaultValue = "";
            dtbTemRef2Pack.Columns["cProd"].DefaultValue = "";
            dtbTemRef2Pack.Columns["cQcProd"].DefaultValue = "";
            dtbTemRef2Pack.Columns["cQnProd"].DefaultValue = "";
            dtbTemRef2Pack.Columns["cShowPack"].DefaultValue = "";
            dtbTemRef2Pack.Columns["cPackStyle"].DefaultValue = "";
            dtbTemRef2Pack.Columns["cQnPackStyle"].DefaultValue = "";
            dtbTemRef2Pack.Columns["nPackSize"].DefaultValue = 0;
            dtbTemRef2Pack.Columns["nQty"].DefaultValue = 0;
            dtbTemRef2Pack.Columns["nLastQty"].DefaultValue = 0;


            dtbTemRef2Pack.TableNewRow += new DataTableNewRowEventHandler(dtbTemPd_TableNewRow);

            this.dtsDataEnv.Tables.Add(dtbTemRef2Pack);
        }

        private void dtbTemPd_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
            e.Row["cGUID"] = Guid.NewGuid().ToString();
        }

        private void pmInitGridProp()
        {

            this.grdTemPack.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
                //this.gridView1.Columns[intCnt].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
                //this.gridView1.Columns[intCnt].OptionsColumn.AllowFocus = false;
                //this.gridView1.Columns[intCnt].OptionsColumn.ReadOnly = true;
            
            }

            int i = 0;

            this.gridView1.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView1.Columns["cQcPackStyle"].VisibleIndex = i++;
            this.gridView1.Columns["cQnPackStyle"].VisibleIndex = i++;
            this.gridView1.Columns["nPackSize"].VisibleIndex = i++;
            this.gridView1.Columns["cShowPack"].VisibleIndex = i++;
            //this.gridView1.Columns["fnMOQty"].VisibleIndex = i++;

            this.gridView1.Columns["nRecNo"].Caption = UIBase.GetAppUIText(new string[] { "No.", "No." });
            this.gridView1.Columns["cQcPackStyle"].Caption = UIBase.GetAppUIText(new string[] { "Pack Code", "Pack Code" });
            this.gridView1.Columns["cQnPackStyle"].Caption = UIBase.GetAppUIText(new string[] { "Pack Name", "Pack Name" });
            this.gridView1.Columns["nPackSize"].Caption = UIBase.GetAppUIText(new string[] { "Pack Size", "Pack Size" });
            this.gridView1.Columns["cShowPack"].Caption = UIBase.GetAppUIText(new string[] { "Pack Detail", "Pack Detail" });

            this.gridView1.Columns["nRecNo"].Width = 35;
            this.gridView1.Columns["cQcPackStyle"].Width = 85;
            this.gridView1.Columns["cQnPackStyle"].Width = 120;
            this.gridView1.Columns["nPackSize"].Width = 85;
            this.gridView1.Columns["cShowPack"].Width = 85;

            this.gridView1.Columns["nPackSize"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["nPackSize"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView1.Columns["cQcPackStyle"].ColumnEdit = this.grcQcPack;
            this.gridView1.Columns["cQnPackStyle"].ColumnEdit = this.grcQcPack;
            this.gridView1.Columns["cShowPack"].ColumnEdit = this.grcShowPack;
            
            //this.pmCalcColWidth();
            this.grdTemPack.Focus();
        }

        private void frmBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if (MessageBox.Show("ต้องการออกจากหน้าจอ ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.DialogResult = DialogResult.Cancel;
                    }
                    break;
            }

        }

        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                string strMenuName = e.Item.Tag.ToString();
                switch (strMenuName)
                {
                    case "LOADSO":
                        this.pmInitPopUpDialog("LOADSO");
                        break;
                    case "CLEARSO":

                        break;
                    case "OK":
                        this.DialogResult = DialogResult.OK;
                        break;
                    case "EXIT":
                        this.DialogResult = DialogResult.Cancel;
                        break;
                }

            }
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "WHPACK":
                    if (this.pofrmGetWHPack == null)
                    {
                        this.pofrmGetWHPack = new frmWHPack(FormActiveMode.PopUp);
                    }
                    break;
                case "SHOW_PACKDET":
                    DataRow dtrTemPack = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
                    if (dtrTemPack == null) return;

                    using (Common.dlgShowPack_Det dlgRefTo = new Common.dlgShowPack_Det())
                    {

                        dlgRefTo.BindData(this.mstrQcProd, dtrTemPack["cQcPackStyle"].ToString(), dtrTemPack["cQnPackStyle"].ToString(), Convert.ToDecimal(dtrTemPack["nPackSize"]), this.dtsDataEnv, this.dtsDataEnv.Tables[this.mstrTemPd]);
                        dlgRefTo.ShowDialog();
                        if (dlgRefTo.DialogResult == DialogResult.OK)
                        {
                            //this.pmLoadRefToOrder(dlgRefTo.RefTable);
                            //this.pmRecalTotPd();
                        }


                    }
                    //
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
            string strOrderBy = "";
            switch (inTextbox)
            {
                case "TXTQCBOOK":
                case "TXTQNBOOK":
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            switch (inPopupForm.TrimEnd().ToUpper())
            {

                case "GRDVIEW1_CQCPACKSTYLE":
                case "GRDVIEW1_CQNPACKSTYLE":

                    DataRow dtrGetVal = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);

                    if (dtrGetVal == null)
                    {
                        dtrGetVal = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrGetVal);
                    }

                    DataRow dtrGetProd = this.pofrmGetWHPack.RetrieveValue();
                    if (dtrGetProd != null)
                    {
                        dtrGetVal["cPackStyle"] = dtrGetProd["fcSkid"].ToString();
                        dtrGetVal["cQcPackStyle"] = dtrGetProd["fcCode"].ToString().TrimEnd();
                        dtrGetVal["cQnPackStyle"] = dtrGetProd["fcName"].ToString().TrimEnd();
                        dtrGetVal["nPackSize"] = dtrGetProd["fnPackSize"].ToString().TrimEnd();
                    }
                    else
                    {
                        //this.pmClr1TemPd();
                    }

                    this.gridView1.UpdateCurrentRow();

                    break;
            }
        }

        private void grcShowPack_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            this.pmInitPopUpDialog("SHOW_PACKDET");
        }

        private void pmGridPopUpButtonClick(string inKeyField)
        {
            DataRow dtrTemPd = null;
            string strOrderBy = "";
            switch (inKeyField.ToUpper())
            {
                case "GRDVIEW1_CQCWHPACK":
                case "GRDVIEW1_CQNWHPACK":
                    strOrderBy = (inKeyField.ToUpper() == "GRDVIEW1_CQCPROD" ? "FCCODE" : "FCNAME");
                    this.pmInitPopUpDialog("WHPACK");

                    dtrTemPd = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);


                    this.pofrmGetWHPack.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetWHPack.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inKeyField);
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
                    string strPrefix = (this.gridView1.Name.ToUpper() == "GRIDVIEW2" ? "GRDVIEW2_" : "GRDVIEW2_");
                    this.pmGridPopUpButtonClick(strPrefix + this.gridView1.FocusedColumn.FieldName);
                    //this.pmGridPopUpButtonClick(gridView2.FocusedColumn.FieldName);
                    break;
            }
        }

        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
                return;

            ColumnView view = this.grdTemPack.MainView as ColumnView;

            //string strValue = "";
            string strValue = e.Value.ToString();
            string strCol = gridView1.FocusedColumn.FieldName;
            DataRow dtrTemPd = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);

            string strPackStyle = dtrTemPd["cPackStyle"].ToString();

            switch (strCol.ToUpper())
            {

                case "CQCPACKSTYLE":
                case "CQNPACKSTYLE":

                    strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cPackStyle"] = "";
                        dtrTemPd["cQcPackStyle"] = "";
                        dtrTemPd["cQnPackStyle"] = "";
                        //this.pmClr1TemPd();
                    }
                    else
                    {

                        this.pmInitPopUpDialog("WHPACK");
                        string strOrderBy = (strCol.ToUpper() == "CQCPACKSTYLE" ? "FCCODE" : "FCNAME");
                        e.Valid = !this.pofrmGetWHPack.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmGetWHPack.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView1.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW1_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCPACKSTYLE" ? dtrTemPd["CQCPACKSTYLE"].ToString().TrimEnd() : dtrTemPd["CQNPACKSTYLE"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cPackStyle"] = "";
                            dtrTemPd["cQcPackStyle"] = "";
                            dtrTemPd["cQnPackStyle"] = "";
                            //this.pmClr1TemPd();
                        }
                    }
                    break;

                case "NQTY":
                    strValue = (strValue != string.Empty ? strValue : "0");
                    decimal decQty = Convert.ToDecimal(strValue);
                    decimal decLastQty = Convert.ToDecimal(dtrTemPd["nLastQty"]);

                    if (decQty != decLastQty)
                    {

                        if (decQty > 0 && strPackStyle == string.Empty)
                        {
                            this.gridView1.SetRowCellValue(this.gridView1.FocusedRowHandle, "nQty", 0);
                            this.gridView1.UpdateCurrentRow();
                            MessageBox.Show("กรุณาระบุ Storage Type ก่อน !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            this.gridView1.SetFocusedValue(decQty);
                            if (this.pmValidQtyCol())
                            {
                                this.mbllRecalTotPd = true;
                                e.Value = this.mdecCurrQty;
                            }
                            else
                            {
                                this.gridView1.SetFocusedValue(decLastQty);
                                e.Value = decLastQty;
                                //e.Valid = false;
                            }
                        }
                        this.mbllRecalTotPd = true;
                    }
                    break;
            }

        }
        private bool pmValidQtyCol()
        {
            return true;
        }


        private void gridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            this.gridView1.UpdateCurrentRow();
            //this.pmSumRMQty();
        }

    }
}
