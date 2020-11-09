
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

namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgGetRefToPO : UIHelper.frmBase
    {

        public dlgGetRefToPO(string inRefType, string inBranch, string inCoor, string inProd)
        {
            InitializeComponent();

            this.mstrRefType = inRefType;
            this.mstrBranch = inBranch;
            this.mstrCoor = inCoor;
            this.mstrProd = inProd;
            this.pmInitForm();
            this.barMainEdit.Items["btnDelSO"].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        private void grdTemPd_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {

        }

        public string FMBookID
        {
            get { return this.txtQcBook.Tag.ToString(); }
        }

        public string FMBookCode
        {
            get { return this.txtQcBook.Text; }
        }

        public string FMBookName
        {
            get { return this.txtQnBook.Text; }
        }


        private DialogForms.dlgGetFMBook pofrmGetBook = null;
        private DialogForms.dlgGetDoc1 pofrmGetDoc1 = null;

        private string mstrRefType = "SO";
        private int mintYear = 0;
        private string mstrBranch = "";
        private string mstrCoor = "";
        private string mstrProd = "";
        private string mstrReferKey = "";
        private string mstrTemPd = "TemPd";

        private DataSet dtsDataEnv = new DataSet();

        public void BindData(DataTable inSource)
        {
            foreach (DataRow dtrTemSO in inSource.Rows)
            {
                DataRow[] da = this.dtsDataEnv.Tables[this.mstrTemPd].Select("cOrderI = '" + dtrTemSO["cOrderI"].ToString() + "'");
                if (da.Length == 0)
                {
                    DataRow dtrNewSO = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                    dtrNewSO["cOrderI"] = dtrTemSO["cOrderI"].ToString();
                    dtrNewSO["cOrderH"] = dtrTemSO["cOrderH"].ToString();
                    dtrNewSO["cCoor"] = dtrTemSO["cCoor"].ToString();
                    dtrNewSO["cProd"] = dtrTemSO["cProd"].ToString();
                    dtrNewSO["QcProd"] = dtrTemSO["QcProd"].ToString();
                    dtrNewSO["QnProd"] = dtrTemSO["QnProd"].ToString();
                    dtrNewSO["fcCode"] = dtrTemSO["fcCode"].ToString();
                    dtrNewSO["fcRefNo"] = dtrTemSO["fcRefNo"].ToString();
                    dtrNewSO["fdDate"] = Convert.ToDateTime(dtrTemSO["fdDate"]);
                    dtrNewSO["fnBackQty"] = Convert.ToDecimal(dtrTemSO["fnBackQty"]);
                    dtrNewSO["fnMOQty"] = Convert.ToDecimal(dtrTemSO["fnMOQty"]);
                    //decSumSOQty = decSumSOQty + Convert.ToDecimal(dtrTemSO["fnMOQty"]);
                    this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrNewSO);

                    //strSORefNo += dtrNewSO["fcRefNo"].ToString().TrimEnd() + ",";
                    //strCoor = dtrTemSO["cCoor"].ToString();
                    //strProd = dtrTemSO["cProd"].ToString();

                }

            }
        }

        private void pmInitForm()
        {

            this.pmMapEvent();
            this.pmSetFormUI();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            this.txtQcBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper, "BOOK", "FCCODE");

            this.txtQcBook.Tag = "";
            this.txtQcBook.Text = "";
            this.txtQnBook.Text = "";

            this.pmDefaultBook();

            objSQLHelper.SetPara(new object[] { this.mstrCoor });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcSkid, fcCode, fcName from COOR where fcSkid = ? ", ref strErrorMsg))
            {
                DataRow dtrCoor = this.dtsDataEnv.Tables["QCoor"].Rows[0];
                this.txtQcCoor.Text = dtrCoor["fcCode"].ToString().TrimEnd();
                this.txtQnCoor.Text = dtrCoor["fcName"].ToString().TrimEnd();
            }

            UIBase.SetDefaultChildAppreance(this);
            this.pmCreateTem();
            this.pmInitGridProp();
        }

        private void pmMapEvent()
        {

            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);

            //this.grdTemOP.Resize += new EventHandler(grdTemOP_Resize);
            //this.gridView1.ColumnWidthChanged += new ColumnEventHandler(gridView1_ColumnWidthChanged);
            //this.gridView1.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
            //this.gridView1.GotFocus += new EventHandler(gridView1_GotFocus);

            this.txtQcBook.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcBook.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcBook.Validating += new CancelEventHandler(txtQcBook_Validating);

        }

        private void pmSetFormUI()
        {

            //this.Text = UIBase.GetAppUIText(new string[] { "เพิ่ม/แก้ไข" + this.mstrFormMenuName, this.mstrFormMenuName + " ENTRY" });

            this.lblBook.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "ระบุเล่มเอกสาร", "Book Code" }) });
            this.lblCoor.Text = string.Format("{0} :", new string[] { UIBase.GetAppUIText(new string[] { "ลูกค้า", "Customer" }) });

            if (this.mstrReferKey == "SO")
            {
                this.lblTitle1.Text = UIBase.GetAppUIText(new string[] { "รายการสินค้าตาม S/O", "Product Items from S/O" });
            }
            else
            {
                this.lblTitle1.Text = UIBase.GetAppUIText(new string[] { "รายการสินค้าตาม P/O", "Product Items from P/O" });
            }
        }

        private void pmDefaultBook()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "BGBook", "select fcSkid, fcCode, fcName from BOOK where fcCorp = ? and fcBranch = ? and fcRefType  = ? order by FCCODE", ref strErrorMsg))
            {
                DataRow dtrBook = this.dtsDataEnv.Tables["QBook"].Rows[0];
                this.txtQcBook.Tag = dtrBook["fcSkid"].ToString();
                this.txtQcBook.Text = dtrBook["fcCode"].ToString().TrimEnd();
                this.txtQnBook.Text = dtrBook["fcName"].ToString().TrimEnd();

            }
        }

        public DataTable RefTable
        {
            get { return this.dtsDataEnv.Tables[this.mstrTemPd]; }
            //set { this.dtsDataEnv.Tables[this.mstrTemPd] = value; }
        }

        private void pmCreateTem()
        {

            this.dtsDataEnv.CaseSensitive = true;

            DataTable dtbTemPd = new DataTable(this.mstrTemPd);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cGUID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cCoor", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cOrderI", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cOrderH", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("fcCode", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("fcRefNo", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("fdDate", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("fnBackQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("fnMOQty", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("QnProd", System.Type.GetType("System.String"));

            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["cGUID"].DefaultValue = "";
            dtbTemPd.Columns["cProd"].DefaultValue = "";
            dtbTemPd.Columns["cCoor"].DefaultValue = "";
            dtbTemPd.Columns["cOrderI"].DefaultValue = "";
            dtbTemPd.Columns["cOrderH"].DefaultValue = "";
            dtbTemPd.Columns["fcCode"].DefaultValue = "";
            dtbTemPd.Columns["fcRefNo"].DefaultValue = "";
            dtbTemPd.Columns["fnBackQty"].DefaultValue = 0;
            dtbTemPd.Columns["fnMOQty"].DefaultValue = 0;

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemPd_TableNewRow);
            
            this.dtsDataEnv.Tables.Add(dtbTemPd);
        }

        private void dtbTemPd_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
            e.Row["cGUID"] = Guid.NewGuid().ToString();
        }

        private void pmInitGridProp()
        {

            this.grdTemPd.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
                this.gridView1.Columns[intCnt].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
                this.gridView1.Columns[intCnt].OptionsColumn.AllowFocus = false;
                this.gridView1.Columns[intCnt].OptionsColumn.ReadOnly = true;
            
            }

            int i = 0;
            this.gridView1.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView1.Columns["fcCode"].VisibleIndex = i++;
            this.gridView1.Columns["fcRefNo"].VisibleIndex = i++;
            this.gridView1.Columns["fdDate"].VisibleIndex = i++;
            this.gridView1.Columns["QcProd"].VisibleIndex = i++;
            this.gridView1.Columns["QnProd"].VisibleIndex = i++;
            this.gridView1.Columns["fnBackQty"].VisibleIndex = i++;
            //this.gridView1.Columns["fnMOQty"].VisibleIndex = i++;

            this.gridView1.Columns["fnMOQty"].AppearanceCell.BackColor = System.Drawing.Color.White;
            this.gridView1.Columns["fnMOQty"].OptionsColumn.AllowFocus = true;
            this.gridView1.Columns["fnMOQty"].OptionsColumn.ReadOnly = false;

            this.gridView1.Columns["nRecNo"].Caption = UIBase.GetAppUIText(new string[] { "No.", "No." });
            this.gridView1.Columns["fcCode"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่ภายใน", "Doc. Code" });
            this.gridView1.Columns["fcRefNo"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "Ref. No" });
            this.gridView1.Columns["fdDate"].Caption = UIBase.GetAppUIText(new string[] { "วันที่", "Doc. Date" });
            this.gridView1.Columns["fnBackQty"].Caption = UIBase.GetAppUIText(new string[] { "P/O Qty", "P/O Qty" });
            //this.gridView1.Columns["fnMOQty"].Caption = UIBase.GetAppUIText(new string[] { "M/O Qty", "M/O Qty" });
            this.gridView1.Columns["QcProd"].Caption = UIBase.GetAppUIText(new string[] { "รหัสสินค้า", "Prod. Code" });
            this.gridView1.Columns["QnProd"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อสินค้า", "Prod. Name" });

            this.gridView1.Columns["nRecNo"].Width = 35;
            this.gridView1.Columns["fcCode"].Width = 85;
            this.gridView1.Columns["fcRefNo"].Width = 120;
            this.gridView1.Columns["fdDate"].Width = 85;
            this.gridView1.Columns["fnBackQty"].Width = 80;
            //this.gridView1.Columns["fnMOQty"].Width = 80;

            this.gridView1.Columns["fnBackQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["fnBackQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView1.Columns["fnMOQty"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["fnMOQty"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView1.Columns["fdDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["fdDate"].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView1.Columns["fnMOQty"].ColumnEdit = this.grcQty;
            //this.pmCalcColWidth();
            this.grdTemPd.Focus();
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
                case "BOOK":
                    if (this.pofrmGetBook == null)
                    {
                        this.pofrmGetBook = new DialogForms.dlgGetFMBook(this.mstrRefType);
                        this.pofrmGetBook.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBook.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "LOADSO":
                    using (Common.dlgQueryPO1 dlg = new Common.dlgQueryPO1(this.mstrRefType, this.mstrBranch, this.txtQcBook.Tag.ToString(), this.mstrCoor, this.mstrProd))
                    {
                        dlg.ShowDialog();
                        if (dlg.DialogResult == DialogResult.OK)
                        {
                            //
                            if (this.mstrReferKey != dlg.RefToKey)
                            {
                                //this.pmClearTemRefTo();
                                this.mstrReferKey = dlg.RefToKey;
                            }

                            //this.txtTotMfgQty.Validated = dlg.RefToQty;
                            foreach (DataRow dtrTemSO in dlg.RetVal.Rows)
                            {

                                if (!Convert.ToBoolean(dtrTemSO["CTAGVALUE"]))
                                {
                                    continue;
                                }

                                decimal decSumQty = 0;
                                DataRow[] da = this.dtsDataEnv.Tables[this.mstrTemPd].Select("cOrderI = '" + dtrTemSO["fcSkid"].ToString() + "'");
                                if (da.Length == 0)
                                {
                                    DataRow dtrNewSO = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                                    dtrNewSO["cOrderI"] = dtrTemSO["fcSkid"].ToString();
                                    dtrNewSO["cOrderH"] = dtrTemSO["fcOrderH"].ToString();
                                    dtrNewSO["cCoor"] = dtrTemSO["fcCoor"].ToString();
                                    dtrNewSO["cProd"] = dtrTemSO["fcProd"].ToString();
                                    dtrNewSO["QcProd"] = dtrTemSO["QcProd"].ToString();
                                    dtrNewSO["QnProd"] = dtrTemSO["QnProd"].ToString();
                                    dtrNewSO["fcCode"] = dtrTemSO["fcCode"].ToString();
                                    dtrNewSO["fcRefNo"] = dtrTemSO["fcRefNo"].ToString();
                                    dtrNewSO["fdDate"] = Convert.ToDateTime(dtrTemSO["fdDate"]);
                                    dtrNewSO["fnBackQty"] = Convert.ToDecimal(dtrTemSO["fnBackQty"]);
                                    dtrNewSO["fnMOQty"] = Convert.ToDecimal(dtrTemSO["fnBackQty"]);
                                    this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrNewSO);

                                    //decSumQty = decSumQty + Convert.ToDecimal(dtrTemSO["fnBackQty"]);
                                    //this.txtQcCoor.Text = dtrTemSO["QcCoor"].ToString();
                                    //this.txtQnCoor.Text = dtrTemSO["QnCoor"].ToString();

                                    //this.mstrCoor = dtrTemSO["fcCoor"].ToString();
                                    //this.mstrProd = dtrTemSO["fcProd"].ToString();

                                }
                            }

                        }
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
            string strOrderBy = "";
            switch (inTextbox)
            {
                case "TXTQCBOOK":
                case "TXTQNBOOK":
                    this.pmInitPopUpDialog("BOOK");
                    strOrderBy = (inTextbox == "TXTQCBOOK" ? "FCCODE" : "FCNAME");
                    this.pofrmGetBook.ValidateField(this.mstrBranch, "", strOrderBy, true);
                    if (this.pofrmGetBook.PopUpResult)
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
                case "TXTQCBOOK":
                case "TXTQNBOOK":
                    if (this.pofrmGetBook != null)
                    {
                        DataRow dtrRetVal = this.pofrmGetBook.RetrieveValue();

                        if (dtrRetVal != null)
                        {
                            this.txtQcBook.Tag = dtrRetVal["fcSkid"].ToString();
                            this.txtQcBook.Text = dtrRetVal["fcCode"].ToString().TrimEnd();
                            this.txtQnBook.Text = dtrRetVal["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcBook.Tag = "";
                            this.txtQcBook.Text = "";
                            this.txtQnBook.Text = "";
                        }
                    }
                    break;
            }
        }

        private void txtQcBook_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCBOOK" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcBook.Tag = "";
                this.txtQcBook.Text = "";
                this.txtQnBook.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("BOOK");
                e.Cancel = !this.pofrmGetBook.ValidateField(this.mstrBranch, txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetBook.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

    }
}
