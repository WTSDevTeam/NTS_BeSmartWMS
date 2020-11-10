
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Threading;

using BeSmartMRP.DialogForms;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;

namespace BeSmartMRP
{

    public partial class frmMainmenu : DevExpress.XtraEditors.XtraForm
    {

        public frmMainmenu()
        {

            InitializeComponent();

            //dtsDataEnv = new DataSet();
            //pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            //this.pmGenMenu();
        
        }

        private DataSet dtsDataEnv = null;
        private WS.Data.Agents.cDBMSAgent pobjSQLUtil = null;

        private void pmGenMenu()
        {

            this.toolStripStatusLabel1.Text = "USER : " + App.AppUserName;

            this.barManager1.Items.Clear();
            DevExpress.XtraBars.BarSubItem oNode = this.pmGetMenuBar(UIBase.GetAppUIText(new string[] { "ระบบงาน", "&Application" }), "", true);
            this.barManager1.Items.Add(oNode);
            this.bar2.LinksPersistInfo.Add(new DevExpress.XtraBars.LinkPersistInfo(oNode));

            DevExpress.XtraBars.BarButtonItem oMenuItem = this.pmGetMenuItem(UIBase.GetAppUIText(new string[] { "ออกจากระบบงาน", "Exit" }), "EXIT");
            this.barManager1.Items.Add(oMenuItem);
            oNode.LinksPersistInfo.Add(new DevExpress.XtraBars.LinkPersistInfo(oMenuItem, false));

            this.barManager1.BeginInit();

            string strErrorMsg = "";
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QAppObj", "APPOBJ", "select * from APPOBJ where CTYPE in ('CATEGORY') and CVISIBLE <> 'N' order by CSEQ ", ref strErrorMsg))
            //if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QAppObj", "APPOBJ", "select * from APPOBJ where CMODULE = 'SM' and CTYPE in ('CATEGORY') and CVISIBLE <> 'N' order by CSEQ ", ref strErrorMsg))
            {
                //this.bar2.BeginUpdate();
                foreach (DataRow dtrMenu in dtsDataEnv.Tables["QAppObj"].Rows)
                {
                    string strDesc = UIBase.GetAppUIText(new string[] { dtrMenu["cDesc"].ToString().Trim(), dtrMenu["cDesc2"].ToString().Trim() });
                    DevExpress.XtraBars.BarSubItem oBar = this.pmGetMenuBar(strDesc, dtrMenu["cTaskName"].ToString().TrimEnd() , true);
                    this.barManager1.Items.Add(oBar);
                    this.bar2.LinksPersistInfo.Add(new DevExpress.XtraBars.LinkPersistInfo(oBar));
                    this.pmLoopSubItem(oBar, dtrMenu["cSeq"].ToString().TrimEnd());
                    int i = oBar.LinksPersistInfo.Count;
                    oBar.Visibility = (oBar.LinksPersistInfo.Count > 0 ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never);
                }
                //this.bar2.EndUpdate();
            }

            //foreach (DevExpress.XtraBars.BarItem oBar1 in this.barManager1.Items)
            //{
            //    DevExpress.XtraBars.BarSubItem oBar2 = oBar1 as DevExpress.XtraBars.BarSubItem;
            //    if (oBar2 != null)
            //    {
            //        int intItems = this.pmCountMenuItem(oBar2);
            //        oBar2.Visibility = (intItems > 0 ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never);
            //    }
            //}

            this.barManager1.EndInit();
        
        }

        private int pmCountMenuItem(DevExpress.XtraBars.BarSubItem inBar)
        {
            int intCount = 0;
            for (int i = 0; i < inBar.LinksPersistInfo.Count; i++)
            {
                DevExpress.XtraBars.LinkPersistInfo lk = inBar.LinksPersistInfo[i];

                DevExpress.XtraBars.BarSubItem oBar2 = lk.Item as DevExpress.XtraBars.BarSubItem;
                if (oBar2 != null)
                {
                    if (oBar2.Links.Count > 0) intCount++;
                }
                else
                {
                    intCount++;
                }

            }
            return intCount;
        }

        private void pmLoopSubItem(DevExpress.XtraBars.BarSubItem inBar, string inSeq)
        {

            string strErrorMsg = "";

            string strTabName = "QAppObj_" + inSeq;
            pobjSQLUtil.SetPara(new object[] {inSeq});
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, strTabName, "APPOBJ", "select * from APPOBJ where CPRSEQ = ? and CVISIBLE <> 'N' order by CSEQ ", ref strErrorMsg))
            {
                foreach (DataRow dtrMenu in dtsDataEnv.Tables[strTabName].Rows)
                {
                    bool bllIsGroup = (dtrMenu["cInGroup"].ToString().Trim() == "Y" ? true : false);
                    if (dtrMenu["CTYPE"].ToString().Trim() == "NODE")
                    {
                        string strDesc = UIBase.GetAppUIText(new string[] { dtrMenu["cDesc"].ToString().Trim(), dtrMenu["cDesc2"].ToString().Trim() });
                        DevExpress.XtraBars.BarSubItem oNode = this.pmGetMenuBar(strDesc, dtrMenu["cTaskName"].ToString().TrimEnd(), false);
                        this.barManager1.Items.Add(oNode);
                        inBar.LinksPersistInfo.Add(new DevExpress.XtraBars.LinkPersistInfo(oNode, bllIsGroup));
                        this.pmLoopSubItem(oNode, dtrMenu["cSeq"].ToString().TrimEnd());
                    }
                    else
                    {
                        bool bllVisible = true;
                        if (dtrMenu["CTYPE"].ToString().Trim() == "MENU"
                            && !App.PermissionManager.CheckPermission(dtrMenu["cTaskName"].ToString().TrimEnd(), AuthenType.CanAccess, App.AppUserName, App.AppUserID))
                        {
                            bllVisible = false;
                        }

                        if (bllVisible)
                        {
                            string strDesc = UIBase.GetAppUIText(new string[] { dtrMenu["cDesc"].ToString().Trim(), dtrMenu["cDesc2"].ToString().Trim() });
                            DevExpress.XtraBars.BarButtonItem oMenuItem = this.pmGetMenuItem(strDesc, dtrMenu["cTaskName"].ToString().TrimEnd());
                            this.barManager1.Items.Add(oMenuItem);
                            inBar.LinksPersistInfo.Add(new DevExpress.XtraBars.LinkPersistInfo(oMenuItem, bllIsGroup));
                        }
                    }
                }
            }

        }

        private DevExpress.XtraBars.BarSubItem pmGetMenuBar(string inText, string inTag, bool inIsBold)
        {
            DevExpress.XtraBars.BarSubItem oBar = new DevExpress.XtraBars.BarSubItem();
            oBar.Caption = inText;
            oBar.Tag = inTag;
            oBar.Appearance.Font = new System.Drawing.Font("Tahoma", 9, (inIsBold ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular));
            return oBar;
        }

        private DevExpress.XtraBars.BarButtonItem pmGetMenuItem(string inText, string inTag)
        {
            DevExpress.XtraBars.BarButtonItem oBar = new DevExpress.XtraBars.BarButtonItem();
            oBar.Caption = inText;
            oBar.Tag = inTag;
            oBar.Appearance.Font = new System.Drawing.Font("Tahoma", 9, System.Drawing.FontStyle.Regular);
            return oBar;
        }

        private void barManager1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!(e.Item is DevExpress.XtraBars.BarSubItem))
            { 
            }

            if (e.Item.Tag != null)
            {

                string strMenuName = e.Item.Tag.ToString().Trim().ToUpper();
                if (!(e.Item is DevExpress.XtraBars.BarSubItem)
                    && App.AppUserName.TrimEnd() != "BIGBOSS"
                    && !this.pmIsSkipMenu(strMenuName))
                {

                    if (!App.PermissionManager.CheckPermission(strMenuName.ToUpper(), AuthenType.CanAccess, App.AppUserName, App.AppUserID))
                    {
                        MessageBox.Show("ไม่มีสิทธิ์ในการใช้งาน !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                this.pmRunMenuInput1(e.Item.Tag.ToString(), e.Item.Caption);
                //this.pmRunMenuOutput1(e.Item.Tag.ToString(), e.Item.Caption);
                //this.pmRunMenuInput2(e.Item.Tag.ToString(), e.Item.Caption);
                //this.pmRunMenuOutput2(e.Item.Tag.ToString(), e.Item.Caption);
                this.pmRunMenuReportSM(e.Item.Tag.ToString(), e.Item.Caption);

            }
        }

        private bool pmIsSkipMenu(string inMenuName)
        {
            bool bllIsSkipMenu = false;
            switch (inMenuName)
            { 
                case "START":
                case "DBCONFIG":
                case "EXIT":
                    bllIsSkipMenu = true;
                    break;
            }
            return bllIsSkipMenu;
        }

        System.Collections.ArrayList aTask = new System.Collections.ArrayList();

        private void pmRunMenuInput1(string inTag, string inMenuName)
		{
            switch (inTag.ToUpper())
            {
                case "START":
                    //frmStartPage ofrmStartPage = frmStartPage.GetInstanse();
                    //ofrmStartPage.MdiParent = this;
                    //ofrmStartPage.Show();
                    //ofrmStartPage.Activate();
                    break;
                case "EXIT":
                    this.Close();
                    break;
                case "DBCONFIG":
                    App.SetAppConfig();
                    break;

                case "EPDCAP01":
                    DatabaseForms.Modify.JENA_001.frmSetResourceCap ofrmDemo01 = new BeSmartMRP.DatabaseForms.Modify.JENA_001.frmSetResourceCap();

                    ofrmDemo01.MdiParent = this;
                    ofrmDemo01.Show();
                    ofrmDemo01.Activate();
                    break;

                case "EMFGPP01":
                    //DatabaseForms.Modify.JENA_001.frmMFPlan01 ofrmMFPlan01 = new BeSmartMRP.DatabaseForms.Modify.JENA_001.frmMFPlan01();
                    Transaction.frmMFGPlanning01 ofrmMFPlan01 = new Transaction.frmMFGPlanning01();
                    if (ofrmMFPlan01.DialogResult == DialogResult.OK)
                    {
                        ofrmMFPlan01.Show();
                        ofrmMFPlan01.Activate();
                    }
                    else
                    {
                        ofrmMFPlan01.Dispose();
                    }

                    break;

                case "SETRIGHT":
                    //Permission.frmSetPermission ofrmSetPermission = new Permission.frmSetPermission();
                    //ofrmSetPermission.MdiParent = this;
                    //ofrmSetPermission.Show();
                    //ofrmSetPermission.Activate();
                    break;

                case "ECORP":
                    DatabaseForms.frmCorp ofrmCorp = BeSmartMRP.DatabaseForms.frmCorp.GetInstanse();
                    ofrmCorp.MdiParent = this;
                    ofrmCorp.Show();
                    ofrmCorp.Activate();
                    break;
                case "EBRANCH":
                    DatabaseForms.frmBranch ofrmBranch = BeSmartMRP.DatabaseForms.frmBranch.GetInstanse();
                    
                    ofrmBranch.MdiParent = this;
                    ofrmBranch.Show();
                    ofrmBranch.Activate();
                    break;
                case "EPLANT":
                    DatabaseForms.frmEMPlant ofrmEMPlant = BeSmartMRP.DatabaseForms.frmEMPlant.GetInstanse();

                    ofrmEMPlant.MdiParent = this;
                    ofrmEMPlant.Show();
                    ofrmEMPlant.Activate();
                    break;
                case "EDEPT":
                    DatabaseForms.frmEMDept ofrmDept = BeSmartMRP.DatabaseForms.frmEMDept.GetInstanse();

                    ofrmDept.MdiParent = this;
                    ofrmDept.Show();
                    ofrmDept.Activate();
                    break;

                case "ESECT":
                    DatabaseForms.frmEMSect ofrmEMSect = BeSmartMRP.DatabaseForms.frmEMSect.GetInstanse();

                    ofrmEMSect.MdiParent = this;
                    ofrmEMSect.Show();
                    ofrmEMSect.Activate();
                    break;

                case "EMFBOM":
                    DatabaseForms.frmMFBOM ofrmBOM = BeSmartMRP.DatabaseForms.frmMFBOM.GetInstanse();
                    ofrmBOM.MdiParent = this;
                    ofrmBOM.Show();
                    ofrmBOM.Activate();
                    break;

                case "EPROD":

                    //Transaction.frmOrder ofrmOrder = new Transaction.frmOrder("PO");
                    //if (ofrmOrder.DialogResult == DialogResult.OK)
                    //{
                    //    ofrmOrder.Show();
                    //    ofrmOrder.Activate();
                    //}
                    //else
                    //{
                    //    ofrmOrder.Dispose();
                    //}

                    DatabaseForms.frmProd ofrmProd = BeSmartMRP.DatabaseForms.frmProd.GetInstanse();
                    ofrmProd.MdiParent = this;
                    ofrmProd.Show();
                    ofrmProd.Activate();
                    break;

                case "EPDGRP":
                    DatabaseForms.frmPdCateg ofrmPdCateg = BeSmartMRP.DatabaseForms.frmPdCateg.GetInstanse();
                    ofrmPdCateg.MdiParent = this;
                    ofrmPdCateg.Show();
                    ofrmPdCateg.Activate();

                    break;

                case "EPDBARCODE":
                    DatabaseForms.frmPdBarcode ofrmPdBarcode = BeSmartMRP.DatabaseForms.frmPdBarcode.GetInstanse();
                    ofrmPdBarcode.MdiParent = this;
                    ofrmPdBarcode.Show();
                    ofrmPdBarcode.Activate();

                    break;

                case "EWHOUSE":

                    DatabaseForms.frmWHouse ofrmWHouse = BeSmartMRP.DatabaseForms.frmWHouse.GetInstanse();
                    ofrmWHouse.MdiParent = this;

                    if (ofrmWHouse.DialogResult == DialogResult.OK)
                    {
                        ofrmWHouse.Show();
                        ofrmWHouse.Activate();
                    }
                    else
                    {
                        ofrmWHouse.Dispose();
                    }

                    break;
                case "EUOM":
                    DatabaseForms.frmEMUOM ofrmEMUOM = BeSmartMRP.DatabaseForms.frmEMUOM.GetInstanse();
                    ofrmEMUOM.MdiParent = this;
                    ofrmEMUOM.Show();
                    ofrmEMUOM.Activate();
                    break;

                case "EWHLOCA":
                    DatabaseForms.frmWHLocation ofrmWHLocation = DatabaseForms.frmWHLocation.GetInstanse();
                    ofrmWHLocation.MdiParent = this;
                    ofrmWHLocation.Show();
                    ofrmWHLocation.Activate();
                    break;

                case "ESUPPL":
                    DatabaseForms.frmEMSuppl ofrmEMSuppl = DatabaseForms.frmEMSuppl.GetInstanse();
                    ofrmEMSuppl.MdiParent = this;
                    ofrmEMSuppl.Show();
                    ofrmEMSuppl.Activate();
                    break;

                case "ECOOR":
                    DatabaseForms.frmCust ofrmCust = DatabaseForms.frmCust.GetInstanse();
                    ofrmCust.MdiParent = this;
                    ofrmCust.Show();
                    ofrmCust.Activate();
                    break;
                case "ECRGRP":
                    DatabaseForms.frmCrGrp ofrmCrGrp = DatabaseForms.frmCrGrp.GetInstanse(CoorType.Customer);
                    ofrmCrGrp.MdiParent = this;
                    ofrmCrGrp.Show();
                    ofrmCrGrp.Activate();
                    break;
                case "ESPGRP":
                    DatabaseForms.frmCrGrp ofrmCrGrp1 = DatabaseForms.frmCrGrp.GetInstanse(CoorType.Supplier);
                    ofrmCrGrp1.MdiParent = this;
                    ofrmCrGrp1.Show();
                    ofrmCrGrp1.Activate();
                    break;
                case "EWHPACK":
                    DatabaseForms.frmWHPack ofrmWHPack = DatabaseForms.frmWHPack.GetInstanse();
                    ofrmWHPack.MdiParent = this;
                    ofrmWHPack.Show();
                    ofrmWHPack.Activate();
                    break;

                case "ERESTYPE_M":
                    DatabaseForms.frmMFResType ofrmResT1 = BeSmartMRP.DatabaseForms.frmMFResType.GetInstanse(MfgResourceType.Machine);
                    ofrmResT1.MdiParent = this;
                    ofrmResT1.Show();
                    ofrmResT1.Activate();
                    break;

                case "ERESTYPE_T":
                    DatabaseForms.frmMFResType ofrmResT2 = BeSmartMRP.DatabaseForms.frmMFResType.GetInstanse(MfgResourceType.Tool);
                    ofrmResT2.MdiParent = this;
                    ofrmResT2.Show();
                    ofrmResT2.Activate();
                    break;

                case "ERESOURCE_M":
                    DatabaseForms.frmMFResource ofrmResM1 = BeSmartMRP.DatabaseForms.frmMFResource.GetInstanse(MfgResourceType.Machine);
                    ofrmResM1.MdiParent = this;
                    ofrmResM1.Show();
                    ofrmResM1.Activate();
                    break;

                case "ERESOURCE_T":
                    DatabaseForms.frmMFResource ofrmResM2 = BeSmartMRP.DatabaseForms.frmMFResource.GetInstanse(MfgResourceType.Tool);
                    ofrmResM2.MdiParent = this;
                    ofrmResM2.Show();
                    ofrmResM2.Activate();
                    break;

                case "EMSTER01":
                    DatabaseForms.frmMfgCost ofrmResMP = BeSmartMRP.DatabaseForms.frmMfgCost.GetInstanse(MfgResourceType.CostM1);
                    ofrmResMP.MdiParent = this;
                    ofrmResMP.Show();
                    ofrmResMP.Activate();
                    break;

                case "EMSTER02":
                    DatabaseForms.frmMfgCost ofrmResMQ = BeSmartMRP.DatabaseForms.frmMfgCost.GetInstanse(MfgResourceType.CostM2);
                    ofrmResMQ.MdiParent = this;
                    ofrmResMQ.Show();
                    ofrmResMQ.Activate();
                    break;

                case "EMSTER03":
                    DatabaseForms.frmMfgCost ofrmResME = BeSmartMRP.DatabaseForms.frmMfgCost.GetInstanse(MfgResourceType.CostM3);
                    ofrmResME.MdiParent = this;
                    ofrmResME.Show();
                    ofrmResME.Activate();
                    break;

                case "EMSTER04":
                    DatabaseForms.frmMfgCost ofrmResMF = BeSmartMRP.DatabaseForms.frmMfgCost.GetInstanse(MfgResourceType.CostM4);
                    ofrmResMF.MdiParent = this;
                    ofrmResMF.Show();
                    ofrmResMF.Activate();
                    break;

                case "EMSTER05":
                    DatabaseForms.frmMfgCost ofrmResMG = BeSmartMRP.DatabaseForms.frmMfgCost.GetInstanse(MfgResourceType.CostM5);
                    ofrmResMG.MdiParent = this;
                    ofrmResMG.Show();
                    ofrmResMG.Activate();
                    break;

                case "ESTDOPR":
                    DatabaseForms.frmMFStdOpr ofrmMFStdOpr = BeSmartMRP.DatabaseForms.frmMFStdOpr.GetInstanse();
                    ofrmMFStdOpr.MdiParent = this;
                    ofrmMFStdOpr.Show();
                    ofrmMFStdOpr.Activate();
                    break;

                case "EMFWKCTR":
                    DatabaseForms.frmMFWorkCenter ofrmMFWorkCenter = BeSmartMRP.DatabaseForms.frmMFWorkCenter.GetInstanse();
                    ofrmMFWorkCenter.MdiParent = this;
                    ofrmMFWorkCenter.Show();
                    ofrmMFWorkCenter.Activate();
                    break;

                case "EMFGBOOK":

                    //DatabaseForms.frmMfgBook ofrmMfgBook = BeSmartMRP.DatabaseForms.frmMfgBook.GetInstanse();
                    DatabaseForms.frmFMBook ofrmMfgBook = BeSmartMRP.DatabaseForms.frmFMBook.GetInstanse();
                    ofrmMfgBook.MdiParent = this;

                    if (ofrmMfgBook.DialogResult == DialogResult.OK)
                    {
                        ofrmMfgBook.Show();
                        ofrmMfgBook.Activate();
                    }
                    else
                    {
                        ofrmMfgBook.Dispose();
                    }

                    break;

                case "EORDER_PO":

                        Transaction.frmOrder ofrmOrder = new Transaction.frmOrder("PO");
                        if (ofrmOrder.DialogResult == DialogResult.OK)
                        {
                            ofrmOrder.Show();
                            ofrmOrder.Activate();
                        }
                        else
                        {
                            ofrmOrder.Dispose();
                        }
                        break;
                case "EORDER_SO":

                        Transaction.frmOrder ofrmOrder2 = new Transaction.frmOrder("SO");
                        if (ofrmOrder2.DialogResult == DialogResult.OK)
                        {
                            ofrmOrder2.Show();
                            ofrmOrder2.Activate();
                        }
                        else
                        {
                            ofrmOrder2.Dispose();
                        }
                        break;
                case "EORDER_QS":

                    Transaction.frmOrder ofrmOrder3 = new Transaction.frmOrder("QS");
                    if (ofrmOrder3.DialogResult == DialogResult.OK)
                    {
                        ofrmOrder3.Show();
                        ofrmOrder3.Activate();
                    }
                    else
                    {
                        ofrmOrder3.Dispose();
                    }
                    break;
                case "EMORDER":

                    Transaction.frmMFWOrder ofrmMFWOrder = BeSmartMRP.Transaction.frmMFWOrder.GetInstanse();
                    if (ofrmMFWOrder.DialogResult == DialogResult.OK)
                    {
                        ofrmMFWOrder.Show();
                        ofrmMFWOrder.Activate();
                    }
                    else
                    {
                        ofrmMFWOrder.Dispose();
                    }

                    break;

                case "EROUTECD":

                    Transaction.frmMFRoute ofrmMFRoute = BeSmartMRP.Transaction.frmMFRoute.GetInstanse();
                    if (ofrmMFRoute.DialogResult == DialogResult.OK)
                    {
                        ofrmMFRoute.Show();
                        ofrmMFRoute.Activate();
                    }
                    else
                    {
                        ofrmMFRoute.Dispose();
                    }

                    break;

                case "EMCLOSE":

                    Transaction.frmMFWClose ofrmMFWClose = BeSmartMRP.Transaction.frmMFWClose.GetInstanse();
                    if (ofrmMFWClose.DialogResult == DialogResult.OK)
                    {
                        ofrmMFWClose.Show();
                        ofrmMFWClose.Activate();
                    }
                    else
                    {
                        ofrmMFWClose.Dispose();
                    }

                    break;

                case "EGENFGCS":

                    Transaction.frmRecalFGPrice ofrmRecalFGPrice = new BeSmartMRP.Transaction.frmRecalFGPrice();

                    string strReportName = AppUtil.StringHelper.SubStr(inMenuName, AppUtil.StringHelper.RAt(".", inMenuName) + 1);

                    ofrmRecalFGPrice.SetTitle(inTag, strReportName, inTag);
                    ofrmRecalFGPrice.MdiParent = this;
                    ofrmRecalFGPrice.Show();
                    ofrmRecalFGPrice.Activate();

                    break;

                case "EADJ":

                    Transaction.frmAdj ofrmAdj = BeSmartMRP.Transaction.frmAdj.GetInstanse();
                    if (ofrmAdj.DialogResult == DialogResult.OK)
                    {
                        ofrmAdj.Show();
                        ofrmAdj.Activate();
                    }
                    else
                    {
                        ofrmAdj.Dispose();
                    }

                    break;

                case "ESTMREQ":

                    Transaction.frmStReq ofrmStReq = BeSmartMRP.Transaction.frmStReq.GetInstanse(DocumentType.Q1);
                    if (ofrmStReq.DialogResult == DialogResult.OK)
                    {
                        ofrmStReq.Show();
                        ofrmStReq.Activate();
                    }
                    else
                    {
                        ofrmStReq.Dispose();
                    }

                    break;
                case "ESTMOVE_WR":

                    Transaction.frmStmove ofrmStmove = BeSmartMRP.Transaction.frmStmove.GetInstanse(DocumentType.WR);
                    if (ofrmStmove.DialogResult == DialogResult.OK)
                    {
                        ofrmStmove.Show();
                        ofrmStmove.Activate();
                    }
                    else
                    {
                        ofrmStmove.Dispose();
                    }

                    break;

                case "ESTMOVE_RW":

                    Transaction.frmStmove ofrmStmoveRW = BeSmartMRP.Transaction.frmStmove.GetInstanse(DocumentType.RW);
                    if (ofrmStmoveRW.DialogResult == DialogResult.OK)
                    {
                        ofrmStmoveRW.Show();
                        ofrmStmoveRW.Activate();
                    }
                    else
                    {
                        ofrmStmoveRW.Dispose();
                    }

                    break;

                case "ESTMOVE_FR":

                    Transaction.frmStmove ofrmStmoveFR = BeSmartMRP.Transaction.frmStmove.GetInstanse(DocumentType.FR);
                    if (ofrmStmoveFR.DialogResult == DialogResult.OK)
                    {
                        ofrmStmoveFR.Show();
                        ofrmStmoveFR.Activate();
                    }
                    else
                    {
                        ofrmStmoveFR.Dispose();
                    }

                    break;

                case "ESTMOVE_TR":

                    Transaction.frmStmove ofrmStmoveTR = BeSmartMRP.Transaction.frmStmove.GetInstanse(DocumentType.TR);
                    if (ofrmStmoveTR.DialogResult == DialogResult.OK)
                    {
                        ofrmStmoveTR.Show();
                        ofrmStmoveTR.Activate();
                    }
                    else
                    {
                        ofrmStmoveTR.Dispose();
                    }

                    break;

                case "ESTMOVE_GD":

                    Transaction.frmStmove ofrmStmoveGD = BeSmartMRP.Transaction.frmStmove.GetInstanse(DocumentType.GD);
                    if (ofrmStmoveGD.DialogResult == DialogResult.OK)
                    {
                        ofrmStmoveGD.Show();
                        ofrmStmoveGD.Activate();
                    }
                    else
                    {
                        ofrmStmoveGD.Dispose();
                    }

                    break;
                case "ESTK_BAL1":

                    //Transaction.frmStmove ofrmStmoveTR2 = BeSmartMRP.Transaction.frmStmove.GetInstanse(DocumentType.TR);
                    //if (ofrmStmoveTR2.DialogResult == DialogResult.OK)
                    //{
                    //    ofrmStmoveTR2.Show();
                    //    ofrmStmoveTR2.Activate();
                    //}
                    //else
                    //{
                    //    ofrmStmoveTR2.Dispose();
                    //}

                Transaction.frmStkBal01 ofrmStkBal01 = new Transaction.frmStkBal01(false);
                    ofrmStkBal01.Show();
                    ofrmStkBal01.Activate();

                    break;

                case "INV_SI":

                    Transaction.frmInvoice ofrmInvSI = new Transaction.frmInvoice("SI");
                    if (ofrmInvSI.DialogResult == DialogResult.OK)
                    {
                        ofrmInvSI.Show();
                        ofrmInvSI.Activate();
                    }
                    else
                    {
                        ofrmInvSI.Dispose();
                    }
                    break;

                case "EJOB":
                    DatabaseForms.frmEMJob ofrmJob = BeSmartMRP.DatabaseForms.frmEMJob.GetInstanse();
                    ofrmJob.MdiParent = this;
                    ofrmJob.Show();
                    ofrmJob.Activate();
                    break;
                case "EXPDB001":
                    DatabaseForms.frmExportDB001 ofrmExpDB01 = new DatabaseForms.frmExportDB001();
                    //ofrmExpDB01.MdiParent = this;
                    ofrmExpDB01.Show();
                    //ofrmExpDB01.Activate();
                    break;
                case "EPROJ":
                    //DatabaseForms.frmEMProj ofrmProj = BeSmartMRP.DatabaseForms.frmEMProj.GetInstanse();
                    //ofrmProj.MdiParent = this;
                    //ofrmProj.Show();
                    //ofrmProj.Activate();

                    System.Runtime.Remoting.ObjectHandle oHandle;
                    UIHelper.IAppMenu ofrm = null;

                    oHandle = Activator.CreateInstance("BeSmartMRP", "BeSmartMRP.DatabaseForms.frmEMProj");
                    ofrm = (UIHelper.IAppMenu)oHandle.Unwrap();
                    ofrm.RunMenu(this, null);

                    //DatabaseForms.frmEMProj ofrmProj = BeSmartMRP.DatabaseForms.frmEMProj.GetInstanse();
                    //ofrmProj.MdiParent = this;
                    //ofrmProj.Show();
                    //ofrmProj.Activate();

                    break;

                case "EHOLIDAY":

                    //System.Runtime.Remoting.ObjectHandle oHandle1;
                    //UIHelper.IAppMenu ofrm1 = null;

                    //oHandle1 = Activator.CreateInstance("BeSmartMRP", "BeSmartMRP.DatabaseForms.frmEMHoliday",);
                    //ofrm1 = (UIHelper.IAppMenu)oHandle1.Unwrap();
                    //ofrm1.RunMenu(this, null);

                    BeSmartMRP.DatabaseForms.frmEMHoliday ofrmEMHoliday = BeSmartMRP.DatabaseForms.frmEMHoliday.GetInstanse();
                    ofrmEMHoliday.MdiParent = this;
                    ofrmEMHoliday.Show();
                    ofrmEMHoliday.Activate();

                    break;
                case "EGENWKCAL":
                    //
                    BeSmartMRP.DatabaseForms.frmGenWkCalendar ofrmGenWkCalendar = BeSmartMRP.DatabaseForms.frmGenWkCalendar.GetInstanse();

                    ofrmGenWkCalendar.SetTitle(inTag, AppUtil.StringHelper.SubStr(inMenuName, AppUtil.StringHelper.RAt(".", inMenuName) + 1), inTag);
                    ofrmGenWkCalendar.MdiParent = this;
                    ofrmGenWkCalendar.Show();
                    ofrmGenWkCalendar.Activate();

                    break;
                case "ESTDWKHR":
                    BeSmartMRP.DatabaseForms.frmEMStdWorkHour ofrmEMStdWorkHour = BeSmartMRP.DatabaseForms.frmEMStdWorkHour.GetInstanse();
                    ofrmEMStdWorkHour.MdiParent = this;
                    ofrmEMStdWorkHour.Show();
                    ofrmEMStdWorkHour.Activate();

                    break;
                case "EWKCAL":
                    BeSmartMRP.DatabaseForms.frmWkCalendar ofrmWkCalendar = BeSmartMRP.DatabaseForms.frmWkCalendar.GetInstanse();
                    ofrmWkCalendar.MdiParent = this;
                    ofrmWkCalendar.Show();
                    ofrmWkCalendar.Activate();

                    break;
                case "EBGTYPE":
                    DatabaseForms.frmBudType ofrmBudType = BeSmartMRP.DatabaseForms.frmBudType.GetInstanse();
                    ofrmBudType.MdiParent = this;
                    ofrmBudType.Show();
                    ofrmBudType.Activate();
                    break;
                case "EBGCHART":
                    DatabaseForms.frmBudChart ofrmBudChart = BeSmartMRP.DatabaseForms.frmBudChart.GetInstanse();
                    ofrmBudChart.MdiParent = this;
                    ofrmBudChart.Show();
                    ofrmBudChart.Activate();
                    break;
                case "EBGALLOC":
                    DatabaseForms.frmBudAllocate ofrmBudAllocate = BeSmartMRP.DatabaseForms.frmBudAllocate.GetInstanse();

                    ofrmBudAllocate.MdiParent = this;
                    if (ofrmBudAllocate.DialogResult == DialogResult.OK)
                    {
                        ofrmBudAllocate.Show();
                        ofrmBudAllocate.Activate();
                    }
                    else
                    {
                        ofrmBudAllocate.Dispose();
                    }
                    break;
                case "EACCHART":
                    DatabaseForms.frmEMAcChart ofrmEMAcChart = BeSmartMRP.DatabaseForms.frmEMAcChart.GetInstanse();
                    ofrmEMAcChart.MdiParent = this;
                    ofrmEMAcChart.Show();
                    ofrmEMAcChart.Activate();
                    break;

                case "EAPPEMPL":
                    DatabaseForms.frmAppEmpl ofrmAppEmpl = BeSmartMRP.DatabaseForms.frmAppEmpl.GetInstanse();
                    ofrmAppEmpl.MdiParent = this;
                    ofrmAppEmpl.Show();
                    ofrmAppEmpl.Activate();
                    break;
                case "EAPPROLE":
                    DatabaseForms.frmAppRole ofrmAppRole = BeSmartMRP.DatabaseForms.frmAppRole.GetInstanse();
                    ofrmAppRole.MdiParent = this;
                    ofrmAppRole.Show();
                    ofrmAppRole.Activate();
                    break;
                case "EAPPLOGIN":
                    DatabaseForms.frmAppLogin ofrmAppLogin = BeSmartMRP.DatabaseForms.frmAppLogin.GetInstanse();
                    ofrmAppLogin.MdiParent = this;
                    ofrmAppLogin.Show();
                    ofrmAppLogin.Activate();
                    break;
                case "EBGBOOK":
                    DatabaseForms.frmBGBook ofrmBGBook = BeSmartMRP.DatabaseForms.frmBGBook.GetInstanse();
                    ofrmBGBook.MdiParent = this;
                    if (ofrmBGBook.DialogResult == DialogResult.OK)
                    {
                        ofrmBGBook.Show();
                        ofrmBGBook.Activate();
                    }
                    else
                    {
                        ofrmBGBook.Dispose();
                    }

                    break;

                case "ESETAUTHEN":
                    Permission.frmSetPermission ofrmSetPermission = new BeSmartMRP.Permission.frmSetPermission();
                    ofrmSetPermission.MdiParent = this;
                    ofrmSetPermission.Show();
                    ofrmSetPermission.Activate();
                    break;

                case "ESETAUTH_X1" :
                    Permission.frmAuth_BySect ofrmAuth_BySect = BeSmartMRP.Permission.frmAuth_BySect.GetInstanse();
                    ofrmAuth_BySect.MdiParent = this;
                    ofrmAuth_BySect.Show();
                    ofrmAuth_BySect.Activate();
                    break;

                case "EBGPRE_PP":
                    DatabaseForms.frmBudTran ofrmBudTran = BeSmartMRP.DatabaseForms.frmBudTran.GetInstanse(SysDef.gc_REFTYPE_BGTRAN1, SysDef.gc_BGTRAN_STEP_PREPARE);
                    ofrmBudTran.MdiParent = this;
                    if (ofrmBudTran.DialogResult == DialogResult.OK)
                    {
                        ofrmBudTran.Show();
                        ofrmBudTran.Activate();
                    }
                    else
                    {
                        ofrmBudTran.Dispose();
                    }
                    break;

                case "EBGPRE_AP":
                    DatabaseForms.frmBudTran ofrmBudTran2 = BeSmartMRP.DatabaseForms.frmBudTran.GetInstanse(SysDef.gc_REFTYPE_BGTRAN1, SysDef.gc_BGTRAN_STEP_APPROVE);
                    ofrmBudTran2.MdiParent = this;
                    if (ofrmBudTran2.DialogResult == DialogResult.OK)
                    {
                        ofrmBudTran2.Show();
                        ofrmBudTran2.Activate();
                    }
                    else
                    {
                        ofrmBudTran2.Dispose();
                    }
                    break;

                case "EBGPRE_PS":
                    DatabaseForms.frmBudTran ofrmBudTran3 = BeSmartMRP.DatabaseForms.frmBudTran.GetInstanse(SysDef.gc_REFTYPE_BGTRAN1, SysDef.gc_BGTRAN_STEP_POST);
                    ofrmBudTran3.MdiParent = this;
                    if (ofrmBudTran3.DialogResult == DialogResult.OK)
                    {
                        ofrmBudTran3.Show();
                        ofrmBudTran3.Activate();
                    }
                    else
                    {
                        ofrmBudTran3.Dispose();
                    }
                    break;

                case "EBGPRE_RV":
                    DatabaseForms.frmBudTran ofrmBudTran4 = BeSmartMRP.DatabaseForms.frmBudTran.GetInstanse(SysDef.gc_REFTYPE_BGTRAN1, SysDef.gc_BGTRAN_STEP_REVISE);
                    ofrmBudTran4.MdiParent = this;
                    if (ofrmBudTran4.DialogResult == DialogResult.OK)
                    {
                        ofrmBudTran4.Show();
                        ofrmBudTran4.Activate();
                    }
                    else
                    {
                        ofrmBudTran4.Dispose();
                    }
                    break;

                case "EBGRECV_1":
                    Transaction.frmBGResv ofrmBGResv1 = BeSmartMRP.Transaction.frmBGResv.GetInstanse(SysDef.gc_REFTYPE_RECV1, SysDef.gc_BGTRAN_STEP_PREPARE);
                    ofrmBGResv1.MdiParent = this;
                    if (ofrmBGResv1.DialogResult == DialogResult.OK)
                    {
                        ofrmBGResv1.Show();
                        ofrmBGResv1.Activate();
                    }
                    else
                    {
                        ofrmBGResv1.Dispose();
                    }
                    break;
                case "EBGRECV_2":
                    Transaction.frmBGResv ofrmBGResv2 = BeSmartMRP.Transaction.frmBGResv.GetInstanse(SysDef.gc_REFTYPE_ADV1, SysDef.gc_BGTRAN_STEP_PREPARE);
                    ofrmBGResv2.MdiParent = this;
                    if (ofrmBGResv2.DialogResult == DialogResult.OK)
                    {
                        ofrmBGResv2.Show();
                        ofrmBGResv2.Activate();
                    }
                    else
                    {
                        ofrmBGResv2.Dispose();
                    }
                    break;

                case "EBGRECV_R1":
                    Transaction.frmBGResv ofrmBGResvR1 = BeSmartMRP.Transaction.frmBGResv.GetInstanse(SysDef.gc_REFTYPE_ADJ1, SysDef.gc_BGTRAN_STEP_PREPARE);
                    ofrmBGResvR1.MdiParent = this;
                    if (ofrmBGResvR1.DialogResult == DialogResult.OK)
                    {
                        ofrmBGResvR1.Show();
                        ofrmBGResvR1.Activate();
                    }
                    else
                    {
                        ofrmBGResvR1.Dispose();
                    }
                    break;

                case "EBGRECV_R2":
                    Transaction.frmBGResv ofrmBGResvR2 = BeSmartMRP.Transaction.frmBGResv.GetInstanse(SysDef.gc_REFTYPE_ADJ4, SysDef.gc_BGTRAN_STEP_PREPARE);
                    ofrmBGResvR2.MdiParent = this;
                    if (ofrmBGResvR2.DialogResult == DialogResult.OK)
                    {
                        ofrmBGResvR2.Show();
                        ofrmBGResvR2.Activate();
                    }
                    else
                    {
                        ofrmBGResvR2.Dispose();
                    }
                    break;

                case "PBGSTAT01":

                    Report.frmPBGStat ofrmReport01 = new Report.frmPBGStat();

                    ofrmReport01.SetTitle(inTag, "รายงานตรวจสอบสถานะการตั้งงบประมาณ", inTag);
                    ofrmReport01.MdiParent = this;
                    ofrmReport01.Show();
                    ofrmReport01.Activate();
                    break;

                case "EMSTER11":
                    Transaction.frmAddCost01 ofrmAddCost01 = new Transaction.frmAddCost01();
                    ofrmAddCost01.Show();
                    break;
                case "EMSTER12":
                    Transaction.frmAddCost02 ofrmAddCost02 = new Transaction.frmAddCost02();
                    ofrmAddCost02.Show();
                    break;
                case "EMSTER13":
                    Transaction.frmAddCost03 ofrmAddCost03 = new Transaction.frmAddCost03();
                    ofrmAddCost03.Show();
                    break;
                case "EMSTER14":
                    Transaction.frmAddCost01 ofrmAddCost04 = new Transaction.frmAddCost01();
                    ofrmAddCost04.Show();
                    break;
                case "EMSTER15":
                    Transaction.frmAddCost01 ofrmAddCost05 = new Transaction.frmAddCost01();
                    ofrmAddCost05.Show();
                    break;
                case "EMSTER16":
                    Transaction.frmAddCost01 ofrmAddCost06 = new Transaction.frmAddCost01();
                    ofrmAddCost06.Show();
                    break;

                case "CHGPWD":

                    using (DatabaseForms.dlgChgPwd dlg = new DatabaseForms.dlgChgPwd())
                    {
                        dlg.ShowDialog();
                    }

                    break;
            } 
        }

        private void pmRunMenuInput2(string inTag, string inMenuName)
        {
            switch (inTag.ToUpper())
            {

                case "EBGRECV_1":
                    Transaction.frmBGResv ofrmBGResv1 = BeSmartMRP.Transaction.frmBGResv.GetInstanse(SysDef.gc_REFTYPE_RECV1, SysDef.gc_BGTRAN_STEP_PREPARE);
                    ofrmBGResv1.MdiParent = this;
                    if (ofrmBGResv1.DialogResult == DialogResult.OK)
                    {
                        ofrmBGResv1.Show();
                        ofrmBGResv1.Activate();
                    }
                    else
                    {
                        ofrmBGResv1.Dispose();
                    }
                    break;
                case "EBGRECV_2":
                    Transaction.frmBGResv ofrmBGResv2 = BeSmartMRP.Transaction.frmBGResv.GetInstanse(SysDef.gc_REFTYPE_ADV1, SysDef.gc_BGTRAN_STEP_PREPARE);
                    ofrmBGResv2.MdiParent = this;
                    if (ofrmBGResv2.DialogResult == DialogResult.OK)
                    {
                        ofrmBGResv2.Show();
                        ofrmBGResv2.Activate();
                    }
                    else
                    {
                        ofrmBGResv2.Dispose();
                    }
                    break;

                case "EBGRECV_R1":
                    Transaction.frmBGResv ofrmBGResvR1 = BeSmartMRP.Transaction.frmBGResv.GetInstanse(SysDef.gc_REFTYPE_ADJ1, SysDef.gc_BGTRAN_STEP_PREPARE);
                    ofrmBGResvR1.MdiParent = this;
                    if (ofrmBGResvR1.DialogResult == DialogResult.OK)
                    {
                        ofrmBGResvR1.Show();
                        ofrmBGResvR1.Activate();
                    }
                    else
                    {
                        ofrmBGResvR1.Dispose();
                    }
                    break;

                case "EBGRECV_R2":
                    Transaction.frmBGResv ofrmBGResvR2 = BeSmartMRP.Transaction.frmBGResv.GetInstanse(SysDef.gc_REFTYPE_ADJ4, SysDef.gc_BGTRAN_STEP_PREPARE);
                    ofrmBGResvR2.MdiParent = this;
                    if (ofrmBGResvR2.DialogResult == DialogResult.OK)
                    {
                        ofrmBGResvR2.Show();
                        ofrmBGResvR2.Activate();
                    }
                    else
                    {
                        ofrmBGResvR2.Dispose();
                    }
                    break;

            }
        }

        private void pmRunMenuOutput1(string inTag, string inReportName)
        {

            string strReportName = AppUtil.StringHelper.SubStr(inReportName, AppUtil.StringHelper.RAt(".", inReportName) + 1);
            strReportName = strReportName.Trim();
            
            switch (inTag.ToUpper())
            {

                case "PBGOUT1_01":
                    Report.frmOption01 ofrmReport01 = new Report.frmOption01("FORM1", false);

                    ofrmReport01.SetTitle(inTag, strReportName, inTag);
                    ofrmReport01.MdiParent = this;
                    ofrmReport01.Show();
                    ofrmReport01.Activate();
                    break;

                case "PBGOUT1_02":
                    Report.frmOption01 ofrmReport02 = new Report.frmOption01("FORM2", false);

                    ofrmReport02.SetTitle(inTag, strReportName, inTag);
                    ofrmReport02.MdiParent = this;
                    ofrmReport02.Show();
                    ofrmReport02.Activate();
                    break;

                case "PBGOUT1_03":
                    Report.frmOption01 ofrmReport03 = new Report.frmOption01("FORM3", false);

                    ofrmReport03.SetTitle(inTag, strReportName, inTag);
                    ofrmReport03.MdiParent = this;
                    ofrmReport03.Show();
                    ofrmReport03.Activate();
                    break;

                case "PBGOUT1_04":
                    Report.frmOption01 ofrmReport04 = new Report.frmOption01("FORM4", false);

                    ofrmReport04.SetTitle(inTag, strReportName, inTag);
                    ofrmReport04.MdiParent = this;
                    ofrmReport04.Show();
                    ofrmReport04.Activate();
                    break;

                case "PBGOUT1_05":
                    Report.frmOption05 ofrmReport05 = new Report.frmOption05("FORM1", false);

                    ofrmReport05.SetTitle(inTag, strReportName, inTag);
                    ofrmReport05.MdiParent = this;
                    ofrmReport05.Show();
                    ofrmReport05.Activate();
                    break;

                case "PBGOUT1_06":
                    Report.frmOption06 ofrmReport06 = new Report.frmOption06();

                    ofrmReport06.SetTitle(inTag, strReportName, inTag);
                    ofrmReport06.MdiParent = this;
                    ofrmReport06.Show();
                    ofrmReport06.Activate();
                    break;

                case "PBGOUT1_07":
                    Report.frmOption07_9 ofrmReport07 = new Report.frmOption07_9("FORM1", false);

                    ofrmReport07.SetTitle(inTag, strReportName, inTag);
                    ofrmReport07.MdiParent = this;
                    ofrmReport07.Show();
                    ofrmReport07.Activate();
                    break;

                case "PBGOUT1_08":
                    Report.frmOption07_9 ofrmReport08 = new Report.frmOption07_9("FORM2", false);

                    ofrmReport08.SetTitle(inTag, strReportName, inTag);
                    ofrmReport08.MdiParent = this;
                    ofrmReport08.Show();
                    ofrmReport08.Activate();
                    break;

                case "PBGOUT1_09":
                    Report.frmOption07_9 ofrmReport09 = new Report.frmOption07_9("FORM3", false);

                    ofrmReport09.SetTitle(inTag, strReportName, inTag);
                    ofrmReport09.MdiParent = this;
                    ofrmReport09.Show();
                    ofrmReport09.Activate();
                    break;

                case "PBGOUT1_10":
                    Report.frmOption10_13 ofrmReport10 = new Report.frmOption10_13("FORM1", false);

                    ofrmReport10.SetTitle(inTag, strReportName, inTag);
                    ofrmReport10.MdiParent = this;
                    ofrmReport10.Show();
                    ofrmReport10.Activate();
                    break;

                case "PBGOUT1_11":
                    Report.frmOption10_13 ofrmReport11 = new Report.frmOption10_13("FORM2", false);

                    ofrmReport11.SetTitle(inTag, strReportName, inTag);
                    ofrmReport11.MdiParent = this;
                    ofrmReport11.Show();
                    ofrmReport11.Activate();
                    break;

                case "PBGOUT1_12":
                    Report.frmOption10_13 ofrmReport12 = new Report.frmOption10_13("FORM3", false);

                    ofrmReport12.SetTitle(inTag, strReportName, inTag);
                    ofrmReport12.MdiParent = this;
                    ofrmReport12.Show();
                    ofrmReport12.Activate();
                    break;

                case "PBGOUT1_13":
                    Report.frmOption10_13 ofrmReport13 = new Report.frmOption10_13("FORM4", false);

                    ofrmReport13.SetTitle(inTag, strReportName, inTag);
                    ofrmReport13.MdiParent = this;
                    ofrmReport13.Show();
                    ofrmReport13.Activate();
                    break;

                case "PBGOUT1_14":
                    Report.frmOption01 ofrmReport14 = new Report.frmOption01("FORM1", true);

                    ofrmReport14.SetTitle(inTag, strReportName, inTag);
                    ofrmReport14.MdiParent = this;
                    ofrmReport14.Show();
                    ofrmReport14.Activate();
                    break;

                case "PBGOUT1_15":
                    Report.frmOption01 ofrmReport15 = new Report.frmOption01("FORM2", true);

                    ofrmReport15.SetTitle(inTag, strReportName, inTag);
                    ofrmReport15.MdiParent = this;
                    ofrmReport15.Show();
                    ofrmReport15.Activate();
                    break;

                case "PBGOUT1_16":
                    Report.frmOption01 ofrmReport16 = new Report.frmOption01("FORM3", true);

                    ofrmReport16.SetTitle(inTag, strReportName, inTag);
                    ofrmReport16.MdiParent = this;
                    ofrmReport16.Show();
                    ofrmReport16.Activate();
                    break;

                case "PBGOUT1_17":
                    Report.frmOption01 ofrmReport17 = new Report.frmOption01("FORM4", true);

                    ofrmReport17.SetTitle(inTag, strReportName, inTag);
                    ofrmReport17.MdiParent = this;
                    ofrmReport17.Show();
                    ofrmReport17.Activate();
                    break;

                case "PBGOUT1_18":
                    Report.frmOption05 ofrmReport18 = new Report.frmOption05("FORM1", true);

                    ofrmReport18.SetTitle(inTag, strReportName, inTag);
                    ofrmReport18.MdiParent = this;
                    ofrmReport18.Show();
                    ofrmReport18.Activate();
                    break;

                case "PBGOUT1_19":
                    Report.frmOption07_9 ofrmReport19 = new Report.frmOption07_9("FORM1", true);

                    ofrmReport19.SetTitle(inTag, strReportName, inTag);
                    ofrmReport19.MdiParent = this;
                    ofrmReport19.Show();
                    ofrmReport19.Activate();
                    break;

                case "PBGOUT1_20":
                    Report.frmOption07_9 ofrmReport20 = new Report.frmOption07_9("FORM3", true);

                    ofrmReport20.SetTitle(inTag, strReportName, inTag);
                    ofrmReport20.MdiParent = this;
                    ofrmReport20.Show();
                    ofrmReport20.Activate();
                    break;

                case "PBGOUT1_21":
                    Report.frmOption10_13 ofrmReport21 = new Report.frmOption10_13("FORM1", true);

                    ofrmReport21.SetTitle(inTag, strReportName, inTag);
                    ofrmReport21.MdiParent = this;
                    ofrmReport21.Show();
                    ofrmReport21.Activate();
                    break;

                case "PBGOUT1_22":
                    Report.frmOption10_13 ofrmReport22 = new Report.frmOption10_13("FORM2", true);

                    ofrmReport22.SetTitle(inTag, strReportName, inTag);
                    ofrmReport22.MdiParent = this;
                    ofrmReport22.Show();
                    ofrmReport22.Activate();
                    break;

                case "PBGOUT1_23":
                    Report.frmOption10_13 ofrmReport23 = new Report.frmOption10_13("FORM3", true);

                    ofrmReport23.SetTitle(inTag, strReportName, inTag);
                    ofrmReport23.MdiParent = this;
                    ofrmReport23.Show();
                    ofrmReport23.Activate();
                    break;

                case "PBGOUT1_24":
                    Report.frmOption10_13 ofrmReport24 = new Report.frmOption10_13("FORM4", true);

                    ofrmReport24.SetTitle(inTag, strReportName, inTag);
                    ofrmReport24.MdiParent = this;
                    ofrmReport24.Show();
                    ofrmReport24.Activate();
                    break;

            }

        }

        private void pmRunMenuOutput2(string inTag, string inReportName)
        {

            string strReportName = AppUtil.StringHelper.SubStr(inReportName, AppUtil.StringHelper.RAt(".", inReportName) + 1);
            strReportName = strReportName.Trim();
            Report.ListMenu.frmOptionList ofrmReportLst = null;
            Type ObjType = null;

            switch (inTag.ToUpper())
            {

                case "PBGOUT2_01":
                    Report.output2.frmOption201 ofrmReport01 = new Report.output2.frmOption201("FORM1", false);

                    ofrmReport01.SetTitle(inTag, strReportName, inTag);
                    ofrmReport01.MdiParent = this;
                    ofrmReport01.Show();
                    ofrmReport01.Activate();
                    
                    break;

                case "PBGOUT2_02":
                    Report.output2.frmOption202 ofrmReport02 = new Report.output2.frmOption202("FORM1", false);

                    ofrmReport02.SetTitle(inTag, strReportName, inTag);
                    ofrmReport02.MdiParent = this;
                    ofrmReport02.Show();
                    ofrmReport02.Activate();
                    break;

                case "PBGOUT2_03":
                    Report.output2.frmOption203_6 ofrmReport03 = new Report.output2.frmOption203_6("FORM1", false);

                    ofrmReport03.SetTitle(inTag, strReportName, inTag);
                    ofrmReport03.MdiParent = this;
                    ofrmReport03.Show();
                    ofrmReport03.Activate();
                    break;

                case "PBGOUT2_04":
                    Report.output2.frmOption203_6 ofrmReport04 = new Report.output2.frmOption203_6("FORM1", false);

                    ofrmReport04.SetTitle(inTag, strReportName, inTag);
                    ofrmReport04.MdiParent = this;
                    ofrmReport04.Show();
                    ofrmReport04.Activate();
                    break;

                case "PBGOUT2_05":
                    Report.output2.frmOption203_6 ofrmReport05 = new Report.output2.frmOption203_6("FORM2", false);

                    ofrmReport05.SetTitle(inTag, strReportName, inTag);
                    ofrmReport05.MdiParent = this;
                    ofrmReport05.Show();
                    ofrmReport05.Activate();
                    break;

                case "PBGOUT2_06":
                    Report.output2.frmOption203_6 ofrmReport06 = new Report.output2.frmOption203_6("FORM2", false);

                    ofrmReport06.SetTitle(inTag, strReportName, inTag);
                    ofrmReport06.MdiParent = this;
                    ofrmReport06.Show();
                    ofrmReport06.Activate();
                    break;

            }

        }

        private void pmRunMenuReportSM(string inTag, string inReportName)
        {

            //string strReportName = AppUtil.StringHelper.SubStr(inReportName, AppUtil.StringHelper.RAt(".", inReportName) + 1);
            string strReportName = inReportName.Trim();
            Report.ListMenu.frmOptionList ofrmReportLst = null;
            bool bllIsList01 = false;
            switch (inTag.ToUpper())
            {

                case "PCORPLS":
                    ofrmReportLst = new Report.ListMenu.frmOptionList(MapTable.Table.EMCorp, "บริษัท");
                    bllIsList01 = true;
                    break;
                case "PBRANCHLST":
                    ofrmReportLst = new Report.ListMenu.frmOptionList(MapTable.Table.EMBranch, "สาขา");
                    bllIsList01 = true;
                    break;
                case "PDEPTLST":
                    ofrmReportLst = new Report.ListMenu.frmOptionList(MapTable.Table.EMDept, "ผ่าย");
                    bllIsList01 = true;
                    break;
                case "PSECTLST":
                    ofrmReportLst = new Report.ListMenu.frmOptionList(MapTable.Table.EMSect, "แผนก");
                    bllIsList01 = true;
                    break;
                case "PPROJLST":
                    ofrmReportLst = new Report.ListMenu.frmOptionList(MapTable.Table.EMProj, "แผนงาน");
                    bllIsList01 = true;
                    break;
                case "PJOBLST":
                    ofrmReportLst = new Report.ListMenu.frmOptionList(MapTable.Table.EMJob, "โครงการ");
                    bllIsList01 = true;
                    break;
                case "PBGTYPELST":
                    ofrmReportLst = new Report.ListMenu.frmOptionList(MapTable.Table.BudgetType, "ประเภทงบประมาณ");
                    bllIsList01 = true;
                    break;
                case "PBGCHARTLST":
                    ofrmReportLst = new Report.ListMenu.frmOptionList(MapTable.Table.BudgetChart, "ผังงบประมาณ");
                    bllIsList01 = true;
                    break;
                case "PBGALLOCLST":
                    ofrmReportLst = new Report.ListMenu.frmOptionList(MapTable.Table.EMAcChart, "ปันส่วน");
                    bllIsList01 = true;
                    break;
                case "PACCHARTLST":
                    ofrmReportLst = new Report.ListMenu.frmOptionList(MapTable.Table.EMAcChart, "ผังบัญชี");
                    bllIsList01 = true;
                    break;
                case "PUOMLST":
                    ofrmReportLst = new Report.ListMenu.frmOptionList(MapTable.Table.EMUOM, "หน่วยนับ");
                    bllIsList01 = true;
                    break;
                case "PAPPEMPLLST":
                    ofrmReportLst = new Report.ListMenu.frmOptionList(MapTable.Table.AppEmpl, "พนักงาน");
                    bllIsList01 = true;
                    break;
                case "PAPPLOGINLST":
                    ofrmReportLst = new Report.ListMenu.frmOptionList(MapTable.Table.AppLogIn, "Login");
                    bllIsList01 = true;
                    break;
                case "PKEEPLOG":
                    Report.frmPKeepLog ofrmPKeepLog = new Report.frmPKeepLog();
                    ofrmPKeepLog.SetTitle(inTag, "พิมพ์รายงานการทำงานแต่ละวัน", inTag);
                    ofrmPKeepLog.MdiParent = this;
                    ofrmPKeepLog.Show();
                    ofrmPKeepLog.Activate();
                    break;
                case "PPRODSTAT":
                    Report.rptPProdStat oPProdStat = new Report.rptPProdStat(DocumentType.MO.ToString(), "FORM1");
                    oPProdStat.SetTitle(inTag, strReportName, inTag);
                    oPProdStat.MdiParent = this;
                    oPProdStat.Show();
                    oPProdStat.Activate();
                    break;
                case "PMOSTAT":
                    Report.rptPMOStat oPMOStat = new Report.rptPMOStat(DocumentType.MO.ToString(), "FORM1");
                    oPMOStat.SetTitle(inTag, strReportName, inTag);
                    oPMOStat.MdiParent = this;
                    oPMOStat.Show();
                    oPMOStat.Activate();
                    break;
                case "PMOLST2":
                    Report.rptPMOStat oPMOStat2 = new Report.rptPMOStat(DocumentType.MO.ToString(), "FORM2");
                    oPMOStat2.SetTitle(inTag, strReportName, inTag);
                    oPMOStat2.MdiParent = this;
                    oPMOStat2.Show();
                    oPMOStat2.Activate();
                    break;
                case "PMOLST3":
                    Report.rptPMOStat oPMOStat3 = new Report.rptPMOStat(DocumentType.MO.ToString(), "FORM3");
                    oPMOStat3.SetTitle(inTag, strReportName, inTag);
                    oPMOStat3.MdiParent = this;
                    oPMOStat3.Show();
                    oPMOStat3.Activate();
                    break;
                case "PRMSCJB":
                    Report.rptPConsum001 oPConsum1 = new Report.rptPConsum001("FORM1");
                    oPConsum1.SetTitle(inTag, strReportName, inTag);
                    oPConsum1.MdiParent = this;
                    oPConsum1.Show();
                    oPConsum1.Activate();
                    break;
                case "PRMSCPG":
                    Report.rptPConsum001 oPConsum2 = new Report.rptPConsum001("FORM2");
                    oPConsum2.SetTitle(inTag, strReportName, inTag);
                    oPConsum2.MdiParent = this;
                    oPConsum2.Show();
                    oPConsum2.Activate();
                    break;
                case "PRMJBSC":
                    Report.rptPConsum001 oPConsum3 = new Report.rptPConsum001("FORM3");
                    oPConsum3.SetTitle(inTag, strReportName, inTag);
                    oPConsum3.MdiParent = this;
                    oPConsum3.Show();
                    oPConsum3.Activate();
                    break;
                case "PRMJBPG":
                    Report.rptPConsum001 oPConsum4 = new Report.rptPConsum001("FORM4");
                    oPConsum4.SetTitle(inTag, strReportName, inTag);
                    oPConsum4.MdiParent = this;
                    oPConsum4.Show();
                    oPConsum4.Activate();
                    break;
                case "PCOST01":
                    Report.rptPCost01 orptPCost01 = new Report.rptPCost01("FORM1");
                    orptPCost01.SetTitle(inTag, strReportName, inTag);
                    orptPCost01.MdiParent = this;
                    orptPCost01.Show();
                    orptPCost01.Activate();
                    break;

                case "PCOST02":
                    Report.rptPCost02 orptPCost02 = new Report.rptPCost02("FORM1");
                    orptPCost02.SetTitle(inTag, strReportName, inTag);
                    orptPCost02.MdiParent = this;
                    orptPCost02.Show();
                    orptPCost02.Activate();
                    break;
                case "PCOST03":
                    Report.rptPCost03 orptPCost03 = new Report.rptPCost03("FORM1");
                    orptPCost03.SetTitle(inTag, strReportName, inTag);
                    orptPCost03.MdiParent = this;
                    orptPCost03.Show();
                    orptPCost03.Activate();
                    break;
                case "PMCLOSELST":
                    Report.rptMCList orptMCList = new Report.rptMCList("FORM1");
                    orptMCList.SetTitle(inTag, strReportName, inTag);
                    orptMCList.MdiParent = this;
                    orptMCList.Show();
                    orptMCList.Activate();
                    break;
                case "XRSTCARD01":
                    Report.XRSTCARD01 orptXRSTCARD01 = new Report.XRSTCARD01("FORM1");
                    orptXRSTCARD01.SetTitle(inTag, strReportName, inTag);
                    orptXRSTCARD01.MdiParent = this;
                    orptXRSTCARD01.Show();
                    orptXRSTCARD01.Activate();
                    break;
                case "XRSTCARD02":
                    Report.XRSTCARD01 orptXRSTCARD02 = new Report.XRSTCARD01("FORM2");
                    orptXRSTCARD02.SetTitle(inTag, strReportName, inTag);
                    orptXRSTCARD02.MdiParent = this;
                    orptXRSTCARD02.Show();
                    orptXRSTCARD02.Activate();
                    break;
                case "XRSTAG01":
                    Report.XRSTAG01 oXRSTAG01 = new Report.XRSTAG01("FORM1");
                    oXRSTAG01.SetTitle(inTag, strReportName, inTag);
                    oXRSTAG01.MdiParent = this;
                    oXRSTAG01.Show();
                    oXRSTAG01.Activate();
                    break;
                case "XRSTAG02":
                    Report.XRSTAG01 oXRSTAG02 = new Report.XRSTAG01("FORM2");
                    oXRSTAG02.SetTitle(inTag, strReportName, inTag);
                    oXRSTAG02.MdiParent = this;
                    oXRSTAG02.Show();
                    oXRSTAG02.Activate();
                    break;
                case "XRSTCARD04":
                    Report.XRSTCARD01 orptXRSTCARD04 = new Report.XRSTCARD01("FORM4");
                    orptXRSTCARD04.SetTitle(inTag, strReportName, inTag);
                    orptXRSTCARD04.MdiParent = this;
                    orptXRSTCARD04.Show();
                    orptXRSTCARD04.Activate();
                    break;
                case "XRSTCARD05":
                    Report.XRSTCARD01 orptXRSTCARD05 = new Report.XRSTCARD01("FORM5");
                    orptXRSTCARD05.SetTitle(inTag, strReportName, inTag);
                    orptXRSTCARD05.MdiParent = this;
                    orptXRSTCARD05.Show();
                    orptXRSTCARD05.Activate();
                    break;

                case "XRSTLOT01":
                    Report.XRSTCARD01 orptXRSTCARD06 = new Report.XRSTCARD01("FORM6");
                    orptXRSTCARD06.SetTitle(inTag, strReportName, inTag);
                    orptXRSTCARD06.MdiParent = this;
                    orptXRSTCARD06.Show();
                    orptXRSTCARD06.Activate();
                    break;

                case "XRSTSALE01":
                    Report.XRSTCARD01 orptXRSTCARD07 = new Report.XRSTCARD01("FORM7");
                    orptXRSTCARD07.SetTitle(inTag, strReportName, inTag);
                    orptXRSTCARD07.MdiParent = this;
                    orptXRSTCARD07.Show();
                    orptXRSTCARD07.Activate();
                    break;

                case "XRSTSALE02":
                    Report.XRSTCARD01 orptXRSTCARD08 = new Report.XRSTCARD01("FORM8");
                    orptXRSTCARD08.SetTitle(inTag, strReportName, inTag);
                    orptXRSTCARD08.MdiParent = this;
                    orptXRSTCARD08.Show();
                    orptXRSTCARD08.Activate();
                    break;
                case "XRSTSALE03":
                    Report.XRSTCARD01 orptXRSTCARD09 = new Report.XRSTCARD01("FORM9");
                    orptXRSTCARD09.SetTitle(inTag, strReportName, inTag);
                    orptXRSTCARD09.MdiParent = this;
                    orptXRSTCARD09.Show();
                    orptXRSTCARD09.Activate();
                    break;
                case "XRSTSALE04":
                    Report.XRSTCARD01 orptXRSTCARD10 = new Report.XRSTCARD01("FORM10");
                    orptXRSTCARD10.SetTitle(inTag, strReportName, inTag);
                    orptXRSTCARD10.MdiParent = this;
                    orptXRSTCARD10.Show();
                    orptXRSTCARD10.Activate();
                    break;
                case "XRSTSALE08":
                    Report.XRSTCARD01 orptXRSTCARD21 = new Report.XRSTCARD01("FORM21");
                    orptXRSTCARD21.SetTitle(inTag, strReportName, inTag);
                    orptXRSTCARD21.MdiParent = this;
                    orptXRSTCARD21.Show();
                    orptXRSTCARD21.Activate();
                    break;

                case "PCOST04":
                    Report.rptPCost03 orptPCost04 = new Report.rptPCost03("FORM3");
                    orptPCost04.SetTitle(inTag, strReportName, inTag);
                    orptPCost04.MdiParent = this;
                    orptPCost04.Show();
                    orptPCost04.Activate();
                    break;

                case "PCOST05":
                    Report.rptPCost03 orptPCost05 = new Report.rptPCost03("FORM3");
                    orptPCost05.SetTitle(inTag, strReportName, inTag);
                    orptPCost05.MdiParent = this;
                    orptPCost05.Show();
                    orptPCost05.Activate();
                    break;
                case "PCOST06":
                    Report.rptPCost03 orptPCost06 = new Report.rptPCost03("FORM3");
                    orptPCost06.SetTitle(inTag, strReportName, inTag);
                    orptPCost06.MdiParent = this;
                    orptPCost06.Show();
                    orptPCost06.Activate();
                    break;

                case "PCOST07":
                    Report.rptPCost03 orptPCost07 = new Report.rptPCost03("FORM3");
                    orptPCost07.SetTitle(inTag, strReportName, inTag);
                    orptPCost07.MdiParent = this;
                    orptPCost07.Show();
                    orptPCost07.Activate();
                    break;

                case "PCOST08":
                    Report.rptPCost03 orptPCost08 = new Report.rptPCost03("FORM3");
                    orptPCost08.SetTitle(inTag, strReportName, inTag);
                    orptPCost08.MdiParent = this;
                    orptPCost08.Show();
                    orptPCost08.Activate();
                    break;

                case "PCOST09":
                    Report.rptPCost03 orptPCost09 = new Report.rptPCost03("FORM3");
                    orptPCost09.SetTitle(inTag, strReportName, inTag);
                    orptPCost09.MdiParent = this;
                    orptPCost09.Show();
                    orptPCost09.Activate();
                    break;

                case "PCOST10":
                    Report.rptPCost03 orptPCost10 = new Report.rptPCost03("FORM3");
                    orptPCost10.SetTitle(inTag, strReportName, inTag);
                    orptPCost10.MdiParent = this;
                    orptPCost10.Show();
                    orptPCost10.Activate();
                    break;

            }

            if (bllIsList01 && ofrmReportLst != null)
            {
                ofrmReportLst.SetTitle(inTag, strReportName, inTag);
                ofrmReportLst.MdiParent = this;
                ofrmReportLst.Show();
                ofrmReportLst.Activate();
            }
            else
            { 
            }

        }

        protected override void OnClosing(CancelEventArgs ce)
        {
            if (this.pmClosingApp())
            {
                base.OnClosing(ce);
                Application.Exit();
            }
            else
                ce.Cancel = true;
        }

        private bool pmClosingApp()
        {
            return (MessageBox.Show(this, "ต้องการที่จะออกจากการใช้งานโปรแกรม ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        private void pmGetCorp()
        {
            string strModule = "";

            dlgGetCorp ofrmGetCorp = new dlgGetCorp();
            ofrmGetCorp.ShowDialog();
            if (ofrmGetCorp.DialogResult == DialogResult.OK)
            {
                App.SetActiveCorp(ofrmGetCorp.RetrieveValue());
                //this.MainStatusBar.Panels["CORPNAME"].Text = App.ActiveCorp.Name;
                //this.Text = App.ActiveCorp.Name.TrimEnd() + " " + strModule + " for MS SQL Server Version date [" + App.AppVersion + "]";
                //this.Text = App.ActiveCorp.Name.TrimEnd() + " - BeSmart MRP 2010 For MS SQL Server Version date [" + App.AppVersion + "]";
                this.Text = App.ActiveCorp.Name.TrimEnd() + " - WeBusiness WMS for MS SQL Server Version date [" + App.AppVersion + "]";
                this.lblVersionDate.Text = "Version date [" + App.AppVersion + "]";

                dtsDataEnv = new DataSet();
                pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                this.pmGenMenu();
            
            }
            else
            {
                this.pmGetLogin();
            }
        }

        private void pmGetLogin()
        {
            using (frmLogin dlg = new frmLogin())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    App.AppDBChkUpdate();

                    this.pmGetCorp();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void frmMainmenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.MdiChildren.Length > 0 | e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    //this.Close();
                    this.pmGetCorp();
                    break;
            }
        }

        private void frmMainmenu_Load(object sender, EventArgs e)
        {
            //frmStartPage ofrmStartPage = frmStartPage.GetInstanse();
            //ofrmStartPage.MdiParent = this;
            //ofrmStartPage.Show();

            //this.bar2.BarItemHorzIndent = 5;
            //if (System.Environment.OSVersion.Version.Major > 5)
            //{
            //    this.bar2.BarItemHorzIndent = 2;
            //}

            if (System.IO.File.Exists(Application.StartupPath + "\\Wallpaper.jpg"))
            {
                this.imgBG.Image = System.Drawing.Image.FromFile(Application.StartupPath + "\\Wallpaper.jpg");
            }

            if (System.IO.File.Exists(Application.StartupPath + "\\Support\\AppLogo.jpg"))
            {
                this.pbxCusLOGO.Image = System.Drawing.Image.FromFile(Application.StartupPath + "\\Support\\AppLogo.jpg");
            }

            if (System.IO.File.Exists(Application.StartupPath + "\\Support\\Announce.txt"))
            {
                string strFileName = Application.StartupPath + "\\Support\\Announce.txt";

                try
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(strFileName, System.Text.Encoding.Default);
                    string strLine = "";
                    this.lblAnnouce.Text = "";
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        this.lblAnnouce.Text += strLine;
                    }
                    sr.Close();
                }
                catch { }

            }

            string strMsg = "";
            if (App.pmAppChkVersion(ref strMsg))
            {
                //MessageBox.Show(strMsg);
            }
            else
            {
                MessageBox.Show(strMsg, "Application confirm message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Application.Exit();
            }

            //DatabaseForms.frmCorp ofrmGetCorp = new BeSmartMRP.DatabaseForms.frmCorp(FormActiveMode.Report);
            //ofrmGetCorp.StartPosition = FormStartPosition.CenterScreen;
            //ofrmGetCorp.ShowDialog();
            //if (ofrmGetCorp.PopUpResult)
            dlgGetCorp ofrmGetCorp = new dlgGetCorp();
            ofrmGetCorp.ShowDialog();
            if (ofrmGetCorp.DialogResult == DialogResult.OK)
            {
                App.SetActiveCorp(ofrmGetCorp.RetrieveValue());
                //this.MainStatusBar.Panels["CORPNAME"].Text = App.ActiveCorp.Name;
                //this.Text = App.ActiveCorp.Name.TrimEnd() + " - BeSmart MRP 2010 For MS SQL Server Version date [" + App.AppVersion + "]";
                this.Text = App.ActiveCorp.Name.TrimEnd() + " - MRP Module for MS SQL Server Version date [" + App.AppVersion + "]";
                this.Text = App.ActiveCorp.Name.TrimEnd() + " - WeBusiness WMS for MS SQL Server Version date [" + App.AppVersion + "]";
                this.lblVersionDate.Text = "Version date [" + App.AppVersion + "]";

                dtsDataEnv = new DataSet();
                pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
                this.pmGenMenu();

                //this.timer1.Enabled = true;
                this.backgroundWorker1.RunWorkerAsync();

            }
            else
            {
                Application.Exit();
            }
        }

        private void xxxfrmMainmenu_Load(object sender, EventArgs e)
        {
            //frmStartPage ofrmStartPage = frmStartPage.GetInstanse();
            //ofrmStartPage.MdiParent = this;
            //ofrmStartPage.Show();

            dlgGetCorp ofrmGetCorp = new dlgGetCorp();
            ofrmGetCorp.ShowDialog();
            if (ofrmGetCorp.DialogResult == DialogResult.OK)
            {
                App.SetActiveCorp(ofrmGetCorp.RetrieveValue());
                //this.MainStatusBar.Panels["CORPNAME"].Text = App.ActiveCorp.Name;
                //this.Text = App.ActiveCorp.Name.TrimEnd() + " " + strModule + " for MS SQL Server Version date [" + App.AppVersion + "]";
            }
            else
            {
                Application.Exit();
            }
        }

        private void xtraTabbedMdiManager1_PageAdded(object sender, DevExpress.XtraTabbedMdi.MdiTabPageEventArgs e)
        {
            this.pmVisibleAbout();
        }

        private void xtraTabbedMdiManager1_PageRemoved(object sender, DevExpress.XtraTabbedMdi.MdiTabPageEventArgs e)
        {
            this.pmVisibleAbout();
        }

        private void pmVisibleAbout()
        {
            this.pnlAbout.Visible = (this.MdiChildren.Length == 0);
        }

        private void pnlAbout_SizeChanged(object sender, EventArgs e)
        {
            //this.pnlCustomer.Location = new Point(Convert.ToInt32((this.pnlAbout.Width - this.pnlCustomer.Width) / 2)+150, this.pnlAbout.Height - this.pnlCustomer.Height - 30);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.pmStartReport();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.pmStartReport();
        }

        private void pmStartReport()
        {

            this.timer1.Enabled = false;
            
            //string strRPTFileName = "";

            //strRPTFileName = Application.StartupPath + @"\RPT\TEMRPT.rpt";
            //ReportDocument rptPreviewReport = new ReportDocument();
            //if (!System.IO.File.Exists(strRPTFileName))
            //{
            //    //MessageBox.Show("File not found " + strRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //rptPreviewReport.Load(strRPTFileName);

            //App.PreviewReport(rptPreviewReport);
        }

        private void navBarControl1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //MessageBox.Show(e.Link.ItemName);
            switch (e.Link.ItemName.ToUpper())
            {
                case "NAVSTMOVE_FR":
                    this.pmRunMenuInput1("ESTMOVE_FR", e.Link.Caption);
                    //Transaction.frmTest ofrm1 = new Transaction.frmTest();
                    //ofrm1.Show();
                    break;
                case "NAVSTMOVE_WR":
                    this.pmRunMenuInput1("ESTMOVE_WR", e.Link.Caption);
                    break;
                case "NAVSTMOVE_TR":
                    //this.pmRunMenuInput1("ESTMOVE_TR", e.Link.Caption);
                    this.pmRunMenuInput1("ESTMREQ", e.Link.Caption);
                    break;
                case "NAVORDER_PO":
                    this.pmRunMenuInput1("EORDER_PO", e.Link.Caption);
                    break;
                case "NAVORDER_SO":
                    this.pmRunMenuInput1("EORDER_SO", e.Link.Caption);
                    break;
                case "NAVORDER_QS":
                    this.pmRunMenuInput1("EORDER_QS", e.Link.Caption);
                    break;
                case "NAVINV_SI":
                    this.pmRunMenuInput1("INV_SI", e.Link.Caption);
                    break;
                case "NAVSTMOVE_GD":
                    this.pmRunMenuInput1("ESTMOVE_GD", e.Link.Caption);
                    break;
                case "NAVSTOCK_BAL1":
                    this.pmRunMenuInput1("ESTK_BAL1", e.Link.Caption);
                    break;
            }

        }

        private void treeList1_Click(object sender, EventArgs e)
        {

        }

        private void treeList1_DoubleClick(object sender, EventArgs e)
        {
            //string x = e.ToString();
            //string x = treeList1.FocusedNode.ToString();
        }

        private void treeList1_TreeListMenuItemClick(object sender, DevExpress.XtraTreeList.TreeListMenuItemClickEventArgs e)
        {
            string x = e.MenuItem.Caption;

        }

        private void navBarControl1_Click(object sender, EventArgs e)
        {

        }

        private void btnMen01_Click(object sender, EventArgs e)
        {
            this.pmRunMenuInput1("EPROD", "รายการสินค้า");
        }
    }

}
