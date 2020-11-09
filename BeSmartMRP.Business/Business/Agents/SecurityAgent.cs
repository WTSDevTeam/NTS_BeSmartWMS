
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using AppUtil;
using AppUtil.SecureHelper;
using BeSmartMRP.Business.Entity;

namespace BeSmartMRP.Business.Agents
{
    public class SecurityAgent
    {

        private const string xd_Encrypt_Key = "ITULOSHCETHTLAEW";
        private const string xd_Encrypt_InitVector = "785688410";
        private const int xd_PASSWORD_LENGTH = 10;
        private const int xd_CIPHER_LENGTH = 24;

        private static string gc_AuthType_BOOK = "B";
        private static string gc_AuthType_SECT = "S";
        private static string gc_AuthType_MENU = "M";

        private const WSEncryption.Symmetric.Provider xd_ENCRYPT_PROVIDER = WSEncryption.Symmetric.Provider.TripleDES;

        public bool CheckLogin(WS.Data.Agents.cDBMSAgent inSQLHelper, string inUserName, string inPassword, ref string ioAppLogin, ref string ioErrorMsg, ref int ioNLimit)
        {
            bool bllResult = false;
            DataSet dtsDataEnv = new DataSet();

            ioAppLogin = "";
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = inSQLHelper;

            pobjSQLUtil.SetPara(new object[] { inUserName.TrimEnd() });
            if (pobjSQLUtil.SQLExec(ref dtsDataEnv, "QEmplR", "EMPLR", "select * from AppLogin where CLOGIN = ? ", ref strErrorMsg))
            {
                foreach (DataRow dtrEmpl in dtsDataEnv.Tables["QEmplR"].Rows)
                {
                    string strHPwd = dtrEmpl["cPwd"].ToString();
                    bool bllIsOK = CompareCipherText(pobjSQLUtil, inPassword.ToUpper().TrimEnd(), dtrEmpl["cRowID"].ToString());
                    if (bllIsOK)
                    {
                        bllResult = true;
                        ioAppLogin = dtrEmpl["cRowID"].ToString();
                    }
                    else
                    {
                        strErrorMsg = "Invalid password !";
                    }
                }
            }
            else
            {
                strErrorMsg = "Not found User name !";
            }
            ioErrorMsg = strErrorMsg;
            if (!bllResult)
            {
                ioNLimit--;
            }
            return bllResult;
        }

        public bool XXXCheckPermission(BeSmartMRP.Business.Entity.AuthenType inChkType, string inAppUser, string inTaskName)
        {
            //return true;
            return this.pmCheckPermission(inTaskName, inChkType, inAppUser);
        }

        public bool CreateCipherText(string inClearText, ref string ioCipherText)
        {
            WSEncryption.Symmetric sym = new WSEncryption.Symmetric(xd_ENCRYPT_PROVIDER, true);
            WSEncryption.Data key = new WSEncryption.Data(xd_Encrypt_Key);
            WSEncryption.Data encryptedData;

            string strClearText = AppUtil.StringHelper.PadR(inClearText, xd_PASSWORD_LENGTH, Convert.ToChar(" "));
            encryptedData = sym.Encrypt(new WSEncryption.Data(strClearText), key);
            ioCipherText = encryptedData.ToBase64();
            return true;
        }

        public bool CompareCipherText(WS.Data.Agents.cDBMSAgent inSQLHelper, string inClearText, string inAppLogin)
        {
            bool bllResult = false;
            DataSet dtsDataEnv = new DataSet();

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = inSQLHelper;

            pobjSQLUtil.SetPara(new object[] { inAppLogin });
            if (pobjSQLUtil.SQLExec(ref dtsDataEnv, "QEmplR", "EMPLR", "select * from AppLogin where CROWID = ? ", ref strErrorMsg))
            {

                string strClearText = AppUtil.StringHelper.PadR(inClearText, xd_PASSWORD_LENGTH, Convert.ToChar(" "));
                string strHPwd = dtsDataEnv.Tables["QEmplR"].Rows[0]["cPwd"].ToString();
                strHPwd = StringHelper.Left(strHPwd, xd_CIPHER_LENGTH);

                WSEncryption.Symmetric sym = new WSEncryption.Symmetric(xd_ENCRYPT_PROVIDER, true);
                WSEncryption.Data key = new WSEncryption.Data(xd_Encrypt_Key);
                WSEncryption.Data encryptedData = new WSEncryption.Data();

                encryptedData.Base64 = strHPwd;
                WSEncryption.Data decryptedData = null;
                decryptedData = sym.Decrypt(encryptedData, key);

                bllResult = (strClearText == decryptedData.ToString());
            }

            return bllResult;
        }

        private WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
        public WS.Data.Agents.cDBMSAgent SQLHelper
        {
            set { mSaveDBAgent = value; }
        }

        private DataSet dtsDataEnv = new DataSet();
        public bool XXXCheckPermission(string inTaskName, Business.Entity.AuthenType inChkType, string inAppUser)
        {
            //return true;
            return this.pmCheckPermission(inTaskName, inChkType, inAppUser);
        }

        public bool CheckPermission(string inTaskName, Business.Entity.AuthenType inChkType, string inLoginName, string inAppUser)
        {
            if (inLoginName.TrimEnd() != "BIGBOSS")
            {
                return this.pmCheckPermission(inTaskName, inChkType, inAppUser);
            }
            else
            {
                return true;
            }
        }

        public bool CheckPermissionBySect(Business.Entity.AuthenType inChkType, string inLoginName, string inAppUser, string inSect)
        {
            if (inLoginName.TrimEnd() != "BIGBOSS")
            {
                return this.pmCheckPermissionBySect(inChkType, inAppUser, inSect);
            }
            else
            {
                return true;
            }
        }

        public bool CheckPermissionByBook(Business.Entity.AuthenType inChkType, string inLoginName, string inAppUser, string inRefType, string inBook)
        {
            if (inLoginName.TrimEnd() != "BIGBOSS")
            {
                return this.pmCheckPermissionByBook(inChkType, inAppUser, inRefType, inBook);
            }
            else
            {
                return true;
            }
        }

        public bool CheckPermission_Add(string inTaskName, Business.Entity.AuthenType inChkType, string inLoginName, string inAppUser)
        {
            if (inLoginName.TrimEnd() != "BIGBOSS")
            {
                return this.pmCheckPermission_Add(inTaskName, inChkType, inAppUser);
            }
            else
            {
                return true;
            }
        }

        private bool pmCheckPermissionBySect(Business.Entity.AuthenType inChkType, string inAppUser, string inSect)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = this.mSaveDBAgent;

            //bool bllResult = true;
            bool bllResult = false;

            objSQLHelper.SetPara(new object[] { gc_AuthType_SECT, inAppUser, inSect });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QAuthDet", "AppAuthDet", "select * from AppAuthDet where cType = ? and cAppLogin = ? and cSect = ?", ref strErrorMsg))
            {
                DataRow dtrAuthDet = this.dtsDataEnv.Tables["QAuthDet"].Rows[0];
                //bllResult = this.pmCheckPerm3(inChkType, dtrAuthDet);
                bllResult = this.pmGetRight(inChkType, dtrAuthDet);
            }

            return bllResult;
        }

        private bool pmCheckPermissionByBook(Business.Entity.AuthenType inChkType, string inAppUser, string inRefType, string inWsBook)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = this.mSaveDBAgent;

            bool bllResult = true;

            objSQLHelper.SetPara(new object[] { gc_AuthType_BOOK, inAppUser, inRefType, inWsBook });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QAuthDet", "AppAuthDet", "select * from AppAuthDet where cType = ? and cAppLogin = ? and cRefType = ? and cBook = ?", ref strErrorMsg))
            {
                DataRow dtrAuthDet = this.dtsDataEnv.Tables["QAuthDet"].Rows[0];
                bllResult = this.pmGetRight(inChkType, dtrAuthDet);
            }

            return bllResult;
        }

        private bool pmCheckPermission(string inTaskName, Business.Entity.AuthenType inChkType, string inAppUser)
        {
            bool bllResult = false;
            string strErrorMsg = "";
            bool bllHasSetRight = false;

            string strSQLStr = "select AppEMRole.cAppRole , AppRole.cLevel from AppEMRole left join AppRole on AppRole.cRowID = AppEMRole.cAppRole where AppEMRole.cEmpl ";
            strSQLStr += " in (select CROWID from APPEMPL where CRCODE in (select CRCODE from APPLOGIN where CROWID = ? ))";
            WS.Data.Agents.cDBMSAgent objSQLHelper = this.mSaveDBAgent;
            objSQLHelper.SetPara(new object[] { inAppUser });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QEmpPost", "EMPPOST", strSQLStr, ref strErrorMsg))
            {

                foreach (DataRow dtrEMPPost in this.dtsDataEnv.Tables["QEmpPost"].Rows)
                {
                    ////กำหนดสิทธิ์ระดับ Login (Login)
                    //bllResult = this.pmCheckLevelC(inTaskName, inChkType, dtrEMPPost["cAppRole"].ToString(), inAppUser, ref bllHasSetRight);
                    //if (bllHasSetRight) return bllResult;

                    //กำหนดสิทธิ์ระดับ ตำแหน่ง (Role)
                    bllResult = this.pmCheckLevelB(inTaskName, inChkType, dtrEMPPost["cAppRole"].ToString(), ref bllHasSetRight);
                    if (bllHasSetRight) return bllResult;

                    ////กำหนดสิทธิ์ระดับ Level (Type)
                    //bllResult = this.pmCheckLevelA(inTaskName, inChkType, dtrEMPPost["cLevel"].ToString(), ref bllHasSetRight);
                    //if (bllHasSetRight) return bllResult;

                    ////กำหนดสิทธิ์ระดับ Base Config (Load from Resource File)
                    //bllResult = this.pmCheckLevel0(inTaskName, inChkType, dtrEMPPost["cLevel"].ToString(), ref bllHasSetRight);
                    //if (bllHasSetRight) return bllResult;

                }

            }
            return bllResult;
        }

        private bool pmCheckLevelC(string inTaskName, Business.Entity.AuthenType inChkType, string inPost, string inAppUser, ref bool ioHasSetRight)
        {
            bool bllResult = false;
            ioHasSetRight = false;

            string strErrorMsg = "";
            bool bllHasSetRight = false;

            string strSQLStr = "select * from APPAUTHC where CTASKNAME = ? and CPOST = ? and CEMPLR = ? ";
            WS.Data.Agents.cDBMSAgent objSQLHelper = this.mSaveDBAgent;
            //objSQLHelper.SetPara(new object[] { inTaskName.PadRight(10), inPost, inAppUser });
            objSQLHelper.SetPara(new object[] { inTaskName, inPost, inAppUser });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QAppAuth", "APPAUTHC", strSQLStr, ref strErrorMsg))
            {
                bllHasSetRight = true;
                DataRow dtrAppAuth = this.dtsDataEnv.Tables["QAppAuth"].Rows[0];
                bllResult = this.pmGetRight(inChkType, dtrAppAuth);
            }
            ioHasSetRight = bllHasSetRight;
            return bllResult;
        }

        private bool pmCheckLevelB(string inTaskName, Business.Entity.AuthenType inChkType, string inPost, ref bool ioHasSetRight)
        {
            bool bllResult = false;
            ioHasSetRight = false;

            string strErrorMsg = "";
            bool bllHasSetRight = false;

            string strSQLStr = "select * from APPAUTHB where CTASKNAME = ? and CPOST = ? ";
            WS.Data.Agents.cDBMSAgent objSQLHelper = this.mSaveDBAgent;
            //objSQLHelper.SetPara(new object[] { inTaskName.PadRight(10), inPost });
            objSQLHelper.SetPara(new object[] { inTaskName, inPost });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QAppAuth", "APPAUTHB", strSQLStr, ref strErrorMsg))
            {
                bllHasSetRight = true;
                DataRow dtrAppAuth = this.dtsDataEnv.Tables["QAppAuth"].Rows[0];
                bllResult = this.pmGetRight(inChkType, dtrAppAuth);
            }
            ioHasSetRight = bllHasSetRight;
            return bllResult;
        }

        private bool pmCheckLevelA(string inTaskName, Business.Entity.AuthenType inChkType, string inLevel, ref bool ioHasSetRight)
        {
            bool bllResult = false;
            ioHasSetRight = false;

            string strErrorMsg = "";
            bool bllHasSetRight = false;

            string strSQLStr = "select * from SRT_APPAUTHA where CTASKNAME = ? and CLEVEL = ? ";
            WS.Data.Agents.cDBMSAgent objSQLHelper = this.mSaveDBAgent;
            //objSQLHelper.SetPara(new object[] { inTaskName.PadRight(10), inLevel });
            objSQLHelper.SetPara(new object[] { inTaskName, inLevel });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QAppAuth", "APPAUTHA", strSQLStr, ref strErrorMsg))
            {
                bllHasSetRight = true;
                DataRow dtrAppAuth = this.dtsDataEnv.Tables["QAppAuth"].Rows[0];
                bllResult = this.pmGetRight(inChkType, dtrAppAuth);
            }
            ioHasSetRight = bllHasSetRight;
            return bllResult;
        }

        private bool pmCheckLevel0(string inTaskName, Business.Entity.AuthenType inChkType, string inLevel, ref bool ioHasSetRight)
        {
            bool bllResult = false;
            ioHasSetRight = false;

            string strErrorMsg = "";
            bool bllHasSetRight = false;

            string strSQLStr = "select * from APPOBJ where CTASKNAME = ? ";
            WS.Data.Agents.cDBMSAgent objSQLHelper = this.mSaveDBAgent;
            //objSQLHelper.SetPara(new object[] { inTaskName.PadRight(10), inLevel });
            objSQLHelper.SetPara(new object[] { inTaskName, inLevel });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QAppAuth", "APPAUTHA", strSQLStr, ref strErrorMsg))
            {
                bllHasSetRight = true;
                DataRow dtrAppAuth = this.dtsDataEnv.Tables["QAppAuth"].Rows[0];
                bllResult = this.pmGetRight0(inChkType, inLevel, dtrAppAuth);
            }
            ioHasSetRight = bllHasSetRight;
            return bllResult;
        }

        private bool pmGetRight(AuthenType inChkType, DataRow inAuth)
        {
            bool bllResult = false;
            DataRow dtrAppAuthz = inAuth;
            switch (inChkType)
            {
                case AuthenType.Visible:
                    //bllResult = (dtrAppAuthz["CRIGVSIBLE"].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.AskPassword:
                    bllResult = false;
                    break;
                case AuthenType.CanAccess:
                    bllResult = (dtrAppAuthz["CACCESS"].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.CanInsert:
                    bllResult = (dtrAppAuthz["CINSERT"].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.CanEdit:
                    bllResult = (dtrAppAuthz["CEDIT"].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.CanDelete:
                    bllResult = (dtrAppAuthz["CDELETE"].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.CanPrint:
                    bllResult = (dtrAppAuthz["CCANPRINT"].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.CanExport:
                    //bllResult = (dtrAppAuthz["CRIGEXPORT"].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.CanApprove:
                    bllResult = (dtrAppAuthz["CAPPROVE"].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.CanApprove2:
                    bllResult = (dtrAppAuthz["CAPPROVE2"].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.IsAllow:
                    bllResult = (dtrAppAuthz["CALLOW"].ToString() == "Y" ? true : false);
                    break;
            }
            return bllResult;
        }

        private bool pmGetRight0(AuthenType inChkType, string inLevel, DataRow inAuth)
        {
            bool bllResult = false;
            DataRow dtrAppAuthz = inAuth;
            switch (inChkType)
            {
                case AuthenType.Visible:
                    //bllResult = (dtrAppAuthz["CRIGVSIBLE"].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.AskPassword:
                    bllResult = false;
                    break;
                case AuthenType.CanAccess:
                    bllResult = (dtrAppAuthz["CACCESS_" + inLevel].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.CanInsert:
                    bllResult = (dtrAppAuthz["CINSERT_" + inLevel].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.CanEdit:
                    bllResult = (dtrAppAuthz["CEDIT_" + inLevel].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.CanDelete:
                    bllResult = (dtrAppAuthz["CDELETE_" + inLevel].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.CanPrint:
                    //bllResult = (dtrAppAuthz["CPRINT"].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.CanExport:
                    //bllResult = (dtrAppAuthz["CRIGEXPORT"].ToString() == "Y" ? true : false);
                    break;
                case AuthenType.CanApprove:
                    bllResult = (dtrAppAuthz["CAPPROVE"].ToString() == "Y" ? true : false);
                    break;
            }
            return bllResult;
        }

        private bool pmCheckPermission_Add(string inTaskName, Business.Entity.AuthenType inChkType, string inAppUser)
        {
            bool bllResult = false;
            string strErrorMsg = "";
            bool bllHasSetRight = false;

            string strSQLStr = "select AppEMRole.cAppRole , AppRole.cLevel from AppEMRole left join AppRole on AppRole.cRowID = AppEMRole.cAppRole where AppEMRole.cEmpl ";
            strSQLStr += " in (select CROWID from APPEMPL where CRCODE in (select CRCODE from APPLOGIN where CROWID = ? ))";
            WS.Data.Agents.cDBMSAgent objSQLHelper = this.mSaveDBAgent;
            objSQLHelper.SetPara(new object[] { inAppUser });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QEmpPost", "EMPPOST", strSQLStr, ref strErrorMsg))
            {

                foreach (DataRow dtrEMPPost in this.dtsDataEnv.Tables["QEmpPost"].Rows)
                {
                    ////กำหนดสิทธิ์ระดับ Login (Login)
                    //bllResult = this.pmCheckLevelC(inTaskName, inChkType, dtrEMPPost["cAppRole"].ToString(), inAppUser, ref bllHasSetRight);
                    //if (bllHasSetRight) return bllResult;

                    //กำหนดสิทธิ์ระดับ ตำแหน่ง (Role)
                    bllResult = this.pmCheckLevelB_Add1(inTaskName, inChkType, dtrEMPPost["cAppRole"].ToString(), ref bllHasSetRight);
                    if (bllHasSetRight) return bllResult;

                }

            }
            return bllResult;
        }

        private bool pmCheckLevelB_Add1(string inTaskName, Business.Entity.AuthenType inChkType, string inPost, ref bool ioHasSetRight)
        {
            bool bllResult = false;
            ioHasSetRight = false;

            string strErrorMsg = "";
            bool bllHasSetRight = false;

            string strSQLStr = "select * from APPAUTHB_ADD1 where CTASKNAME = ? and CPOST = ? ";
            WS.Data.Agents.cDBMSAgent objSQLHelper = this.mSaveDBAgent;
            //objSQLHelper.SetPara(new object[] { inTaskName.PadRight(10), inPost });
            objSQLHelper.SetPara(new object[] { inTaskName, inPost });
            if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QAppAuth", "APPAUTHB", strSQLStr, ref strErrorMsg))
            {
                bllHasSetRight = true;
                DataRow dtrAppAuth = this.dtsDataEnv.Tables["QAppAuth"].Rows[0];
                bllResult = this.pmGetRight(inChkType, dtrAppAuth);
            }
            ioHasSetRight = bllHasSetRight;
            return bllResult;
        }

    }

}
