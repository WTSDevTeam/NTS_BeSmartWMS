
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

namespace BeSmartMRP.Transaction.Common.MRP
{
    public partial class dlgGetFROption01 : UIHelper.frmBase
    {

        public dlgGetFROption01(string inRefType)
        {
            InitializeComponent();
            this.mstrRefType = inRefType;
            this.pmInitForm();
        }

        public void SetTitle(string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
        }

        public string BranchID
        {
            get { return this.txtQcBranch.Tag.ToString(); }
            set
            {
                this.txtQcBranch.Tag = value;

                string strErrorMsg = "";
                WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

                objSQLHelper.SetPara(new object[] { this.txtQcBranch.Tag.ToString() });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select fcSkid, fcCode, fcName from BRANCH where FCSKID = ? ", ref strErrorMsg))
                {

                    DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                    this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                    this.txtQcBranch.Text = dtrBranch["fcCode"].ToString().TrimEnd();
                    this.txtQnBranch.Text = dtrBranch["fcName"].ToString().TrimEnd();
                    this.pmDefaultBook();
                }
            }
        }

        public string BranchCode
        {
            get { return this.txtQcBranch.Text; }
        }

        public string BranchName
        {
            get { return this.txtQnBranch.Text; }
        }

        public string CoorID
        {
            get { return this.txtQcWHouse.Tag.ToString(); }
        }

        public string CoorCode
        {
            get { return this.txtQcWHouse.Text; }
        }

        public string CoorName
        {
            get { return this.txtQnWHouse.Text; }
        }

        public string BookID
        {
            get { return this.txtQcBook.Tag.ToString(); }
        }

        public string BookCode
        {
            get { return this.txtQcBook.Text; }
        }

        public string BookName
        {
            get { return this.txtQnBook.Text; }
        }

        public string WHouseID
        {
            get { return this.txtQcWHouse.Tag.ToString(); }
        }

        public string Lot
        {
            get { return this.txtLot.Text.TrimEnd(); }
        }
        
        public DateTime DocDate
        {
            get { return this.txtBegDate.DateTime.Date; }
        }

        public decimal Qty
        {
            get { return this.txtQty.Value; }
            set 
            {
                this.txtQty.Properties.MinValue = 0;
                this.txtQty.Properties.MaxValue = value;
                this.txtQty.Value = value; 
            }
        }

        private DialogForms.dlgGetBranch pofrmGetBranch = null;
        private DialogForms.dlgGetFMBook pofrmGetBook = null;
        private DialogForms.dlgGetCoor pofrmGetCoor = null;
        private DialogForms.dlgGetWHouse pofrmGetWareHouse = null;

        private string mstrRefType = "FR";
        private string mstrSect = "";
        private string mstrFrWhType = SysDef.gc_WHOUSE_TYPE_NORMAL;

        private DataSet dtsDataEnv = new DataSet();

        private void pmInitForm()
        {

            this.txtQcBranch.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcBranch.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcBranch.Validating += new CancelEventHandler(txtQcBranch_Validating);

            this.txtQcWHouse.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcWHouse.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcWHouse.Validating += new CancelEventHandler(txtQcWHouse_Validating);

            this.txtQcBook.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcBook.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcBook.Validating += new CancelEventHandler(txtQcMfgBook_Validating);
            
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.txtQcBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper2, QMfgBookInfo.TableName, QMfgBookInfo.Field.Code);
            this.txtQnBook.Properties.MaxLength = MapTable.GetMaxLength(objSQLHelper2, QMfgBookInfo.TableName, QMfgBookInfo.Field.Name);

            this.btnOK.Click += new EventHandler(btnOK_Click);

            this.txtQcBook.Tag = "";
            this.txtQcBook.Text = "";
            this.txtQnBook.Text = "";

            int intYear = DateTime.Now.Year;

            string strErrorMsg = "";

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select fcSkid, fcCode, fcName from BRANCH where FCCORP = ? order by FCCODE", ref strErrorMsg))
            {

                DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                this.txtQcBranch.Text = dtrBranch["fcCode"].ToString().TrimEnd();
                this.txtQnBranch.Text = dtrBranch["fcName"].ToString().TrimEnd();

            }

            this.pmDefaultBook();

            this.txtBegDate.DateTime = DateTime.Now;

            this.lblTitle.Text = UIBase.GetAppUIText(new string[] { "ระบุ Option การสร้างใบเบิก", "Gen Finish Good Recieve Option" });
            this.lblBranch.Text = UIBase.GetAppUIText(new string[] { "ระบุสาขา :", "Branch Code :" });
            this.lblWHouse.Text = UIBase.GetAppUIText(new string[] { "ระบุคลังสินค้า :", "Warehouse Code :" });
            this.lblBook.Text = UIBase.GetAppUIText(new string[] { "ระบุเล่มเอกสาร :", "Book Code :" });
            this.lblLot.Text = UIBase.GetAppUIText(new string[] { "ระบุ LOT :", "LOT :" });
            this.lblDate.Text = UIBase.GetAppUIText(new string[] { "วันที่เอกสาร :", "Document Date :" });

            UIBase.SetDefaultChildAppreance(this);
        }

        private void pmDefaultBook()
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, this.txtQcBranch.Tag.ToString(), this.mstrRefType });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "BOOK", "select fcSkid, fcCode, fcName from Book where fcCorp = ? and fcBranch = ? and fcRefType  = ? order by FCCODE", ref strErrorMsg))
            {
                DataRow dtrBook = this.dtsDataEnv.Tables["QBook"].Rows[0];
                this.txtQcBook.Tag = dtrBook["fcSkid"].ToString();
                this.txtQcBook.Text = dtrBook["fcCode"].ToString().TrimEnd();
                this.txtQnBook.Text = dtrBook["fcName"].ToString().TrimEnd();

            }
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "BRANCH":
                    if (this.pofrmGetBranch == null)
                    {
                        this.pofrmGetBranch = new BeSmartMRP.DialogForms.dlgGetBranch();
                        this.pofrmGetBranch.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBranch.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "COOR":
                    if (this.pofrmGetCoor == null)
                    {
                        this.pofrmGetCoor = new DialogForms.dlgGetCoor(CoorType.Supplier);
                        this.pofrmGetCoor.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetCoor.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "WHOUSE":
                    if (this.pofrmGetWareHouse == null)
                    {
                        this.pofrmGetWareHouse = new DialogForms.dlgGetWHouse();
                        this.pofrmGetWareHouse.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetWareHouse.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "BOOK":
                    if (this.pofrmGetBook == null)
                    {
                        this.pofrmGetBook = new DialogForms.dlgGetFMBook(this.mstrRefType);
                        this.pofrmGetBook.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetBook.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                case "TXTQCBRANCH":
                case "TXTQNBRANCH":
                    this.pmInitPopUpDialog("BRANCH");
                    strOrderBy = (inTextbox == "TXTQCBRANCH" ? "FCCODE" : "FCNAME");
                    this.pofrmGetBranch.ValidateField(inPara1, strOrderBy, true);
                    if (this.pofrmGetBranch.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCCOOR":
                case "TXTQNCOOR":
                    this.pmInitPopUpDialog("COOR");
                    strOrderBy = (inTextbox == "TXTQCCOOR" ? "FCCODE" : "FCNAME");
                    this.pofrmGetCoor.ValidateField(inPara1, strOrderBy, true);
                    if (this.pofrmGetCoor.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCBOOK":
                case "TXTQNBOOK":
                    this.pmInitPopUpDialog("BOOK");
                    strOrderBy = (inTextbox == "TXTQCBOOK" ? "FCCODE" : "FCNAME");
                    this.pofrmGetBook.ValidateField(this.txtQcBranch.Tag.ToString(), "", strOrderBy, true);
                    if (this.pofrmGetBook.PopUpResult)
                    {
                        this.pmRetrievePopUpVal(inTextbox);
                    }
                    break;
                case "TXTQCWHOUSE":
                case "TXTQNWHOUSE":
                    this.pmInitPopUpDialog("WHOUSE");
                    string strPrefix = ("TXTQCWHOUSE".IndexOf(inTextbox) > -1 ? "FCCODE" : "FCNAME");
                    this.pofrmGetWareHouse.ValidateField(this.txtQcBranch.Tag.ToString(), "(" + this.pmSplitToSQLStr(this.mstrFrWhType) + ")", "", strPrefix, true);
                    if (this.pofrmGetWareHouse.PopUpResult)
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
                        DataRow dtrBranch = this.pofrmGetBranch.RetrieveValue();

                        if (dtrBranch != null)
                        {
                            if (this.txtQcBranch.Tag.ToString() != dtrBranch["fcSkid"].ToString())
                            {
                                this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                                this.pmDefaultBook();
                            }

                            this.txtQcBranch.Tag = dtrBranch["fcSkid"].ToString();
                            this.txtQcBranch.Text = dtrBranch["fcCode"].ToString().TrimEnd();
                            this.txtQnBranch.Text = dtrBranch["fcName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcBranch.Tag = "";
                            this.txtQcBranch.Text = "";
                            this.txtQnBranch.Text = "";

                            this.txtQcBook.Tag = "";
                            this.txtQcBook.Text = "";
                            this.txtQnBook.Text = "";
                        
                        }
                    }
                    break;
                case "TXTQCCOOR":
                case "TXTQNCOOR":

                    DataRow dtrGetVal = this.pofrmGetCoor.RetrieveValue();
                    if (dtrGetVal != null)
                    {

                        this.txtQcWHouse.Tag = dtrGetVal["fcSkid"].ToString();
                        this.txtQcWHouse.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                        this.txtQnWHouse.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcWHouse.Tag = "";
                        this.txtQcWHouse.Text = "";
                        this.txtQnWHouse.Text = "";

                        this.txtQcBook.Tag = "";
                        this.txtQcBook.Text = "";
                        this.txtQnBook.Text = "";
                    
                    }
                    break;
                case "TXTQCWHOUSE":
                case "TXTQNWHOUSE":
                    dtrGetVal = this.pofrmGetWareHouse.RetrieveValue();
                    if (dtrGetVal != null)
                    {
                        //if (this.txtQcWHouse.Tag.ToString() != dtrGetVal["fcSkid"].ToString())
                        //{
                        //}
                        this.txtQcWHouse.Tag = dtrGetVal["fcSkid"].ToString();
                        this.txtQcWHouse.Text = dtrGetVal["fcCode"].ToString().TrimEnd();
                        this.txtQnWHouse.Text = dtrGetVal["fcName"].ToString().TrimEnd();
                    }
                    else
                    {
                        this.txtQcWHouse.Tag = "";
                        this.txtQcWHouse.Text = "";
                        this.txtQnWHouse.Text = "";
                    }
                    break;
                case "TXTQCBOOK":
                case "TXTQNBOOK":
                    if (this.pofrmGetBook != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetBook.RetrieveValue();

                        if (dtrPDGRP != null)
                        {
                            this.txtQcBook.Tag = dtrPDGRP["fcSkid"].ToString();
                            this.txtQcBook.Text = dtrPDGRP["fcCode"].ToString().TrimEnd();
                            this.txtQnBook.Text = dtrPDGRP["fcName"].ToString().TrimEnd();
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

        private void txtQcBranch_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCBRANCH" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcBranch.Tag = "";
                this.txtQcBranch.Text = "";
                this.txtQnBranch.Text = "";

                this.txtQcBook.Tag = "";
                this.txtQcBook.Text = "";
                this.txtQnBook.Text = "";
            
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

        private void txtQcCoor_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCCOOR" ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcWHouse.Tag = "";
                this.txtQcWHouse.Text = "";
                this.txtQnWHouse.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("COOR");
                e.Cancel = !this.pofrmGetCoor.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetCoor.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcWHouse_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = "TXTQCWHOUSE".IndexOf(txtPopUp.Name.ToUpper()) > -1 ? "FCCODE" : "FCNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcWHouse.Tag = "";
                this.txtQcWHouse.Text = "";
                this.txtQnWHouse.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("WHOUSE");
                e.Cancel = !this.pofrmGetWareHouse.ValidateField(this.txtQcBranch.Tag.ToString(), "(" + this.pmSplitToSQLStr(this.mstrFrWhType) + ")", txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetWareHouse.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void txtQcMfgBook_Validating(object sender, CancelEventArgs e)
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
                e.Cancel = !this.pofrmGetBook.ValidateField(this.txtQcBranch.Tag.ToString(), txtPopUp.Text, strOrderBy, false);
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (pmValidOption())
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private bool pmValidOption()
        {
            bool bllResult = false;
            string strErrorMsg = "";
            if (this.txtQcBranch.Text.Trim() == "")
            {
                strErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุสาขา !", "Branch Code is not define !" });
                this.txtQcBranch.Focus();
            }
            else if (this.txtQcBook.Text.Trim() == "")
            {
                strErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุเล่มเอกสาร !", "Book Code is not define !" });
                this.txtQcBook.Focus();
            }
            else
            {
                bllResult = true;
            }

            if (strErrorMsg != string.Empty)
            {
                MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            return bllResult;
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
