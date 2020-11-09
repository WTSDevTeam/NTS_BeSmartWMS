
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

namespace BeSmartMRP.Transaction.Common.MRP
{
    public partial class dlgGetDoc_MO : UIHelper.frmBase
    {


        public static int MAXLENGTH_CODE = 15;
        public static int MAXLENGTH_REFNO = 25;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private string mstrRefTable = QMFWOrderHDInfo.TableName;
        private string mstrSortKey = "CCODE";

        private bool bllWaitApprove = true;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        private string mstrPlant = "";
        private string mstrBranch = "";
        private string mstrCoor = "";
        private string mstrRefType = "";
        private string mstrMfgBook = "";
        private string mstrStep = "";

        private string mstrFilterStep = "CSTEP";

        public bool PopUpResult
        {
            get { return this.mbllPopUpResult; }
        }

        public bool IsShowDialog
        {
            get { return this.mbllIsShowDialog; }
        }

        public dlgGetDoc_MO(string inFilterStep)
        {
            InitializeComponent();
            this.mstrFilterStep = inFilterStep;
            this.pmInitForm();
        }

        public dlgGetDoc_MO()
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
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dlgGetDoc_MO_KeyDown);

        }

        private void pmSetBrowView()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strStep = "";
            string strFilterStep = "";
            switch (mstrFilterStep)
            {
                case "CSTEP":
                    this.mstrStep = SysDef.gc_REF_STEP_CUT_STOCK;
                    strFilterStep = " WORDERH." + this.mstrFilterStep.ToUpper() + " = '" + this.mstrStep + "'";
                    strStep = "WORDERH.CSTEP ";
                    break;
                case "CMSTEP":
                    this.mstrStep = SysDef.gc_REF_STEP_CREATE;
                    strFilterStep = " WORDERH." + this.mstrFilterStep.ToUpper() + " = '" + this.mstrStep + "'";
                    strStep = "WORDERH.CMSTEP as CSTEP ";
                    break;
                case "COPSTEP":
                    this.mstrStep = SysDef.gc_REF_OPSTEP_FINISH;
                    //strFilterStep = " WORDERH." + this.mstrFilterStep.ToUpper() + " <> ? and WORDERH.CMSTEP = '' ";
                    strFilterStep = " WORDERH.CMSTEP = '' ";
                    strStep = "CSTEP = case ";
                    strStep += " when WORDERH.COPSTEP = '" + SysDef.gc_REF_OPSTEP_CREATE + "' then '' ";
                    strStep += " when WORDERH.COPSTEP = '" + SysDef.gc_REF_OPSTEP_START + "' then 'PARTIAL' ";
                    strStep += " when WORDERH.COPSTEP = '" + SysDef.gc_REF_OPSTEP_FINISH + "' then 'FINISH' ";
                    strStep += " end ";
                    break;
            }

            string strSQLExec = "";
            strSQLExec = "select ";
            strSQLExec += " WORDERH.CROWID, " + strStep + ", WORDERH.CCODE, WORDERH.CREFNO, WORDERH.DDATE";
            strSQLExec += " from {0} WORDERH ";
            strSQLExec += " where WORDERH.CCORP = ? and WORDERH.CBRANCH = ? and WORDERH.CPLANT = ? and WORDERH.CREFTYPE = ? and WORDERH.CMFGBOOK = ? ";
            if (this.mstrCoor.Trim() != string.Empty)
            {
                strSQLExec += " and WORDERH.CCOOR = ? ";
            }
            strSQLExec += " and " + strFilterStep + " and WORDERH.CSTAT <> 'C' ";

            //this.mstrStep = (this.bllWaitApprove == true ? SysDef.gc_REF_STEP_WAIT_APPROVE : SysDef.gc_REF_STEP_CUT_STOCK);
            
            strSQLExec = string.Format(strSQLExec, new string[] { this.mstrRefTable });

            object[] pAPara = null;
            if (this.mstrCoor.Trim() != string.Empty)
            {
                //pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrMfgBook, this.mstrCoor, this.mstrStep };
                pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrMfgBook, this.mstrCoor };
            }
            else
            {
                //pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrMfgBook, this.mstrStep };
                pAPara = new object[] { App.ActiveCorp.RowID, this.mstrBranch, this.mstrPlant, this.mstrRefType, this.mstrMfgBook };
            }
            pobjSQLUtil.SetPara(pAPara);
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, this.mstrRefTable, strSQLExec, ref strErrorMsg);

        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            dvBrow.Sort = this.mstrSortKey;

            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            this.gridView1.Columns["CSTEP"].Caption = UIBase.GetAppUIText(new string[] { "STEP", "STEP" });
            this.gridView1.Columns["CCODE"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่ภายใน", "DOC. CODE" });
            this.gridView1.Columns["CREFNO"].Caption = UIBase.GetAppUIText(new string[] { "เลขที่อ้างอิง", "Ref. No." });
            this.gridView1.Columns["DDATE"].Caption = UIBase.GetAppUIText(new string[] { "วันที่", "Date" });

            this.gridView1.Columns["DDATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["DDATE"].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView1.Columns["CSTEP"].Width = 15;
            this.gridView1.Columns["CCODE"].Width = 30;
            this.gridView1.Columns["DDATE"].Width = 30;

            int i = 0;
            this.gridView1.Columns["CSTEP"].VisibleIndex = i++;
            this.gridView1.Columns["CCODE"].VisibleIndex = i++;
            this.gridView1.Columns["CREFNO"].VisibleIndex = i++;
            this.gridView1.Columns["DDATE"].VisibleIndex = i++;

            this.pmSetSortKey(this.mstrSortKey, true);
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

        private void dlgGetDoc_MO_KeyDown(object sender, KeyEventArgs e)
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

        public bool ValidateField(string inBranch, string inPlant, string inRefType, string inMfgBook, string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            inOrderBy = inOrderBy.ToUpper();
            inSearchStr = (inOrderBy == "CCODE" ? inSearchStr.PadRight(MAXLENGTH_CODE) : inSearchStr.PadRight(MAXLENGTH_REFNO));

            if (this.mstrSortKey != inOrderBy)
            {
                this.mstrSortKey = inOrderBy;
                this.pmSetSortKey(this.mstrSortKey, true);
            }

            if (this.mstrSearchStr != inSearchStr || this.mbllIsFormQuery == false
                || this.mstrMfgBook != inMfgBook)
            {

                this.mstrBranch = inBranch;
                this.mstrPlant = inPlant;
                this.mstrRefType = inRefType;
                this.mstrMfgBook = inMfgBook;
                
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

        public bool ValidateField2(string inBranch, string inPlant, string inCoor, string inRefType, string inMfgBook, string inSearchStr, string inOrderBy, bool inForcePopUp)
        {
            bool bllIsVField = true;
            this.mbllPopUpResult = false;

            inOrderBy = inOrderBy.ToUpper();
            inSearchStr = (inOrderBy == "CCODE" ? inSearchStr.PadRight(MAXLENGTH_CODE) : inSearchStr.PadRight(MAXLENGTH_REFNO));

            if (this.mstrSortKey != inOrderBy)
            {
                this.mstrSortKey = inOrderBy;
                this.pmSetSortKey(this.mstrSortKey, true);
            }

            if (this.mstrSearchStr != inSearchStr || this.mbllIsFormQuery == false
                || this.mstrMfgBook != inMfgBook
                || this.mstrCoor != inCoor)
            {

                this.mstrBranch = inBranch;
                this.mstrPlant = inPlant;
                this.mstrCoor = inCoor;
                this.mstrRefType = inRefType;
                this.mstrMfgBook = inMfgBook;

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
