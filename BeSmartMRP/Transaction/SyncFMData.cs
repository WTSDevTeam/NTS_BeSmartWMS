using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO;

using WS.Data;
using WS.Data.Agents;
using AppUtil;


namespace BeSmartMRP.Transaction
{
    public class SyncFMData
    {

        public SyncFMData()
        {
            //App.FileConfig = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            //string strProcessFile = AppDomain.CurrentDomain.BaseDirectory + "process.chk";

            //if (System.IO.File.Exists(App.FileConfig))
            //{
            //    App.LoadAppConfig();
            //}
        }

        public void ProcessExport(string inDir, DateTime inBegDate, DateTime inEndDate)
        {
            this.pm1Process(inDir,inBegDate, inEndDate);
        }

        public void ProcessImport(string inDir, string inBakDir)
        {
            this.pm2Process(inDir, inBakDir);
        }

        #region "Constant Field"

        public const string xdCMRem = "Rem";
        public const string xdCMRem2 = "Rm2";
        public const string xdCMRem3 = "Rm3";
        public const string xdCMRem4 = "Rm4";
        public const string xdCMRem5 = "Rm5";
        public const string xdCMRem6 = "Rm6";
        public const string xdCMRem7 = "Rm7";
        public const string xdCMRem8 = "Rm8";
        public const string xdCMRem9 = "Rm9";
        public const string xdCMRem10 = "RmA";

        public const string x_CMemDetail = "Det";
        public const string x_CMem2Detail = "Dt2";
        public const string x_CMRem = "Rem";
        public const string x_CMRem2 = "Rm2";
        public const string x_CMAd11 = "A11";
        public const string x_CMAd21 = "A21";
        public const string x_CMAd31 = "A31";
        public const string x_CMAd12 = "A12";
        public const string x_CMAd22 = "A22";
        public const string x_CMAd32 = "A32";

        public const string xdCMCtAd11 = "C11";
        public const string xdCMCtAd21 = "C21";
        public const string xdCMCtAd31 = "C31";
        public const string xdCMCtAd12 = "C12";
        public const string xdCMCtAd22 = "C22";
        public const string xdCMCtAd32 = "C32";

        public const string x_CMCtZip = "CZp";
        public const string x_CMCtTel = "CTl";
        public const string x_CMCtFax = "CFx";
        public const string x_CMTBusi = "TBu";
        public const string x_PrcSCoor = "A";		//Sale Coor ส่วนลดประจำตัว
        public const string x_CMWebSite = "Web";
        public const string x_CMEmail = "Ema";

        public const string x_CMTel = "Tel";
        public const string x_CMFax = "Fax";
        public const string x_CMTxDesti = "Des";
        public const string x_CMTaxId = "Tax";
        public const string x_CMRemLayH = "Lay";

        public const string x_CMCtNam2 = "CN2";
        public const string x_CMCtPos2 = "CP2";
        public const string x_CMCtNam3 = "CN3";
        public const string x_CMCtPos3 = "CP3";

        public const string x_CMCauseBlkLst = "BLK";
        public const string x_CMMoBileTel = "MTl";
        public const string xdCMMId = "MId";		//25/03/47 By Pic

        #endregion

        DataSet dtsDataEnv = new DataSet();
        DataSet dtsDataEnvEx = new DataSet("Export_");

        WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        System.Data.IDbConnection mdbConn = null;
        System.Data.IDbTransaction mdbTran = null;

        WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
        System.Data.IDbConnection mdbConn2 = null;
        System.Data.IDbTransaction mdbTran2 = null;

        private string mstrExportFolder = "";

        private void pm1Process(string inDir, DateTime inBegDate, DateTime inEndDate)
        {

            this.mstrExportFolder = inDir;
            bool bllResult = pmSyncGLRef(inBegDate, inEndDate);
            WriteLog2("Complete : SyncInvoice");
        }

        private void XXXpm1Process(string inDir,DateTime inBegDate, DateTime inEndDate)
        {

            this.mstrExportFolder = inDir;

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            //WS.Data.Agents.cDBMSAgent pobjSQLUtil_Del = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString2, App.DatabaseReside);

            string message = "";

            string strSQLStr = "select * from GLREF where FCCORP = ? and FCRFTYPE in ('S','E','F') and FDDATE between ? and ? order by FDDATE,FCCODE";
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, inBegDate, inEndDate });
            if (pobjSQLUtil.SQLExec(ref dtsDataEnv, "QSyncDB", "SYNCDBDATA", strSQLStr, ref strErrorMsg))
            {
                //try
                //{
                foreach (DataRow dtrSCoor in dtsDataEnv.Tables["QSyncDB"].Rows)
                {
                    bool bllResult = false;
                    //string strCorp = dtrSCoor["QcCorp"].ToString().TrimEnd();
                    string strCode = dtrSCoor["FCREFNO"].ToString().TrimEnd();
                    
                    //bllResult = pmSyncGLRef("", strCode, dtrSCoor["FCSKID"].ToString());
                    WriteLog2("Complete : SyncInvoice");

                    //string strOldCode = dtrSCoor["TAB_OLDCODE"].ToString().TrimEnd();
                    //switch (dtrSCoor["TAB_NAME"].ToString().TrimEnd().ToUpper())
                    //{
                    //    case "GLREF":
                    //        bllResult = pmSyncGLRef("", strCode, dtrSCoor["FCSKID"].ToString());
                    //        WriteLog2("Complete : SyncInvoice");
                    //        break;
                    //}

                    if (bllResult)
                    {
                        //pobjSQLUtil.SetPara(new object[] { dtrSCoor["TAB_NAME"].ToString(), strCorp, strCode });
                        //pobjSQLUtil.SQLExec("delete from SYNCDBDATA where TAB_NAME = ? and QCCORP = ? and TAB_CODE = ? ", ref strErrorMsg);
                    }

                }

                message = "Complete Process :" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ");

                //}
                //catch (Exception ex)
                //{
                //    //MessageBox.Show(ex.StackTrace.ToString() + "\r\n" + ex.Message);
                //    WriteLog(ex);
                //}
            }
            else
            {
                //MessageBox.Show("ErrorMsg:\r\n" + strErrorMsg);
                //if (strErrorMsg != string.Empty)
                //{
                WriteLog2("ErrorMsg: " + strErrorMsg);
                //}

                //message = "Cannot Connect to Server";
                //evt.WriteEntry(message, EventLogEntryType.Error);
            }


        }

        //Import Process
        private void pm2Process(string inDir, string inBakDir)
        {
            this.WriteLog2("ImportGLRef : Start Create Dir : " + inDir);
            string[] filePaths = Directory.GetFiles(inDir, "*.XML");
            //if (!Directory.Exists(inDir + "\\BAK\\")) {
            //    Directory.CreateDirectory(inDir + "\\BAK\\");
            //}
            if (!Directory.Exists(inBakDir + "\\BAK\\"))
            {
                Directory.CreateDirectory(inBakDir + "\\BAK\\");
            }

            if (!Directory.Exists(inDir + "\\ERROR\\"))
            {
                Directory.CreateDirectory(inDir + "\\ERROR\\");
            }
            this.WriteLog2("ImportGLRef : Finish Create Dir");

            string strErrorMsg = "";
            foreach (string strFileName in filePaths)
            {

                string strFileNameOnly = strFileName.Replace(inDir + "\\", "");
                strFileNameOnly = strFileNameOnly.Substring(0, strFileNameOnly.Length - 4);
                string strFile_XSD = strFileName.Substring(0, strFileName.Length - 4) + ".XSD";
                string strFile_XML = strFileName;

                DataSet dsImport = new DataSet();
                dsImport.ReadXmlSchema(strFile_XSD);
                dsImport.ReadXml(strFile_XML);

                this.WriteLog2("ImportGLRef : File- " + strFile_XML);

                string strImportMsg = "";
                if (pmImportGLRef(dsImport, ref strImportMsg))
                {
                    //TODO: Import 1 GLRef

                    //Move fiel to BAK
                    //this.pmMoveFile(strFile_XSD, inDir + "\\BAK\\" + strFileNameOnly + ".XSD", ref strErrorMsg);
                    //this.pmMoveFile(strFile_XML, inDir + "\\BAK\\" + strFileNameOnly + ".XML", ref strErrorMsg);
                    this.pmMoveFile(strFile_XSD, inBakDir + "\\BAK\\" + strFileNameOnly + ".XSD", ref strErrorMsg);
                    this.pmMoveFile(strFile_XML, inBakDir + "\\BAK\\" + strFileNameOnly + ".XML", ref strErrorMsg);
                }
                else
                {
                    //Move fiel to ERROR
                    this.WriteLog2("ImportGLRef : Error - " + strImportMsg);
                    this.pmMoveFile(strFile_XSD, inDir + "\\ERROR\\" + strFileNameOnly + ".XSD", ref strErrorMsg);
                    this.pmMoveFile(strFile_XML, inDir + "\\ERROR\\" + strFileNameOnly + ".XML", ref strErrorMsg);
                }
            }
        }

        private bool pmImportGLRef(DataSet inSource, ref string ioErrorMsg)
        {

            bool bllResult = false;
            DataTable dtGLRef = inSource.Tables["QGLRef"];
            if (pmInitValueBeforeSave(inSource, ref ioErrorMsg))
            {
                #region "Save GLRef"
                //Check Has Code and Delete before Save

                foreach (DataRow dtrGLRef in inSource.Tables["QGLRef"].Rows)
                {
                    //DataRow dtrGLRef = inSource.Tables["QGLRef"].Rows[0];
                    string strErrorMsg = "";
                    string strDel_GLRef = "";
                    if (pmChkHasCode(ref strDel_GLRef, dtrGLRef["fcCorp"].ToString(), dtrGLRef["fcBranch"].ToString(), dtrGLRef["fcRefType"].ToString(), dtrGLRef["fcBook"].ToString(), dtrGLRef["fcCode"].ToString()))
                    {
                        this.pmDeleteData(strDel_GLRef);
                    }
                }
                //SaveGLRef
                pmSaveGLRef(inSource);
                bllResult = true;
                #endregion
            }
            return bllResult;
        }

        private bool XXXpmImportGLRef(DataSet inSource, ref string ioErrorMsg)
        {

            bool bllResult = false;
            if (pmInitValueBeforeSave(inSource, ref ioErrorMsg))
            {
                //Check Has Code and Delete before Save
                DataRow dtrGLRef = inSource.Tables["QGLRef"].Rows[0];
                string strErrorMsg = "";
                string strDel_GLRef = "";
                if (pmChkHasCode(ref strDel_GLRef, dtrGLRef["fcCorp"].ToString(), dtrGLRef["fcBranch"].ToString(), dtrGLRef["fcRefType"].ToString(), dtrGLRef["fcBook"].ToString(), dtrGLRef["fcCode"].ToString()))
                {

                    this.pmDeleteData(strDel_GLRef);

                    //WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
                    //object[] pAPara = pAPara = new object[1] { strDel_GLRef };
                    //pobjSQLUtil.SQLExec("delete from REFPROD where fcGLRef = ?", ref strErrorMsg);

                    //pAPara = new object[1] { strDel_GLRef };
                    //pobjSQLUtil.SQLExec("delete from GLREF where fcSkid = ?", ref strErrorMsg);
                }
                //SaveGLRef
                if (dtrGLRef["CTYPE"].ToString() != "D")
                {
                    pmSaveGLRef(inSource);
                }
                bllResult = true;
            }
            return bllResult;
        }

        private void pmDeleteData(string inGLRef)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = pAPara = new object[1] { inGLRef };
            pobjSQLUtil.SetPara(pAPara);
            pobjSQLUtil.SQLExec("delete from REFPROD where fcGLRef = ?", ref strErrorMsg);

            pAPara = new object[1] { inGLRef };
            pobjSQLUtil.SetPara(pAPara);
            pobjSQLUtil.SQLExec("delete from GLREF where fcSkid = ?", ref strErrorMsg);
        }

        private void pmSaveGLRef(DataSet inSource)
        {

            bool bllIsCommit = false;
            bool bllResult = false;

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            this.mSaveDBAgent.AppID = "$%";
            this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                string strError_Issue = "";
                foreach (DataRow dtrGLRef in inSource.Tables["QGLRef"].Rows)
                {
                    this.pmSave1GLRef(inSource, dtrGLRef, ref strError_Issue);
                }

                this.mdbTran.Commit();

            }
            catch (Exception ex)
            {
                this.mdbTran.Rollback();
                this.WriteLog2("ImportGLRef : pmSaveGLRef : Error - " + ex.Message);

                //App.WriteEventsLog(ex);

#if xd_RUNMODE_DEBUG
                MessageBox.Show(ex.Message);
#endif
            }
            finally
            {
                this.mdbConn.Close();
            }

        }


        private string mstrEditRowID_Load = "";
        private string mstrEditRowID = "";
        private string mstrHTable = "GLREF";
        private string mstrITable = "REFPROD";

        private bool pmSave1GLRef(DataSet inSource, DataRow inGLRef, ref string ioErrorMsg)
        {
            bool bllResult = true;
            bool bllIsNewRow = false;
            string strErrorMsg = "";
            string strRowID = "";

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            DataRow dtrCurrRow = null;
            //DataRow dtrGLRef = inSource.Tables["QGLRef"].Rows[0];
            DataRow dtrGLRef = inGLRef;

            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrHTable, this.mstrHTable, "select * from " + this.mstrHTable + " where 0=1", ref strErrorMsg);
            strRowID = App.mRunRowID(this.mstrHTable);
            this.mstrEditRowID = strRowID;
            bllIsNewRow = true;

            this.mstrEditRowID_Load = dtrGLRef["fcSkid"].ToString();
            dtrGLRef["fcSkid"] = this.mstrEditRowID;

            object[] pAPara = null;
            strRowID = this.mstrEditRowID;

            this.pmSaveRefProd(inSource, inGLRef, ref strErrorMsg);

            //if (this.mFormEditMode == UIHelper.AppFormState.Insert)
            //{
            //    this.pmUpdateStockForInsert();
            //    this.pmUpdateCoorBal(dtrGLRef, this.mintUpdBalSign);
            //}

            //Update GLRef
            string strSQLUpdateStr = "";

            DataRow dtrSaveGLRef = this.dtsDataEnv.Tables[this.mstrHTable].NewRow();
            CopyDataRow2(dtrGLRef, ref dtrSaveGLRef);

            cDBMSAgent.GenUpdateSQLString(dtrSaveGLRef, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
            //strSQLUpdateStr = strSQLUpdateStr.Replace("QGLREF", "GLREF");

            this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
            //if (strErrorMsg.Trim() != string.Empty)
            //{
            //    MessageBox.Show(strErrorMsg);
            //}

            return bllResult;
        }

        private void CopyDataRow2(DataRow inSource, ref DataRow inDest)
        {
            for (int intCnt = 0; intCnt < inSource.Table.Columns.Count; intCnt++)
            {
                DataColumn dtcUpdStruc = inSource.Table.Columns[intCnt];
                if (HasField(inSource.Table.Columns[intCnt].ColumnName, inDest))
                {
                    DataColumn dtcDestCol = inDest.Table.Columns[inSource.Table.Columns[intCnt].ColumnName];
                    if (dtcDestCol.ReadOnly == true)
                        continue;

                    switch (dtcUpdStruc.DataType.ToString().ToUpper())
                    {
                        case "SYSTEM.STRING":
                            inDest[dtcDestCol] = (Convert.IsDBNull(inSource[dtcUpdStruc]) ? "" : inSource[dtcUpdStruc].ToString());
                            break;
                        case "SYSTEM.INT16":
                            inDest[dtcDestCol] = (Convert.IsDBNull(inSource[dtcUpdStruc]) ? 0 : (int)inSource[dtcUpdStruc]);
                            break;
                        case "SYSTEM.INT32":
                            inDest[dtcDestCol] = (Convert.IsDBNull(inSource[dtcUpdStruc]) ? 0 : Convert.ToInt32(inSource[dtcUpdStruc]));
                            break;
                        case "SYSTEM.INT64":
                            inDest[dtcDestCol] = (Convert.IsDBNull(inSource[dtcUpdStruc]) ? 0 : Convert.ToInt64(inSource[dtcUpdStruc]));
                            break;
                        case "SYSTEM.DECIMAL":
                            inDest[dtcDestCol] = (Convert.IsDBNull(inSource[dtcUpdStruc]) ? 0 : Convert.ToDecimal(inSource[dtcUpdStruc]));
                            break;
                        case "SYSTEM.DATETIME":
                            if (Convert.IsDBNull(inSource[dtcUpdStruc]) == false)
                                inDest[dtcDestCol] = Convert.ToDateTime(inSource[dtcUpdStruc]);
                            break;
                    }
                }
            }
        }

        private bool HasField(string inChkField, DataRow inSource)
        {
            foreach (DataColumn dtcCheck in inSource.Table.Columns)
            {
                if (dtcCheck.ColumnName.ToUpper().TrimEnd() == inChkField.ToUpper().TrimEnd())
                    return true;
            }
            return false;
        }

        private bool HasField(string inChkField, DataTable inSource)
        {
            foreach (DataColumn dtcCheck in inSource.Columns)
            {
                if (dtcCheck.ColumnName.ToUpper().TrimEnd() == inChkField.ToUpper().TrimEnd())
                    return true;
            }
            return false;
        }

        //private void pmUpdateCoorBal(DataRow inGLRef, int inUpdSign)
        //{
        //    bool bllIsNewRow = false;
        //    string strErrorMsg = "";
        //    DataRow dtrCoorBal;

        //    object[] pAPara = new object[] { inGLRef["fcCorp"].ToString(), inGLRef["fcBranch"].ToString(), SysDef.gc_CUST_TYPE, inGLRef["fcCoor"].ToString() };
        //    if (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "CoorBal", "COORBAL", "select * from CoorBal where fcCorp=? and fcBranch = ? and fcCoorType = ? and fcCoor = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran))
        //    {
        //        dtrCoorBal = this.dtsDataEnv.Tables["CoorBal"].NewRow();
        //        bllIsNewRow = true;
        //        string strRowID = App.mRunRowID("COORBAL");
        //        dtrCoorBal["fcSkid"] = strRowID;
        //        dtrCoorBal["fcCorp"] = inGLRef["fcCorp"].ToString();
        //        dtrCoorBal["fcBranch"] = inGLRef["fcBranch"].ToString();
        //        //dtrCoorBal["fcCoorType"] = SysDef.gc_CUST_TYPE;
        //        dtrCoorBal["fcCoor"] = inGLRef["fcCoor"].ToString();
        //        //dtrCoorBal["fcCreateAp"] = App.AppID;
        //        dtrCoorBal["fnAmt"] = 0;
        //    }
        //    else
        //    {
        //        bllIsNewRow = false;
        //        dtrCoorBal = this.dtsDataEnv.Tables["CoorBal"].Rows[0];
        //    }
        //    dtrCoorBal["fnAmt"] = Convert.ToDecimal(dtrCoorBal["fnAmt"]) + (Convert.ToDecimal(inGLRef["fnAmt"]) + Convert.ToDecimal(inGLRef["fnVatAmt"])) * 1 * inUpdSign;
        //    dtrCoorBal["fcLUpdApp"] = "";
        //    dtrCoorBal["fcEAfterR"] = "E";

        //    //Update CoorBal
        //    string strSQLUpdateStr = "";
        //    pAPara = null;
        //    cDBMSAgent.GenUpdateSQLString(dtrCoorBal, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
        //    this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

        //}

        private bool pmSaveRefProd(DataSet inSource, DataRow inGLRef, ref string ioErrorMsg)
        {
            bool bllResult = true;
            string strRowID = "";
            string strErrorMsg = "";
            object[] pAPara = null;
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //DataRow dtrGLRef = inSource.Tables["QGLRef"].Rows[0];
            DataRow dtrGLRef = inGLRef;

            //MessageBox.Show(this.dtsDataEnv.Tables[this.mstrTemPd3].Rows.Count.ToString());
            DataRow[] dtrAllTemPd = inSource.Tables["QRefProd"].Select("FCGLREF = '" + this.mstrEditRowID_Load + "'");
            foreach (DataRow dtrTemPd in dtrAllTemPd)
            {
                bool bllIsNewRow = true;

                pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrITable, this.mstrITable, "select * from " + this.mstrITable + " where 0=1", ref strErrorMsg);
                DataRow dtrTemPd3 = this.dtsDataEnv.Tables[this.mstrITable].NewRow();

                strRowID = App.mRunRowID("RefProd");
                dtrTemPd["fcSkid"] = strRowID;
                dtrTemPd["fcGLRef"] = dtrGLRef["fcSkid"].ToString();
                dtrTemPd["fcCorp"] = dtrGLRef["fcCorp"].ToString();
                dtrTemPd["fcBranch"] = dtrGLRef["fcBranch"].ToString();
                dtrTemPd["fcGLRef"] = dtrGLRef["fcSkid"].ToString();
                dtrTemPd["fcCoor"] = dtrGLRef["fcCoor"].ToString();
                //dtrTemPd["fcProd"] = inTemPd["cProd"].ToString();
                //dtrTemPd["fcUm"] = inTemPd["cUOM"].ToString();

                //dtrTemPd["fcUmStd"] = inTemPd["cUOMStd"].ToString();
                //dtrTemPd["fcStUm"] = inTemPd["cUOMStd"].ToString();
                //dtrTemPd["fcStUmStd"] = inTemPd["cUOMStd"].ToString();

                //dtrTemPd["fcSect"] = inTemPd["cSect"].ToString();
                //dtrTemPd["fcDept"] = inTemPd["cDept"].ToString();

                //dtrTemPd["fcJob"] = inTemPd["cJob"].ToString();
                //dtrTemPd["fcProj"] = inTemPd["cProj"].ToString();

                CopyDataRow2(dtrTemPd, ref dtrTemPd3);

                string strSQLUpdateStr = "";
                cDBMSAgent.GenUpdateSQLString(dtrTemPd3, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
                strSQLUpdateStr = strSQLUpdateStr.Replace("QREFPROD", "REFPROD");
                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

            }
            return true;
        }

        private Decimal pmToDecimal(object inConvert)
        {
            Decimal decRetVal = 0;
            //decRetVal = (Convert.IsDBNull(inConvert) ? 0 : Convert.ToDecimal(inConvert));
            if (Convert.IsDBNull(inConvert))
            {
                decRetVal = 0;
            }
            else
            {
                decRetVal = Convert.ToDecimal(inConvert);
            }
            return decRetVal;
        }

        private bool pmToDateTime(string inDateStr, int inBDDate, ref DateTime ioDate)
        {
            bool bllResult = false;
            DateTime dttDate = DateTime.Now;
            try
            {
                string[] aDate = inDateStr.Split("/".ToCharArray());
                if (aDate.Length == 3)
                {
                    int intYear = Convert.ToInt32(aDate[2]);
                    if (inBDDate == 1)
                    {
                        intYear = Convert.ToInt32(aDate[2]) - 543;
                    }

                    ioDate = new DateTime(Convert.ToInt32(aDate[2]), Convert.ToInt32(aDate[1]), Convert.ToInt32(aDate[0]));
                }
            }
            catch (Exception ex)
            {
                bllResult = false;
            }
            return bllResult;
        }

        private bool pmChkHasCode(ref string ioGLRef, string inCorp, string inBranch, string inRefType, string inBook, string inCode)
        {
            bool bllResult = false;
            string strErrorMsg = "";
            string strSQLStr = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            object[] pAPara = new object[] { inCorp, inBranch, inRefType, inBook, inCode };
            strSQLStr = "select fcSkid,fcCode,fcATStep from GLRef where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcBook = ? and fcCode = ? ";
            pobjSQLUtil.SetPara(pAPara);
            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QDupCode", "GLTRAN", strSQLStr, ref strErrorMsg))
            {
                ioGLRef = this.dtsDataEnv.Tables["QDupCode"].Rows[0]["fcSkid"].ToString();
                bllResult = true;
            }
            return bllResult;
        }

        private bool pmInitValueBeforeSave(DataSet inSource, ref string ioErrorMsg)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            bool bllResult = true;

            string strCorp = "";
            string strBranch = "";
            string strBook = "";
            string strCoor = "";
            string strFrWHouse = "";
            string strToWHouse = "";
            string strFrWHLoca = "";
            string strToWHLoca = "";

            foreach (DataRow dtrGLRef in inSource.Tables["QGLRef"].Rows)
            {
                //DataTable dtrGLRef = inSource.Tables["QGLRef"];

                #region "Load GLRef Value"
                //string strQcCorp = dtrGLRef.Rows[0]["QcCorp"].ToString().TrimEnd();
                //string strQcBranch = dtrGLRef.Rows[0]["QcBranch"].ToString().TrimEnd();
                //string strQcBook = dtrGLRef.Rows[0]["QcBook"].ToString().TrimEnd();
                //string strRfType = dtrGLRef.Rows[0]["fcRfType"].ToString().TrimEnd();
                //string strRefType = dtrGLRef.Rows[0]["fcRefType"].ToString().TrimEnd();
                //string strQcCoor = dtrGLRef.Rows[0]["QcCoor"].ToString().TrimEnd();
                //string strQcFrWHouse = dtrGLRef.Rows[0]["QcFrWHouse"].ToString().TrimEnd();
                //string strQcToWHouse = dtrGLRef.Rows[0]["QcToWHouse"].ToString().TrimEnd();
                ////string strQcFrWHLoca = dtrGLRef.Rows[0]["QcFrWHLoca"].ToString().TrimEnd();
                ////string strQcToWHLoca = dtrGLRef.Rows[0]["QcCorp"].ToString().TrimEnd();

                string strQcCorp = dtrGLRef["QcCorp"].ToString().TrimEnd();
                string strQcBranch = dtrGLRef["QcBranch"].ToString().TrimEnd();
                string strQcBook = dtrGLRef["QcBook"].ToString().TrimEnd();
                string strRfType = dtrGLRef["fcRfType"].ToString().TrimEnd();
                string strRefType = dtrGLRef["fcRefType"].ToString().TrimEnd();
                string strQcCoor = dtrGLRef["QcCoor"].ToString().TrimEnd();
                string strQnCoor = dtrGLRef["QnCoor"].ToString().TrimEnd();
                string strQcDeliCoor = dtrGLRef["QcDeliCoor"].ToString().TrimEnd();
                string strQcFrWHouse = dtrGLRef["QcFrWHouse"].ToString().TrimEnd();
                string strQcToWHouse = dtrGLRef["QcToWHouse"].ToString().TrimEnd();

                strCorp = "";
                strBranch = "";
                strBook = "";
                strCoor = "";
                strFrWHouse = "";
                strToWHouse = "";
                strFrWHLoca = "";
                strToWHLoca = "";

                objSQLHelper.SetPara(new object[] { strQcCorp });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCorp", "CORP", "select fcSkid from CORP where fcCode = ?", ref strErrorMsg))
                {
                    dtrGLRef["fcCorp"] = this.dtsDataEnv.Tables["QCorp"].Rows[0]["fcSkid"].ToString();
                    strCorp = this.dtsDataEnv.Tables["QCorp"].Rows[0]["fcSkid"].ToString();
                }
                else
                {
                    ioErrorMsg = "ไม่พบบริษัทรหัส " + strQcCorp + " !";
                    bllResult = false;
                }

                objSQLHelper.SetPara(new object[] { strCorp, strQcBranch });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBranch", "BRANCH", "select fcSkid from Branch where fcCorp = ? and fcCode = ?", ref strErrorMsg))
                {
                    DataRow dtrBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0];
                    dtrGLRef["fcBranch"] = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcSkid"].ToString();
                    strBranch = this.dtsDataEnv.Tables["QBranch"].Rows[0]["fcSkid"].ToString();
                }
                else
                {
                    ioErrorMsg = "ไม่พบสาขารหัส " + strQcBranch + " !";
                    bllResult = false;
                }

                objSQLHelper.SetPara(new object[] { strCorp, strBranch, strRefType, strQcBook });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QBook", "BOOK", "select fcSkid from Book where fcCorp = ? and fcBranch = ? and fcReftype= ? and fcCode = ?", ref strErrorMsg))
                {
                    dtrGLRef["fcBook"] = this.dtsDataEnv.Tables["QBook"].Rows[0]["fcSkid"].ToString();
                }
                else
                {
                    ioErrorMsg = "ไม่พบเล่มเอกสารรหัส " + strQcBook + " !";
                    bllResult = false;
                }

                if (strQcCoor.Trim() != "")
                {
                    string fcCoorType = (("S,E,F").IndexOf(strRfType) > -1 ? "fcIsCust" : "fcIsSupp");
                    objSQLHelper.SetPara(new object[] { strCorp, strQcCoor });
                    if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QCoor", "COOR", "select fcSkid from COOR where fcCorp = ? and " + fcCoorType + " = 'Y' and fcCode = ?", ref strErrorMsg))
                    {
                        dtrGLRef["fcCoor"] = this.dtsDataEnv.Tables["QCoor"].Rows[0]["fcSkid"].ToString();
                    }
                    else
                    {
                        //ioErrorMsg = "ไม่พบรหัสลูกค้า " + strQcCoor + " !";
                        string strCoorID = "";
                        bllResult = pmNewCoor(true, strCorp, strQcCoor, strQcCoor, ref strCoorID);
                        if (bllResult)
                        {
                            dtrGLRef["fcCoor"] = strCoorID;
                        }
                        else
                        {
                            ioErrorMsg = "ไม่พบรหัสลูกค้า " + strQcCoor + " !";
                            bllResult = false;
                        }
                    }
                }


                if (strQcFrWHouse.Trim() != "")
                {
                    objSQLHelper.SetPara(new object[] { strCorp, strBranch, strQcFrWHouse });
                    if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select fcSkid from WHOUSE where fcCorp = ? and fcBranch = ? and fcCode = ?", ref strErrorMsg))
                    {
                        dtrGLRef["fcFrWHouse"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcSkid"].ToString();
                    }
                    else
                    {
                        ioErrorMsg = "ไม่พบคลังรหัส " + strQcFrWHouse + " !";
                        bllResult = false;
                    }
                }

                if (strQcToWHouse.Trim() != "")
                {
                    objSQLHelper.SetPara(new object[] { strCorp, strBranch, strQcToWHouse });
                    if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select fcSkid from WHOUSE where fcCorp = ? and fcBranch = ? and fcCode = ?", ref strErrorMsg))
                    {
                        dtrGLRef["fcToWHouse"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcSkid"].ToString();
                    }
                    else
                    {
                        ioErrorMsg = "ไม่พบคลังรหัส " + strQcToWHouse + " !";
                        bllResult = false;
                    }
                }
                #endregion
            }

            DataTable dtrRefProd = inSource.Tables["QRefProd"];
            foreach (DataRow dtrPRefProd in dtrRefProd.Rows)
            {
                #region "Load RefProd Value"
                //QCWHOUSE,WHLOCATION.FCCODE as QCWHLOCA
                string strQcWHouse = dtrPRefProd["QCWHOUSE"].ToString();
                string strQcWHLoca = dtrPRefProd["QCWHLOCA"].ToString();
                string strQcProd = dtrPRefProd["QCPROD"].ToString();

                string strQcUM = dtrPRefProd["QCUM"].ToString();
                string strQcUMStd = dtrPRefProd["QCUMSTD"].ToString();
                string strQcStUM = dtrPRefProd["QCSTUM"].ToString();
                string strQcStUMStd = dtrPRefProd["QCSTUMSTD"].ToString();

                #region "Load UM"
                objSQLHelper.SetPara(new object[] { strCorp, strQcUM });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid from UM where fcCorp = ? and fcCode = ?", ref strErrorMsg))
                {
                    dtrPRefProd["fcUM"] = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                }
                else
                {
                    ioErrorMsg = "ไม่พบหน่วยนับรหัส " + strQcUM + " !";
                    bllResult = false;
                }

                objSQLHelper.SetPara(new object[] { strCorp, strQcUMStd });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid from UM where fcCorp = ? and fcCode = ?", ref strErrorMsg))
                {
                    dtrPRefProd["fcUmStd"] = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                }
                else
                {
                    ioErrorMsg = "ไม่พบหน่วยนับรหัส " + strQcUMStd + " !";
                    bllResult = false;
                }

                objSQLHelper.SetPara(new object[] { strCorp, strQcStUM });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid from UM where fcCorp = ? and fcCode = ?", ref strErrorMsg))
                {
                    dtrPRefProd["fcStUm"] = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                }
                else
                {
                    ioErrorMsg = "ไม่พบหน่วยนับรหัส " + strQcStUM + " !";
                    bllResult = false;
                }

                objSQLHelper.SetPara(new object[] { strCorp, strQcStUMStd });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select fcSkid from UM where fcCorp = ? and fcCode = ?", ref strErrorMsg))
                {
                    dtrPRefProd["fcStUmStd"] = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcSkid"].ToString();
                }
                else
                {
                    ioErrorMsg = "ไม่พบหน่วยนับรหัส " + strQcStUMStd + " !";
                    bllResult = false;
                }
                #endregion

                objSQLHelper.SetPara(new object[] { strCorp, strBranch, strQcWHouse });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select * from WHOUSE where fcCorp = ? and fcBranch = ? and fcCode = ?", ref strErrorMsg))
                {
                    dtrPRefProd["fcWHouse"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcSkid"].ToString();
                    if (strQcWHLoca == "")
                    {
                        //if ("S,E,F".IndexOf(strRfType) > -1)
                        //{
                        //    dtrPRefProd["fcWHLoca"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["FCWHLOCA_OUTPUT"].ToString();
                        //}
                        //else
                        //{
                        //    dtrPRefProd["fcWHLoca"] = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["FCWHLOCA_INPUT"].ToString();
                        //}
                    }
                }
                else
                {
                    ioErrorMsg = "ไม่พบคลังรหัส " + strQcWHouse + " !";
                    bllResult = false;
                }

                if (strQcWHLoca.Trim() != string.Empty)
                {
                    objSQLHelper.SetPara(new object[] { strCorp, strQcWHLoca });
                    if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QWHLoca", "WHLOCATION", "select fcSkid from WHLOCATION where fcCorp = ? and fcCode = ?", ref strErrorMsg))
                    {
                        dtrPRefProd["fcWHLoca"] = this.dtsDataEnv.Tables["QWHLoca"].Rows[0]["fcSkid"].ToString();
                    }
                    else
                    {
                        ioErrorMsg = "ไม่พบที่เก็บรหัส " + strQcWHLoca + " !";
                        bllResult = false;
                    }
                }

                objSQLHelper.SetPara(new object[] { strCorp, strQcProd });
                if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select fcSkid from PROD where fcCorp = ? and fcCode = ?", ref strErrorMsg))
                {
                    dtrPRefProd["fcProd"] = this.dtsDataEnv.Tables["QProd"].Rows[0]["fcSkid"].ToString();
                }
                else
                {
                    ioErrorMsg = "ไม่พบสินค้ารหัส " + strQcProd + " !";
                    bllResult = false;
                }
                #endregion
            }


            if (bllResult)
            {
                //this.mstrAtStep = SysDef.gc_ATSTEP_VOUCHER_WAIT;
            }

            return bllResult;
        }

        #region "Export Service"
        private bool pmSyncGLRef(DateTime inBegDate, DateTime inEndDate)
        {

            bool bllResult = false;
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            WS.Data.Agents.cDBMSAgent pobjSQLUtil_Del = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString2, App.DatabaseReside);
            string strSQLStr = "";

            strSQLStr = " select CORP.FCCODE as QCCORP,COOR.FCCODE as QCCOOR,COOR.FCNAME as QNCOOR ";
            strSQLStr += " ,SECT.FCCODE as QCSECT,DEPT.FCCODE as QCDEPT ";
            strSQLStr += " ,JOB.FCCODE as QCJOB,PROJ.FCCODE as QCPROJ ";
            strSQLStr += " ,C2.FCCODE as QCDELICOOR,C2.FCNAME as QNCOOR ";

            strSQLStr += " ,BRANCH.FCCODE as QCBRANCH ";
            strSQLStr += " ,BOOK.FCCODE as QCBOOK ";
            strSQLStr += " ,FRWH.FCCODE as QCFRWHOUSE ";
            strSQLStr += " ,TOWH.FCCODE as QCTOWHOUSE ";

            strSQLStr += " ,GLREF.* from GLREF ";
            strSQLStr += " left join COOR on COOR.FCSKID = GLREF.FCCOOR ";
            strSQLStr += " left join COOR C2 on C2.FCSKID = GLREF.FCDELICOOR ";
            strSQLStr += " left join SECT on SECT.FCSKID = GLREF.FCSECT ";
            strSQLStr += " left join DEPT on DEPT.FCSKID = SECT.FCDEPT ";
            strSQLStr += " left join JOB on JOB.FCSKID = GLREF.FCJOB ";
            strSQLStr += " left join PROJ on PROJ.FCSKID = JOB.FCPROJ ";

            strSQLStr += " inner join CORP on CORP.FCSKID = GLREF.FCCORP ";
            strSQLStr += " inner join BRANCH on BRANCH.FCSKID = GLREF.FCBRANCH ";
            strSQLStr += " inner join BOOK on BOOK.FCSKID = GLREF.FCBOOK ";
            strSQLStr += " left join WHOUSE FRWH on FRWH.FCSKID = GLREF.FCFRWHOUSE ";
            strSQLStr += " left join WHOUSE TOWH on TOWH.FCSKID = GLREF.FCTOWHOUSE ";

            //strSQLStr += " where GLREF.FCSKID in (select TAB_ROWID from SYNCDBDATA where TAB_NAME = 'GLREF') ";
            //strSQLStr += " where GLREF.FCSKID = ? ";
            strSQLStr += " where GLREF.FCCORP = ? and GLREF.FCRFTYPE in ('S','E','F') and GLREF.FDDATE between ? and ?  and FCISHOLD <> 'Y' order by GLREF.FDDATE,GLREF.FCCODE";

            string strSQL_RefProd = "select PROD.FCCODE as QCPROD,CORP.FCCODE as QCCORP";
            strSQL_RefProd += " ,SECT.FCCODE as QCSECT,DEPT.FCCODE as QCDEPT ";
            strSQL_RefProd += " ,JOB.FCCODE as QCJOB,PROJ.FCCODE as QCPROJ ";
            strSQL_RefProd += " ,WHOUSE.FCCODE as QCWHOUSE,WHLOCATION.FCCODE as QCWHLOCA ";
            strSQL_RefProd += " ,U1.FCCODE as QCUM,U2.FCCODE as QCUMSTD,U3.FCCODE as QCSTUM,U4.FCCODE as QCSTUMSTD ";

            strSQL_RefProd += " ,REFPROD.* from REFPROD";
            strSQL_RefProd += " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQL_RefProd += " left join SECT on SECT.FCSKID = REFPROD.FCSECT ";
            strSQL_RefProd += " left join DEPT on DEPT.FCSKID = SECT.FCDEPT ";
            strSQL_RefProd += " left join JOB on JOB.FCSKID = REFPROD.FCJOB ";
            strSQL_RefProd += " left join PROJ on PROJ.FCSKID = JOB.FCPROJ ";
            strSQL_RefProd += " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQL_RefProd += " left join WHLOCATION on WHLOCATION.FCSKID = REFPROD.FCWHLOCA ";
            strSQL_RefProd += " left join CORP on CORP.FCSKID = REFPROD.FCCORP ";
            strSQL_RefProd += " left join UM U1 on U1.FCSKID = REFPROD.FCUM ";
            strSQL_RefProd += " left join UM U2 on U2.FCSKID = REFPROD.FCUMSTD ";
            strSQL_RefProd += " left join UM U3 on U3.FCSKID = REFPROD.FCSTUM ";
            strSQL_RefProd += " left join UM U4 on U4.FCSKID = REFPROD.FCSTUMSTD ";

            //strSQL_RefProd += " where REFPROD.FCGLREF = ? order by REFPROD.FCSEQ";
            strSQL_RefProd += " where REFPROD.FCGLREF in (select FCSKID from GLREF where GLREF.FCCORP = ? and GLREF.FCRFTYPE in ('S','E','F') and GLREF.FDDATE between ? and ? and FCISHOLD <> 'Y' )";
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, inBegDate, inEndDate });
            if (pobjSQLUtil.SQLExec(ref dtsDataEnvEx, "QGLRef", "GLREF", strSQLStr, ref strErrorMsg))
            {

                pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, inBegDate, inEndDate });
                if (pobjSQLUtil.SQLExec(ref dtsDataEnvEx, "QRefProd", "REFPROD", strSQL_RefProd, ref strErrorMsg))
                {
                }

                try
                {

                    string strSyncFilePath = this.mstrExportFolder;

                    DataRow dtrGLRef = dtsDataEnvEx.Tables["QGLRef"].Rows[0];
                    string strPrefix_FileName = Guid.NewGuid().ToString();

                    string[] aDir = this.mstrExportFolder.Split(",".ToCharArray());
                    foreach (string strExDir in aDir)
                    {
                        strSyncFilePath = strExDir;
                        dtsDataEnvEx.WriteXmlSchema(strSyncFilePath + "\\" + strPrefix_FileName + ".xsd");
                        dtsDataEnvEx.WriteXml(strSyncFilePath + "\\" + strPrefix_FileName + ".xml");
                    }

                    bllResult = true;
                }
                catch (Exception ex)
                { }

            }
            else
            {
                bllResult = true;
            }
            return bllResult;
        }
        #endregion

        private bool pmGenUpdateSQLString(DataRow inDataStruc, string inPKField, bool inIsNewRowState, ref string ioSQLUpdateStr, ref object[] ioSQLPara, bool inAddTabName)
        {

            ArrayList pASQLPara = new ArrayList();
            bool bllIsNewRowState = inIsNewRowState;
            string strPKField = inPKField.TrimEnd().ToUpper();
            string strRetSQLStr = "";
            string strTableName = inDataStruc.Table.TableName.ToUpper();
            DataRow dtrStrucRow = inDataStruc;
            bool mNotUpperSQLExecString = false;

            if (mNotUpperSQLExecString)
            {
                strPKField = inPKField.TrimEnd();
                strTableName = inDataStruc.Table.TableName;
            }
            else
            {
                strPKField = inPKField.TrimEnd().ToUpper();
                strTableName = inDataStruc.Table.TableName.ToUpper();
            }

            //ioSQLPara = new Object[inDataStruc.Table.Columns.Count];
            if (bllIsNewRowState == true)
            {
                strRetSQLStr = "insert into " + strTableName + "( ";
            }
            else
            {
                strRetSQLStr = "update " + strTableName + " set ";
            }

            for (int intCnt = 0; intCnt < inDataStruc.Table.Columns.Count; intCnt++)
            {
                DataColumn dtcUpdStruc = inDataStruc.Table.Columns[intCnt];
                if ((!bllIsNewRowState && dtcUpdStruc.ColumnName.ToUpper() == strPKField)
                    || (Convert.IsDBNull(dtrStrucRow[dtcUpdStruc]))
                    || dtcUpdStruc.DataType.ToString().ToUpper() == "SYSTEM.BYTE[]")
                {
                }
                else
                {
                    int intCnt2 = (bllIsNewRowState == true ? intCnt : intCnt - 1);
                    switch (dtcUpdStruc.DataType.ToString().ToUpper())
                    {
                        case "SYSTEM.STRING":
                            string strVal = dtrStrucRow[dtcUpdStruc].ToString();
                            if (strVal.TrimEnd() == DBEnum.NullString)
                            {
                                pASQLPara.Add(Convert.DBNull);
                            }
                            else
                            {
                                pASQLPara.Add(dtrStrucRow[dtcUpdStruc].ToString());
                            }
                            break;
                        case "SYSTEM.INT16":
                            pASQLPara.Add(Convert.ToInt16(dtrStrucRow[dtcUpdStruc]));
                            break;
                        case "SYSTEM.INT32":
                            pASQLPara.Add(Convert.ToInt32(dtrStrucRow[dtcUpdStruc]));
                            break;
                        case "SYSTEM.INT64":
                            pASQLPara.Add(Convert.ToInt64(dtrStrucRow[dtcUpdStruc]));
                            break;
                        case "SYSTEM.DECIMAL":
                            pASQLPara.Add(Convert.ToDecimal(dtrStrucRow[dtcUpdStruc]));
                            break;
                        case "SYSTEM.DOUBLE":
                            pASQLPara.Add(Convert.ToDouble(dtrStrucRow[dtcUpdStruc]));
                            break;
                        case "SYSTEM.DATETIME":
                            DateTime dttVal = Convert.ToDateTime(dtrStrucRow[dtcUpdStruc]);
                            //string strSQLDateVal = pmGetSQLDateTime(dttVal);
                            if (dttVal.CompareTo(DBEnum.NullDate) == 0)
                            {
                                pASQLPara.Add(Convert.DBNull);
                            }
                            else
                            {
                                pASQLPara.Add(dttVal);
                                //pASQLPara.Add(strSQLDateVal);
                            }
                            break;
                        case "SYSTEM.BYTE[]":
                            break;
                    }
                    strRetSQLStr += (inAddTabName ? strTableName + "." : "") + dtcUpdStruc.ColumnName + (bllIsNewRowState == true ? ", " : " = ?, ");
                }
            }
            if (bllIsNewRowState == true)
            {
                string strQMPading = StringHelper.Replicate("?,", pASQLPara.Count);
                strRetSQLStr = StringHelper.Left(strRetSQLStr, strRetSQLStr.Length - 2) + ") values " + "(" + StringHelper.Left(strQMPading, strQMPading.Length - 1) + ")";
                ioSQLPara = new Object[pASQLPara.Count];
                pASQLPara.CopyTo(ioSQLPara);
            }
            else
            {
                strRetSQLStr = StringHelper.Left(strRetSQLStr, strRetSQLStr.Length - 2) + " where " + strPKField + " = ?";
                ioSQLPara = new Object[pASQLPara.Count + 1];
                if (dtrStrucRow[strPKField] is System.String)
                {
                    ioSQLPara[pASQLPara.Count] = dtrStrucRow[strPKField].ToString();
                }
                else if (dtrStrucRow[strPKField] is System.Int32)
                {
                    ioSQLPara[pASQLPara.Count] = Convert.ToInt32(dtrStrucRow[strPKField]);
                }
                pASQLPara.CopyTo(ioSQLPara);
            }
            ioSQLUpdateStr = strRetSQLStr;
            return true;
        }

        private bool pmNewCoor(bool inIsAdd, string inCorp, string inCode, string inName, ref string ioRowID)
        {
            string strErrorMsg = "";
            bool bllIsNewRow = false;
            bool bllIsCommit = false;

            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string gcTemStr01 = "";

            string strAddr11 = "";
            string strAddr21 = "";
            string strAddr31 = "";
            string strAddr12 = "";
            string strAddr22 = "";
            string strAddr32 = "";

            string strTel = "";
            string strFax = "";
            string strRemark = "";
            string strRemark2 = "";
            string strTaxID = "";

            string strCTAddr11 = "";
            string strCTAddr21 = "";
            string strCTAddr31 = "";
            string strCTAddr12 = "";
            string strCTAddr22 = "";
            string strCTAddr32 = "";
            string strCTZip = "";
            string strRemarkLayH = "";
            string strCause = "";
            string strMTel = "";
            string strMTaxID = "";
            string strWebSite = "";
            string strEMail = "";
            string strCTTel = "";
            string strCTFax = "";
            string strQnBizType = "";

            this.mstrEditRowID = "";

            DataRow dtrSaveInfo = null;
            objSQLHelper.SQLExec(ref this.dtsDataEnv, "COOR", "COOR", "select * from COOR where 0=1", ref strErrorMsg);
            bllIsNewRow = true;
            WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
            this.mstrEditRowID = objConn.RunRowID("COOR");

            dtrSaveInfo = this.dtsDataEnv.Tables["COOR"].NewRow();

            //gcTemStr01 = (Convert.IsDBNull(dtrSaveInfo["fmMemData"]) ? "" : dtrSaveInfo["fmMemData"].ToString().TrimEnd());
            //gcTemStr01 += (Convert.IsDBNull(dtrSaveInfo["fmMemData2"]) ? "" : dtrSaveInfo["fmMemData2"].ToString().TrimEnd());
            //gcTemStr01 += (Convert.IsDBNull(dtrSaveInfo["fmMemData3"]) ? "" : dtrSaveInfo["fmMemData3"].ToString().TrimEnd());
            //gcTemStr01 += (Convert.IsDBNull(dtrSaveInfo["fmMemData4"]) ? "" : dtrSaveInfo["fmMemData4"].ToString().TrimEnd());
            //gcTemStr01 += (Convert.IsDBNull(dtrSaveInfo["fmMemData5"]) ? "" : dtrSaveInfo["fmMemData5"].ToString().TrimEnd());

            //strAddr11 = BizRule.GetMemData(gcTemStr01, x_CMAd11);
            //strAddr21 = BizRule.GetMemData(gcTemStr01, x_CMAd21);
            //strAddr31 = BizRule.GetMemData(gcTemStr01, x_CMAd31);
            //strAddr12 = BizRule.GetMemData(gcTemStr01, x_CMAd12);
            //strAddr22 = BizRule.GetMemData(gcTemStr01, x_CMAd22);
            //strAddr32 = BizRule.GetMemData(gcTemStr01, x_CMAd32);

            //strFax = BizRule.GetMemData(gcTemStr01, x_CMFax);
            //strMTel = BizRule.GetMemData(gcTemStr01, x_CMMoBileTel);

            //strMTaxID = BizRule.GetMemData(gcTemStr01, xdCMMId);
            //strTaxID = BizRule.GetMemData(gcTemStr01, x_CMTaxId);

            //strCTAddr11 = BizRule.GetMemData(gcTemStr01, xdCMCtAd11);
            //strCTAddr21 = BizRule.GetMemData(gcTemStr01, xdCMCtAd21);
            //strCTAddr31 = BizRule.GetMemData(gcTemStr01, xdCMCtAd31);

            //strCTAddr12 = BizRule.GetMemData(gcTemStr01, xdCMCtAd12);
            //strCTAddr22 = BizRule.GetMemData(gcTemStr01, xdCMCtAd22);
            //strCTAddr32 = BizRule.GetMemData(gcTemStr01, xdCMCtAd32);
            //strCTZip = BizRule.GetMemData(gcTemStr01, x_CMCtZip);
            //strCTTel = BizRule.GetMemData(gcTemStr01, x_CMCtTel);
            //strCTFax = BizRule.GetMemData(gcTemStr01, x_CMCtFax);

            //strRemark = BizRule.GetMemData(gcTemStr01, x_CMRem);
            //strRemark2 = BizRule.GetMemData(gcTemStr01, x_CMRem2);
            //strRemarkLayH = BizRule.GetMemData(gcTemStr01, x_CMRemLayH);
            //strWebSite = BizRule.GetMemData(gcTemStr01, x_CMWebSite);
            //strEMail = BizRule.GetMemData(gcTemStr01, x_CMEmail);

            //strCause = BizRule.GetMemData(gcTemStr01, x_CMCauseBlkLst);

            //gcTemStr01 += BizRule.SetMemData(strAddr21.Trim(), x_CMAd21);


            string strCrGrp = "";
            ioRowID = this.mstrEditRowID;
            dtrSaveInfo["fcSkid"] = this.mstrEditRowID;
            dtrSaveInfo["fcCorp"] = inCorp;

            //Page Edit 1
            dtrSaveInfo["fcCode"] = inCode;
            dtrSaveInfo["fcName"] = inName;
            dtrSaveInfo["fcName2"] = inName;
            dtrSaveInfo["fcFChr"] = AppUtil.StringHelper.GetFChr(inName);
            dtrSaveInfo["fcSName"] = inName;
            dtrSaveInfo["fcSName2"] = inName;
            dtrSaveInfo["fcTel"] = "";
            dtrSaveInfo["fcContactN"] = "";
            dtrSaveInfo["fnCredTerm"] = 0;
            dtrSaveInfo["fnCredLim"] = 0;
            dtrSaveInfo["fcZip"] = "";
            dtrSaveInfo["fcCrGrp"] = strCrGrp;

            //strCrGrp
            strAddr11 = "";
            strAddr21 = "";
            strAddr31 = "";

            strTel = "";
            strFax = "";
            strTaxID = "";
            strEMail = "";

            dtrSaveInfo["fcIsCust"] = "Y";
            dtrSaveInfo["fcIsSupp"] = "";
            dtrSaveInfo["fcFChrS"] = AppUtil.StringHelper.GetFChr(inName);

            //if (strFirstContact.EditValue != null)
            //    dtrSaveInfo["fdFstCont"] = objSQLHelper.GetDBServerDateTime();
            //else
            //    dtrSaveInfo["fdFstCont"] = Convert.DBNull;

            //Page Edit 2
            dtrSaveInfo["fcPersonty"] = "";


            string gcTemStr02, gcTemStr03, gcTemStr04, gcTemStr05, gcTemStr06;
            int intVarCharLen = 500;
            gcTemStr02 = (gcTemStr01.Length > 0 ? StringHelper.SubStr(gcTemStr01, 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr03 = (gcTemStr01.Length > intVarCharLen ? StringHelper.SubStr(gcTemStr01, intVarCharLen + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr04 = (gcTemStr01.Length > intVarCharLen * 2 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 2 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr05 = (gcTemStr01.Length > intVarCharLen * 3 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 3 + 1, intVarCharLen) : DBEnum.NullString);
            gcTemStr06 = (gcTemStr01.Length > intVarCharLen * 4 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 4 + 1, intVarCharLen) : DBEnum.NullString);

            dtrSaveInfo["fmMemData"] = gcTemStr02;
            dtrSaveInfo["fmMemData2"] = gcTemStr03;
            dtrSaveInfo["fmMemData3"] = gcTemStr04;
            dtrSaveInfo["fmMemData4"] = gcTemStr05;
            dtrSaveInfo["fmMemData5"] = gcTemStr06;

            //dtrSaveInfo["fcLastUpdBy"] = App.FMAppUserID;
            dtrSaveInfo["ftLastUpd"] = objSQLHelper.GetDBServerDateTime();
            dtrSaveInfo["ftLastEdit"] = objSQLHelper.GetDBServerDateTime();

            this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
            //this.mSaveDBAgent.AppID = App.AppID;
            this.mSaveDBAgent.AppID = "M2";
            this.mdbConn = this.mSaveDBAgent.GetDBConnection();

            bool bllResult = false;
            try
            {

                this.mdbConn.Open();
                this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

                string strSQLUpdateStr = "";
                object[] pAPara = null;
                cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);

                this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

                this.mdbTran.Commit();
                bllIsCommit = true;
                bllResult = true;
            }
            //catch (System.Data.OleDb.OleDbException ex)
            catch (Exception ex)
            {
                if (!bllIsCommit)
                {
                    this.mdbTran.Rollback();
                }
                //App.WriteEventsLog(ex);

                //string strMsg = "";
                //strMsg += dtrSaveInfo["fcCode"].ToString() + "X\r\n";
                //strMsg += dtrSaveInfo["fcName"].ToString() + "X\r\n";
                //strMsg += dtrSaveInfo["fcName2"].ToString() + "X\r\n";
                //strMsg += dtrSaveInfo["fcFChr"].ToString() + "X\r\n";
                //strMsg += dtrSaveInfo["fcSName"].ToString() + "X\r\n";
                //strMsg += dtrSaveInfo["fcSName2"].ToString() + "X\r\n";
                //strMsg += dtrSaveInfo["fcTel"].ToString() + "X\r\n";

                //strMsg += dtrSaveInfo["fmMemData"].ToString() + "X\r\n";
                //strMsg += dtrSaveInfo["fmMemData2"].ToString() + "X\r\n";
                //strMsg += dtrSaveInfo["fmMemData3"].ToString() + "X\r\n";
                //strMsg += dtrSaveInfo["fmMemData4"].ToString() + "X\r\n";
                //strMsg += dtrSaveInfo["fmMemData5"].ToString() + "X\r\n";

                //MessageBox.Show(strMsg);

#if xd_RUNMODE_DEBUG
				MessageBox.Show("Message : " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
#endif
            }

            finally
            {
                this.mdbConn.Close();
            }
            return bllResult;
        }

        private string xd_EVENTS_FILENAME = "Event.Log";
        private void WriteLog(Exception e)
        {
#if xd_RUNMODE_DEBUG
			MessageBox.Show(e.Exception.TargetSite.ToString() + "\n" + e.Exception.Message.ToString(), "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + xd_EVENTS_FILENAME;
            System.IO.FileMode OpenFileMode = (System.IO.File.Exists(strFileName) ? System.IO.FileMode.Append : System.IO.FileMode.OpenOrCreate);
            System.IO.FileStream myLog = new System.IO.FileStream(strFileName, OpenFileMode, System.IO.FileAccess.Write, System.IO.FileShare.Write);
            TextWriterTraceListener myListener = new TextWriterTraceListener(myLog);
            Trace.Listeners.Add(myListener);
            Trace.WriteLine("");
            Trace.WriteLine(DateTime.Now.ToString("dd/MMMM/yyyy HH:mm:ss"));
            Trace.WriteLine("Exception Message :");
            Trace.WriteLine(e.Message.ToString());
            Trace.WriteLine("Exception StackTrace :");
            Trace.WriteLine(e.StackTrace.ToString());
            Trace.WriteLine("OS : " + System.Environment.OSVersion.ToString());
            Trace.WriteLine("User Name : " + System.Environment.UserName);
            Trace.WriteLine("Machine Name : " + System.Environment.MachineName);

            Trace.Flush();
            myLog.Close();
            myLog.Dispose();
        }

        private void WriteLog2(string e)
        {
#if xd_RUNMODE_DEBUG
			MessageBox.Show(e.Exception.TargetSite.ToString() + "\n" + e.Exception.Message.ToString(), "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "\\" + xd_EVENTS_FILENAME;
            System.IO.FileMode OpenFileMode = (System.IO.File.Exists(strFileName) ? System.IO.FileMode.Append : System.IO.FileMode.OpenOrCreate);
            System.IO.FileStream myLog = new System.IO.FileStream(strFileName, OpenFileMode, System.IO.FileAccess.Write, System.IO.FileShare.Write);
            TextWriterTraceListener myListener = new TextWriterTraceListener(myLog);
            Trace.Listeners.Add(myListener);
            Trace.WriteLine("");
            Trace.WriteLine(DateTime.Now.ToString("dd/MMMM/yyyy HH:mm:ss"));
            Trace.WriteLine("Log Message :" + e);
            //Trace.WriteLine(e);
            //Trace.WriteLine("Exception StackTrace :");
            //Trace.WriteLine(e.StackTrace.ToString());
            //Trace.WriteLine("OS : " + System.Environment.OSVersion.ToString());
            //Trace.WriteLine("User Name : " + System.Environment.UserName);
            //Trace.WriteLine("Machine Name : " + System.Environment.MachineName);

            Trace.Flush();
            //myLog.Close();
            //myLog.Dispose();
        }


        private bool pmMoveFile(string inSourceFile, string inDestFile, ref string ioErrorMsg)
        {
            bool bllResult = false;
            try
            {
                if (System.IO.File.Exists(inDestFile))
                {
                    //string strBakFileName = inDestFile + ".bak";
                    //System.IO.File.Copy(inDestFile, strBakFileName);
                    if (System.IO.File.Exists(inSourceFile))  //ป้องกันกรณี Error ที่ Source ไม่มี file ทำให้ Dest หายไปด้วย
                    {
                        System.IO.File.Delete(inDestFile);
                    }
                }

                System.IO.File.Move(inSourceFile, inDestFile);
                bllResult = true;
            }
            catch (Exception ex)
            {
                ioErrorMsg = ex.Message;
                bllResult = false;
            }

            return bllResult;
        }

        private void WriteErrorLog(string e)
        {
#if xd_RUNMODE_DEBUG
			MessageBox.Show(e.Exception.TargetSite.ToString() + "\n" + e.Exception.Message.ToString(), "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
#endif
            string strFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Error.Log";
            System.IO.FileMode OpenFileMode = (System.IO.File.Exists(strFileName) ? System.IO.FileMode.Append : System.IO.FileMode.OpenOrCreate);
            System.IO.FileStream myLog = new System.IO.FileStream(strFileName, OpenFileMode, System.IO.FileAccess.Write, System.IO.FileShare.Write);
            TextWriterTraceListener myListener = new TextWriterTraceListener(myLog);
            Trace.Listeners.Add(myListener);
            Trace.WriteLine("");
            Trace.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            Trace.WriteLine("Log Message :" + e);
            //Trace.WriteLine(e);
            //Trace.WriteLine("Exception StackTrace :");
            //Trace.WriteLine(e.StackTrace.ToString());
            //Trace.WriteLine("OS : " + System.Environment.OSVersion.ToString());
            //Trace.WriteLine("User Name : " + System.Environment.UserName);
            //Trace.WriteLine("Machine Name : " + System.Environment.MachineName);

            Trace.Flush();
            //myLog.Close();
            //myLog.Dispose();
        }


    }
}
