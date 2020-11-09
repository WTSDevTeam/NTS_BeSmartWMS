
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

namespace BeSmartMRP.DatabaseForms.Common
{

    public partial class dlgMfgFilter01 : UIHelper.frmBase
    {

        public dlgMfgFilter01(string inRefType)
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

        public string PlantID
        {
            get { return this.txtQcPlant.Tag.ToString(); }
        }

        public string PlantCode
        {
            get { return this.txtQcPlant.Text; }
        }

        public string PlantName
        {
            get { return this.txtQnPlant.Text; }
        }

        private DatabaseForms.frmEMPlant pofrmGetPlant = null;

        private string mstrRefType = "";

        private DataSet dtsDataEnv = new DataSet();

        private void pmInitForm()
        {

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strErrorMsg = "";

            objSQLHelper.SetPara(new object[] { App.ActiveCorp.RowID });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QPlant", QEMPlantInfo.TableName, "select cRowID, cCode, cName from " + QEMPlantInfo.TableName + " order by CCODE", ref strErrorMsg))
            {

                DataRow dtrPlant = this.dtsDataEnv.Tables["QPlant"].Rows[0];
                this.txtQcPlant.Tag = dtrPlant[QEMPlantInfo.Field.RowID].ToString();
                this.txtQcPlant.Text = dtrPlant[QEMPlantInfo.Field.Code].ToString().TrimEnd();
                this.txtQnPlant.Text = dtrPlant[QEMPlantInfo.Field.Name].ToString().TrimEnd();

            }

            this.txtQcPlant.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtPopUp_ButtonClick);
            this.txtQcPlant.KeyDown += new KeyEventHandler(txtPopUp_KeyDown);
            this.txtQcPlant.Validating += new CancelEventHandler(txtQcPlant_Validating);

            UIBase.SetDefaultChildAppreance(this);

        }

        private void pmInitPopUpDialog(string inDialogName)
        {
            switch (inDialogName.TrimEnd().ToUpper())
            {
                case "PLANT":
                    if (this.pofrmGetPlant == null)
                    {
                        this.pofrmGetPlant = new frmEMPlant(FormActiveMode.PopUp);
                        this.pofrmGetPlant.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPlant.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
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
                case "TXTQCPLANT":
                case "TXTQNPLANT":
                    this.pmInitPopUpDialog("PLANT");
                    strOrderBy = (inTextbox == "TXTQCPLANT" ? "CCODE" : "CNAME");
                    this.pofrmGetPlant.ValidateField(inPara1, strOrderBy, true);
                    if (this.pofrmGetPlant.PopUpResult)
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
                case "TXTQCPLANT":
                case "TXTQNPLANT":
                    if (this.pofrmGetPlant != null)
                    {
                        DataRow dtrPlant = this.pofrmGetPlant.RetrieveValue();

                        if (dtrPlant != null)
                        {
                            if (this.txtQcPlant.Tag.ToString() != dtrPlant[QEMPlantInfo.Field.RowID].ToString())
                            {
                            }

                            this.txtQcPlant.Tag = dtrPlant[QEMPlantInfo.Field.RowID].ToString();
                            this.txtQcPlant.Text = dtrPlant[QEMPlantInfo.Field.Code].ToString().TrimEnd();
                            this.txtQnPlant.Text = dtrPlant[QEMPlantInfo.Field.Name].ToString().TrimEnd();
                        }
                        else
                        {
                            this.txtQcPlant.Tag = "";
                            this.txtQcPlant.Text = "";
                            this.txtQnPlant.Text = "";
                        }
                    }
                    break;
            }
        }

        private void txtQcPlant_Validating(object sender, CancelEventArgs e)
        {
            DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
            string strOrderBy = txtPopUp.Name.ToUpper() == "TXTQCPLANT" ? "CCODE" : "CNAME";

            if (txtPopUp.Text == "")
            {
                this.txtQcPlant.Tag = "";
                this.txtQcPlant.Text = "";
                this.txtQnPlant.Text = "";
            }
            else
            {
                this.pmInitPopUpDialog("PLANT");
                e.Cancel = !this.pofrmGetPlant.ValidateField(txtPopUp.Text, strOrderBy, false);
                if (this.pofrmGetPlant.PopUpResult)
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
