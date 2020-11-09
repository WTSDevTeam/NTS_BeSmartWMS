
#define xd_RUNMODE_DEBUG

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using DevExpress.XtraGrid.Views.Base;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;

namespace BeSmartMRP.Permission
{
    public partial class frmAuth_BySect : UIHelper.frmBase
    {

        public static string TASKNAME = "ESETAUTH_X1";

        public static int MAXLENGTH_CODE = 11;
        public static int MAXLENGTH_NAME = 150;
        public static int MAXLENGTH_NAME2 = 150;

        private const int xd_PAGE_BROWSE = 0;
        private const int xd_PAGE_EDIT1 = 1;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias = "Brow_Alias";
        private int mintSaveBrowViewRowIndex = -1;
        private string mstrSortKey = "CCODE";

        private string mstrRefTable = MapTable.Table.AppAuthDet;
        private string mstrITable = MapTable.Table.AppAuthDet;

        private string mstrTemPd = "TemPd";

        private string mstrEditRowID = "";
        private UIHelper.AppFormState mFormEditMode;

        private string mstrOldCode = "";
        private string mstrOldName = "";
        private int mstrOldType = -1;

        private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
        private bool mbllPopUpResult = true;
        private bool mbllIsShowDialog = false;
        private string mstrSearchStr = "";
        private bool mbllIsFormQuery = false;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        //private DatabaseForms.frmEMAcChart pofrmGetAcChart = null;
        //private DatabaseForms.frmBudChart pofrmGetBudChart = null;
        //private DatabaseForms.frmAppLogin pofrmGetLogin = null;

        private UIHelper.IfrmDBBase pofrmGetEMSect = null;
        private UIHelper.IfrmDBBase pofrmGetLogin = null;

        public frmAuth_BySect()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        public frmAuth_BySect(FormActiveMode inMode)
        {
            InitializeComponent();

            this.mFormActiveMode = inMode;
            this.pmInitForm();
        }

        private static frmAuth_BySect mInstanse = null;

        public static frmAuth_BySect GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmAuth_BySect();
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

        public FormActiveMode ActiveMode
        {
            get { return this.mFormActiveMode; }
            set { this.mFormActiveMode = value; }
        }

        public bool PopUpResult
        {
            get { return this.mbllPopUpResult; }
        }

        public bool IsShowDialog
        {
            get { return this.mbllIsShowDialog; }
        }

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");
            this.pmCreateTem();
            this.pmInitGridProp_TemPd();

            this.pmInitializeComponent();

            UIBase.WaitClear();
        }

        private void pmInitializeComponent()
        {
            this.txtQcAppLogin.Properties.MaxLength = MAXLENGTH_CODE;

            //UIHelper.UIBase.CreateStandardToolbar(this.barMainEdit, this.barMain);
            //if (this.mFormActiveMode == FormActiveMode.PopUp
            //    || this.mFormActiveMode == FormActiveMode.Report)
            //{
            //    this.ShowInTaskbar = false;
            //    this.ControlBox = false;
            //}
            this.pmBlankFormData();
        }

        private void pmSetBrowView() {}
        private void pmInitGridProp() { }
        private void grdBrowView_Resize(object sender, EventArgs e) {}
        private void gridView1_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e) {}
        private void pmRecalColWidth() {}
        
        private void pmInitGridProp_TemPd()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemPd].DefaultView;

            this.grdTemPd.DataSource = this.dtsDataEnv.Tables[this.mstrTemPd];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = true;
            }

            this.gridView2.Columns["nRecNo"].Visible = false;
            this.gridView2.Columns["cRowID"].Visible = false;
            this.gridView2.Columns["cSect"].Visible = false;
            this.gridView2.Columns["cRemark"].Visible = false;

            this.gridView2.Columns["nRecNo"].Caption = "No.";
            this.gridView2.Columns["cQcSect"].Caption = "รหัสแผนก";
            this.gridView2.Columns["cQnSect"].Caption = "ชื่อแผนก";
            this.gridView2.Columns["cRemark"].Caption = "หมายเหตุ";

            this.gridView2.Columns["nRecNo"].Width = 5;
            this.gridView2.Columns["cQcSect"].Width = 15;
            this.gridView2.Columns["cQnSect"].Width = 45;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.grcQcSect.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QEMSectInfo.TableName, QEMSectInfo.Field.Code);
            this.grcQnSect.MaxLength = MapTable.GetMaxLength(pobjSQLUtil, QEMSectInfo.TableName, QEMSectInfo.Field.Name);
            this.grcRemark.MaxLength = 150;

            //this.gridView2.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView2.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.gridView2.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["cQcSect"].ColumnEdit = this.grcQcSect;
            this.gridView2.Columns["cQnSect"].ColumnEdit = this.grcQnSect;
            this.gridView2.Columns["cRemark"].ColumnEdit = this.grcRemark;

        }

        private void pmSetSortKey(string inColumn, bool inIsClear) {}

        private void pmGotoBrowPage() {}

        private void pmRefreshBrowView() {}

        private void pmSetPageStatus(bool inIsEnable)
        {
            for (int intCnt = 0; intCnt < this.pgfMainEdit.TabPages.Count; intCnt++)
            {
                this.pgfMainEdit.TabPages[intCnt].PageEnabled = inIsEnable;
            }
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "APPLOGIN":
                    if (this.pofrmGetLogin == null)
                    {
                        this.pofrmGetLogin = new DatabaseForms.frmAppLogin(FormActiveMode.Report);
                        //this.pofrmGetEMSect.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetEMSect.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "SECT":
                    if (this.pofrmGetEMSect == null)
                    {
                        this.pofrmGetEMSect = new DatabaseForms.frmEMSect(FormActiveMode.PopUp);
                        //this.pofrmGetEMSect.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetEMSect.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                case "TXTQCAPPLOGIN":
                    this.pmInitPopUpDialog("APPLOGIN");
                    string strPrefix = (inTextbox == "TXTQCAPPLOGIN" ? QAppLogInInfo.Field.LoginName : QAppLogInInfo.Field.LoginName);
                    this.pofrmGetLogin.ValidateField(inPara1, strPrefix, true);
                    if (this.pofrmGetLogin.PopUpResult)
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
                case "TXTQCAPPLOGIN":
                    if (this.pofrmGetLogin != null)
                    {
                        DataRow dtrPDGRP = this.pofrmGetLogin.RetrieveValue();

                        this.pmBlankFormData();
                        if (dtrPDGRP != null)
                        {
                            this.txtQcAppLogin.Tag = dtrPDGRP[MapTable.ShareField.RowID].ToString();
                            this.txtQcAppLogin.Text = dtrPDGRP[QAppLogInInfo.Field.LoginName].ToString().TrimEnd();
                            //this.txtQnEmpl.Text = dtrPDGRP["cName"].ToString().TrimEnd();
                            this.pmLoadFormData();
                        }
                        else
                        {
                            this.txtQcAppLogin.Tag = "";
                            this.txtQcAppLogin.Text = "";
                            //this.txtQnEmpl.Text = "";
                            this.pmBlankFormData();
                        }
                    }
                    break;

                case "GRDVIEW2_CQCSECT":
                case "GRDVIEW2_CQNSECT":
                    DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

                    if (dtrTemPd == null)
                    {
                        dtrTemPd = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();
                        this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrTemPd);
                    }

                    if (this.pofrmGetEMSect != null)
                    {
                        DataRow dtrAcChart = this.pofrmGetEMSect.RetrieveValue();
                        if (dtrAcChart != null)
                        {
                            dtrTemPd["cSect"] = dtrAcChart[MapTable.ShareField.RowID].ToString();
                            dtrTemPd["cQcSect"] = dtrAcChart[QEMSectInfo.Field.Code].ToString().TrimEnd();
                            dtrTemPd["cQnSect"] = dtrAcChart[QEMSectInfo.Field.Name].ToString().TrimEnd();
                        }
                        else
                        {
                            dtrTemPd["cSect"] = "";
                            dtrTemPd["cQcSect"] = "";
                            dtrTemPd["cQnSect"] = "";
                        }

                        this.gridView2.UpdateCurrentRow();
                    }
                    break;
            }
        }

        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                string strMenuName = e.Item.Tag.ToString();
                WsToolBar oToolButton = AppEnum.GetToolBarEnum(strMenuName);
                switch (oToolButton)
                {
                    case WsToolBar.Print:
                        this.pmPrintData();
                        break;
                    case WsToolBar.Save:
                        this.pmSaveData();
                        break;
                    case WsToolBar.Exit:
                        this.Close();
                        break;
                }

            }
        }

        private void pmDeleteData() { }

        //private bool pmCheckHasUsed(string inRowID, string inCode, ref string ioErrorMsg) { }
        private bool pmDeleteRow(string inRowID, string inCode, string inName, ref string ioErrorMsg)
        {
            
            string strErrorMsg = "";

            object[] pAPara = null;
            bool bllIsCommit = false;
            bool bllResult = false;
            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            this.mSaveDBAgent.AppID = App.AppID;
            this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where cBGChartHD = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                pAPara = new object[1] { inRowID };
                this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrRefTable + " where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;

                WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Delete, TASKNAME, inCode, inName, App.FMAppUserID, App.AppUserName);

                bllResult = true;
            }
            catch (Exception ex)
            {
                ioErrorMsg = ex.Message;
                bllResult = false;

                if (!bllIsCommit)
                {
                    this.mdbTran.Rollback();
                }
                App.WriteEventsLog(ex);
            }
            finally
            {
                this.mdbConn.Close();
            }
            return bllResult;
        }

        private void pmSearchData() {}

        private void pmEnterForm() {}

        private int pmGetRowID(string inTag)
        {
            for (int intCnt = 0; intCnt < this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Count; intCnt++)
            {
                if (this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows[intCnt]["cRowID"].ToString().TrimEnd() == inTag)
                    return intCnt;
            }
            return -1;
        }

        private void pmSaveData()
        {
            string strErrorMsg = "";
            if (this.pmValidBeforeSave(ref strErrorMsg))
            {
                UIBase.WaitWind("กำลังบันทึกข้อมูล...");
                this.pmUpdateRecord();
                //dlg.WaitWind(WsWinUtil.GetStringfromCulture(this.WsLocale, new string[] { "บันทึกเรียบร้อย", "Save Complete" }), 500);
                UIBase.WaitWind("บันทึกเรียบร้อย");
                if (this.mFormEditMode == UIHelper.AppFormState.Edit)
                    this.pmGotoBrowPage();
                else
                    this.pmInsertLoop();

                UIBase.WaitClear();
            }
            if (strErrorMsg != string.Empty)
            {
                MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("บันทึกเรียบร้อย", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool pmValidBeforeSave(ref string ioErrorMsg)
        {
            bool bllResult = true;

            if (this.txtQcAppLogin.Text.TrimEnd() == string.Empty)
            {
                ioErrorMsg = "ยังไม่ได้ระบุ รหัสผังงบประมาณ";
                this.txtQcAppLogin.Focus();
                return false;
            }
            else
                bllResult = true;

            return bllResult;
        }

        private void pmUpdateRecord()
        {
            string strErrorMsg = "";
            bool bllIsNewRow = false;
            bool bllIsCommit = false;

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
			this.mSaveDBAgent.AppID = App.AppID;
			this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                this.pmUpdateAuthDet();

                this.mdbTran.Commit();

                bllIsCommit = true;

                if (this.mFormEditMode == UIHelper.AppFormState.Insert)
                {
                    KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Insert, TASKNAME, this.txtQcAppLogin.Text, "", App.FMAppUserID, App.AppUserName);
                }
                else if (this.mFormEditMode == UIHelper.AppFormState.Edit)
                {
                    if (this.mstrOldCode == this.txtQcAppLogin.Text)
                    {
                        KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Update, TASKNAME, this.txtQcAppLogin.Text, "", App.FMAppUserID, App.AppUserName);
                    }
                    else 
                    {
                        KeepLogAgent.KeepLogChgValue(objSQLHelper, KeepLogType.Update, TASKNAME, this.txtQcAppLogin.Text, "", App.FMAppUserID, App.AppUserName, this.mstrOldCode, this.mstrOldName);
                    }
                }

			}
			catch (Exception ex)
			{
                if (!bllIsCommit)
                {
                    this.mdbTran.Rollback();
                }
                App.WriteEventsLog(ex);
#if xd_RUNMODE_DEBUG
				MessageBox.Show("Message : " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
#endif
			}

			finally
			{
				this.mdbConn.Close();
			}

        }

        private void pmUpdateAuthDet()
        {
            string strErrorMsg = "";
            object[] pAPara = null;

            foreach (DataRow dtrTemPd in this.dtsDataEnv.Tables[this.mstrTemPd].Rows)
            {
                bool bllIsNewRow = false;

                DataRow dtrBudCI = null;
                if (dtrTemPd["cSect"].ToString().TrimEnd() != string.Empty)
                {

                    string strRowID = "";
                    if ((dtrTemPd["cRowID"].ToString().TrimEnd() == string.Empty)
                        && (!this.mSaveDBAgent.BatchSQLExec("select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrBudCI = this.dtsDataEnv.Tables[this.mstrITable].NewRow();

                        strRowID = App.mRunRowID(this.mstrITable);
                        bllIsNewRow = true;
                        //dtrBudCI["cCreateAp"] = App.AppID;
                        dtrTemPd["cRowID"] = strRowID;
                        dtrBudCI["cRowID"] = dtrTemPd["cRowID"].ToString();
                    }
                    else
                    {
                        this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where cRowID = ?", new object[1] { dtrTemPd["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran);
                        dtrBudCI = this.dtsDataEnv.Tables[this.mstrITable].Rows[0];

                        strRowID = dtrTemPd["cRowID"].ToString();
                        bllIsNewRow = false;
                    }

                    this.pmReplRecordAuthDet(bllIsNewRow, dtrTemPd, ref dtrBudCI);

                    string strSQLUpdateStr = "";
                    cDBMSAgent.GenUpdateSQLString(dtrBudCI, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                }
                else
                {

                    //Delete RefProd
                    if (dtrTemPd["cRowID"].ToString().TrimEnd() != string.Empty)
                    {

                        pAPara = new object[] { dtrTemPd["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrITable + " where CROWID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                    }

                }

            }
        }

        private bool pmReplRecordAuthDet(bool inState, DataRow inTemPd, ref DataRow ioRefProd)
        {
            bool bllIsNewRec = inState;

            DataRow dtrBudCI = ioRefProd;
            //DataRow dtrBudCH = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];

            dtrBudCI["cCorp"] = App.ActiveCorp.RowID;
            dtrBudCI["cType"] = "S";
            dtrBudCI["cAppLogin"] = this.txtQcAppLogin.Tag.ToString();
            dtrBudCI["cSect"] = inTemPd["cSect"].ToString();
            dtrBudCI["CACCESS"] = "Y";
            //dtrBudCI["cRemark"] = inTemPd["cRemark"].ToString().TrimEnd();
            //dtrBudCI["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inTemPd["nRecNo"]), 2);

            return true;
        }


        private void pmCreateTem()
        {

            DataTable dtbTemPdVer = new DataTable(this.mstrTemPd);

            dtbTemPdVer.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cSect", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPdVer.Columns.Add("cQcSect", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cQnSect", System.Type.GetType("System.String"));
            dtbTemPdVer.Columns.Add("cRemark", System.Type.GetType("System.String"));

            dtbTemPdVer.Columns["cRowID"].DefaultValue = "";
            dtbTemPdVer.Columns["cSect"].DefaultValue = "";
            dtbTemPdVer.Columns["cQcSect"].DefaultValue = "";
            dtbTemPdVer.Columns["cQnSect"].DefaultValue = "";
            dtbTemPdVer.Columns["cRemark"].DefaultValue = "";

            this.dtsDataEnv.Tables.Add(dtbTemPdVer);

        }

        private void pmLoadEditPage()
        {
            this.pmSetPageStatus(true);
            this.pgfMainEdit.TabPages[0].PageEnabled = false;
            this.pgfMainEdit.SelectedTabPageIndex = 1;
            this.pmBlankFormData();
            //this.txtQcAppLogin.Focus();
            this.txtQcAppLogin.Focus();
            if (this.mFormEditMode == UIHelper.AppFormState.Edit)
            {
                this.pmLoadFormData();
            }
        }

        private void pmBlankFormData()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where 0=1", ref strErrorMsg);
            
            this.mstrEditRowID = "";

            this.txtQcAppLogin.Tag = "";this.txtQcAppLogin.Text = "";this.txtQnEmpl.Text = "";

            this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Clear();

            this.gridView2.FocusedColumn = this.gridView2.Columns["cQcSect"];

        }

        private void pmLoadFormData()
        {
            //DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            this.mstrEditRowID = this.txtQcAppLogin.Tag.ToString();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QAppAuthDet", this.mstrRefTable, "select * from " + this.mstrRefTable + " where cAppLogin = ?", ref strErrorMsg))
            {
                int intRow = 0;
                foreach (DataRow dtrBudCI in this.dtsDataEnv.Tables["QAppAuthDet"].Rows)
                {
                    DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemPd].NewRow();

                    intRow++;
                    dtrNewRow["nRecNo"] = intRow;
                    dtrNewRow["cRowID"] = dtrBudCI["cRowID"].ToString();
                    //dtrNewRow["cRemark"] = dtrBudCI["cRemark"].ToString().TrimEnd();
                    dtrNewRow["cSect"] = dtrBudCI["cSect"].ToString();

                    pobjSQLUtil.SetPara(new object[] { dtrNewRow["cSect"].ToString() });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QSect", "SECT", "select * from EMSECT where cRowID = ?", ref strErrorMsg))
                    {
                        dtrNewRow["cQcSect"] = this.dtsDataEnv.Tables["QSect"].Rows[0]["cCode"].ToString().TrimEnd();
                        dtrNewRow["cQnSect"] = this.dtsDataEnv.Tables["QSect"].Rows[0]["cName"].ToString().TrimEnd();
                    }

                    this.dtsDataEnv.Tables[this.mstrTemPd].Rows.Add(dtrNewRow);
                }
            }
            this.pmLoadOldVar();
        }

        private void pmLoadOldVar()
        {
            this.mstrOldCode = this.txtQcAppLogin.Text;
        }

        private void pmInsertLoop()
        {
            if (this.mFormActiveMode != FormActiveMode.PopUp
                || this.mFormActiveMode != FormActiveMode.Report)
                this.pmLoadEditPage();
            else
                this.pmGotoBrowPage();
        }

        private void frmAuth_BySect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        
        }

        private void txtQcAppLogin_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCAPPLOGIN" ? QAppLogInInfo.Field.LoginName : QAppLogInInfo.Field.LoginName;

            if (txtPopUp.Text == "")
            {
                this.txtQcAppLogin.Tag = "";
                this.txtQcAppLogin.Text = "";
                //this.txtQnEmpl.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("APPLOGIN");
                e.Cancel = !this.pofrmGetLogin.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetLogin.PopUpResult)
                {
                    this.pmRetrievePopUpVal(txtPopUp.Name);
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void pmGridPopUpButtonClick(string inKeyField)
        {
            switch (inKeyField.ToUpper())
            {
                case "CQCSECT":
                case "CQNSECT":

                    string strOrderBy = (inKeyField.ToUpper() == "CQCSECT" ? QEMSectInfo.Field.Code : QEMSectInfo.Field.Name);
                    this.pmInitPopUpDialog("SECT");
                    this.pofrmGetEMSect.ValidateField("", strOrderBy, true);
                    if (this.pofrmGetEMSect.PopUpResult)
                    {
                        this.pmRetrievePopUpVal("GRDVIEW2_"+inKeyField);
                    }
                    break;
            }
        }

        private void gridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value == null)
                return;

            ColumnView view = this.grdTemPd.MainView as ColumnView;

            string strCol = gridView2.FocusedColumn.FieldName;
            DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

            switch (strCol.ToUpper())
            {
                case "CQCSECT":
                case "CQNSECT":

                    string strValue = e.Value.ToString();
                    if (strValue.Trim() == string.Empty)
                    {
                        e.Value = "";
                        dtrTemPd["cSect"] = "";
                        dtrTemPd["cQcSect"] = "";
                        dtrTemPd["cQnSect"] = "";
                    }
                    else
                    {
                        this.pmInitPopUpDialog("SECT");
                        string strOrderBy = (strCol.ToUpper() == "CQCSECT" ? QEMSectInfo.Field.Code : QEMSectInfo.Field.Name);
                        e.Valid = !this.pofrmGetEMSect.ValidateField(strValue, strOrderBy, false);

                        if (this.pofrmGetEMSect.PopUpResult)
                        {
                            e.Valid = true;
                            this.gridView2.UpdateCurrentRow();
                            this.pmRetrievePopUpVal("GRDVIEW2_" + strCol);
                            e.Value = (strCol.ToUpper() == "CQCSECT" ? dtrTemPd["cQcSect"].ToString().TrimEnd() : dtrTemPd["cQnSect"].ToString().TrimEnd());
                        }
                        else
                        {
                            e.Value = "";
                            dtrTemPd["cSect"] = "";
                            dtrTemPd["cQcSect"] = "";
                            dtrTemPd["cQnSect"] = "";
                        }
                    }
                    break;
            }

        }

        private void grcAcChart_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            this.pmGridPopUpButtonClick(gridView2.FocusedColumn.FieldName);
        }

        private void grcAcChart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.F3:
                    this.pmGridPopUpButtonClick(gridView2.FocusedColumn.FieldName);
                    break;
            }
        }

        private void gridView2_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            int rowIndex = e.RowHandle;
            if (rowIndex >= 0)
            {
                rowIndex++;
                e.Info.DisplayText = rowIndex.ToString();
                DataRow dtrTemPd = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
                if (dtrTemPd != null)
                {
                    dtrTemPd["nRecNo"] = rowIndex.ToString();
                }

            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            this.pmEnterForm();
        }


        private void pmPrintData()
        {

            string strErrorMsg = "";
            string strSQLText = "";
            string strJoinTable = "";
            string strJoinFld = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);


            string strFld = " APPLOGIN.CLOGIN, APPROLE.CCODE as QCPOST, APPROLE.CNAME as QNPOST ";
            strFld += " ,EMSECT.CCODE as QCSECT , EMSECT.CNAME as QNSECT ";

            string strSQLExec = "select " + strFld + " from APPAUTHDET ";
            strSQLExec += " left join APPLOGIN on APPLOGIN.CROWID = APPAUTHDET.CAPPLOGIN ";
            strSQLExec += " left join APPEMPL on APPEMPL.CRCODE = APPLOGIN.CRCODE ";
            strSQLExec += " left join APPEMROLE on APPEMROLE.CEMPL = APPEMPL.CROWID ";
            strSQLExec += " left join APPROLE on APPROLE.CROWID = APPEMROLE.CAPPROLE ";
            strSQLExec += " left join EMSECT on EMSECT.CROWID = APPAUTHDET.CSECT ";
            strSQLExec += " where APPAUTHDET.CCORP = ? and APPAUTHDET.CTYPE = ? ";
            strSQLExec += " order by APPLOGIN.CLOGIN, EMSECT.CCODE ";

            Report.LocalDataSet.DTSLIST01 dtsPreviewReport = new Report.LocalDataSet.DTSLIST01();

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID, "S" });
            objSQLHelper.SQLExec(ref this.dtsDataEnv, "QList", "BGTRANHD", strSQLExec, ref strErrorMsg);
            foreach (DataRow dtrList in this.dtsDataEnv.Tables["QList"].Rows)
            {
                DataRow dtrPreview = dtsPreviewReport.XRLSTAUTHDET.NewRow();

                dtrPreview["Login"] = dtrList["cLogin"].ToString();
                dtrPreview["QcPost"] = dtrList["QcPost"].ToString();
                dtrPreview["QnPost"] = dtrList["QnPost"].ToString();
                dtrPreview["QcSect"] = dtrList["QcSect"].ToString();
                dtrPreview["QnSect"] = dtrList["QnSect"].ToString();
                dtrPreview["cAccess"] = "Y";

                dtsPreviewReport.XRLSTAUTHDET.Rows.Add(dtrPreview);

            }

            if (dtsPreviewReport.XRLSTAUTHDET.Rows.Count != 0)
            {
                this.pmPreviewReport(dtsPreviewReport);
            }
            else
            {
                MessageBox.Show(this, "ไม่มีข้อมูล", "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData)
        {

            //string strRPTFileName = "";

            //strRPTFileName = Application.StartupPath + @"\RPT\XRLSTAUTHDET01.rpt";

            //ReportDocument rptPreviewReport = new ReportDocument();
            //if (!System.IO.File.Exists(strRPTFileName))
            //{
            //    MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //rptPreviewReport.Load(strRPTFileName);

            //rptPreviewReport.SetDataSource(inData);

            //this.pACrPara.Clear();

            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.ActiveCorp.Name);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, this.Text);
            ////AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, "พิมพ์รหัส" + this.mstrTableText + "ตั้งแต่ : " + this.txtBegQcProj1.Text + " ถึง " + this.txtEndQcProj1.Text);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, TASKNAME);
            //AppUtil.ReportHelper.mAddCrPara(ref this.pACrPara, App.AppUserName);

            //ParameterFieldDefinitions prmCRPara = rptPreviewReport.DataDefinition.ParameterFields;
            //prmCRPara["PFREPORTTITLE1"].ApplyCurrentValues((ParameterValues)this.pACrPara[0]);
            //prmCRPara["PFREPORTTITLE2"].ApplyCurrentValues((ParameterValues)this.pACrPara[1]);
            ////prmCRPara["PFREPORTTITLE3"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            //prmCRPara["PFRPTNAME"].ApplyCurrentValues((ParameterValues)this.pACrPara[2]);
            //prmCRPara["PFPRINTBY"].ApplyCurrentValues((ParameterValues)this.pACrPara[3]);

            //App.PreviewReport(this, false, rptPreviewReport);

        }

    }
}
