#define xd_RUNMODE_DEBUG

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;

namespace BeSmartMRP.Permission
{

    public partial class frmSetPermission : UIHelper.frmBase
    {

        private string TASKNAME = "ESETAUTHEN";

        private DataSet dtsDataEnv = new DataSet();
        private string mstrRefTable = MapTable.Table.AppObj;
        private string mstrTemObj = "TemObj";
        private bool bllIsAskForSave = false;

        private int mintSaveStdRigIdx = 0;

        //private DialogForms.dlgGetPost pofrmGetPost = null;
        //private DialogForms.dlgGetEmplR pofrmGetEmplR = null;

        private DatabaseForms.frmAppRole pofrmGetPost = null;
        private DatabaseForms.frmAppLogin pofrmGetEmplR = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        public frmSetPermission()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        private void pmInitForm()
        {
            UIBase.WaitWind("Loading Form...");
            this.pmInitializeComponent();

            this.pmLoadFormData(true, this.pmGetLevel(this.cmbStdRight.SelectedIndex), "", "");
            this.pmInitGridProp();

            UIBase.WaitClear();
        }

        protected override void OnClosing(CancelEventArgs ce)
        {
            if (this.pmClosingApp())
            {
                base.OnClosing(ce);
            }
            else
                ce.Cancel = true;
        }

        private bool pmClosingApp()
        {
            return (MessageBox.Show(this, "ต้องการปิดการทำงาน ?", "Application Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
        }

        private void pmInitializeComponent()
        {
            
            this.txtQcPost.Tag = "";
            this.txtQcPost.Text = "";
            this.txtQnPost.Text = "";

            this.txtQcEmpl.Tag = "";
            this.txtQcEmpl.Text = "";
            this.txtQcEmpl.Text = "";

            this.pmCreateTem();
            this.rdoType.SelectedIndex = 1;
            this.pmSetStatus();

            this.cmbStdRight.Properties.Items.Clear();
            this.cmbStdRight.Properties.Items.Add("Top Manager");
            this.cmbStdRight.Properties.Items.Add("Middle Manager");
            this.cmbStdRight.Properties.Items.Add("Operator");

            this.bllIsAskForSave = false;
            this.cmbStdRight.SelectedIndex = 0;
        
        }

        //private string mstrRefTable = "APPOBJ";
        private void pmLoadFormData(bool IsStdRig, PostLevel inLevel, string inPost, string inEmplR)
        {

            switch (App.AppModule)
            {
                case "POS1_SYS":
                    this.mstrRefTable = "APPOBJ_POS";
                    break;
                default:
                    this.mstrRefTable = "APPOBJ";
                    break;
            }


            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.dtsDataEnv.Tables[this.mstrTemObj].Rows.Clear();
            
            //string strSQLStr = "select * from AppObj order by cSeq";
            //OleDbConnection conn = new OleDbConnection(App.xd_Access_ConnectionString);
            //OleDbCommand cmd = new OleDbCommand(strSQLStr, conn);

            try
            {


                                
                //if (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "AppAuthB_ADD1", "AppAuthB_ADD1", "select * from [APPAUTHB_ADD1] where cPost = ? and cTaskName = ?", new object[] { strPost, SysDef.gc_AUTH_RIG_CHGTRANDATE }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                bool bllIsAllow = false;
                this.pmLoadAppAuthB_Add1(inPost, SysDef.gc_AUTH_RIG_CHGTRANDATE, ref bllIsAllow);
                this.bllAuth_Add_Rig1 = bllIsAllow;
                //conn.Open();
                //System.Data.OleDb.OleDbDataReader dtrAppObj = (System.Data.OleDb.OleDbDataReader)cmd.ExecuteReader();
                string strSQLStr = " SELECT * FROM " + this.mstrRefTable + " as APPOBJ WHERE CVISIBLE <> 'N'  ";
                strSQLStr += "and (select pr.CVISIBLE from " + this.mstrRefTable + " pr where pr.CSEQ = substring(APPOBJ.CSEQ,1,2)) <> 'N' ";
                strSQLStr += " ORDER BY CSEQ ";
                //if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QAppObj", "APPOBJ", "select * from AppObj where cVisible <> 'N' order by cSeq", ref strErrorMsg))
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QAppObj", "APPOBJ", strSQLStr, ref strErrorMsg))
                //if (dtrAppObj.HasRows)
                {
                    bool bllIsEnter = false;
                    bool bllIsInsert = false;
                    bool bllIsUpdate = false;
                    bool bllIsDelete = false;
                    bool bllIsApprove = false;
                    bool bllIsApprove2 = false;
                    bool bllIsPrint = false;

                    //while (dtrAppObj.Read())
                    foreach (DataRow dtrAppObj in this.dtsDataEnv.Tables["QAppObj"].Rows)
                    {

                        DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemObj].NewRow();
                        string strTaskName = dtrAppObj["cTaskName"].ToString();
                        dtrNewRow["ID"] = dtrAppObj["cSeq"].ToString();
                        dtrNewRow["ParentID"] = dtrAppObj["cPRSeq"].ToString();
                        dtrNewRow["cMenuSeq"] = dtrAppObj["cSeq"].ToString();
                        dtrNewRow["cTaskName"] = strTaskName;
                        dtrNewRow["cText"] = dtrAppObj["cDesc"].ToString();

                        string strSuffix = this.pmGetLevelStr(inLevel);

                        dtrNewRow["IsEnter"] = false;
                        dtrNewRow["IsInsert"] = false;
                        dtrNewRow["IsUpdate"] = false;
                        dtrNewRow["IsDelete"] = false;
                        dtrNewRow["IsPrint"] = true;

                        //dtrNewRow["IsEnter"] = (dtrAppObj["cAccess_" + strSuffix].ToString() == "Y");
                        //dtrNewRow["IsInsert"] = (dtrAppObj["cInsert_" + strSuffix].ToString() == "Y");
                        //dtrNewRow["IsUpdate"] = (dtrAppObj["cEdit_" + strSuffix].ToString() == "Y");
                        //dtrNewRow["IsDelete"] = (dtrAppObj["cDelete_" + strSuffix].ToString() == "Y");

                        if (IsStdRig)
                        {
                            if (this.pmLoadStdRight(inLevel, strTaskName, ref bllIsEnter, ref bllIsInsert, ref bllIsUpdate, ref bllIsDelete))
                            {
                                dtrNewRow["IsEnter"] = bllIsEnter;
                                dtrNewRow["IsInsert"] = bllIsInsert;
                                dtrNewRow["IsUpdate"] = bllIsUpdate;
                                dtrNewRow["IsDelete"] = bllIsDelete;
                                dtrNewRow["IsPrint"] = true;
                            }
                        }
                        else 
                        {
                            if (inEmplR.TrimEnd() == string.Empty)
                            {
                                if (this.pmLoadAppAuthB(inPost, strTaskName, ref bllIsEnter, ref bllIsInsert, ref bllIsUpdate, ref bllIsDelete, ref bllIsApprove, ref bllIsApprove2, ref bllIsPrint))
                                {
                                    dtrNewRow["IsEnter"] = bllIsEnter;
                                    dtrNewRow["IsInsert"] = bllIsInsert;
                                    dtrNewRow["IsUpdate"] = bllIsUpdate;
                                    dtrNewRow["IsDelete"] = bllIsDelete;
                                    dtrNewRow["IsApprove"] = bllIsApprove;
                                    dtrNewRow["IsApprove2"] = bllIsApprove2;
                                    dtrNewRow["IsPrint"] = bllIsPrint;

                                }
                            }
                            else 
                            {
                                if (this.pmLoadAppAuthC(inPost, inEmplR, strTaskName, ref bllIsEnter, ref bllIsInsert, ref bllIsUpdate, ref bllIsDelete, ref bllIsPrint))
                                {
                                    dtrNewRow["IsEnter"] = bllIsEnter;
                                    dtrNewRow["IsInsert"] = bllIsInsert;
                                    dtrNewRow["IsUpdate"] = bllIsUpdate;
                                    dtrNewRow["IsDelete"] = bllIsDelete;
                                    dtrNewRow["IsPrint"] = bllIsPrint;
                                    
                                }
                            }
                        }

                        this.dtsDataEnv.Tables[this.mstrTemObj].Rows.Add(dtrNewRow);

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //conn.Close();
            }

            this.grdTemObj.ExpandAll();

        }

        private bool pmLoadStdRight(PostLevel inLevel, string inTaskName, ref bool ioIsEnter, ref bool ioIsInsert, ref bool ioIsUpdate, ref bool ioIsDelete)
        {
            bool bllFoundRight = false;
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strLevel = this.pmGetLevelStr(inLevel);

            objSQLHelper.SetPara(new object[] { strLevel, inTaskName });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QAuthA", this.mstrRefTable, "select * from AppAuthA where cLevel = ? and cTaskName = ?", ref strErrorMsg))
            {

                DataRow dtrAuthA = this.dtsDataEnv.Tables["QAuthA"].Rows[0];

                ioIsEnter = (dtrAuthA["cAccess"].ToString() == "Y");
                ioIsInsert = (dtrAuthA["cInsert"].ToString() == "Y");
                ioIsUpdate = (dtrAuthA["cEdit"].ToString() == "Y");
                ioIsDelete = (dtrAuthA["cDelete"].ToString() == "Y");
                bllFoundRight = true;
            }
            return bllFoundRight;
        
        }

        private bool pmLoadAppAuthB(string inPost, string inTaskName, ref bool ioIsEnter, ref bool ioIsInsert, ref bool ioIsUpdate, ref bool ioIsDelete, ref bool ioIsApprove, ref bool ioIsApprove2, ref bool ioIsPrint)
        {
            bool bllFoundRight = false;
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { inPost, inTaskName });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QAuthB", this.mstrRefTable, "select * from AppAuthB where cPost = ? and cTaskName = ?", ref strErrorMsg))
            {

                DataRow dtrAuthB = this.dtsDataEnv.Tables["QAuthB"].Rows[0];

                ioIsEnter = (dtrAuthB["cAccess"].ToString() == "Y");
                ioIsInsert = (dtrAuthB["cInsert"].ToString() == "Y");
                ioIsUpdate = (dtrAuthB["cEdit"].ToString() == "Y");
                ioIsDelete = (dtrAuthB["cDelete"].ToString() == "Y");
                ioIsApprove = (dtrAuthB["cApprove"].ToString() == "Y");
                ioIsApprove2 = (dtrAuthB["cApprove2"].ToString() == "Y");
                ioIsPrint = (dtrAuthB["CCANPRINT"].ToString() == "Y");
                
                bllFoundRight = true;
            }
            return bllFoundRight;

        }

        private bool pmLoadAppAuthB_Add1(string inPost, string inTaskName, ref bool ioIsAllow)
        {
            bool bllFoundRight = false;
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { inPost, inTaskName });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QAuthB", this.mstrRefTable, "select * from AppAuthB_Add1 where cPost = ? and cTaskName = ?", ref strErrorMsg))
            {

                DataRow dtrAuthB = this.dtsDataEnv.Tables["QAuthB"].Rows[0];

                ioIsAllow = (dtrAuthB["CALLOW"].ToString() == "Y");

                bllFoundRight = true;
            }
            return bllFoundRight;

        }

        private bool pmLoadAppAuthC(string inPost, string inEmplR, string inTaskName, ref bool ioIsEnter, ref bool ioIsInsert, ref bool ioIsUpdate, ref bool ioIsDelete, ref bool ioIsPrint)
        {
            bool bllFoundRight = false;
            string strErrorMsg = "";

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            objSQLHelper.SetPara(new object[] { inPost, inEmplR, inTaskName });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QAuthB", this.mstrRefTable, "select * from AppAuthC where cPost = ? and cEmplR = ? and cTaskName = ?", ref strErrorMsg))
            {

                DataRow dtrAuthB = this.dtsDataEnv.Tables["QAuthB"].Rows[0];

                ioIsEnter = (dtrAuthB["cAccess"].ToString() == "Y");
                ioIsInsert = (dtrAuthB["cInsert"].ToString() == "Y");
                ioIsUpdate = (dtrAuthB["cEdit"].ToString() == "Y");
                ioIsDelete = (dtrAuthB["cDelete"].ToString() == "Y");
                ioIsPrint = (dtrAuthB["CCANPRINT"].ToString() == "Y");
                bllFoundRight = true;
            }
            return bllFoundRight;

        }

        private void pmCreateTem()
        {
            DataTable dtbTemObj = new DataTable(this.mstrTemObj);
            dtbTemObj.Columns.Add("ID", System.Type.GetType("System.String"));
            dtbTemObj.Columns.Add("ParentID", System.Type.GetType("System.String"));
            dtbTemObj.Columns.Add("cMenuSeq", System.Type.GetType("System.String"));
            dtbTemObj.Columns.Add("cTaskName", System.Type.GetType("System.String"));
            dtbTemObj.Columns.Add("cText", System.Type.GetType("System.String"));
            dtbTemObj.Columns.Add("IsEnter", System.Type.GetType("System.Boolean"));
            dtbTemObj.Columns.Add("IsInsert", System.Type.GetType("System.Boolean"));
            dtbTemObj.Columns.Add("IsUpdate", System.Type.GetType("System.Boolean"));
            dtbTemObj.Columns.Add("IsDelete", System.Type.GetType("System.Boolean"));
            dtbTemObj.Columns.Add("IsApprove", System.Type.GetType("System.Boolean"));
            dtbTemObj.Columns.Add("IsApprove2", System.Type.GetType("System.Boolean"));
            dtbTemObj.Columns.Add("IsPrint", System.Type.GetType("System.Boolean"));

            dtbTemObj.Columns["IsEnter"].DefaultValue = false;
            dtbTemObj.Columns["IsInsert"].DefaultValue = false;
            dtbTemObj.Columns["IsUpdate"].DefaultValue = false;
            dtbTemObj.Columns["IsDelete"].DefaultValue = false;
            dtbTemObj.Columns["IsApprove"].DefaultValue = false;
            dtbTemObj.Columns["IsApprove2"].DefaultValue = false;
            dtbTemObj.Columns["IsPrint"].DefaultValue = true;

            this.dtsDataEnv.Tables.Add(dtbTemObj);
        }

        private void pmInitGridProp()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemObj].DefaultView;
            dvBrow.Sort = "cMenuSeq";

            this.grdTemObj.DataSource = this.dtsDataEnv.Tables[this.mstrTemObj];
            this.grdTemObj.PopulateColumns();
            this.grdTemObj.BestFitColumns();
            this.grdTemObj.ExpandAll();

            this.grdTemObj.Columns["cMenuSeq"].OptionsColumn.AllowEdit = false;
            this.grdTemObj.Columns["cTaskName"].OptionsColumn.AllowEdit = false;
            this.grdTemObj.Columns["cText"].OptionsColumn.AllowEdit = false;

            for (int intCnt = 0; intCnt < this.grdTemObj.Columns.Count; intCnt++)
            {
                this.grdTemObj.Columns[intCnt].BestFit();
            }

            this.grdTemObj.Columns["cText"].Width = (this.grdTemObj.Columns["cText"].Width > 300 ? 300 : this.grdTemObj.Columns["cText"].Width);

            this.grdTemObj.Columns["cMenuSeq"].Caption = "Seq.";
            this.grdTemObj.Columns["cTaskName"].Caption = "Task Name";
            this.grdTemObj.Columns["cText"].Caption = "Menu Name";
            this.grdTemObj.Columns["IsEnter"].Caption = "เข้า";
            this.grdTemObj.Columns["IsInsert"].Caption = "เพิ่ม";
            this.grdTemObj.Columns["IsUpdate"].Caption = "แก้ไข";
            this.grdTemObj.Columns["IsDelete"].Caption = "ลบ";
            this.grdTemObj.Columns["IsApprove"].Caption = "อนุมัติ";
            this.grdTemObj.Columns["IsApprove2"].Caption = "Pass/Reject";
            this.grdTemObj.Columns["IsPrint"].Caption = "พิมพ์";


        }

        private bool bllAuth_Add_Rig1 = false;
        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                string strMenuName = e.Item.Tag.ToString();
                switch (strMenuName)
                {
                    case "ADD1":
                        using (BeSmartMRP.Permission.frmAuth_Add1 dlgAuth1 = new BeSmartMRP.Permission.frmAuth_Add1())
                        {
                            dlgAuth1.txtAuth1.IsOn = this.bllAuth_Add_Rig1;
                            dlgAuth1.ShowDialog();
                            if (dlgAuth1.DialogResult == DialogResult.OK)
                            {
                                this.bllAuth_Add_Rig1 = dlgAuth1.txtAuth1.IsOn;
                            }
                        }
                        break;
                    default:

                        #region "Default Toolbar Menu"
                        WsToolBar oToolButton = AppEnum.GetToolBarEnum(strMenuName);
                        switch (oToolButton)
                        {
                            case WsToolBar.Save:
                                if (this.txtQcPost.Text == "")
                                {
                                    MessageBox.Show(this, "กรุณาระบุตำแหน่งก่อน Save !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    this.txtQcPost.Focus();
                                }
                                else
                                {
                                    this.pmSaveAuthB();
                                }

                                break;
                            case WsToolBar.Exit:
                                this.Close();
                                break;
                        }

                        break;
                        #endregion
                }

            }
        }

        private void rdoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pmSetStatus();
        }

        private void pmSetStatus()
        {
            this.pnlStandard.Enabled = (this.rdoType.SelectedIndex == 0);
            this.pnlPost.Enabled = (this.rdoType.SelectedIndex == 1);

            PostLevel mLevel = this.pmGetLevel(this.cmbStdRight.SelectedIndex);
            if (this.pnlStandard.Enabled)
            {
                this.pmLoadFormData(true, mLevel, "", "");
            }
            else
            {
                this.pmLoadFormData(false, mLevel, this.txtQcPost.Tag.ToString(), this.txtQcEmpl.Tag.ToString());
            }
        
        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "POST":
                    if (this.pofrmGetPost == null)
                    {
                        //this.pofrmGetPost = new DialogForms.dlgGetPost();
                        this.pofrmGetPost = new DatabaseForms.frmAppRole(FormActiveMode.Report);
                        this.pofrmGetPost.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPost.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
                    }
                    break;
                case "EMPL":
                    if (this.pofrmGetEmplR == null)
                    {
                        //this.pofrmGetEmplR = new DialogForms.dlgGetEmplR();
                        this.pofrmGetEmplR = new DatabaseForms.frmAppLogin(FormActiveMode.Report);
                        this.pofrmGetEmplR.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetEmplR.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                case "TXTQCPOST":
                case "TXTQNPOST":
                    this.pmInitPopUpDialog("POST");
                    this.pofrmGetPost.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCPOST" ? "CCODE" : "CNAME"), true);
                    if (this.pofrmGetPost.PopUpResult)
                    {
                        this.pmRetrievePopUpVal("POST");
                    }
                    break;
                case "TXTQCEMPL":
                case "TXTQNEMPL":
                    this.pmInitPopUpDialog("EMPL");
                    this.pofrmGetEmplR.ValidateField(inPara1, (inTextbox.ToUpper() == "TXTQCEMPL" ? "CLOGIN" : "CRCODE"), true);
                    if (this.pofrmGetEmplR.PopUpResult)
                    {
                        this.pmRetrievePopUpVal("EMPL");
                    }
                    break;
            }
        }

        private void pmRetrievePopUpVal(string inPopupForm)
        {

            switch (inPopupForm.TrimEnd().ToUpper())
            {
                case "POST":
                    if (this.pofrmGetPost != null)
                    {
                        DataRow dtrPost = this.pofrmGetPost.RetrieveValue();

                        if (dtrPost != null)
                        {
                            if (this.txtQcPost.Tag.ToString() != dtrPost["cRowID"].ToString())
                            {
                                this.txtQcPost.Tag = dtrPost["cRowID"].ToString();
                                this.pmLoadFormData(false, this.pmGetLevelByStr(this.txtQcPost.Tag.ToString()), this.txtQcPost.Tag.ToString(), this.txtQcEmpl.Tag.ToString());
                            }

                            this.txtQcPost.Text = dtrPost["cCode"].ToString().TrimEnd();
                            this.txtQnPost.Text = dtrPost["cName"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.pmBlankPost();
                        }
                    }
                    break;
                case "EMPL":
                    if (this.pofrmGetEmplR != null)
                    {
                        DataRow dtrEmpl = this.pofrmGetEmplR.RetrieveValue();

                        if (dtrEmpl != null)
                        {

                            if (this.txtQcEmpl.Tag.ToString() != dtrEmpl["fcSkid"].ToString())
                            {
                                this.txtQcEmpl.Tag = dtrEmpl["fcSkid"].ToString();
                                this.pmLoadFormData(false, PostLevel.Top, this.txtQcPost.Tag.ToString(), this.txtQcEmpl.Tag.ToString());
                            }

                            
                            this.txtQcEmpl.Text = dtrEmpl["fcLogin"].ToString().TrimEnd();
                            this.txtQnEmpl.Text = dtrEmpl["fcRCode"].ToString().TrimEnd();
                        }
                        else
                        {
                            this.pmBlankEmpl();
                        }
                    }
                    break;
            }
        }

        private void pmBlankPost()
        {
            this.txtQcPost.Tag = "";
            this.txtQcPost.Text = "";
            this.txtQnPost.Text = "";
            this.pmLoadFormData(false, PostLevel.Top, this.txtQcPost.Tag.ToString(), this.txtQcEmpl.Tag.ToString());
        }

        private void pmBlankEmpl()
        {
            this.txtQcEmpl.Tag = "";
            this.txtQcEmpl.Text = "";
            this.txtQnEmpl.Text = "";
            this.pmLoadFormData(false, PostLevel.Top, this.txtQcPost.Tag.ToString(), this.txtQcEmpl.Tag.ToString());
        }

        private void txtQcPost_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCPOST" ? "CCODE" : "CNAME");
            if (txtPopUp.Text == "")
            {
                this.pmBlankPost();
            }
            else
            {
                this.pmInitPopUpDialog("POST");
                e.Cancel = !this.pofrmGetPost.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetPost.PopUpResult)
                {
                    this.pmRetrievePopUpVal("POST");
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }
        private void txtQcEmpl_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCEMPL" ? "CLOGIN" : "CRCODE");
            if (txtPopUp.Text == "")
            {
                this.pmBlankEmpl();
            }
            else
            {
                this.pmInitPopUpDialog("EMPL");
                e.Cancel = !this.pofrmGetEmplR.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetEmplR.PopUpResult)
                {
                    this.pmRetrievePopUpVal("EMPL");
                }
                else
                {
                    txtPopUp.Text = txtPopUp.OldEditValue.ToString();
                }
            }

        }

        private void frmSetPermission_KeyDown(object sender, KeyEventArgs e)
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

        private void cmbStdRight_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.bllIsAskForSave)
            {
                DialogResult mResult = MessageBox.Show("Save Data", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                PostLevel mLevel = this.pmGetLevel(this.mintSaveStdRigIdx);
                PostLevel mLoadLevel = this.pmGetLevel(this.cmbStdRight.SelectedIndex);

                switch (mResult)
                {
                    case DialogResult.Yes:
                        this.pmSaveStdRig(mLevel);
                        this.pmLoadFormData(true, mLoadLevel, "", "");
                        break;
                    case DialogResult.No:
                        this.pmLoadFormData(true, mLoadLevel, "", "");
                        break;
                    case DialogResult.Cancel:
                        this.bllIsAskForSave = false;
                        this.cmbStdRight.SelectedIndex = this.mintSaveStdRigIdx;
                        break;
                }

                this.mintSaveStdRigIdx = this.cmbStdRight.SelectedIndex;
            }
            this.bllIsAskForSave = true;


        }

        private PostLevel pmGetLevel(int inIdx)
        {
            PostLevel mLevel = PostLevel.Operator;
            switch (inIdx)
            {
                case 0:
                    mLevel = PostLevel.Top;
                    break;
                case 1:
                    mLevel = PostLevel.Middle;
                    break;
                case 2:
                    mLevel = PostLevel.Operator;
                    break;
            }
            return mLevel;
        }

        private PostLevel pmGetLevelByStr(string inPoststr)
        {
            PostLevel mLevel = PostLevel.Operator;
            switch (inPoststr)
            {
                case "T":
                    mLevel = PostLevel.Top;
                    break;
                case "M":
                    mLevel = PostLevel.Middle;
                    break;
                case "O":
                    mLevel = PostLevel.Operator;
                    break;
            }
            return mLevel;
        }

        private string pmGetLevelStr(PostLevel inLevel)
        {
            string strLevel = "T";
            switch (inLevel)
            {
                case PostLevel.Top:
                    strLevel = "T";
                    break;
                case PostLevel.Middle:
                    strLevel = "M";
                    break;
                case PostLevel.Operator:
                    strLevel = "O";
                    break;
            }
            return strLevel;
        }

        private string pmGetLevelByPost(string inPost)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            PostLevel mLevel = PostLevel.Top;
            string strLevel = "T";
            objSQLHelper.SetPara(new object[] { inPost });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPost", "POST", "select * from APPROLE where cRowID = ?", ref strErrorMsg))
            {
                strLevel = this.dtsDataEnv.Tables["QPost"].Rows[0]["cLevel"].ToString().TrimEnd();
                mLevel = this.pmGetLevelByStr(strLevel);
            }
            return strLevel;
        }

        private void pmSaveStdRig(PostLevel inLevel)
        {

            UIBase.WaitWind("Saving Data Please wait...");

            this.grdTemObj.RefreshDataSource();
            this.cmbStdRight.Focus();

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.xd_Access_ConnectionString, DBMSType.MSAccess);

            string strEditRowID = "";
            string strTaskName = "";
            string strErrorMsg = "";
            string strSuffix = this.pmGetLevelStr(inLevel);

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
			this.mSaveDBAgent.AppID = App.AppID;
			this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                bool bllIsEnter = false;
                bool bllIsInsert = false;
                bool bllIsUpdate = false;
                bool bllIsDelete = false;

                bool bllDefaIsEnter = false;
                bool bllDefaIsInsert = false;
                bool bllDefaIsUpdate = false;
                bool bllDefaIsDelete = false;

                foreach (DataRow dtrTemObj in this.dtsDataEnv.Tables[this.mstrTemObj].Rows)
                {
                    strTaskName = dtrTemObj["cTaskName"].ToString();

                    string strLevel = this.pmGetLevelStr(inLevel); ;

                    bool bllIsNewRow = false;
                    DataRow dtrDefaRow = null;
                    DataRow dtrNewRow = null;

                    if (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "AppAuthA", "AppAuthA", "select * from AppAuthA where cLevel = ? and cTaskName = ?", new object[] { strLevel, strTaskName }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                    {
                        bllIsNewRow = true;
                        WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                        strEditRowID = objConn.RunRowID("AppAuthA");
                        dtrNewRow = this.dtsDataEnv.Tables["AppAuthA"].NewRow();
                        dtrNewRow["cRowID"] = strEditRowID;
                        dtrNewRow["cCreateBy"] = App.FMAppUserID;
                    }
                    else
                    {
                        bllIsNewRow = false;
                        dtrNewRow = this.dtsDataEnv.Tables["AppAuthA"].Rows[0];
                    }

                    objSQLHelper2.SetPara(new object[] { strTaskName });
                    if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QAppObj", "APPOBJ", "select * from " + this.mstrRefTable + " where cTaskName = ?", ref strErrorMsg))
                    {
                        dtrDefaRow = this.dtsDataEnv.Tables["QAppObj"].Rows[0];

                        bllDefaIsEnter = (dtrDefaRow["cAccess_" + strSuffix].ToString() == "Y" ? true : false);
                        bllDefaIsInsert = (dtrDefaRow["cInsert_" + strSuffix].ToString() == "Y" ? true : false);
                        bllDefaIsUpdate = (dtrDefaRow["cEdit_" + strSuffix].ToString() == "Y" ? true : false);
                        bllDefaIsDelete = (dtrDefaRow["cDelete_" + strSuffix].ToString() == "Y" ? true : false);

                    }

                    dtrNewRow["cLevel"] = strLevel;
                    dtrNewRow["cTaskName"] = dtrTemObj["cTaskName"].ToString();

                    bllIsEnter = (Convert.ToBoolean(dtrTemObj["IsEnter"]) == true ? true : false);
                    bllIsInsert = (Convert.ToBoolean(dtrTemObj["IsInsert"]) == true ? true : false);
                    bllIsUpdate = (Convert.ToBoolean(dtrTemObj["IsUpdate"]) == true ? true : false);
                    bllIsDelete = (Convert.ToBoolean(dtrTemObj["IsDelete"]) == true ? true : false);

                    dtrNewRow["cAccess"] = (bllIsEnter ? "Y" : "N");
                    dtrNewRow["cInsert"] = (bllIsInsert ? "Y" : "N");
                    dtrNewRow["cEdit"] = (bllIsUpdate ? "Y" : "N");
                    dtrNewRow["cDelete"] = (bllIsDelete ? "Y" : "N");
                    dtrNewRow["dLastUpd"] = this.mSaveDBAgent.GetDBServerDateTime();
                    dtrNewRow["cLastUpdBy"] = App.FMAppUserID;

                    string strSQLUpdateStr = "";
                    object[] pAPara = null;
                    if (bllDefaIsEnter == bllIsEnter
                        && bllDefaIsInsert == bllIsInsert
                        && bllDefaIsUpdate == bllIsUpdate
                        && bllDefaIsDelete == bllIsDelete)
                    {
                        pAPara = new object[] { dtrNewRow["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from AppAuthA where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    }
                    else
                    {
                        cDBMSAgent.GenUpdateSQLString(dtrNewRow, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                        this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    }

                }

                this.mdbTran.Commit();
            
            }
            catch (Exception ex)
            {
                this.mdbTran.Rollback();
                App.WriteEventsLog(ex);
#if xd_RUNMODE_DEBUG
				MessageBox.Show("Message : " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
#endif
            }

            finally
            {
                this.mdbConn.Close();
            }
            UIBase.WaitClear();
            MessageBox.Show(this, "Save Complete", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pmSaveAuthB()
        {
            UIBase.WaitWind("Saving Data Please wait...");

            this.grdTemObj.RefreshDataSource();
            this.cmbStdRight.Focus();

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.xd_Access_ConnectionString, DBMSType.MSAccess);

            string strEditRowID = "";
            string strTaskName = "";
            string strErrorMsg = "";
            string strSuffix = "T";

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            this.mSaveDBAgent.AppID = App.AppID;
            this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                bool bllIsEnter = false;
                bool bllIsInsert = false;
                bool bllIsUpdate = false;
                bool bllIsDelete = false;
                bool bllIsApprove = false;
                bool bllIsApprove2 = false;
                bool bllIsPrint = false;

                bool bllDefaIsEnter = false;
                bool bllDefaIsInsert = false;
                bool bllDefaIsUpdate = false;
                bool bllDefaIsDelete = false;
                bool bllDefaIsApprove = false;
                bool bllDefaIsApprove2 = false;

                string strPost = this.txtQcPost.Tag.ToString();
                string strLevel = this.pmGetLevelByPost(strPost);
                strSuffix = strLevel;

                bool bllIsNewRow = false;
                DataRow dtrNewRow = null;
                object[] pAPara = null;

                if (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "AppAuthB_ADD1", "AppAuthB_ADD1", "select * from [APPAUTHB_ADD1] where cPost = ? and cTaskName = ?", new object[] { strPost, SysDef.gc_AUTH_RIG_CHGTRANDATE }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                {
                    bllIsNewRow = true;
                    WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                    strEditRowID = objConn.RunRowID("AppAuthB_ADD1");
                    dtrNewRow = this.dtsDataEnv.Tables["AppAuthB_ADD1"].NewRow();
                    dtrNewRow["cRowID"] = strEditRowID;
                    dtrNewRow["cCreateBy"] = App.FMAppUserID;
                }
                else
                {
                    bllIsNewRow = false;
                    dtrNewRow = this.dtsDataEnv.Tables["AppAuthB_ADD1"].Rows[0];
                }
                dtrNewRow["cLevel"] = strLevel;
                dtrNewRow["cPost"] = strPost;
                dtrNewRow["cTaskName"] = SysDef.gc_AUTH_RIG_CHGTRANDATE;
                dtrNewRow["CALLOW"] = (this.bllAuth_Add_Rig1 ? "Y" : "N");
                string strSQLUpdateStr1 = "";
                cDBMSAgent.GenUpdateSQLString(dtrNewRow, "CROWID", bllIsNewRow, ref strSQLUpdateStr1, ref pAPara);
                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr1, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);


                foreach (DataRow dtrTemObj in this.dtsDataEnv.Tables[this.mstrTemObj].Rows)
                {
                    strTaskName = dtrTemObj["cTaskName"].ToString();

                    DataRow dtrDefaRow = null;
                    bllIsNewRow = false;
                    dtrNewRow = null;

                    if (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "AppAuthB", "AppAuthB", "select * from AppAuthB where cPost = ? and cTaskName = ?", new object[] { strPost, strTaskName }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                    {
                        bllIsNewRow = true;
                        WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                        strEditRowID = objConn.RunRowID("AppAuthB");
                        dtrNewRow = this.dtsDataEnv.Tables["AppAuthB"].NewRow();
                        dtrNewRow["cRowID"] = strEditRowID;
                        dtrNewRow["cCreateBy"] = App.FMAppUserID;
                    }
                    else
                    {
                        bllIsNewRow = false;
                        dtrNewRow = this.dtsDataEnv.Tables["AppAuthB"].Rows[0];
                    }

                    bllDefaIsEnter = false;
                    bllDefaIsInsert = false;
                    bllDefaIsUpdate = false;
                    bllDefaIsDelete = false;

                    //objSQLHelper2.SetPara(new object[] { strTaskName });
                    //if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QAppObj", "APPOBJ", "select * from AppObj where cTaskName = ?", ref strErrorMsg))
                    //{
                    //    dtrDefaRow = this.dtsDataEnv.Tables["QAppObj"].Rows[0];

                    //    bllDefaIsEnter = (dtrDefaRow["cAccess_" + strSuffix].ToString() == "Y" ? true : false);
                    //    bllDefaIsInsert = (dtrDefaRow["cInsert_" + strSuffix].ToString() == "Y" ? true : false);
                    //    bllDefaIsUpdate = (dtrDefaRow["cEdit_" + strSuffix].ToString() == "Y" ? true : false);
                    //    bllDefaIsDelete = (dtrDefaRow["cDelete_" + strSuffix].ToString() == "Y" ? true : false);

                    //}

                    dtrNewRow["cLevel"] = strLevel;
                    dtrNewRow["cPost"] = strPost;
                    dtrNewRow["cTaskName"] = dtrTemObj["cTaskName"].ToString();

                    bllIsEnter = (Convert.ToBoolean(dtrTemObj["IsEnter"]) == true ? true : false);
                    bllIsInsert = (Convert.ToBoolean(dtrTemObj["IsInsert"]) == true ? true : false);
                    bllIsUpdate = (Convert.ToBoolean(dtrTemObj["IsUpdate"]) == true ? true : false);
                    bllIsDelete = (Convert.ToBoolean(dtrTemObj["IsDelete"]) == true ? true : false);
                    bllIsApprove = (Convert.ToBoolean(dtrTemObj["IsApprove"]) == true ? true : false);
                    bllIsApprove2 = (Convert.ToBoolean(dtrTemObj["IsApprove2"]) == true ? true : false);
                    bllIsPrint = (Convert.ToBoolean(dtrTemObj["IsPrint"]) == true ? true : false);

                    dtrNewRow["cAccess"] = (bllIsEnter ? "Y" : "N");
                    dtrNewRow["cInsert"] = (bllIsInsert ? "Y" : "N");
                    dtrNewRow["cEdit"] = (bllIsUpdate ? "Y" : "N");
                    dtrNewRow["cDelete"] = (bllIsDelete ? "Y" : "N");
                    dtrNewRow["cApprove"] = (bllIsApprove ? "Y" : "N");
                    dtrNewRow["cApprove2"] = (bllIsApprove2 ? "Y" : "N");
                    dtrNewRow["cCanPrint"] = (bllIsPrint ? "Y" : "N");
                    dtrNewRow["dLastUpd"] = this.mSaveDBAgent.GetDBServerDateTime();
                    dtrNewRow["cLastUpdBy"] = App.FMAppUserID;

                    string strSQLUpdateStr = "";
                    
                    pAPara = null;
                    if (bllDefaIsEnter == bllIsEnter
                        && bllDefaIsInsert == bllIsInsert
                        && bllDefaIsUpdate == bllIsUpdate
                        && bllDefaIsDelete == bllIsDelete
                        && bllDefaIsApprove == bllIsApprove
                        && bllDefaIsApprove2 == bllIsApprove2)
                    {
                        pAPara = new object[] { dtrNewRow["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from AppAuthB where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    }
                    else
                    {
                        cDBMSAgent.GenUpdateSQLString(dtrNewRow, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                        this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    }

                }

                this.mdbTran.Commit();

                KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Update, TASKNAME, "", "", App.FMAppUserID, App.AppUserName);

            }
            catch (Exception ex)
            {
                this.mdbTran.Rollback();
                App.WriteEventsLog(ex);
#if xd_RUNMODE_DEBUG
                MessageBox.Show("Message : " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
#endif
            }

            finally
            {
                this.mdbConn.Close();
            }
            UIBase.WaitClear();
            MessageBox.Show(this, "Save Complete", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pmSaveAuthC()
        {

            UIBase.WaitWind("Saving Data Please wait...");

            this.grdTemObj.RefreshDataSource();
            this.cmbStdRight.Focus();

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.xd_Access_ConnectionString, DBMSType.MSAccess);

            string strEditRowID = "";
            string strTaskName = "";
            string strErrorMsg = "";
            //PostLevel mPost = this.pmGetLevelByStr(this.txtQcPost.Tag.ToString());
            string strSuffix = "T";

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            this.mSaveDBAgent.AppID = App.AppID;
            this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                bool bllIsEnter = false;
                bool bllIsInsert = false;
                bool bllIsUpdate = false;
                bool bllIsDelete = false;

                bool bllDefaIsEnter = false;
                bool bllDefaIsInsert = false;
                bool bllDefaIsUpdate = false;
                bool bllDefaIsDelete = false;

                foreach (DataRow dtrTemObj in this.dtsDataEnv.Tables[this.mstrTemObj].Rows)
                {
                    strTaskName = dtrTemObj["cTaskName"].ToString();

                    string strLevel = this.pmGetLevelByPost(this.txtQcPost.Tag.ToString());
                    strSuffix = strLevel;

                    bool bllIsNewRow = false;
                    DataRow dtrDefaRow = null;
                    DataRow dtrNewRow = null;

                    if (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "AppAuthC", "AppAuthC", "select * from AppAuthC where cPost = ? and cEmplR = ? and cTaskName = ?", new object[] { this.txtQcPost.Tag.ToString(), this.txtQcEmpl.Tag.ToString(), strTaskName }, ref strErrorMsg, this.mdbConn, this.mdbTran))
                    {
                        bllIsNewRow = true;
                        WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
                        strEditRowID = objConn.RunRowID("AppAuthC");
                        dtrNewRow = this.dtsDataEnv.Tables["AppAuthC"].NewRow();
                        dtrNewRow["cRowID"] = strEditRowID;
                        dtrNewRow["cCreateBy"] = App.FMAppUserID;
                    }
                    else
                    {
                        bllIsNewRow = false;
                        dtrNewRow = this.dtsDataEnv.Tables["AppAuthC"].Rows[0];
                    }

                    objSQLHelper2.SetPara(new object[] { strTaskName });
                    if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QAppObj", "APPOBJ", "select * from " + this.mstrRefTable + " where cTaskName = ?", ref strErrorMsg))
                    {
                        dtrDefaRow = this.dtsDataEnv.Tables["QAppObj"].Rows[0];

                        bllDefaIsEnter = (dtrDefaRow["cAccess_" + strSuffix].ToString() == "Y" ? true : false);
                        bllDefaIsInsert = (dtrDefaRow["cInsert_" + strSuffix].ToString() == "Y" ? true : false);
                        bllDefaIsUpdate = (dtrDefaRow["cEdit_" + strSuffix].ToString() == "Y" ? true : false);
                        bllDefaIsDelete = (dtrDefaRow["cDelete_" + strSuffix].ToString() == "Y" ? true : false);

                    }

                    dtrNewRow["cLevel"] = strLevel;
                    dtrNewRow["cPost"] = this.txtQcPost.Tag.ToString();
                    dtrNewRow["cEmplR"] = this.txtQcEmpl.Tag.ToString();
                    dtrNewRow["cTaskName"] = dtrTemObj["cTaskName"].ToString();

                    bllIsEnter = (Convert.ToBoolean(dtrTemObj["IsEnter"]) == true ? true : false);
                    bllIsInsert = (Convert.ToBoolean(dtrTemObj["IsInsert"]) == true ? true : false);
                    bllIsUpdate = (Convert.ToBoolean(dtrTemObj["IsUpdate"]) == true ? true : false);
                    bllIsDelete = (Convert.ToBoolean(dtrTemObj["IsDelete"]) == true ? true : false);

                    dtrNewRow["cAccess"] = (bllIsEnter ? "Y" : "N");
                    dtrNewRow["cInsert"] = (bllIsInsert ? "Y" : "N");
                    dtrNewRow["cEdit"] = (bllIsUpdate ? "Y" : "N");
                    dtrNewRow["cDelete"] = (bllIsDelete ? "Y" : "N");
                    dtrNewRow["dLastUpd"] = this.mSaveDBAgent.GetDBServerDateTime();
                    dtrNewRow["cLastUpdBy"] = App.FMAppUserID;

                    string strSQLUpdateStr = "";
                    object[] pAPara = null;
                    if (bllDefaIsEnter == bllIsEnter
                        && bllDefaIsInsert == bllIsInsert
                        && bllDefaIsUpdate == bllIsUpdate
                        && bllDefaIsDelete == bllIsDelete)
                    {
                        pAPara = new object[] { dtrNewRow["cRowID"].ToString() };
                        this.mSaveDBAgent.BatchSQLExec("delete from AppAuthC where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    }
                    else
                    {
                        cDBMSAgent.GenUpdateSQLString(dtrNewRow, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                        this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
                    }

                }

                this.mdbTran.Commit();

            }
            catch (Exception ex)
            {
                this.mdbTran.Rollback();
                App.WriteEventsLog(ex);
#if xd_RUNMODE_DEBUG
                MessageBox.Show("Message : " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
#endif
            }

            finally
            {
                this.mdbConn.Close();
            }
            UIBase.WaitClear();
            MessageBox.Show(this, "Save Complete", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
        }

        private void grdTemObj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && this.grdTemObj.Selection.Count > 0)
            {
                //MessageBox.Show(this.grdTemObj.Selection.Count.ToString());
                //MessageBox.Show(grdTemObj.FocusedColumn.FieldName);
                string strMsg = "";
                for (int i = 0; i < this.grdTemObj.Selection.Count; i++)
                {
                    DevExpress.XtraTreeList.Nodes.TreeListNode tn = this.grdTemObj.Selection[i];
                    //strMsg += tn.GetValue("cTaskName").ToString() + "\r\n";
                    bool bllIsEnter = Convert.ToBoolean(tn.GetValue("IsEnter"));
                    bool bllIsInsert = Convert.ToBoolean(tn.GetValue("IsInsert"));
                    bool bllIsUpdate = Convert.ToBoolean(tn.GetValue("IsUpdate"));
                    bool bllIsDelete = Convert.ToBoolean(tn.GetValue("IsDelete"));
                    bool bllIsApprove = Convert.ToBoolean(tn.GetValue("IsApprove"));
                    bool bllIsApprove2 = Convert.ToBoolean(tn.GetValue("IsApprove2"));

                    tn.SetValue("IsEnter", !bllIsEnter);
                    tn.SetValue("IsInsert", !bllIsInsert);
                    tn.SetValue("IsUpdate", !bllIsUpdate);
                    tn.SetValue("IsDelete", !bllIsDelete);
                    tn.SetValue("IsApprove", !bllIsApprove);
                    tn.SetValue("IsApprove2", !bllIsApprove2);

                }
                //MessageBox.Show(strMsg);
                //DevExpress.XtraTreeList.TreeListMultiSelection x = this.grdTemObj.Selection;
            }
        }

    }
}