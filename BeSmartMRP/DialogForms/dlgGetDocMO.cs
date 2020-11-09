
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;

namespace BeSmartMRP.DialogForms
{
    public partial class dlgGetDocMO : UIHelper.frmBase
    {

        public static int MAXLENGTH_CODE = 15;
        public static int MAXLENGTH_NAME = 150;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QMFWOrderHDInfo.TableName;
        private string mstrSortKey = "CCODE";

        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        private string mstrBranch = "";
        private string mstrPlant = "";
        private string mstrRefType = DocumentType.MO.ToString();
        private string mstrBook = "";
        private string mstrFilterStep = "";

        public bool PopUpResult
        {
            get { return this.mbllPopUpResult; }
        }

        public bool IsShowDialog
        {
            get { return this.mbllIsShowDialog; }
        }

        public string FIlterStep
        {
            get { return this.mstrFilterStep; }
            set { this.mstrFilterStep = value; }
        }

        public dlgGetDocMO()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");
            this.pmInitializeComponent();
            this.pmSetBrowView();

            this.pmInitGridProp();
            //this.gridView1.MoveLast();

            UIBase.WaitClear();
        }

        private void pmInitializeComponent()
        {
            UIHelper.UIBase.CreatePopUpToolbar(this.barMainEdit, this.barMain);

            this.barMainEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barMainEdit_ItemClick);
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dlgGetDocMO_KeyDown);

        }

        private void pmSetBrowView()
        {

            object[] pAPara = null;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLExec = "";
            strSQLExec = "select ";
            strSQLExec += " MFWORDERHD.CROWID, MFWORDERHD.CCODE, MFWORDERHD.CREFNO, MFWORDERHD.DDATE";
            strSQLExec += " from MFWORDERHD ";
            strSQLExec += " where MFWORDERHD.CCORP = ? and MFWORDERHD.CBRANCH = ? and MFWORDERHD.CPLANT = ? and MFWORDERHD.CREFTYPE = ? and MFWORDERHD.CMFGBOOK = ? ";

            //if (this.mstrSortKey == "CCODE")
            //{
            //    strSQLExec += " and MFWORDERHD.DDATE between ? and ? ";
            //    pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook };
            //}
            //else
            //{
            //    strSQLExec += " and MFWORDERHD.CCODE between ? and ? ";
            //    pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrRefType, this.mstrBook, this.mstrJob, this.mintBGYear };
            //}

            if (this.mstrFilterStep.Trim() != "")
            {
                strSQLExec += this.mstrFilterStep;
            }
            else
            {
                strSQLExec += " and MFWORDERHD.CSTAT <> 'C' and MFWORDERHD.CMSTEP <> 'P' ";
            }
            pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrBook };

            pobjSQLUtil.SetPara(pAPara);
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, this.mstrRefTable, strSQLExec, ref strErrorMsg);

        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = "CCODE";

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView1.Columns["CCODE"].VisibleIndex = i++;
            this.gridView1.Columns["CREFNO"].VisibleIndex = i++;
            this.gridView1.Columns["DDATE"].VisibleIndex = i++;

            this.gridView1.Columns["CCODE"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่เอกสาร", "Code" });
            this.gridView1.Columns["DDATE"].Caption = UIBase.GetAppUIText(new string[] { "วันที่เอกสาร", "Date" });
            this.gridView1.Columns["CREFNO"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "Ref. No" });

            this.gridView1.Columns["DDATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["DDATE"].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView1.Columns["CCODE"].Width = 15;
            this.gridView1.Columns["DDATE"].Width = 15;
            this.gridView1.Columns["CREFNO"].Width = 25;

            this.pmSetSortKey("CCODE", true);
        }

        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                string strMenuName = e.Item.Tag.ToString();
                WsToolBar oToolButton = AppEnum.GetToolBarEnum(strMenuName);
                switch (oToolButton)
                {
                    case WsToolBar.Enter:
                        this.mbllPopUpResult = true;
                        this.pmEnterForm();
                        break;
                    case WsToolBar.Exit:
                        //this.DialogResult = DialogResult.Cancel;
                        this.mbllPopUpResult = false;
                        this.Hide();
                        break;
                    case WsToolBar.Refresh:
                        this.pmRefreshBrowView();
                        break;
                }

            }
        }

        private void dlgGetDocMO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.pmEnterForm();
                    break;
                case Keys.Escape:
                    this.mbllPopUpResult = false;
                    this.Hide();
                    break;
            }

        }

        private void pmRefreshBrowView()
        {
            this.pmSetBrowView();
            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];
            if (this.gridView1.RowCount > 0)
                this.gridView1.FocusedRowHandle = 0;
        }

        private void pmSetSortKey(string inColumn, bool inIsClear)
        {
            if (inIsClear)
            {
                this.gridView1.SortInfo.Clear();
                this.gridView1.SortInfo.Add(this.gridView1.Columns[this.mstrSortKey], DevExpress.Data.ColumnSortOrder.Ascending);
                this.gridView1.RefreshData();
            }

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].AppearanceCell.BackColor = Color.White;
            }
            this.mstrSortKey = inColumn.ToUpper();
            this.gridView1.Columns[this.mstrSortKey].AppearanceCell.BackColor = Color.WhiteSmoke;
            this.gridView1.FocusedColumn = this.gridView1.Columns[this.mstrSortKey];
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            this.pmEnterForm();
        }

        private void pmEnterForm()
        {
            this.mbllPopUpResult = true;
            this.Hide();
        }

        public bool ValidateField(string inBranch, string inPlant, string inRefType, string inBook, string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            inOrderBy = inOrderBy.ToUpper();
            inSearchStr = inSearchStr.PadRight(MAXLENGTH_CODE);

            if (this.mstrSortKey != inOrderBy)
            {
                this.mstrSortKey = inOrderBy;
                this.pmSetSortKey(this.mstrSortKey, true);
            }

            if (this.mstrSearchStr != inSearchStr || this.mbllIsFormQuery == false
                || this.mstrBranch != inBranch
                || this.mstrPlant != inPlant
                || this.mstrRefType != inRefType
                || this.mstrBook != inBook)
            {

                this.mstrBranch = inBranch;
                this.mstrPlant = inPlant;
                this.mstrRefType= inRefType;
                this.mstrBook = inBook;
                
                string strSearch = inSearchStr.TrimEnd();
                if (strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_STAR ||
                    strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_2SLASH)
                {
                    strSearch = "";
                }

                this.mstrSearchStr = "%" + strSearch + "%";
                this.pmRefreshBrowView();
            }

            int intSeekVal = this.gridView1.LocateByValue(0, this.gridView1.Columns[this.mstrSortKey], inSearchStr);
            bllIsVField = (intSeekVal < 0);

            this.mbllIsShowDialog = false;
            if (inForcePopUp || bllIsVField)
            {

                this.gridView1.FocusedColumn = this.gridView1.Columns[this.mstrSortKey];

                this.gridView1.MoveLast();
                this.gridView1.StartIncrementalSearch(inSearchStr.TrimEnd());
                if (this.gridView1.FocusedRowHandle >= this.gridView1.RowCount - 1)
                {
                    string strSeekNear = BeSmartMRP.Business.Component.BizRule.GetNearString(this.dtsDataEnv.Tables[this.mstrBrowViewAlias], inSearchStr.Trim(), inOrderBy);
                    strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == "CCODE" ? MAXLENGTH_CODE : MAXLENGTH_CODE);
                    this.gridView1.StartIncrementalSearch(strSeekNear);
                }

                this.ShowDialog();
                this.mbllIsShowDialog = true;
            }
            else
            {
                this.gridView1.FocusedRowHandle = intSeekVal;
                this.mbllPopUpResult = true;
            }

            return this.mbllPopUpResult;
        }

        public DataRow RetrieveValue()
        {

            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow == null) return null;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[1] { dtrBrow["cRowID"].ToString() });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVFLD_Table", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cRowID = ? ", ref strErrorMsg))
            {
                return this.dtsDataEnv.Tables["QVFLD_Table"].Rows[0];
            }
            return null;
        }




    }
}
