
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using WS.Data;
using WS.Data.Agents;

namespace BeSmartMRP.Data
{

    public class DBChkUpdate
    {

        private string mstrConnectionString = "";
        private DBMSType mDatabaseReside = DBMSType.MSSQLServer;

        public DBChkUpdate(string inConnectionString, DBMSType inDBType)
        {
            this.mstrConnectionString = inConnectionString;
            this.mDatabaseReside = inDBType;
        }

        public bool CheckUpdate()
        {
            return this.pmCheckUpdate();
        }

        private bool pmCheckUpdate()
        {
            bool bllResult = true;

            cDBMSAgent objSQLHelper = new cDBMSAgent(this.mstrConnectionString, this.mDatabaseReside);

            //bllResult = bllResult && this.pmUpdate_2009_12_02(objSQLHelper);
            //bllResult = bllResult && this.pmUpdate_2009_12_07(objSQLHelper);
            //bllResult = bllResult && this.pmUpdate_2009_12_14(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_01_04(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_01_05(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_01_07(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_01_08(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_01_11(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_01_12(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_01_25(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_01_26(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_02_02(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_02_08(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_02_15(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_02_17(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_02_22(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_03_09(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_03_16(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_04_09(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_05_05(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_05_07(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_05_24(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_06_22(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_09_27(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2010_10_26(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2011_07_04(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2012_03_09(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2012_04_26(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2012_05_03(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2012_07_08(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2013_08_21(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2014_07_22(objSQLHelper);
            //bllResult = bllResult && this.pmUpdate_2014_08_24(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2014_08_31(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2014_09_03(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2015_04_24(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2015_07_16(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2015_11_27(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2016_06_06(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2016_06_15(objSQLHelper);
            bllResult = bllResult && this.pmUpdate_2016_08_19(objSQLHelper);

            //

            return bllResult;
        }

        private bool pmUpdate_2009_12_02(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";
            cDBMSAgent objSQLHelper = inSQLHelper;
            //Alter Column
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_PD ADD CREMARK2 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_PD ADD CREMARK3 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_PD ADD CREMARK4 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_PD ADD CREMARK5 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_PD ADD CREMARK6 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_PD ADD CREMARK7 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_PD ADD CREMARK8 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_PD ADD CREMARK9 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_PD ADD CREMARK10 CHAR(200) default ' ' not null", ref strErrorMsg);

            //objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CREMARK2 CHAR(200) default ' ' not null", ref strErrorMsg);
            //objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CREMARK3 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CREMARK4 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CREMARK5 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CREMARK6 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CREMARK7 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CREMARK8 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CREMARK9 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CREMARK10 CHAR(200) default ' ' not null", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE APPLOGIN ADD CLOCALE_UI CHAR(5) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE APPLOGIN ADD CLOCALE_RP CHAR(5) default ' ' not null", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE APPOBJ ADD CDESC2 CHAR(150) default ' ' not null", ref strErrorMsg);

            //XAAPPCONFIG
            string strSQLStr = " CREATE TABLE SMAPPCONFIG ( CROWID char(8) default ' ' not null , CREGMOD char(50) default ' ' not null , CLICNO char(50) default ' ' not null , CAPPVERSION char(20) default ' ' not null ";
            strSQLStr += " , CDBVERSION char(20) default ' ' not null , CCON000 char(2) default ' ' not null , CCON001 char(2) default ' ' not null , CCON002 char(2) default ' ' not null ";
            strSQLStr += " , CCON003 char(2) default ' ' not null , CCON004 char(2) default ' ' not null , CCON005 char(2) default ' ' not null , CCON006 char(2) default ' ' not null ";
            strSQLStr += " , CCON007 char(2) default ' ' not null , CCON008 char(2) default ' ' not null , CCON009 char(2) default ' ' not null , CCON010 char(2) default ' ' not null ";
            strSQLStr += " , CCON011 char(10) default ' ' not null , CCON012 char(10) default ' ' not null , CCON013 char(10) default ' ' not null , CCON014 char(10) default ' ' not null ";
            strSQLStr += " , CCON015 char(10) default ' ' not null , CCON016 char(10) default ' ' not null , CCON017 char(10) default ' ' not null , CCONN18 char(10) default ' ' not null ";
            strSQLStr += " , CCONN19 char(10) default ' ' not null , CCONN20 char(10) default ' ' not null , CCON021 char(20) default ' ' not null , CCON022 char(20) default ' ' not null ";
            strSQLStr += " , CCON023 char(20) default ' ' not null , CCON024 char(20) default ' ' not null , CCON025 char(20) default ' ' not null , CCON026 char(20) default ' ' not null ";
            strSQLStr += " , CCON027 char(20) default ' ' not null , CCON028 char(20) default ' ' not null , CCON029 char(20) default ' ' not null , CCON030 char(20) default ' ' not null , Primary key ( CROWID ) )";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);


            this.pmUpdateMenuName(objSQLHelper, "CPAD1", "&Production", "&Production");
            this.pmUpdateMenuName(objSQLHelper, "EMORDER", "บันทึกเอกสาร Manufacturing Order [MO]", "Manufacturing Order Entry [MO]");
            this.pmUpdateMenuName(objSQLHelper, "ESTMREQ", "บันทึกใบขอเบิกสินค้า/วัตถุดิบไปผลิต [MQ]", "Request Material Requistion Note [MQ]");
            this.pmUpdateMenuName(objSQLHelper, "EROUTECD", "บันทึกเอกสารใบส่งงานระหว่าง Work Center [Route Card]", "Route Card Journal");
            this.pmUpdateMenuName(objSQLHelper, "EBGPRE_PS", "บันทึกเอกสารใบปิดการผลิต [MC]", "Close Manufacturing Order [MC]");
            this.pmUpdateMenuName(objSQLHelper, "CPAD2", "&Inventory", "&Inventory");
            this.pmUpdateMenuName(objSQLHelper, "ESTMOVE_WR", "บันทึกใบเบิกวัตถุดิบเพื่อผลิต [WR]", "Raw Materials Issue Slip [WR]");
            this.pmUpdateMenuName(objSQLHelper, "ESTMOVE_RW", "บันทึกใบรับคืนวัตถุดิบจากการผลิต [RW]", "Return Raw Materials [RW]");
            this.pmUpdateMenuName(objSQLHelper, "ESTMOVE_FR", "บันทึกใบรับสินค้าสำเร็จรูป [FR]", "Finished Recive Goods [FR]");
            this.pmUpdateMenuName(objSQLHelper, "ESTMOVE_TR", "บันทึกใบโอนสินค้าข้ามคลัง [TR]", "Tranfer Items between Wharehouses [TR]");
            this.pmUpdateMenuName(objSQLHelper, "EADJ", "บันทึกใบปรับยอดสินค้า [AJ]", "Adjust Material/Product Items [AJ]");
            this.pmUpdateMenuName(objSQLHelper, "CPAD3", "&BOM", "&BOM");
            this.pmUpdateMenuName(objSQLHelper, "EMFBOM", "เพิ่ม/แก้ไขฐานข้อมูล BOM", "Bills of Materials Entry");
            this.pmUpdateMenuName(objSQLHelper, "CPAD4", "&Routing", "&Routing");
            this.pmUpdateMenuName(objSQLHelper, "ERESTYPE_M", "ฐานข้อมูลประเภทเครื่องจักร", "Machine Type Items");
            this.pmUpdateMenuName(objSQLHelper, "ERESTYPE_T", "ฐานข้อมูลประเภทอุปกรณ์", "Tooling Type Items");
            this.pmUpdateMenuName(objSQLHelper, "ERESOURCE_M", "ฐานข้อมูลเครื่องจักร", "Machine Items");
            this.pmUpdateMenuName(objSQLHelper, "ERESOURCE_T", "ฐานข้อมูลอุปกรณ์", "Tooling Item");
            this.pmUpdateMenuName(objSQLHelper, "ESTDOPR", "ฐานข้อมูล Standard Operation", "Standard Operation Items");
            this.pmUpdateMenuName(objSQLHelper, "EMFWKCTR", "ฐานข้อมูล Work Center", "Work Center Entry");
            this.pmUpdateMenuName(objSQLHelper, "CDATABASE", "&Database", "&Database");
            this.pmUpdateMenuName(objSQLHelper, "NSYSTEM", "ระบบงาน", "System Management");
            this.pmUpdateMenuName(objSQLHelper, "EAPPEMPL", "เพิ่ม/แก้ไขพนักงานในระบบ", "Employee Files");
            this.pmUpdateMenuName(objSQLHelper, "EAPPLOGIN", "เพิ่ม/แก้ไขรายชื่อ Login ในระบบ", "Login Files");
            this.pmUpdateMenuName(objSQLHelper, "ESETAUTHEN", "กำหนดสิทธิ์การทำงานของ User", "User Authorization");
            this.pmUpdateMenuName(objSQLHelper, "NRPTCHK3", "รายงานตรวจสอบ", "Print List");
            this.pmUpdateMenuName(objSQLHelper, "PAPPEMPLLST", "พิมพ์ทะเบียน พนักงานในระบบ", "Print Employee List");
            this.pmUpdateMenuName(objSQLHelper, "PAPPLOGINLST", "พิมพ์ทะเบียน ชื่อ Login ในระบบ", "Print Login List");
            this.pmUpdateMenuName(objSQLHelper, "PKEEPLOG", "พิมพ์รายงานการทำงานแต่ละวัน", "Print Application Log File");
            this.pmUpdateMenuName(objSQLHelper, "CAPPCFG", "&Configuration", "&Configuration");
            this.pmUpdateMenuName(objSQLHelper, "CHGPWD", "เปลี่ยนรหัสผ่าน", "Change your password");
            this.pmUpdateMenuName(objSQLHelper, "DBCONFIG", "กำหนดการเชื่อมต่อฐานข้อมูล", "Config Database Connection");

            return true;

        }

        private bool pmUpdate_2009_12_07(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";
            cDBMSAgent objSQLHelper = inSQLHelper;
            string strSQLStr = "";

            //Create Table MFWORDERHD
            strSQLStr = "CREATE TABLE MFWORDERHD ( CROWID char(8) default ' ' not null , CRFTYPE char(1) default ' ' not null , CREFTYPE char(2) default ' ' not null , CCORP char(8) default ' ' not null ";
            strSQLStr += " , CBRANCH char(8) default ' ' not null , CPLANT char(8) default ' ' not null , CDEPT char(8) default ' ' not null , CSECT char(8) default ' ' not null ";
            strSQLStr += ", CJOB char(8) default ' ' not null , CPROJ char(8) default ' ' not null , CSTAT char(1) default ' ' not null , CSTEP char(1) default ' ' not null , CMSTEP char(1) default ' ' not null ";
            strSQLStr += ", CCOOR char(8) default ' ' not null , CCODE char(15) default ' ' not null , CREFNO char(20) default ' ' not null , DDATE datetime  not null , DDUEDATE datetime  ";
            strSQLStr += ", CPRIORITY char(1) default ' ' not null , CMFGBOOK char(8) default ' ' not null , CMFGPROD char(8) default ' ' not null , CBOMHD char(8) default ' ' not null ";
            strSQLStr += ", NMFGQTY decimal(16,4) default 0 not null , NREFQTY decimal(16,4) default 0 not null , CMFGUM char(8) default ' ' not null , NMUMQTY decimal(16,4) default 0 not null ";
            strSQLStr += ", CSCRAP char(20) default ' ' not null , NSCRAPQTY decimal(16,4) default 0 not null , NQTY decimal(16,4) default 0 not null , DSTART datetime  , DFINISH datetime  ";
            strSQLStr += ", CRESOURCE char(8) default ' ' not null , CWKCTRH char(8) default ' ' not null , CMEMDATA varchar(500) default ' ' , CMEMDATA2 varchar(500) default ' ' ";
            strSQLStr += ", CMEMDATA3 varchar(500) default ' ' , CMEMDATA4 varchar(500) default ' ' , CMEMDATA5 varchar(500) default ' ' , CREMARK1 char(200) default ' ' not null ";
            strSQLStr += ", CREMARK2 char(200) default ' ' not null , CREMARK3 char(200) default ' ' not null , CREMARK4 char(200) default ' ' not null , CREMARK5 char(200) default ' ' not null ";
            strSQLStr += ", CREMARK6 char(200) default ' ' not null , CREMARK7 char(200) default ' ' not null , CREMARK8 char(200) default ' ' not null , CREMARK9 char(200) default ' ' not null ";
            strSQLStr += ", CREMARK10 char(200) default ' ' not null , NLOCKED decimal(1,0) default 0 not null , NRECEIVEQTY decimal(16,4) default 0 not null , CBATCHNO char(10) default ' ' not null ";
            strSQLStr += ", CBATCHRUN char(10) default ' ' not null , CLEVEL char(4) default ' ' not null , CMASTER char(8) default ' ' not null , CPARENT char(8) default ' ' not null ";
            strSQLStr += ", CISAPPROVE char(1) default ' ' not null , CAPPROVEBY char(8) default ' ' not null , DAPPROVE datetime default CURRENT_TIMESTAMP , CCREATEBY char(8) default ' ' not null ";
            strSQLStr += ", DCREATE datetime default CURRENT_TIMESTAMP not null , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , CCANCLEBY char(8) default ' ' not null ";
            strSQLStr += ", DCANCEL datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);


            //Create Table MFWORDERIT_PD
            strSQLStr = " CREATE TABLE MFWORDERIT_PD ( CROWID char(8) default ' ' not null , CSTAT char(1) default ' ' not null , CSTEP char(10) default ' ' not null , CCORP char(8) default ' ' not null , CBRANCH char(8) default ' ' not null ";
            strSQLStr += "  , CPLANT char(8) default ' ' not null , CDEPT char(8) default ' ' not null , CSECT char(8) default ' ' not null , CJOB char(8) default ' ' not null , CWORDERH char(8) default ' ' not null ";
            strSQLStr += "  , DDATE datetime  not null , CCOOR char(8) default ' ' not null , CREFPDTYPE char(1) default ' ' not null , CIOTYPE char(1) default ' ' not null , CRFTYPE char(1) default ' ' not null ";
            strSQLStr += "  , CREFTYPE char(2) default ' ' not null , CISMAINPD char(1) default ' ' not null , CPRODTYPE char(1) default ' ' not null , CPROD char(8) default ' ' not null ";
            strSQLStr += "  , CUOM char(8) default ' ' not null , CUOMSTD char(8) default ' ' not null , NUOMQTY decimal(12,4) default 0 not null , NQTY decimal(12,4) default 0 not null ";
            strSQLStr += "  , NPORTION decimal(16,4) default 0 not null , CSTUOM char(8) default ' ' not null , CSTUOMSTD char(8) default ' ' not null , NSTUOMQTY decimal(12,4) default 0 not null ";
            strSQLStr += "  , NSTQTY decimal(12,4) default 0 not null , NPRICE decimal(15,4) default 0 not null , NCOST decimal(15,4) default 0 not null , NDISCAMT decimal(15,4) default 0 not null ";
            strSQLStr += "  , NDISCPCN decimal(6,2) default 0 not null , CSEQ char(2) default ' ' not null , CLOT char(20) default ' ' not null , CWHOUSE char(8) default ' ' not null ";
            strSQLStr += "  , CRESOURCE char(8) default ' ' not null , CWKCTRH char(8) default ' ' not null , COPSEQ char(4) default ' ' not null , CPROJ char(8) default ' ' not null ";
            strSQLStr += "  , NCOSTAMT decimal(16,4) default 0 not null , NPRICEKE decimal(15,4) default 0 not null , NCOSTADJ decimal(16,4) default 0 not null , CSCRAP char(20) default ' ' not null , CREMARK1 varchar(500) default ' ' ";
            strSQLStr += "  , CREMARK2 varchar(500) default ' ' , CREMARK3 varchar(500) default ' ' , CREMARK4 varchar(500) default ' ' , CREMARK5 varchar(500) default ' ' , CREMARK6 varchar(500) default ' ' ";
            strSQLStr += "  , CREMARK7 varchar(500) default ' ' , CREMARK8 varchar(500) default ' ' , CREMARK9 varchar(500) default ' ' , CREMARK10 varchar(500) default ' ' , CMEMDATA varchar(500) default ' ' ";
            strSQLStr += "  , CQCPASS char(1) default ' ' not null , NBACKQTY1 decimal(16,4) default 0 not null , NBACKQTY2 decimal(16,4) default 0 not null , CSTEP1 char(1) default ' ' not null ";
            strSQLStr += "  , CSTEP2 char(1) default ' ' not null , DEXPIRE datetime  , DMFGDATE datetime  , DDELIVERY datetime  , CBOMIT_PD char(8) default ' ' not null , CPROCURE char(1) default ' ' not null , CCREATEBY char(8) default ' ' not null ";
            strSQLStr += "  , CMODIFYBY char(8) default ' ' not null , DACCESS datetime default CURRENT_TIMESTAMP not null , DMODIFY datetime default CURRENT_TIMESTAMP not null , DCREATE datetime default CURRENT_TIMESTAMP not null , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            //Create Table MFWORDERIT_STDOP
            strSQLStr = " CREATE TABLE MFWORDERIT_STDOP ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null , CBRANCH char(8) default ' ' not null , CPLANT char(8) default ' ' not null , CREFTYPE char(2) default ' ' not null , CTYPE char(1) default ' ' not null , CWORDERH char(8) default ' ' not null ";
            strSQLStr += "  , CMOPR char(8) default ' ' not null , CWKCTRH char(8) default ' ' not null , CREMARK1 char(200) default ' ' not null , CREMARK2 char(200) default ' ' not null ";
            strSQLStr += "  , CREMARK3 char(200) default ' ' not null , CREMARK4 char(200) default ' ' not null , CREMARK5 char(200) default ' ' not null , CREMARK6 char(200) default ' ' not null ";
            strSQLStr += "  , CREMARK7 char(200) default ' ' not null , CREMARK8 char(200) default ' ' not null , CREMARK9 char(200) default ' ' not null , CREMARK10 char(200) default ' ' not null ";
            strSQLStr += "  , COPSEQ char(4) default ' ' not null , CSUBOP char(2) default ' ' not null , CNEXTOP char(4) default ' ' not null , CSEQ char(2) default ' ' not null ";
            strSQLStr += "  , NT_QUEUE decimal(16,4) default 0 not null , NT_SETUP decimal(16,4) default 0 not null , NT_PROCESS decimal(16,4) default 0 not null , NT_TEAR decimal(16,4) default 0 not null ";
            strSQLStr += "  , NT_WAIT decimal(16,4) default 0 not null , NT_MOVE decimal(16,4) default 0 not null , DCREATE datetime default CURRENT_TIMESTAMP not null , DACCESS datetime default CURRENT_TIMESTAMP not null ";
            strSQLStr += "  , DMODIFY datetime default CURRENT_TIMESTAMP not null , CCREATEBY char(8) default ' ' not null , CMODIFYBY char(8) default ' ' not null , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            strSQLStr = " CREATE TABLE MFGBOOK ( CROWID char(8) default ' ' not null , CACTIVE char(1) default ' ' not null , DACTIVE datetime default CURRENT_TIMESTAMP , CREFTYPE char(2) default ' ' not null ";
            strSQLStr += " , CCORP char(8) default ' ' not null , CPLANT char(8) default ' ' not null , CBRANCH char(8) default ' ' not null , CCODE char(4) default ' ' not null ";
            strSQLStr += " , CNAME char(30) default ' ' not null , CNAME2 char(30) default ' ' not null , CSTARTCODE char(7) default ' ' not null , DLOCKED datetime  , CPREFIX char(8) default ' ' not null ";
            strSQLStr += " , CDEPT char(8) default ' ' not null , CJOB char(8) default ' ' not null , CFGWHOUSE char(8) default ' ' not null , CWRWHOUSE char(8) default ' ' not null ";
            strSQLStr += " , CRWWHOUSE char(8) default ' ' not null , CCREATEBY char(8) default ' ' not null , DCREATE datetime default CURRENT_TIMESTAMP not null , CLASTUPDBY char(8) default ' ' not null ";
            strSQLStr += " , DLASTUPDBY datetime default CURRENT_TIMESTAMP not null , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            objSQLHelper.SQLExec("delete from DOCTYPE", ref strErrorMsg);

            pmInSertDocType(inSQLHelper, "MO", "M", "เอกสารใบสั่งผลิต [MO]", "MANUFACTURING ORDER [MO]");
            pmInSertDocType(inSQLHelper, "MC", "C", "ใบปิดการผลิต [MC]", "CLOSE MANUFACTURING ORDER [MC]");
            pmInSertDocType(inSQLHelper, "MQ", "Q", "ใบขอเบิกสินค้า/วัตถุดิบไปผลิต [MQ]", "REQUEST MATERIAL REQUISTION NOTE [MQ]");
            pmInSertDocType(inSQLHelper, "RJ", "O", "ใบส่งงานระหว่าง Work Center [RJ]", "ROUTE CARD JOURNAL [RJ]");

            pmUpdateMenuSeq(inSQLHelper, "NRPTCHK3", "880399", "8803");
            pmUpdateMenuSeq(inSQLHelper, "PAPPEMPLLST", "88039901", "880399");
            pmUpdateMenuSeq(inSQLHelper, "PAPPLOGINLST", "88039902", "880399");
            pmUpdateMenuSeq(inSQLHelper, "PKEEPLOG", "88039903", "880399");

            pmInSertMenu(inSQLHelper, "EMFGBOOK", "M2", "880305", "8803", "เพิ่ม/แก้ไขเล่มเอกสาร", "Document Book Items", "Menu", "Input", "");

            return true;

        }

        private bool pmUpdate_2009_12_14(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";
            cDBMSAgent objSQLHelper = inSQLHelper;
            //Alter Column
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CRESTYPE CHAR(1) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CRESOURCE CHAR(8) default ' ' not null", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CRESTYPE CHAR(1) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CRESOURCE CHAR(8) default ' ' not null", ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_01_04(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SQLExec("alter table MFRESOURCE alter column CCODE CHAR(20)", ref strErrorMsg);
            objSQLHelper.SQLExec("alter table MFSTDOPR alter column CCODE CHAR(20)", ref strErrorMsg);
            objSQLHelper.SQLExec("alter table MFBOMIT_STDOP alter column COPSEQ CHAR(20)", ref strErrorMsg);
            objSQLHelper.SQLExec("alter table MFBOMIT_STDOP alter column CNEXTOP CHAR(20)", ref strErrorMsg);

            objSQLHelper.SQLExec("alter table MFWORDERIT_PD alter column COPSEQ CHAR(20)", ref strErrorMsg);
            objSQLHelper.SQLExec("alter table MFWORDERIT_PD alter column CNEXTOP CHAR(20)", ref strErrorMsg);

            objSQLHelper.SQLExec("alter table MFWORDERIT_STDOP alter column COPSEQ CHAR(20)", ref strErrorMsg);
            objSQLHelper.SQLExec("alter table MFWORDERIT_STDOP alter column CNEXTOP CHAR(20)", ref strErrorMsg);

            objSQLHelper.SQLExec("alter table MFBOMIT_PD add CMFGBOMHD char(8) default ' ' not null ", ref strErrorMsg);
            objSQLHelper.SQLExec("alter table MFWORDERIT_PD ADD CMFGBOMHD CHAR(8) default ' ' not null", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CQCREMARK1 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CQCREMARK2 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CQCREMARK3 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CQCREMARK4 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CQCREMARK5 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CQCREMARK6 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CQCREMARK7 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CQCREMARK8 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CQCREMARK9 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CQCREMARK10 CHAR(200) default ' ' not null", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CQCREMARK1 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CQCREMARK2 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CQCREMARK3 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CQCREMARK4 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CQCREMARK5 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CQCREMARK6 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CQCREMARK7 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CQCREMARK8 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CQCREMARK9 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CQCREMARK10 CHAR(200) default ' ' not null", ref strErrorMsg);

            //Create Table : MFWCTRANHD
            strSQLStr = " CREATE TABLE MFWCTRANHD ( CROWID char(8) default ' ' not null , CRFTYPE char(1) default ' ' not null , CREFTYPE char(2) default ' ' not null , CCORP char(8) default ' ' not null ";
            strSQLStr += "  , CBRANCH char(8) default ' ' not null , CPLANT char(8) default ' ' not null , CDEPT char(8) default ' ' not null , CSECT char(8) default ' ' not null ";
            strSQLStr += "  , CJOB char(8) default ' ' not null , CPROJ char(8) default ' ' not null , CSTAT char(1) default ' ' not null , CSTEP char(1) default ' ' not null , CMSTEP char(1) default ' ' not null ";
            strSQLStr += "  , CCOOR char(8) default ' ' not null , CCODE char(7) default ' ' not null , CREFNO char(15) default ' ' not null , DDATE datetime  not null , DDUEDATE datetime  ";
            strSQLStr += "  , CPRIORITY char(1) default ' ' not null , CMFGBOOK char(8) default ' ' not null , CMACHINE char(8) default ' ' not null , CFRWKCTRH char(8) default ' ' not null ";
            strSQLStr += "  , CTOWKCTRH char(8) default ' ' not null , CFROPSEQ char(4) default ' ' not null , CFRSUBOP char(2) default ' ' not null , CTOOPSEQ char(4) default ' ' not null ";
            strSQLStr += "  , CTOSUBOP char(2) default ' ' not null , CMEMDATA varchar(500) default ' ' , CMEMDATA2 varchar(500) default ' ' , CMEMDATA3 varchar(500) default ' ' , CMEMDATA4 varchar(500) default ' ' ";
            strSQLStr += "  , CMEMDATA5 varchar(500) default ' ' , NLOCKED decimal(1,0) default 0 not null , NRECEIVEQT decimal(16,4) default 0 not null , CCREATEBY char(8) default ' ' not null ";
            strSQLStr += "  , DCREATE datetime default CURRENT_TIMESTAMP not null , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , CCANCLEBY char(8) default ' ' not null ";
            strSQLStr += "  , DCANCEL datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);


            strSQLStr = " CREATE TABLE MFWCTRANIT ( CROWID char(8) default ' ' not null , CSTAT char(1) default ' ' not null , CSTEP char(10) default ' ' not null , CCORP char(8) default ' ' not null , CBRANCH char(8) default ' ' not null ";
            strSQLStr += " , CPLANT char(8) default ' ' not null , CDEPT char(8) default ' ' not null , CSECT char(8) default ' ' not null , CJOB char(8) default ' ' not null , CWCTRANH char(8) default ' ' not null ";
            strSQLStr += " , DDATE datetime  not null , CCOOR char(8) default ' ' not null , CREFPDTYPE char(1) default ' ' not null , CIOTYPE char(1) default ' ' not null , CRFTYPE char(1) default ' ' not null ";
            strSQLStr += " , CREFTYPE char(2) default ' ' not null , CPRODTYPE char(1) default ' ' not null , CPROD char(8) default ' ' not null , CUOM char(8) default ' ' not null ";
            strSQLStr += " , CUOMSTD char(8) default ' ' not null , NUOMQTY decimal(12,4) default 0 not null , NQTY decimal(12,4) default 0 not null , NPORTION decimal(16,4) default 0 not null ";
            strSQLStr += " , CSTUOM char(8) default ' ' not null , CSTUOMSTD char(8) default ' ' not null , NSTUOMQTY decimal(12,4) default 0 not null , NSTQTY decimal(12,4) default 0 not null ";
            strSQLStr += " , NPRICE decimal(15,4) default 0 not null , NCOST decimal(15,4) default 0 not null , CSEQ char(2) default ' ' not null , CLOT char(20) default ' ' not null ";
            strSQLStr += " , CWHOUSE char(8) default ' ' not null , CMACHINE char(8) default ' ' not null , CFRWKCTRH char(8) default ' ' not null , CTOWKCTRH char(8) default ' ' not null ";
            strSQLStr += " , CFROPSEQ char(4) default ' ' not null , CFRSUBOP char(2) default ' ' not null , CTOOPSEQ char(4) default ' ' not null , CTOSUBOP char(2) default ' ' not null ";
            strSQLStr += " , CPROJ char(8) default ' ' not null , NCOSTAMT decimal(16,4) default 0 not null , NPRICEKE decimal(15,4) default 0 not null , NCOSTADJ decimal(16,4) default 0 not null ";
            strSQLStr += " , NWASTEQTY decimal(16,4) default 0 not null , NGOODQTY decimal(16,4) default 0 not null , CQCPASS char(1) default ' ' not null , CREMARK varchar(500) default ' ' ";
            strSQLStr += " , CREMARK2 varchar(500) default ' ' , CREMARK3 varchar(500) default ' ' , CREMARK4 varchar(500) default ' ' , CREMARK5 varchar(500) default ' ' , CREMARK6 varchar(500) default ' ' ";
            strSQLStr += " , CREMARK7 varchar(500) default ' ' , CREMARK8 varchar(500) default ' ' , CREMARK9 varchar(500) default ' ' , CREMARK10 varchar(500) default ' ' , CMEMDATA varchar(500) default ' ' ";
            strSQLStr += " , CCREATEBY char(8) default ' ' not null , DCREATE datetime default CURRENT_TIMESTAMP not null , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP ";
            strSQLStr += " , CCANCLEBY char(8) default ' ' not null , DCANCEL datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_01_05(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SQLExec("alter table MFWORDERHD alter column CREFNO CHAR(25)", ref strErrorMsg);
            objSQLHelper.SQLExec("alter table MFWCTRANHD alter column CREFNO CHAR(25)", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD DSTART datetime", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD DFINISH datetime", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CSTEP CHAR(1) default ' ' not null", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD CREMARK1 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD CREMARK2 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD CREMARK3 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD CREMARK4 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD CREMARK5 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD CREMARK6 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD CREMARK7 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD CREMARK8 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD CREMARK9 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD CREMARK10 CHAR(200) default ' ' not null", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD CFRMOPR CHAR(8) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCTRANHD ADD CTOMOPR CHAR(8) default ' ' not null", ref strErrorMsg);
            
            strSQLStr = " CREATE TABLE REFDOC ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null , CBRANCH char(8) default ' ' not null , CPLANT char(8) default ' ' not null , CCHILDTYPE char(2) default ' ' not null ";
            strSQLStr += " , CCHILDH char(8) default ' ' not null , CCHILDI char(8) default ' ' not null , CMASTERTYP char(2) default ' ' not null , CMASTERH char(8) default ' ' not null ";
            strSQLStr += " , CMASTERI char(8) default ' ' not null , NQTY decimal(16,4) default 0 not null , NUMQTY decimal(16,4) default 0 not null , TDATETIME datetime default CURRENT_TIMESTAMP not null , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_01_07(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SQLExec("alter table MFWCTRANHD alter column CFROPSEQ CHAR(20)", ref strErrorMsg);
            objSQLHelper.SQLExec("alter table MFWCTRANHD alter column CTOOPSEQ CHAR(20)", ref strErrorMsg);

            objSQLHelper.SQLExec("alter table MFWCTRANIT ADD NLOSSQTY1 decimal(16,4) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("alter table MFWCTRANIT ADD NHOLDQTY decimal(16,4) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("alter table MFWCTRANIT ADD CWORDERI char(8) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("alter table MFWCTRANIT ADD DFINISH datetime", ref strErrorMsg);

            objSQLHelper.SQLExec("EXEC sp_rename @objname = 'MFWCTRANIT.CREMARK', @newname = 'CREMARK1', @objtype = 'COLUMN'", ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_01_08(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            pmUpdateMenuSeq(inSQLHelper, "CHGPWD", "9902", "99");
            pmUpdateMenuSeq(inSQLHelper, "DBCONFIG", "9903", "99");

            pmInSertMenu(inSQLHelper, "SETPREF", "M2", "9901", "99", "กำหนดค่าของระบบ", "Set Application Preferences", "Menu", "Input", "");

            return true;

        }

        private bool pmUpdate_2010_01_11(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SQLExec("ALTER TABLE MFBOMHD ADD CROUNDCTRL CHAR(1) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERHD ADD CROUNDCTRL CHAR(1) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_PD ADD CROUNDCTRL CHAR(1) default ' ' not null", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_PD ADD CROUNDCTRL CHAR(1) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_PD ADD NMOQTY decimal(16,4) default 0 not null ", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_PD ADD NSCRAPQTY decimal(16,4) default 0 not null ", ref strErrorMsg);

            objSQLHelper.NotUpperSQLExecString = true;
            objSQLHelper.SQLExec("EXEC sp_rename @objname = 'MFWCTRANIT.CREMARK', @newname = 'CREMARK1', @objtype = 'COLUMN'", ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_01_12(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SQLExec("ALTER TABLE MFGBOOK ADD CWHOUSE_FG CHAR(200) default ' ' not null", ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_01_25(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SQLExec("ALTER TABLE MFWORDERHD ADD COPSTEP CHAR(1) default ' ' not null", ref strErrorMsg);

            strSQLStr = " CREATE TABLE MFWCLOSEHD ( CROWID char(8) default ' ' not null , CRFTYPE char(1) default ' ' not null , CREFTYPE char(2) default ' ' not null , CCORP char(8) default ' ' not null ";
            strSQLStr += " , CBRANCH char(8) default ' ' not null , CPLANT char(8) default ' ' not null , CDEPT char(8) default ' ' not null , CSECT char(8) default ' ' not null ";
            strSQLStr += " , CJOB char(8) default ' ' not null , CPROJ char(8) default ' ' not null , CSTAT char(1) default ' ' not null , CSTEP char(1) default ' ' not null , CMSTEP char(1) default ' ' not null ";
            strSQLStr += " , CCOOR char(8) default ' ' not null , CCODE char(15) default ' ' not null , CREFNO char(20) default ' ' not null , DDATE datetime  not null , DDUEDATE datetime  ";
            strSQLStr += " , CPRIORITY char(1) default ' ' not null , CMFGBOOK char(8) default ' ' not null , CMFGPROD char(8) default ' ' not null , CBOMHD char(8) default ' ' not null ";
            strSQLStr += " , NMFGQTY decimal(16,4) default 0 not null , NREFQTY decimal(16,4) default 0 not null , CMFGUM char(8) default ' ' not null , NMUMQTY decimal(16,4) default 0 not null ";
            strSQLStr += " , CSCRAP char(20) default ' ' not null , NSCRAPQTY decimal(16,4) default 0 not null , NQTY decimal(16,4) default 0 not null , DSTART datetime  , DFINISH datetime  ";
            strSQLStr += " , CRESOURCE char(8) default ' ' not null , CWKCTRH char(8) default ' ' not null , CMEMDATA varchar(500) default ' ' , CMEMDATA2 varchar(500) default ' ' ";
            strSQLStr += " , CMEMDATA3 varchar(500) default ' ' , CMEMDATA4 varchar(500) default ' ' , CMEMDATA5 varchar(500) default ' ' , CREMARK1 char(200) default ' ' not null ";
            strSQLStr += " , CREMARK2 char(200) default ' ' not null , CREMARK3 char(200) default ' ' not null , CREMARK4 char(200) default ' ' not null , CREMARK5 char(200) default ' ' not null ";
            strSQLStr += " , CREMARK6 char(200) default ' ' not null , CREMARK7 char(200) default ' ' not null , CREMARK8 char(200) default ' ' not null , CREMARK9 char(200) default ' ' not null ";
            strSQLStr += " , CREMARK10 char(200) default ' ' not null , NLOCKED decimal(1,0) default 0 not null , NRECEIVEQT decimal(16,4) default 0 not null , NCOSTDL decimal(16,4) default 0 not null ";
            strSQLStr += " , NCOSTOH decimal(16,4) default 0 not null , NCOSTADJ1 decimal(16,4) default 0 not null , NCOSTADJ2 decimal(16,4) default 0 not null , NCOSTADJ3 decimal(16,4) default 0 not null ";
            strSQLStr += " , NCOSTADJ4 decimal(16,4) default 0 not null , NCOSTADJ5 decimal(16,4) default 0 not null , NCOSTADJ6 decimal(16,4) default 0 not null , NCOSTADJ7 decimal(16,4) default 0 not null ";
            strSQLStr += " , NCOSTADJ8 decimal(16,4) default 0 not null , NCOSTADJ9 decimal(16,4) default 0 not null , NCOSTADJ10 decimal(16,4) default 0 not null , CBATCHNO char(10) default ' ' not null ";
            strSQLStr += " , CBATCHRUN char(10) default ' ' not null , CLEVEL char(4) default ' ' not null , CMASTER char(8) default ' ' not null , CPARENT char(8) default ' ' not null ";
            strSQLStr += " , CISAPPROVE char(1) default ' ' not null , CAPPROVEBY char(8) default ' ' not null , DAPPROVE datetime default CURRENT_TIMESTAMP , CCREATEBY char(8) default ' ' not null ";
            strSQLStr += " , DCREATE datetime default CURRENT_TIMESTAMP not null , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , CCANCLEBY char(8) default ' ' not null ";
            strSQLStr += " , DCANCEL datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            strSQLStr = "CREATE TABLE MFWCLOSEIT_PD ( CROWID char(8) default ' ' not null , CSTAT char(1) default ' ' not null , CSTEP char(10) default ' ' not null , CCORP char(8) default ' ' not null , CBRANCH char(8) default ' ' not null ";
            strSQLStr += " , CPLANT char(8) default ' ' not null , CDEPT char(8) default ' ' not null , CSECT char(8) default ' ' not null , CJOB char(8) default ' ' not null , CWCLOSEH char(8) default ' ' not null ";
            strSQLStr += " , DDATE datetime  not null , CCOOR char(8) default ' ' not null , CREFPDTYPE char(1) default ' ' not null , CIOTYPE char(1) default ' ' not null , CRFTYPE char(1) default ' ' not null ";
            strSQLStr += " , CREFTYPE char(2) default ' ' not null , CISMAINPD char(1) default ' ' not null , CPRODTYPE char(1) default ' ' not null , CPROD char(8) default ' ' not null ";
            strSQLStr += " , CUOM char(8) default ' ' not null , CUOMSTD char(8) default ' ' not null , NUOMQTY decimal(12,4) default 0 not null , NQTY decimal(12,4) default 0 not null ";
            strSQLStr += " , CSCRAP char(20) default ' ' not null , NSCRAPQTY decimal(16,4) default 0 not null , NPORTION decimal(16,4) default 0 not null , CSTUOM char(8) default ' ' not null , CSTUOMSTD char(8) default ' ' not null , NSTUOMQTY decimal(12,4) default 0 not null ";
            strSQLStr += " , NSTQTY decimal(12,4) default 0 not null , NPRICE decimal(15,4) default 0 not null , NCOST decimal(15,4) default 0 not null , NDISCAMT decimal(15,4) default 0 not null ";
            strSQLStr += " , NDISCPCN decimal(6,2) default 0 not null , CSEQ char(2) default ' ' not null , CLOT char(20) default ' ' not null , CWHOUSE char(8) default ' ' not null ";
            strSQLStr += " , CRESOURCE char(8) default ' ' not null , CWKCTRH char(8) default ' ' not null , COPSEQ char(20) default ' ' not null , CNEXTOP char(20) default ' ' not null ";
            strSQLStr += " , CPROJ char(8) default ' ' not null , NCOSTAMT decimal(16,4) default 0 not null , NPRICEKE decimal(15,4) default 0 not null , NCOSTADJ decimal(16,4) default 0 not null ";
            strSQLStr += " , CREMARK2 varchar(500) default ' ' , CREMARK1 varchar(500) default ' ' , CREMARK3 varchar(500) default ' ' , CREMARK4 varchar(500) default ' ' , CREMARK5 varchar(500) default ' ' ";
            strSQLStr += " , CREMARK6 varchar(500) default ' ' , CREMARK7 varchar(500) default ' ' , CREMARK8 varchar(500) default ' ' , CREMARK9 varchar(500) default ' ' , CREMARK10 varchar(500) default ' ' ";
            strSQLStr += " , CMEMDATA varchar(500) default ' ' , CQCPASS char(1) default ' ' not null , NBACKQTY1 decimal(16,4) default 0 not null , NBACKQTY2 decimal(16,4) default 0 not null ";
            strSQLStr += " , CSTEP1 char(1) default ' ' not null , CSTEP2 char(1) default ' ' not null , DEXPIRE datetime  , DMFGDATE datetime  , DDELIVERY datetime  , CBOMIT_PD char(8) default ' ' not null ";
            strSQLStr += " , CPROCURE char(1) default ' ' not null , CROUNDCTRL char(1) default ' ' not null , CMFGBOMHD char(8) default ' ' not null , CCREATEBY char(8) default ' ' not null ";
            strSQLStr += " , CMODIFYBY char(8) default ' ' not null , DACCESS datetime default CURRENT_TIMESTAMP not null , DMODIFY datetime default CURRENT_TIMESTAMP not null , DCREATE datetime default CURRENT_TIMESTAMP not null , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            strSQLStr = "CREATE TABLE MFWCLOSEIT_STDOP ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null , CPLANT char(8) default ' ' not null , CBRANCH char(8) default ' ' not null , CREFTYPE char(2) default ' ' not null ";
            strSQLStr += " , CTYPE char(1) default ' ' not null , CWCLOSEH char(8) default ' ' not null , CMOPR char(8) default ' ' not null , CWKCTRH char(8) default ' ' not null ";
            strSQLStr += " , CREMARK1 char(200) default ' ' not null , CREMARK2 char(200) default ' ' not null , CREMARK3 char(200) default ' ' not null , CREMARK4 char(200) default ' ' not null ";
            strSQLStr += " , CREMARK5 char(200) default ' ' not null , CREMARK6 char(200) default ' ' not null , CREMARK7 char(200) default ' ' not null , CREMARK8 char(200) default ' ' not null ";
            strSQLStr += " , CREMARK9 char(200) default ' ' not null , CREMARK10 char(200) default ' ' not null , COPSEQ char(20) default ' ' not null , CSUBOP char(20) default ' ' not null ";
            strSQLStr += " , CNEXTOP char(20) default ' ' not null , CSEQ char(2) default ' ' not null , NT_QUEUE decimal(16,4) default 0 not null , NT_SETUP decimal(16,4) default 0 not null ";
            strSQLStr += " , NT_PROCESS decimal(16,4) default 0 not null , NT_TEAR decimal(16,4) default 0 not null , NT_WAIT decimal(16,4) default 0 not null , NT_MOVE decimal(16,4) default 0 not null ";
            strSQLStr += " , CRESTYPE char(1) default ' ' not null , CRESOURCE char(8) default ' ' not null , CQCREMARK1 char(200) default ' ' not null , CQCREMARK2 char(200) default ' ' not null ";
            strSQLStr += " , CQCREMARK3 char(200) default ' ' not null , CQCREMARK4 char(200) default ' ' not null , CQCREMARK5 char(200) default ' ' not null , CQCREMARK6 char(200) default ' ' not null ";
            strSQLStr += " , CQCREMARK7 char(200) default ' ' not null , CQCREMARK8 char(200) default ' ' not null , CQCREMARK9 char(200) default ' ' not null , CQCREMARK10 char(200) default ' ' not null ";
            strSQLStr += " , DCREATE datetime default CURRENT_TIMESTAMP not null , DACCESS datetime default CURRENT_TIMESTAMP not null , DMODIFY datetime default CURRENT_TIMESTAMP not null ";
            strSQLStr += " , CCREATEBY char(8) default ' ' not null , CMODIFYBY char(8) default ' ' not null , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_01_26(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SQLExec("ALTER TABLE MFWCLOSEIT_PD ADD NMOQTY decimal(16,4) default 0 not null ", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCLOSEIT_STDOP ADD NQTY decimal(16,4) default 0 not null ", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCLOSEIT_STDOP ADD NWASTEQTY decimal(16,4) default 0 not null ", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCLOSEIT_STDOP ADD NLOSSQTY1 decimal(16,4) default 0 not null ", ref strErrorMsg);

            objSQLHelper.SQLExec("alter table MFWCLOSEHD alter column CREFNO CHAR(25)", ref strErrorMsg);

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_02_02(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            strSQLStr = "";
            strSQLStr = " CREATE TABLE DOCCHAIN ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null , CBRANCH char(8) default ' ' not null , CPLANT char(8) default ' ' not null , CCHILDTYPE char(2) default ' ' not null ";
            strSQLStr += "  , CCHILDH char(8) default ' ' not null , CCHILDI char(8) default ' ' not null , CMASTERTYP char(2) default ' ' not null , CMASTERH char(8) default ' ' not null ";
            strSQLStr += "  , CMASTERI char(8) default ' ' not null , NQTY decimal(16,4) default 0 not null , NUMQTY decimal(16,4) default 0 not null , TDATETIME datetime default CURRENT_TIMESTAMP not null , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            pmInSertMenu(inSQLHelper, "EPROD", "M2", "1305", "13", "เพิ่ม/แก้ไข รายการสินค้า /วัตถุดิบ", "Product/Rawmat Items", "Menu", "Input", "");

            return true;

        }

        private bool pmUpdate_2010_02_08(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";

            string strUpd01 = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SQLExec("ALTER TABLE MFGBOOK ADD CWHOUSE_RM CHAR(200) default ' ' not null", ref strErrorMsg);

            strErrorMsg = string.Empty;
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERHD ADD NBOMOUTQTY decimal(16,8) default 0 not null ", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERHD ADD NRMQTYFAC1 decimal(16,8) default 0 not null ", ref strErrorMsg);

            if (strErrorMsg == string.Empty)
            {
                objSQLHelper.SQLExec(ref dtsDataEnv, "QM1", "MFWORDERHD", "select CROWID,CBOMHD,NQTY from MFWORDERHD ", ref strErrorMsg);
                foreach (DataRow dtrM1 in dtsDataEnv.Tables["QM1"].Rows)
                {
                    objSQLHelper.SetPara(new object[1] { dtrM1["CBOMHD"].ToString() });

                    if (objSQLHelper.SQLExec(ref dtsDataEnv, "QBOM", "MFBOMHD", "select NMFGQTY from MFBOMHD where CROWID = ? ", ref strErrorMsg))
                    {
                        DataRow dtrBOM = dtsDataEnv.Tables["QBOM"].Rows[0];

                        decimal decMfgQty = Convert.ToDecimal(dtrBOM["NMFGQTY"]);
                        decimal decQty = Convert.ToDecimal(dtrM1["NQTY"]);
                        decimal decQtyFactor1 = decQty / decMfgQty;
                        
                        objSQLHelper.SetPara(new object[] { decMfgQty, decQtyFactor1, dtrM1["CROWID"].ToString() });
                        strUpd01 = "update MFWORDERHD set  NBOMOUTQTY = ? , NRMQTYFAC1 = ? where CROWID = ? ";
                        objSQLHelper.SQLExec(strUpd01, ref strErrorMsg);
                    }
                    
                }

            }

            strErrorMsg = string.Empty;
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_PD ADD NREFQTY decimal(16,8) default 0 not null ", ref strErrorMsg);
            if (strErrorMsg == string.Empty)
            {
                objSQLHelper.SQLExec(ref dtsDataEnv, "QM2", "MFWORDERIT_PD", "select CROWID, CBOMIT_PD from MFWORDERIT_PD where CBOMIT_PD <> '' ", ref strErrorMsg);
                foreach (DataRow dtrM2 in dtsDataEnv.Tables["QM2"].Rows)
                {
                    objSQLHelper.SetPara(new object[1] { dtrM2["CBOMIT_PD"].ToString() });
                    if (objSQLHelper.SQLExec(ref dtsDataEnv, "QBOMIT", "MFBOMIT_PD", "select NQTY from MFBOMIT_PD where CROWID = ? ", ref strErrorMsg))
                    {
                        DataRow dtrBOM = dtsDataEnv.Tables["QBOMIT"].Rows[0];
                        objSQLHelper.SetPara(new object[] { Convert.ToDecimal(dtrBOM["NQTY"]), dtrM2["CROWID"].ToString() });
                        strUpd01 = "update MFWORDERIT_PD set NREFQTY= ? where CROWID = ? ";
                        objSQLHelper.SQLExec(strUpd01, ref strErrorMsg);
                    }

                }

            }

            return true;

        }

        private bool pmUpdate_2010_02_15(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";

            string strUpd01 = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SQLExec("ALTER TABLE MFGBOOK ADD CMBOOK_PR CHAR(500) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFGBOOK ADD CMBOOK_PO CHAR(500) default ' ' not null", ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_02_17(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";

            string strUpd01 = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SQLExec("ALTER TABLE MFGBOOK ADD CGENDUPPR CHAR(1) default ' ' not null", ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_02_22(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";

            string strUpd01 = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            //Create Table : REFDOC_STMOVE
            strSQLStr = " CREATE TABLE REFDOC_STMOVE ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null , CBRANCH char(8) default ' ' not null , CPLANT char(8) default ' ' not null , CCHILDTYPE char(2) default ' ' not null ";
            strSQLStr += "  , CCHILDH char(8) default ' ' not null , CCHILDI char(8) default ' ' not null , CMASTERTYP char(2) default ' ' not null , CMASTERH char(8) default ' ' not null ";
            strSQLStr += "  , CMASTERI char(8) default ' ' not null , NQTY decimal(16,4) default 0 not null , NUMQTY decimal(16,4) default 0 not null , TDATETIME datetime default CURRENT_TIMESTAMP not null , Primary key ( CROWID ) ) ";
            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_03_09(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";

            string strUpd01 = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            pmInSertMenu(inSQLHelper, "CCHK_CPAD1", "M2", "1109", "11", "รายงานตรวจสอบ", "Print Report", "Node", "BAR", "Y");
            pmInSertMenu(inSQLHelper, "PMOSTAT", "M2", "110901", "1109", "รายงานสถานะใบสั่งผลิต", "Status of Manufacturing Order [MO]", "MENU", "REPORT", "");
            pmInSertMenu(inSQLHelper, "PMOLST2", "M2", "110902", "1109", "รายงานการใช้วัตถุดิบในการผลิต", "Material Consumption List", "MENU", "REPORT", "");
            pmInSertMenu(inSQLHelper, "PROUTELST", "M2", "110904", "1109", "รายงานผลผลิต ดี/เสีย ในแต่ละขั้นตอน", "Summary of Process output", "MENU", "REPORT", "");
            pmInSertMenu(inSQLHelper, "PMCLOSELST", "M2", "110909", "1109", "รายงานสรุปผลการผลิต", "Summary of Production", "MENU", "REPORT", "");

            return true;

        }

        private bool pmUpdate_2010_03_16(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";

            string strUpd01 = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            pmInSertMenu(inSQLHelper, "PRMSCJB", "M2", "110905", "1109", "รายงานการใช้วัตถุดิบแยกแผนกแยกโครงการ", "Material Consumption List by Section/Job", "MENU", "REPORT", "Y");
            pmInSertMenu(inSQLHelper, "PRMSCPG", "M2", "110906", "1109", "รายงานการใช้วัตถุดิบแยกแผนกเรียงตามกลุ่มวัตถุดิบ", "Material Consumption List by Section/Raw Mat. group", "MENU", "REPORT", "");
            pmInSertMenu(inSQLHelper, "PRMJBSC", "M2", "110907", "1109", "รายงานการใช้วัตถุดิบแยกโครงการเรียงตามแผนก", "Material Consumption List by Job/Section", "MENU", "REPORT", "");
            pmInSertMenu(inSQLHelper, "PRMJBPG", "M2", "110908", "1109", "รายงานการใช้วัตถุดิบรวมทุกแผนกแยกโครงการเรียงตามกลุ่มวัตถุดิบ", "Material Consumption List by Job/Raw Mat. group", "MENU", "REPORT", "");

            return true;

        }

        private bool pmUpdate_2010_04_09(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";

            string strUpd01 = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            //Create Table : COSTLINE
            strSQLStr = " CREATE TABLE COSTLINE ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null , CCOSTTYPE char(3) default ' ' not null , CTYPE char(1) default ' ' not null , CREFTAB char(25) default ' ' not null , CMASTERH char(8) default ' ' not null ";
            strSQLStr += " , NAMT decimal(16,4) default 0 not null , CCOSTBY char(1) default ' ' not null , CCREATEBY char(8) default ' ' not null , DCREATE datetime default CURRENT_TIMESTAMP not null ";
            strSQLStr += " , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) ) ";
         
            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            pmInSertMenu(inSQLHelper, "PCOST01", "M2", "1390", "13", "รายงานประมาณการต้นทุน", "Cost estimation report", "MENU", "REPORT", "Y");
            pmInSertMenu(inSQLHelper, "PCOST02", "M2", "110910", "1109", "รายงานเปรียบเทียบต้นทุนมาตรฐานกับต้นทุนจริง", "Standard cost to actual cost comparing report", "MENU", "REPORT", "");

            pmUpdateMenuName2(inSQLHelper, "PROUTELST", "รายงานผลผลิตของดี/ของเสีย ในแต่ละขั้นตอนการผลิต", "Production of Good/waste In each step of production report", "110904");

            //Alter Column
            objSQLHelper.SQLExec("ALTER TABLE MFSTDOPR ADD CREMARK1 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFSTDOPR ADD CREMARK2 CHAR(200) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFSTDOPR ADD CREMARK3 CHAR(200) default ' ' not null", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE MFRESOURCE ADD CSUPPL CHAR(8) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("alter table MFRESOURCE ADD NBUYPRICE decimal(16,4) default 0 not null", ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_05_05(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";

            string strUpd01 = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            strErrorMsg = "";
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERHD ADD CPRSTEP CHAR(1) default ' ' not null", ref strErrorMsg);
            if (strErrorMsg == "")
            {
                objSQLHelper.SQLExec(ref dtsDataEnv, "QUpd01", "MFWORDERHD", "select CROWID, CPRSTEP from MFWORDERHD where CROWID in (select CCHILDH from REFDOC where CMASTERTYP = 'PR' and CCHILDTYPE = 'MO')", ref strErrorMsg);
                foreach (DataRow dtrQUpd01 in dtsDataEnv.Tables["QUpd01"].Rows)
                {
                    strUpd01 = "update MFWORDERHD set CPRSTEP = 'P' where CROWID = ? ";
                    objSQLHelper.SetPara(new object[] { dtrQUpd01["CROWID"].ToString() });
                    objSQLHelper.SQLExec(strUpd01, ref strErrorMsg);
                }
            }

            strErrorMsg = "";
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_PD ADD CPRSTEP CHAR(1) default ' ' not null", ref strErrorMsg);
            if (strErrorMsg == "")
            {
                objSQLHelper.SQLExec(ref dtsDataEnv, "QUpd02", "MFWORDERIT_PD", "select CROWID, CPRSTEP from MFWORDERIT_PD where CROWID in (select CCHILDI from REFDOC where CMASTERTYP = 'PR' and CCHILDTYPE = 'MO') ", ref strErrorMsg);
                foreach (DataRow dtrQUpd01 in dtsDataEnv.Tables["QUpd02"].Rows)
                {
                    strUpd01 = "update MFWORDERIT_PD set CPRSTEP = 'P' where CROWID = ? ";
                    objSQLHelper.SetPara(new object[] { dtrQUpd01["CROWID"].ToString() });
                    objSQLHelper.SQLExec(strUpd01, ref strErrorMsg);
                }
            }

            pmInSertMenu(inSQLHelper, "PMOLST3", "M2", "110903", "1109", "รายงานการใช้วัตถุดิบในการผลิตเรียงตามวัตถุดิบ", "Where use Material list by MO", "MENU", "REPORT", "");

            return true;

        }

        private bool pmUpdate_2010_05_07(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";

            string strUpd01 = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            strErrorMsg = "";
            objSQLHelper.SQLExec("ALTER TABLE MFWCLOSEIT_PD ADD CREF2REFTY CHAR(2) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCLOSEIT_PD ADD CREF2HEAD CHAR(8) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCLOSEIT_PD ADD CREF2ITEM CHAR(8) default ' ' not null", ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_05_24(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";

            string strUpd01 = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            strErrorMsg = "";
            pmUpdateMenuSeq(inSQLHelper, "CPAD3", "18", "");
            pmUpdateMenuSeq(inSQLHelper, "EMFBOM", "1801", "18");
            pmUpdateMenuSeq(inSQLHelper, "EPROD", "1802", "18");
            pmUpdateMenuSeq(inSQLHelper, "PCOST01", "1803", "18");

            pmUpdateMenuSeq(inSQLHelper, "CPAD4", "19", "");
            pmUpdateMenuSeq(inSQLHelper, "ERESTYPE_M", "1901", "19");
            pmUpdateMenuSeq(inSQLHelper, "ERESTYPE_T", "1902", "19");
            pmUpdateMenuSeq(inSQLHelper, "ERESOURCE_M", "1903", "19");
            pmUpdateMenuSeq(inSQLHelper, "ERESOURCE_T", "1904", "19");
            pmUpdateMenuSeq(inSQLHelper, "ESTDOPR", "1905", "19");
            pmUpdateMenuSeq(inSQLHelper, "EMFWKCTR", "1906", "19");

            pmInSertMenu(inSQLHelper, "CPAD31", "M2", "14", "", "ต้นทุน", "C&osting", "CATEGORY", "BAR", "");
            pmInSertMenu(inSQLHelper, "EGENFGCS", "M2", "1405", "14", "ประมวลผลราคาต้นทุนสินค้าสำเร็จรูปจากใบปิดการผลิต [MC]", "Calculate Cost of Finished goods from Manufacturing Close [MC]", "MENU", "INPUT", "");
            pmUpdateMenuSeq(inSQLHelper, "PCOST02", "1410", "14");
            pmInSertMenu(inSQLHelper, "PCOST03", "M2", "1411", "14", "รายงานสรุปต้นทุนสินค้าจากการผลิต", "Report of Cost of Manufacturing goods.", "MENU", "REPORT", "");

            strUpd01 = "update APPOBJ set CINGROUP = 'Y' where CROWID = ? ";
            objSQLHelper.SetPara(new object[] { "PCOST02" });
            objSQLHelper.SQLExec(strUpd01, ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2010_06_22(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";

            string strUpd01 = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            strErrorMsg = "";

            pmUpdateMenuSeq(inSQLHelper, "PMOSTAT", "110902", "1109");
            pmUpdateMenuSeq(inSQLHelper, "PMOLST2", "110903", "1109");
            pmUpdateMenuSeq(inSQLHelper, "PMOLST3", "110904", "1109");
            pmUpdateMenuSeq(inSQLHelper, "PROUTELST", "110905", "1109");
            pmUpdateMenuSeq(inSQLHelper, "PRMSCPG", "110906", "1109");
            pmUpdateMenuSeq(inSQLHelper, "PRMJBSC", "110907", "1109");
            pmUpdateMenuSeq(inSQLHelper, "PRMJBPG", "110908", "1109");
            pmUpdateMenuSeq(inSQLHelper, "PMCLOSELST", "110909", "1109");

            pmInSertMenu(inSQLHelper, "PPRODSTAT", "M2", "110901", "1109", "รายงานการตัดสินใจการสั่งผลิต", "Reports Decision to Manufacturing order.", "MENU", "REPORT", "");

            return true;

        }

        private bool pmUpdate_2010_09_27(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strErrorMsg = "";

            string strUpd01 = "";
            string strSQLStr = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            strErrorMsg = "";

            pmInSertMenu(inSQLHelper, "CPAD0", "M2", "01", "", "วางแผนการผลิต", "Planning", "CATEGORY", "BAR", "");
            pmInSertMenu(inSQLHelper, "EPDCAP01", "M2", "0101", "01", "Production Capacity", "Production Capacity", "MENU", "INPUT", "");
            pmInSertMenu(inSQLHelper, "EMFGPP01", "M2", "0102", "01", "Production Planning", "Production Planning", "MENU", "INPUT", "Y");

            strErrorMsg = "";
            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD NCAPFACTOR1 decimal(16,4) default 0 not null ", ref strErrorMsg);
            if (strErrorMsg == "")
            {
                objSQLHelper.SQLExec("update MFBOMIT_STDOP set NCAPFACTOR1 = 1 where NCAPFACTOR1 = 0", ref strErrorMsg);
            }

            strErrorMsg = "";
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD NCAPFACTOR1 decimal(16,4) default 0 not null ", ref strErrorMsg);
            if (strErrorMsg == "")
            {
                objSQLHelper.SQLExec("update MFWORDERIT_STDOP set NCAPFACTOR1 = 1 where NCAPFACTOR1 = 0", ref strErrorMsg);
            }

            strErrorMsg = "";
            objSQLHelper.SQLExec("ALTER TABLE MFWCLOSEIT_STDOP ADD NCAPFACTOR1 decimal(16,4) default 0 not null ", ref strErrorMsg);
            if (strErrorMsg == "")
            {
                objSQLHelper.SQLExec("update MFWCLOSEIT_STDOP set NCAPFACTOR1 = 1 where NCAPFACTOR1 = 0", ref strErrorMsg);
            }

            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD NCAPPRESS decimal(16,4) default 0 not null ", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD NCAPPRESS decimal(16,4) default 0 not null ", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWCLOSEIT_STDOP ADD NCAPPRESS decimal(16,4) default 0 not null ", ref strErrorMsg);

            strSQLStr = " CREATE TABLE MFPDCAPHD ( CROWID char(8) default ' ' not null , CACTIVE char(1) default ' ' not null , DACTIVE datetime default CURRENT_TIMESTAMP , CCORP char(8) default ' ' not null ";
            strSQLStr += " , CTYPE char(1) default ' ' not null , CCODE char(15) default ' ' not null , CNAME char(40) default ' ' not null , CNAME2 char(40) default ' ' not null ";
            strSQLStr += " , CSECT char(8) default ' ' not null , CWKCTRH char(8) default ' ' not null , NWKHOUR decimal(16,4) default 0 not null , CCREATEBY char(8) default ' ' not null ";
            strSQLStr += " , DCREATE datetime default CURRENT_TIMESTAMP not null , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            strSQLStr = " CREATE TABLE MFPDCAPIT ( CROWID char(8) default ' ' not null , CACTIVE char(1) default ' ' not null , DACTIVE datetime default CURRENT_TIMESTAMP , CCORP char(8) default ' ' not null ";
            strSQLStr += "  , CTYPE char(1) default ' ' not null , CSECT char(8) default ' ' not null , CJOB char(8) default ' ' not null , CPDCAPH char(8) default ' ' not null , CRESTYPE char(8) default ' ' not null ";
            strSQLStr += "  , CRESOURCE char(8) default ' ' not null , NCAP1HOUR decimal(16,4) default 0 not null , CQNUM char(70) default ' ' not null , NCAP1DAY decimal(16,4) default 0 not null ";
            strSQLStr += "  , NRESCOUNT decimal(5,0) default 0 not null , NOT1 decimal(16,4) default 0 not null , NOT2 decimal(16,4) default 0 not null , NOT3 decimal(16,4) default 0 not null ";
             strSQLStr += " , NOT4 decimal(16,4) default 0 not null , NOT5 decimal(16,4) default 0 not null , NOT6 decimal(16,4) default 0 not null , NOT7 decimal(16,4) default 0 not null ";
            strSQLStr += "  , NOT8 decimal(16,4) default 0 not null , NOT9 decimal(16,4) default 0 not null , NOT10 decimal(16,4) default 0 not null , CCREATEBY char(8) default ' ' not null ";
            strSQLStr += "  , DCREATE datetime default CURRENT_TIMESTAMP not null , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            this.pmUpdateMenuName(objSQLHelper, "ERESTYPE_M", "ฐานข้อมูลประเภทเครื่องจักร", "Machine Type");
            this.pmUpdateMenuName(objSQLHelper, "ERESTYPE_T", "ฐานข้อมูลประเภทอุปกรณ์", "Tooling Type");

            return true;

        }

        private bool pmUpdate_2010_10_26(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SQLExec("ALTER TABLE MFBOMIT_STDOP ADD CISPLAN CHAR(1) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE MFWORDERIT_STDOP ADD CISPLAN CHAR(1) default ' ' not null", ref strErrorMsg);

            strSQLStr = " CREATE TABLE MFPLANITEM ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null , CBRANCH char(8) default ' ' not null , CPLANT char(8) default ' ' not null , CMFGBOOK char(8) default ' ' not null ";
            strSQLStr += " , CTYPE char(1) default ' ' not null , CSTEP char(1) default ' ' not null , CSTAT char(1) default ' ' not null , CWORDERH char(8) default ' ' not null ";
            strSQLStr += " , CWORDEROP char(8) default ' ' not null , DSTART datetime default null , DFINISH datetime default null , NQTY1 decimal(16,4) default 0 not null ";
            strSQLStr += " , NQTY2 decimal(16,4) default 0 not null , CREMARK1 char(100) default ' ' not null , CREMARK2 char(100) default ' ' not null , CREMARK3 char(100) default ' ' not null ";
            strSQLStr += " , CREMARK4 char(100) default ' ' not null , CREMARK5 char(100) default ' ' not null , CCREATEBY char(8) default ' ' not null , DCREATE datetime default CURRENT_TIMESTAMP not null ";
            strSQLStr += " , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2011_07_04(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            pmUpdateMenuSeq(inSQLHelper, "EPDCAP01", "0401", "01");
            pmUpdateMenuSeq(inSQLHelper, "EMFGPP01", "0402", "01");

            pmInSertMenu(inSQLHelper, "CCAL_CPAD1", "M2", "0101", "01", "ปฏิทินการทำงาน", "Working Calendar", "NODE", "BAR", "Y");
            pmInSertMenu(inSQLHelper, "ESTDWKHR", "M2", "010101", "0101", "ตารางทำงานมาตรฐาน", "Standard Work Hour", "MENU", "INPUT", "");
            pmInSertMenu(inSQLHelper, "EHOLIDAY", "M2", "010102", "0101", "บันทึกข้อมูลวันหยุดประจำปี", "บันทึกข้อมูลวันหยุดประจำปี", "MENU", "INPUT", "");
            pmInSertMenu(inSQLHelper, "EGENWKCAL", "M2", "010103", "0101", "สร้างปฏิทินการทำงาน", "Generate Working Calendar", "MENU", "INPUT", "Y");
            pmInSertMenu(inSQLHelper, "EWKCAL", "M2", "010104", "0101", "แก้ไขปฏิทินการทำงาน", "Working Calendar Entry", "MENU", "INPUT", "");


            //Alter Column
            objSQLHelper.SQLExec("ALTER TABLE EMPLANT ADD CWORKHOUR CHAR(8) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMPLANT ADD CHOLIDAY CHAR(8) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMPLANT ADD CWORKCAL CHAR(8) default ' ' not null", ref strErrorMsg);

            strSQLStr = "CREATE TABLE EMHOLIDAY ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null , CACTIVE char(1) default ' ' not null , DACTIVE datetime default CURRENT_TIMESTAMP";
            strSQLStr += " , CCODE char(4) default ' ' not null , CNAME char(30) default ' ' not null , CNAME2 char(30) default ' ' not null , NYEAR decimal(4,0) default 0 not null";
            strSQLStr += " , DDATE datetime  not null , CCREATEBY char(8) default ' ' not null , DCREATE datetime default CURRENT_TIMESTAMP not null , CLASTUPDBY char(8) default ' ' not null";
            strSQLStr += " , DLASTUPDBY datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) )";
            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            strSQLStr = "CREATE TABLE EMHOLIDAYIT ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null , CHOLIDAYH char(8) default ' ' not null , NYEAR decimal(4,0) default 0 not null";
            strSQLStr += " , DDATE datetime  not null , CREMARK char(150) default ' ' not null , CCREATEBY char(8) default ' ' not null , DCREATE datetime default CURRENT_TIMESTAMP not null";
            strSQLStr += " , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) )";
            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            strSQLStr = "CREATE TABLE EMWORKHOUR ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null , CACTIVE char(1) default ' ' not null , DACTIVE datetime default CURRENT_TIMESTAMP";
            strSQLStr += " , CCODE char(4) default ' ' not null , CNAME char(30) default ' ' not null , CNAME2 char(30) default ' ' not null , DCREATE datetime default CURRENT_TIMESTAMP not null";
            strSQLStr += " , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) )";
            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            strSQLStr = "CREATE TABLE EMWORKHOURIT ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null , CWKHOURH char(8) default ' ' not null , HR_D1 decimal(5,0) default 0 not null";
            strSQLStr += " , HR_D2 decimal(5,0) default 0 not null , HR_D3 decimal(5,0) default 0 not null , HR_D4 decimal(5,0) default 0 not null , HR_D5 decimal(5,0) default 0 not null";
            strSQLStr += " , HR_D6 decimal(5,0) default 0 not null , HR_D7 decimal(5,0) default 0 not null , OT_D1 decimal(5,0) default 0 not null , OT_D2 decimal(5,0) default 0 not null";
            strSQLStr += " , OT_D3 decimal(5,0) default 0 not null , OT_D4 decimal(5,0) default 0 not null , OT_D5 decimal(5,0) default 0 not null , OT_D6 decimal(5,0) default 0 not null";
            strSQLStr += " , OT_D7 decimal(5,0) default 0 not null , DCREATE datetime default CURRENT_TIMESTAMP not null , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) )";
            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            strSQLStr = "CREATE TABLE EMWORKCALENDAR ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null , CACTIVE char(1) default ' ' not null , DACTIVE datetime default CURRENT_TIMESTAMP";
            strSQLStr += "  , CCODE char(4) default ' ' not null , CNAME char(30) default ' ' not null , CNAME2 char(30) default ' ' not null , CREMARK char(100) default ' ' not null";
            strSQLStr += "  , NYEAR decimal(4,0) default 0 not null , NMONTH decimal(2,0) default 0 not null , DDATE datetime  not null , CCREATEBY char(8) default ' ' not null , DCREATE datetime default CURRENT_TIMESTAMP not null";
            strSQLStr += "  , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) )";
            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2012_03_09(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            this.pmUpdateMenuName(objSQLHelper, "EHOLIDAY", "บันทึกข้อมูลวันหยุดประจำปี", "Holiday and Noworking Day Master File");

            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD CCREATEBY char(8) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD HR_D1 decimal(5,0) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD HR_D2 decimal(5,0) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD HR_D3 decimal(5,0) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD HR_D4 decimal(5,0) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD HR_D5 decimal(5,0) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD HR_D6 decimal(5,0) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD HR_D7 decimal(5,0) default 0 not null", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD OT_D1 decimal(5,0) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD OT_D2 decimal(5,0) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD OT_D3 decimal(5,0) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD OT_D4 decimal(5,0) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD OT_D5 decimal(5,0) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD OT_D6 decimal(5,0) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ADD OT_D7 decimal(5,0) default 0 not null", ref strErrorMsg);

            strSQLStr = "CREATE TABLE EMWORKCALENDARIT ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null";
            strSQLStr += " , CWORKCALH char(8) default ' ' not null , DDATE datetime default null, CDAY char(2) default ' ' not null , NWORKHR decimal(5,0) default 0 not null";
            strSQLStr += " , NOTHR decimal(5,0) default 0 not null , DCREATE datetime default CURRENT_TIMESTAMP not null , CLASTUPDBY char(8) default ' ' not null";
            strSQLStr += " , DLASTUPDBY datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) )";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            strSQLStr = "CREATE TABLE EMWORKCALENDARIT_RANGE ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null ";
            strSQLStr += " , CWORKCALH char(8) default ' ' not null , CWORKCALIT char(8) default ' ' not null , CDAY char(2) default ' ' not null, CTYPE char(2) default ' ' not null ";
            strSQLStr += " , DDATE datetime default null, DBEGTIME datetime default null, DENDTIME datetime default null ";
            strSQLStr += " , CSEQ char(2) default ' ' not null ";
            strSQLStr += " , DCREATE datetime default CURRENT_TIMESTAMP not null ";
            strSQLStr += " , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE EMWORKCALENDAR ADD NTOTWORKHR decimal(5,0) default 0 not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKCALENDAR ADD NTOTOTHR decimal(5,0) default 0 not null", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE EMWORKCALENDAR ADD CWORKHOUR CHAR(8) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKCALENDAR ADD CHOLIDAY CHAR(8) default ' ' not null", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE EMWORKCALENDAR ADD CPLANT CHAR(8) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKCALENDARIT ADD CPLANT CHAR(8) default ' ' not null", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKCALENDARIT_RANGE ADD CPLANT CHAR(8) default ' ' not null", ref strErrorMsg);

            strSQLStr = "CREATE TABLE EMWORKHOURIT_RANGE ( CROWID char(8) default ' ' not null , CCORP char(8) default ' ' not null ";
            strSQLStr += " , CWKHOURH char(8) default ' ' not null , CDAY char(2) default ' ' not null, CTYPE char(2) default ' ' not null ";
            strSQLStr += " , DDATE datetime default null, DBEGTIME datetime default null, DENDTIME datetime default null ";
            strSQLStr += " , CSEQ char(2) default ' ' not null ";
            strSQLStr += " , DCREATE datetime default CURRENT_TIMESTAMP not null ";
            strSQLStr += " , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) ) ";
            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2012_04_26(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN HR_D1 decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN HR_D2 decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN HR_D3 decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN HR_D4 decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN HR_D5 decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN HR_D6 decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN HR_D7 decimal(9,4)", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN OT_D1 decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN OT_D2 decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN OT_D3 decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN OT_D4 decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN OT_D5 decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN OT_D6 decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKHOUR ALTER COLUMN OT_D7 decimal(9,4)", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE EMWORKCALENDARIT ALTER COLUMN NWORKHR decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKCALENDARIT ALTER COLUMN NOTHR decimal(9,4)", ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE EMWORKCALENDAR ALTER COLUMN NTOTWORKHR decimal(9,4)", ref strErrorMsg);
            objSQLHelper.SQLExec("ALTER TABLE EMWORKCALENDAR ALTER COLUMN NTOTOTHR decimal(9,4)", ref strErrorMsg);


            objSQLHelper.SetPara(new object[] { "EPDCAP01" });
            objSQLHelper.SQLExec("update APPOBJ set CVISIBLE = 'N' where CROWID = ? ", ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2012_05_03(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;

            strSQLStr = "CREATE TABLE MFWORDERIT_OPPLAN ( CROWID char(8) default ' ' not null ";
            strSQLStr += " , CCORP char(8) default ' ' not null ";
            strSQLStr += " , CBRANCH char(8) default ' ' not null ";
            strSQLStr += " , CPLANT char(8) default ' ' not null ";
            strSQLStr += " , CWORDERH char(8) default ' ' not null ";
            strSQLStr += " , CWORDERI_OP char(8) default ' ' not null ";
            strSQLStr += " , CWKCTRH char(8) default ' ' not null ";
            strSQLStr += " , COPSEQ char(20) default ' ' not null ";
            strSQLStr += " , CTYPE char(2) default ' ' not null ";
            strSQLStr += " , NUSEHOUR decimal(9,4) default 0 not null ";
            strSQLStr += " , DDATE datetime default null ";
            strSQLStr += " , DBEGTIME datetime default null ";
            strSQLStr += " , DENDTIME datetime default null ";
            strSQLStr += " , DCREATE datetime default CURRENT_TIMESTAMP not null ";
            strSQLStr += " , CLASTUPDBY char(8) default ' ' not null , DLASTUPDBY datetime default CURRENT_TIMESTAMP , Primary key ( CROWID ) ) ";

            objSQLHelper.SQLExec(strSQLStr, ref strErrorMsg);

            objSQLHelper.SQLExec("ALTER TABLE MFWORDERHD ADD CISPLAN char(1) default ' ' not null", ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2012_07_08(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;
            objSQLHelper.SQLExec("update APPOBJ set  CSEQ = '110910' where CROWID = 'PROUTELST'", ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2013_08_21(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;
            pmInSertMenu(inSQLHelper, "XRSTCARD01", "M2", "1301001", "13", "รายงานเคลื่อนไหวสินค้า/วัตถุดิบ", "Stock movement report", "Menu", "Report", "");
            pmInSertMenu(inSQLHelper, "XRSTCARD02", "M2", "1301002", "13", "รายงานเคลื่อนไหวสินค้า/วัตถุดิบ ระดับคลังสินค้า", "Stock movement report", "Menu", "Report", "");
            pmInSertMenu(inSQLHelper, "XRSTCARD03", "M2", "1301003", "13", "รายงานมูลค่าสินค้า/วัตถุดิบคงเหลือ", "Stock movement report", "Menu", "Report", "");
            pmInSertMenu(inSQLHelper, "XRSTAG01", "M2", "1301004", "13", "รายงานวิเคราะห์อายุสินค้า", "Stock movement report", "Menu", "Report", "");
            pmInSertMenu(inSQLHelper, "XRSTAG02", "M2", "1301005", "13", "รายงานวิเคราะห์อายุสินค้าแยกตามกลุ่มสินค้า", "Stock movement report", "Menu", "Report", "");
            pmInSertMenu(inSQLHelper, "XRSTLOT01", "M2", "1301006", "13", "รายงาน LOT สินค้ารับเข้า", "Stock movement report", "Menu", "Report", "");
            pmInSertMenu(inSQLHelper, "XRSTLOT02", "M2", "1301007", "13", "รายการ LOT สินค้าและวัตถุดิบคงเหลือ", "Stock movement report", "Menu", "Report", "");
            pmInSertMenu(inSQLHelper, "XRSTCARD04", "M2", "1301010", "13", "รายงานคงเหลือแต่ละคลังแยกตามกลุ่มสินค้า", "Stock movement report", "Menu", "Report", "");

            //รายงานคงเหลือแต่ละคลังแยกตามกลุ่มสินค้า
            //pmInSertMenu(inSQLHelper, "XRSTCARD01", "M2", "1301001", "13", "รายงานเคลื่อนไหวสินค้า/วัตถุดิบ", "Stock movement report", "Menu", "Report", "");

            return true;

        }

        private bool pmUpdate_2014_07_22(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;
            this.pmUpdateMenuName(objSQLHelper, "EPDGRP", "เพิ่ม/แก้ไขรายการกลุ่มสินค้า/วัตถุดิบ", "Product Category Entry");
            this.pmUpdateMenuName(objSQLHelper, "EWHLOCA", "เพิ่ม/แก้ไข Location", "Location Entry");

            //objSQLHelper.SQLExec("delete from APPOBJ where CROWID in ('PORDPLST', 'EORDPOINT','EAPPLOGIN','EJOB','EPROJ','PPROJLST','PJOBLST')", ref strErrorMsg);
            objSQLHelper.SQLExec("delete from APPOBJ where CROWID in ('PORDPLST', 'EORDPOINT','EJOB','EPROJ','PPROJLST','PJOBLST')", ref strErrorMsg);

            pmInSertMenu(inSQLHelper, "ECOOR", "SM", "880108", "8801", "เพิ่ม/แก้ไข รายชื่อลูกค้า", "Customer Entry", "MENU", "INPUT", "");
            pmInSertMenu(inSQLHelper, "EPDBARCODE", "SM", "880208", "8802", "เพิ่ม/แก้ไขฐานข้อมูล Barcode", "Product Barcode Entry", "MENU", "INPUT", "");

            objSQLHelper.SQLExec("update APPOBJ set CVISIBLE = 'N' where CROWID in ( 'CPAD1','CPAD3','CPAD4')", ref strErrorMsg);


            objSQLHelper.SQLExec("update APPOBJ set CPRSEQ = '12' where CROWID = 'ESTMREQ'", ref strErrorMsg);

            return true;

        }

        private bool pmUpdate_2014_08_31(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;
            pmInSertMenu(inSQLHelper, "EXPDB001", "SM", "880308", "8803", "Export ฐานข้อมูลสำหรับ Mobile Computer", "Export Database for Mobile Computer", "MENU", "INPUT", "");

            return true;

        }

        private bool pmUpdate_2014_09_03(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;
            pmInSertMenu(inSQLHelper, "XRSTSALE01", "M2", "1301015", "13", "รายงาน TOP Sale Report", "รายงาน TOP Sale Report", "MENU", "REPORT", "");

            return true;

        }

        private bool pmUpdate_2015_04_24(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;
            pmInSertMenu(inSQLHelper, "XRSTSALE02", "M2", "1301016", "13", "รายงานการคืนสินค้า", "รายงานการคืนสินค้า", "MENU", "REPORT", "");
            pmInSertMenu(inSQLHelper, "XRSTSALE03", "M2", "1301017", "13", "รายงานสินค้าเข้า", "รายงานสินค้าเข้า", "MENU", "REPORT", "");
            pmInSertMenu(inSQLHelper, "XRSTSALE04", "M2", "1301018", "13", "รายงานแสดงประวัติ การขายของลูกค้า", "รายงานแสดงประวัติ การขายของลูกค้า", "MENU", "REPORT", "");
            pmInSertMenu(inSQLHelper, "XRSTSALE05", "M2", "1301019", "13", "รายงานสรุปยอดขายตามกลุ่มสินค้า [กราฟวงกลม]", "รายงานสรุปยอดขายตามกลุ่มสินค้า [กราฟวงกลม]", "MENU", "REPORT", "");
            //รายงานแสดงประวัติ การขายของลูกค้า

            return true;

        }

        private bool pmUpdate_2015_07_16(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;
            pmInSertMenu(inSQLHelper, "XRSTSALE06", "M2", "1301031", "13", "รายงานสรุปยอดขายแยกเอกสารการขาย", "รายงานสรุปยอดขายแยกเอกสารการขาย", "MENU", "REPORT", "");

            return true;

        }

        private bool pmUpdate_2015_11_27(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            //EPDBARCODE          	SM	880208    	8802      	EPDBARCODE          
            cDBMSAgent objSQLHelper = inSQLHelper;
            pmInSertMenu_POS(inSQLHelper, "EWKGRP", "SM", "880210", "8802", "Work Station Group Entry", "Work Station Group Entry", "MENU", "INPUT", "");
            pmInSertMenu_POS(inSQLHelper, "EWKSTAT", "SM", "880211", "8802", "Work Station Entry", "Work Station Entry", "MENU", "INPUT", "");

            return true;

        }

        private bool pmUpdate_2016_06_06(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;
            pmInSertMenu_POS(inSQLHelper, "XRSTSALE09", "M2", "1301034", "13", "รายงานยอดขาย-แยกประเภทการรับชำระเงิน", "รายงานยอดขาย-แยกประเภทการรับชำระเงิน", "MENU", "REPORT", "");
            pmInSertMenu_POS(inSQLHelper, "XRSTSALE23", "M2", "1301035", "13", "รายงานแสดงประวัติ การขายของลูกค้า", "รายงานแสดงประวัติ การขายของลูกค้า", "MENU", "REPORT", "");

            return true;

        }

        private bool pmUpdate_2016_06_15(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;
            pmInSertMenu_POS(inSQLHelper, "XRVATSALE1", "M2", "1301036", "13", "รายงานภาษีขาย", "รายงานภาษีขาย", "MENU", "REPORT", "");

            return true;

        }

        private bool pmUpdate_2016_08_19(cDBMSAgent inSQLHelper)
        {

            DataSet dtsDataEnv = new DataSet();
            string strSQLStr = "";
            string strErrorMsg = "";

            cDBMSAgent objSQLHelper = inSQLHelper;
            pmInSertMenu_POS(inSQLHelper, "PEXTR01", "SM", "880310", "8803", "Export เอกสารของระบบ", "Export เอกสารของระบบ", "MENU", "REPORT", "");
            pmInSertMenu_POS(inSQLHelper, "PIMTR01", "SM", "880311", "8803", "Import เอกสารของระบบ", "Import เอกสารของระบบ", "MENU", "REPORT", "");

            return true;

        }

        private void pmInSertMenu(cDBMSAgent inSQLHelper, string inTaskName, string inModule, string inSeq, string inPRSeq, string inDesc, string inDesc2, string inType, string inType2, string inGroup)
        {

            string strErrorMsg = "";
            string strUpd01 = "";
            DataSet dtsDataEnv = new DataSet();
            cDBMSAgent objSQLHelper = inSQLHelper;

            //เพิ่มเมนู
            objSQLHelper.SetPara(new object[1] { inTaskName });
            if (!objSQLHelper.SQLExec(ref dtsDataEnv, "QChk", "APPOBJ", "select * from APPOBJ where CTASKNAME = ? ", ref strErrorMsg))
            {
                objSQLHelper.SetPara(new object[] { inTaskName, inModule, inSeq, inPRSeq, inTaskName, inDesc, inDesc2, inType.ToUpper(), inType2.ToUpper(), inGroup });
                strUpd01 = "insert into APPOBJ (CROWID, CMODULE , CSEQ , CPRSEQ , CTASKNAME , CDESC , CDESC2 , CTYPE , CTYPE2 , CINGROUP , CVISIBLE , NIMG)  ";
                strUpd01 += " values (? , ? , ? , ? , ? , ? , ? , ? , ? , ? , '' , 0 )";
                objSQLHelper.SQLExec(strUpd01, ref strErrorMsg);
            }

        }

        private void pmInSertMenu_POS(cDBMSAgent inSQLHelper, string inTaskName, string inModule, string inSeq, string inPRSeq, string inDesc, string inDesc2, string inType, string inType2, string inGroup)
        {

            string strErrorMsg = "";
            string strUpd01 = "";
            DataSet dtsDataEnv = new DataSet();
            cDBMSAgent objSQLHelper = inSQLHelper;

            //เพิ่มเมนู
            objSQLHelper.SetPara(new object[1] { inTaskName });
            if (!objSQLHelper.SQLExec(ref dtsDataEnv, "QChk", "APPOBJ", "select * from APPOBJ_POS where CTASKNAME = ? ", ref strErrorMsg))
            {
                objSQLHelper.SetPara(new object[] { inTaskName, inModule, inSeq, inPRSeq, inTaskName, inDesc, inDesc2, inType.ToUpper(), inType2.ToUpper(), inGroup });
                strUpd01 = "insert into APPOBJ_POS (CROWID, CMODULE , CSEQ , CPRSEQ , CTASKNAME , CDESC , CDESC2 , CTYPE , CTYPE2 , CINGROUP , CVISIBLE , NIMG)  ";
                strUpd01 += " values (? , ? , ? , ? , ? , ? , ? , ? , ? , ? , '' , 0 )";
                objSQLHelper.SQLExec(strUpd01, ref strErrorMsg);
            }

        }

        private void pmInSertDocType(cDBMSAgent inSQLHelper, string inRefType, string inRfType, string inName, string inName2)
        {
            string strErrorMsg = "";
            string strUpd01 = "";
            DataSet dtsDataEnv = new DataSet();
            cDBMSAgent objSQLHelper = inSQLHelper;

            //เพิ่มเมนู
            objSQLHelper.SetPara(new object[1] { inRefType });
            if (!objSQLHelper.SQLExec(ref dtsDataEnv, "QChk", "DOCTYPE", "select * from DOCTYPE where CCODE = ? ", ref strErrorMsg))
            {
                objSQLHelper.SetPara(new object[] { inRefType, inRefType, inRfType, inName, inName2 });
                strUpd01 = "insert into DOCTYPE (CROWID, CCODE , CRFTYPE , CNAME , CNAME2 )  ";
                strUpd01 += " values (? , ? , ? , ? , ? )";
                objSQLHelper.SQLExec(strUpd01, ref strErrorMsg);
            }

        }

        private void pmUpdateMenuName(cDBMSAgent inSQLHelper, string inTaskName, string inDesc, string inDesc2)
        {
            string strErrorMsg = "";
            string strUpd01 = "";
            DataSet dtsDataEnv = new DataSet();
            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SetPara(new object[3] { inDesc, inDesc2, inTaskName });
            objSQLHelper.SQLExec("update APPOBJ set CDESC = ? , CDESC2 = ? where CROWID = ? ", ref strErrorMsg);

        }

        private void pmUpdateMenuSeq(cDBMSAgent inSQLHelper, string inTaskName, string inSeq, string inPRSeq)
        {
            string strErrorMsg = "";
            string strUpd01 = "";
            DataSet dtsDataEnv = new DataSet();
            cDBMSAgent objSQLHelper = inSQLHelper;

            objSQLHelper.SetPara(new object[3] { inSeq, inPRSeq, inTaskName });
            objSQLHelper.SQLExec("update APPOBJ set CSEQ = ? , CPRSEQ = ? where CROWID = ? ", ref strErrorMsg);

        }

        private void pmUpdateMenuName2(cDBMSAgent inSQLHelper, string inTaskName, string inDesc, string inDesc2, string inSeq)
        {
            string strErrorMsg = "";
            string strUpd01 = "";
            DataSet dtsDataEnv = new DataSet();
            cDBMSAgent objSQLHelper = inSQLHelper;

            if (inDesc.Trim() == string.Empty)
            {
                //Update SEQ อย่างเดียว
                objSQLHelper.SetPara(new object[2] { inSeq, inTaskName });
                objSQLHelper.SQLExec("update APPOBJ set CSEQ = ? where CROWID = ? ", ref strErrorMsg);
            }
            else
            {
                objSQLHelper.SetPara(new object[4] { inDesc, inDesc2, inSeq, inTaskName });
                objSQLHelper.SQLExec("update APPOBJ set CDESC = ? , CDESC2 = ? , CSEQ = ? where CROWID = ? ", ref strErrorMsg);
            }

        }

        private bool pmHasTable(cDBMSAgent inSQLHelper, string inTableName)
        {

            bool bllHasTable = false;
            string strErrorMsg = "";
            DataSet dtsDataEnv = new DataSet();
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = inSQLHelper;

            pobjSQLUtil.NotUpperSQLExecString = true;
            pobjSQLUtil.SetPara(new object[] { inTableName });
            if (pobjSQLUtil.SQLExec(ref dtsDataEnv, "QChk", "SYSOBJECTS", "select * from sysobjects where type = 'U' and name = ?", ref strErrorMsg))
            {
                bllHasTable = true;
            }

            return bllHasTable;
        }

    }


}
