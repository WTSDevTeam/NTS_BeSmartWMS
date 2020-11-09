
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
using AppUtil.SecureHelper;

using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Component;

namespace BeSmartMRP.DatabaseForms
{
    public partial class frmExportDB001 : UIHelper.frmBase
    {
        public frmExportDB001()
        {
            InitializeComponent();
        }

        protected internal DataSet dtsDataEnv = new DataSet("WBS");

        private void txtDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            dlgSaveFile1.ShowDialog(this);
            if (dlgSaveFile1.SelectedPath != string.Empty)
            {
                this.txtDir.Text = dlgSaveFile1.SelectedPath;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.pmExportData();
        }

        private void pmExportData()
        {
            pmSetBrowView_CUST();
            pmSetBrowView_SUPPL();
            pmSetBrowView_WHOUSE();
            pmSetBrowView_WHLoca();
            pmSetBrowView_Prod();

            if (this.txtDir.Text.Trim() != string.Empty && System.IO.Directory.Exists(this.txtDir.Text.Trim()))
            {
                this.dtsDataEnv.WriteXmlSchema(this.txtDir.Text + "\\WBS.XSD");
                this.dtsDataEnv.WriteXml(this.txtDir.Text + "\\WBS.XML");
                MessageBox.Show("Export Complete");
            }
            else
            {
                MessageBox.Show("ระบุ folder สำหรับ Export ข้อมูลไม่ถูกต้อง");
            }
        }

        private void pmSetBrowView_CUST()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strEmplRTab = strFMDBName + ".dbo.EMPLR";

            string strStat = "CSTATUS = case {0}.FCSTATUS when 'I' then 'INACTIVE' when '' then 'ACTIVE' end";

            string strSQLExec = "select {0}.FCSKID as FCSKID, {0}.FCCODE as CCODE, {0}.FCSNAME as CSNAME, {0}.FCNAME as CNAME, " + strStat + ", {0}.FDINACTIVE as DINACTIVE, {0}.FTDATETIME as DCREATE, {0}.FTLASTUPD as DLASTUPD from {0} ";
            strSQLExec += " where {0}.FCCORP = ? and {0}.FCISCUST = 'Y' ";
            strSQLExec = string.Format(strSQLExec, new string[] { "COOR" });

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { App.gcCorp });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "CUST", "CUST", strSQLExec, ref strErrorMsg);

        }

        private void pmSetBrowView_SUPPL()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strEmplRTab = strFMDBName + ".dbo.EMPLR";

            string strStat = "CSTATUS = case {0}.FCSTATUS when 'I' then 'INACTIVE' when '' then 'ACTIVE' end";

            string strSQLExec = "select {0}.FCSKID as FCSKID, {0}.FCCODE as CCODE, {0}.FCSNAME as CSNAME, {0}.FCNAME as CNAME, " + strStat + ", {0}.FDINACTIVE as DINACTIVE, {0}.FTDATETIME as DCREATE, {0}.FTLASTUPD as DLASTUPD from {0} ";
            strSQLExec += " where {0}.FCCORP = ? and {0}.FCISSUPP = 'Y' ";
            strSQLExec = string.Format(strSQLExec, new string[] { "COOR" });

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { App.gcCorp });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "SUPPL", "SUPPL", strSQLExec, ref strErrorMsg);

        }

        private void pmSetBrowView_WHOUSE()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLExec = "select BRANCH.FCCODE as QCBRANCH,EMWHOUSE.FCSKID, EMWHOUSE.FCCODE as CCODE, EMWHOUSE.FCNAME as CNAME, EMWHOUSE.FCNAME2 as CNAME2";
            strSQLExec += " , LC1.FCCODE as QCWHLOCA_INPUT, LC2.FCCODE as QCWHLOCA_OUTPUT, LC3.FCCODE as QCWHLOCA_STOCK ";
            strSQLExec += " , EMWHOUSE.FTLASTUPD as FDCREATE, EMWHOUSE.FTDATETIME as FDLASTUPDBY  ";
            strSQLExec += " from WHOUSE as EMWHOUSE ";
            strSQLExec += " left join BRANCH on BRANCH.FCSKID = EMWHOUSE.FCBRANCH ";
            strSQLExec += " left join WHLOCATION LC1 on LC1.FCSKID = EMWHOUSE.FCWHLOCA_INPUT ";
            strSQLExec += " left join WHLOCATION LC2 on LC2.FCSKID = EMWHOUSE.FCWHLOCA_OUTPUT ";
            strSQLExec += " left join WHLOCATION LC3 on LC3.FCSKID = EMWHOUSE.FCWHLOCA_STOCK ";
            strSQLExec += " where EMWHOUSE.FCCORP = ? ";

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID  });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "WHOUSE", "WHOUSE", strSQLExec, ref strErrorMsg);

        }

        private void pmSetBrowView_WHLoca()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            string strSQLExec = "select EMWHLOCA.FCSKID, EMWHLOCA.FCCODE as CCODE, EMWHLOCA.FCNAME as CNAME, EMWHLOCA.FCNAME2 as CNAME2, EMWHLOCA.FTLASTUPD as FDCREATE, EMWHLOCA.FTDATETIME as FDLASTUPDBY from {0} EMWHLOCA ";
            strSQLExec += " where EMWHLOCA.FCCORP = ? ";
            strSQLExec = string.Format(strSQLExec, new string[] { "WHLOCATION" });

            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID });
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "WHLOCATION", "WHLOCATION", strSQLExec, ref strErrorMsg);

        }

        private void pmSetBrowView_Prod()
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
            string strEmplRTab = strFMDBName + ".dbo.EMPLR";

            string strStat = "CSTATUS = case {0}.FCSTATUS when 'I' then 'INACTIVE' when '' then 'ACTIVE' end";

            //string strSQLExec = "select {0}.FCSKID as CROWID, {0}.FCCODE as CCODE, {0}.FCSNAME as CSNAME , {0}.FCNAME as CNAME, {0}.FNPRICE as NPRICE, " + strStat + ", {0}.FDINACTIVE as DINACTIVE, {0}.FCTYPE as CTYPE , {0}.FTDATETIME as DCREATE, {0}.FTLASTUPD as DLASTUPD from {0} ";
            string strSQLExec = "select {0}.FCSKID as CROWID, {0}.FCCODE as CCODE, {0}.FCSNAME as CSNAME , {0}.FCNAME as CNAME, {0}.FNPRICE as NPRICE, {0}.FNSTDCOST as STDCOST, " + strStat + ", {0}.FDINACTIVE as DINACTIVE, {0}.FCTYPE as CTYPE , {0}.FTDATETIME as DCREATE, {0}.FTLASTUPD as DLASTUPD ";
            strSQLExec += " , UM.FCCODE as QCUM,UM.FCNAME as QNUM,UM1.FCCODE as QCUM1 ,UM1.FCNAME as QNUM1,UM2.FCCODE as QCUM2 ,UM2.FCNAME as QNUM2,{0}.FNSTUMQTY1 ";
            strSQLExec += " from {0} ";
            strSQLExec += " left join UM on PROD.FCUM = UM.FCSKID";
            strSQLExec += " left join UM UM1 on PROD.FCUM1 = UM1.FCSKID";
            strSQLExec += " left join UM UM2 on PROD.FCSTUM1 = UM2.FCSKID";
            strSQLExec += " where {0}.FCCORP = ?";

            strSQLExec = string.Format(strSQLExec, new string[] { MapTable.Table.Product, "EMPLR" });

            pobjSQLUtil.SetPara(new object[] { App.gcCorp });
            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "PROD", "PROD", strSQLExec, ref strErrorMsg);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
