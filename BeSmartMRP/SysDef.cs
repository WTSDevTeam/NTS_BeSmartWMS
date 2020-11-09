using System;
using System.Collections.Generic;
using System.Text;
using AppUtil.SecureHelper;

namespace BeSmartMRP
{

    public class SysDef
    {

        public SysDef() { }

        public const string xd_Encrypt_Key = "ITULOSHCETHTLAEW";
        public const string xd_Encrypt_InitVector = "785688410";
        public const int xd_PASSWORD_LENGTH = 10;
        public const WSEncryption.Symmetric.Provider xd_ENCRYPT_PROVIDER = WSEncryption.Symmetric.Provider.TripleDES;

        //AppObj Type
        public static string gc_AppObj_Root_Seq = "00";

        public static string gc_AppObj_Type_Root = "H";
        public static string gc_AppObj_Type_Pad = "0";
        public static string gc_AppObj_Type_Bar_Input = "1";
        public static string gc_AppObj_Type_Bar_Report = "2";
        public static string gc_AppObj_Type_Bar_Process = "3";
        public static string gc_AppObj_Type_Bar_Other = "9";

        public static string gc_CUST_TYPE = "C";
        public static string gc_SUPP_TYPE = "S";

        public static string gc_VAT_NOT_PRN = "4";	// ไม่มี vat

        public static string gc_PAYTYPE_ATS = "ATS";	// ทำรายการบัญชีอัตโนมัติ
        public static string gc_PAYTYPE_CD = "CD ";	// บัตรเครดิต
        public static string gc_PAYTYPE_CG = "CG ";	// ถอนเงินผ่านเคลียริ่ง
        public static string gc_PAYTYPE_CW = "CW ";	// ถอนโดยเช็ค
        public static string gc_PAYTYPE_BB = "BB ";	// ฝากโดยเช็ค เงินตราตปท.
        public static string gc_PAYTYPE_CL = "CL ";	// ฝากโดยเช็คธนาคารอื่น
        public static string gc_PAYTYPE_HC = "HC ";	// ฝากโดยเช็คธนาคาร
        public static string gc_PAYTYPE_CM = "CM ";	// ค่าธรรมเนียม
        public static string gc_PAYTYPE_CR = "CR ";	// เช็คคืน
        public static string gc_PAYTYPE_CS = "CS ";	// ถอนเงินสด
        public static string gc_PAYTYPE_DD = "DD ";	// ฝากโดยดราฟท์
        public static string gc_PAYTYPE_INW = "INW";	// ดอกเบี้ย
        public static string gc_PAYTYPE_LC = "LC ";	// เล็ตเตอร์ออฟเครดิต
        public static string gc_PAYTYPE_PC = "PC ";	// ฝากโดยเงินสด
        public static string gc_PAYTYPE_TRD = "TRD";	// ฝากโดยการโอน
        public static string gc_PAYTYPE_TRW = "TRW";	// ถอนโดยการโอน
        public static string gc_PAYTYPE_CHI = "CHI";	// เงินสดรับ
        public static string gc_PAYTYPE_CHO = "CHO";	// เงินสดจ่าย
        public static string gc_PAYTYPE_RF = "RF ";	// Refer ไปดูที่ payment ของเอกสารใบอื่น (ใช้ที่ใบเสร็จของ สวนจิตร)
        public static string gc_PAYTYPE_OC = "OV ";	// ค่า OV
        public static string gc_PAYTYPE_OCI = "OCI";	// เงินเกิน
        public static string gc_PAYTYPE_COU = "COU";	// Coupon
        public static string gc_PAYTYPE_MO = "MO ";	// ธนาณัติ Money Order
        public static string gc_PAYTYPE_CC = "CC ";	// ฝากโดยเช็คเรียกเก็บ
        public static string gc_PAYTYPE_RP = "RP"; 	// ใบเสร็จรับเงินจากใบหักเงินประกันผลงาน (ซื้อ)
        public static string gc_PAYTYPE_RS = "RS"; 	// ใบเสร็จรับเงินจากใบหักเงินประกันผลงาน (ขาย)

        public static string gc_Root_NameSpace = "BeSmartMRP";
        public static string gc_Locale_NameSpace = "BeSmartMRP.UIHelper.AppLocale";
        public static string gc_ToolBar_NameSpace = "BeSmartMRP.UIHelper.WsToolBar";
        public static string gc_DocType_NameSpace = "BeSmartMRP.Business.Entity.DocumentType";

        public static string gc_DEFAULT_VALDATEKEY_PREFIX_STAR = "*";
        public static string gc_DEFAULT_VALDATEKEY_PREFIX_2SLASH = "//";

        public static string gc_UM_STAT_ACTIVE = "A";	//UM Status
        public static string gc_UM_DEFAULT_TYPE = "$$";	//UM Standard Type

        public static string gc_STEP_ORDER_CUT = "C";
        public static string gc_STEP_CREATED = "1";
        public static string gc_STEP_IGNORE = "I";
        public static string gc_STEP_PAY = "P";

        public static string gc_REF_STEP2_NORMAL = " ";	// ปกติ ไม่ต้องมีเอกสารอื่นมาตัด
        public static string gc_REF_STEP2_WAIT = "1";	// รอทำ PR
        public static string gc_REF_STEP2_PR = "3";	// ทำ PR ครบแล้ว
        public static string gc_REF_STEP2_CLOSED = "5";	// Closed แล้ว

        public static string gc_STAT_NORMAL = " ";
        public static string gc_STAT_CANCEL = "C";

        public static string gc_REFPD_TYPE_PRODUCT = "P";
        public static string gc_REFPD_TYPE_FORMULA = "F";
        public static string gc_REFPD_TYPE_STRUCTURE = "S";
        public static string gc_REFPD_TYPE_REMARK = "R";

        public static string gc_WHOUSE_TYPE_WIP = "W";		//Work In Process
        public static string gc_WHOUSE_TYPE_NORMAL = " ";		//Normal
        public static string gc_WHOUSE_TYPE_OFFICE_SUPPLY = "O";		//Office Supply
        public static string gc_WHOUSE_TYPE_WIP_MRP = "M";		//Work In Process MRP
        public static string gc_WHOUSE_TYPE_DAMAGE = "D";		//คลังของเสีย
        public static string gc_WHOUSE_TYPE_STORAGE = "S";		//คลังโกดัง

        //GlRef.Step , OrderH.Step , LayH.Step
        public static string gc_REF_STEP_WAIT_APPROVE = "&";
        //public static string gc_REF_STEP_CREATE = ")";
        public static string gc_REF_STEP_CREATE = "";
        public static string gc_REF_STEP_HOLD = "-";
        public static string gc_REF_STEP_CUT_STOCK = "1";
        public static string gc_REF_STEP_PAY = "P";
        public static string gc_REF_STEP_IGNORE = "I";
        public static string gc_REF_STEP_CLOSED = "L";
        public static string gc_REF_STEP_COMPLETE = "P";

        public static string gc_REF_STEP_WAIT_ISSUE = "5";

        //Branch.PATStep , GLRef.AtStep , PayInH.AtStep
        public static string gc_ATSTEP_VOUCHER_POST = " ";
        public static string gc_ATSTEP_VOUCHER_INIT = "I";
        public static string gc_ATSTEP_VOUCHER_WAIT = "X";
        public static string gc_ATSTEP_VOUCHER_NONE = "N";
        public static string gc_ATSTEP_VOUCHER_PROCESS = "P";
        public static string gc_ATSTEP_VOUCHER_BRANCH = "B";

        public static string gc_STEPX1_COLLECT = "1";	//รวบรวมข้อมูล
        public static string gc_STEPX1_CREATED = "3";	//ขั้นจัดทำ
        public static string gc_STEPX1_WAITAPPR = "5";	//รออนุมัติ
        public static string gc_STEPX1_WAITORD = "7";	//รอรับคำสั่ง
        public static string gc_STEPX1_COORAPPR = "9";	//ลูกค้าอนุมัติ

        public static string gc_REFTYPE_P_ORDER = "PO";   // N ใบสั่งซื้อ (Order ระบบซื้อ)                                  N
        public static string gc_REFTYPE_S_ORDER = "SO";   // O ใบรับคำสั่งซื้อ (Order ระบบขาย)                              N
        public static string gc_REFTYPE_P_REQUEST = "PR";   // w ใบขอซื้อ (ระบบซื้อ)
        public static string gc_REFTYPE_P_HIRE = "PH";   // w ใบขอจ้าง (ระบบซื้อ)
        public static string gc_REFTYPE_P_CONTACT = "PJ";   // สัญญาจ้าง
        public static string gc_REFTYPE_S_REQUEST = "SH";   // x ใบขอซื้อ (ระบบขาย)
        public static string gc_REFTYPE_W_ORDER = "WO";   // Q ใบสั่งผลิต (Work Order)                              N
        public static string gc_REFTYPE_P_LEND = "BS";   // I ใบยืมสินค้าจากผู้จำหน่าย                               N
        public static string gc_REFTYPE_P_RET = "BT";   // I ใบคืนสินค้าจากการยืมให้ผู้จำหน่าย
        public static string gc_REFTYPE_S_LEND = "SS";   // I ใบยืมสินค้าของลูกค้า                              N
        public static string gc_REFTYPE_S_RET = "ST";   // I ใบคืนสินค้าจากการยืมของลูกค้า
        public static string gc_REFTYPE_S_REP_F = "SF";   // ใบรับรู้การขาย (ระบบขาย)

        public static string gc_REFTYPE_S_QUATATION = "QS";   // q ใบเสนอราคา (ระบบขาย)

        public static string gc_REFTYPE_P_CR_C = "BC";   //F ใบลดหนี้ เงินสด (ซื้อ)                                               N
        public static string gc_REFTYPE_P_CR_A = "BA";   //F ใบลดหนี้ เงินสด บริการ (ซื้อ)                                               N
        public static string gc_REFTYPE_P_CR_M = "BM";   //F ใบลดหนี้ เงินเชื่อ (ซื้อ)                                               N
        public static string gc_REFTYPE_P_CR_N = "BN";   //F ใบลดหนี้ เงินเชื่อ บริการ (ซื้อ)                                               N
        public static string gc_REFTYPE_P_DR_D = "BD";   //E ใบเพิ่มหนี้ เงินสด (ซื้อ)                                            N
        public static string gc_REFTYPE_P_DR_B = "BB";   //E ใบเพิ่มหนี้ เงินสด บริการ (ซื้อ)                                            N
        public static string gc_REFTYPE_P_DR_X = "BX";   //E ใบเพิ่มหนี้ เงินเชื่อ (ซื้อ)                                            N
        public static string gc_REFTYPE_P_DR_Z = "BZ";   //E ใบเพิ่มหนี้ เงินเชื่อ บริการ (ซื้อ)                                            N
        public static string gc_REFTYPE_P_INV_R = "BR";   //B ใบส่งของ/ใบกำกับภาษี/ใบเสร็จรับเงิน(บิลเงินสด) (ซื้อ)        Y Y
        public static string gc_REFTYPE_P_INV_I = "BI";   //B ใบส่งของ/ใบกำกับภาษี/ใบแจ้งหนี้ (ซื้อ)                       N Y
        public static string gc_REFTYPE_P_INV_Q = "BQ";   //B ใบส่งของ/ใบกำกับภาษี (ซื้อ)                                  N Y
        public static string gc_REFTYPE_P_INV_U = "BU";   //B ใบส่งของ/ใบกำกับภาษี/ใบเสร็จรับเงิน(บิลเงินสด บริการ) (ซื้อ)        Y Y
        public static string gc_REFTYPE_P_INV_V = "BV";   //B ใบส่งของ/ใบแจ้งหนี้(งานบริการ,VAT ไม่ถึงกำหนด) (ซื้อ)        N N
        public static string gc_REFTYPE_P_INV_Y = "BY";   //B ใบส่งของ/พิมพ์ใบแจ้งหนี้แยก (งานบริการ,VAT ไม่ถึงกำหนด) (ซื้อ) N N
        public static string gc_REFTYPE_S_CR_A = "SA";   //F ใบลดหนี้ เงินสด บริการ (ขาย)                                               N
        public static string gc_REFTYPE_S_CR_M = "SM";   //F ใบลดหนี้ เงินเชื่อ (ขาย)                                               N
        public static string gc_REFTYPE_S_CR_N = "SN";   //F ใบลดหนี้ เงินเชื่อ บริการ (ขาย)                                               N
        public static string gc_REFTYPE_S_CR_C = "SC";   //F ใบลดหนี้ เงินสด (ขาย)                                               N
        public static string gc_REFTYPE_S_DR_B = "SB";   //E ใบเพิ่มหนี้ เงินสด บริการ (ขาย)                                            N
        public static string gc_REFTYPE_S_DR_D = "SD";   //E ใบเพิ่มหนี้ เงินสด (ขาย)                                            N
        public static string gc_REFTYPE_S_DR_X = "SX";   //E ใบเพิ่มหนี้ เงินเชื่อ (ขาย)                                            N
        public static string gc_REFTYPE_S_DR_Z = "SZ";   //E ใบเพิ่มหนี้ เงินเชื่อ บริการ (ขาย)                                            N
        public static string gc_REFTYPE_S_INV_G = "SG";   //Y ใบส่งของ/ใบกำกับภาษี/ให้ไม่คิดมูลค่า (ขาย)                    N Y
        public static string gc_REFTYPE_S_INV_I = "SI";   //S ใบส่งของ/ใบกำกับภาษี/ใบแจ้งหนี้ (ขาย)                        N Y
        public static string gc_REFTYPE_S_INV_Q = "SQ";   //S ใบส่งของ/ใบกำกับภาษี (ขาย)                                   N Y
        public static string gc_REFTYPE_S_INV_R = "SR";   //S ใบส่งของ/ใบกำกับภาษี/ใบเสร็จรับเงิน(บิลเงินสด) (ขาย)         Y Y
        public static string gc_REFTYPE_S_INV_U = "SU";   //S ใบส่งของ/ใบกำกับภาษี/ใบเสร็จรับเงิน(บิลเงินสด บริการ) (ขาย)         Y Y
        public static string gc_REFTYPE_S_INV_V = "SV";   //S ใบส่งของ/ใบแจ้งหนี้(งานบริการ,VAT ไม่ถึงกำหนด) (ขาย)         N N
        public static string gc_REFTYPE_S_INV_W = "SW";   //Y SLIP POS (ขาย)                    N Y
        public static string gc_REFTYPE_S_INV_Y = "SY";   //S ใบส่งของ/พิมพ์ใบแจ้งหนี้แยก (งานบริการ,VAT ไม่ถึงกำหนด) (ขาย) N N

        public static string gc_REFTYPE_P_BILL = "PI";   //P ใบเสร็จรับเงิน (ซื้อ)                                        N
        public static string gc_REFTYPE_P_BILL_VAT = "PV";   //J ใบเสร็จรับเงิน/ใบกำกับภาษี (ซื้อ) (งานบริการ)                N Y
        public static string gc_REFTYPE_S_BILL = "RI";   //R ใบเสร็จรับเงิน (ขาย)                                         N
        public static string gc_REFTYPE_S_BILL_VAT = "RV";   //K ใบเสร็จรับเงิน/ใบกำกับภาษี (ขาย) (งานบริการ)                 N Y

        public static string gc_REFTYPE_ADJ = "AJ";   //A ใบปรับปรุงยอด                                                N
        public static string gc_REFTYPE_ADJ_LOCATION = "AL";   //A ใบปรับยอด (Location)                                               N
        public static string gc_REFTYPE_COUNT_STOCK = "CS";   //V ใบตรวจนับสินค้า                                              N
        public static string gc_REFTYPE_TRANSFER = "TR";   //T ใบโอนสินค้า                                                  N
        public static string gc_REFTYPE_BR_TRANSFER = "TB";   //X ใบโอนสินค้าระหว่างสาขา                                       N
        public static string gc_REFTYPE_RAW_WITH = "WR";   //W ใบเบิกวัตถุดิบ                                               N
        public static string gc_REFTYPE_FINISH_RCV = "FR";   //G ใบรับสินค้าสำเร็จ                                            N
        public static string gc_REFTYPE_WITHDRAW = "WX";   //W ใบเบิกวัสดุสิ้นเปลือง                                        N
        public static string gc_REFTYPE_WORK_END = "WE";   //V ใบปิดการผลิต                                                 N
        public static string gc_REFTYPE_RET_RAW_WITH = "RW";   //d ใบรับคืนจากการเบิกวัตถุดิบ
        public static string gc_REFTYPE_RET_WITHDRAW = "RX";   //d ใบรับคืนจากการเบิกวัสดุ

        public static string gc_REFTYPE_S_TAX_3 = "WP";   //4 ใบภาษีหัก ณ ที่จ่าย บุคคลธรรมดาถูกหัก ไม่ต้องนำส่ง           N
        public static string gc_REFTYPE_S_TAX_5 = "WL";   //6 ใบภาษีหัก ณ ที่จ่าย บริษัทถูกหัก ไม่นำส่งแต่ไว้ตรวจสอบรายการ N
        public static string gc_REFTYPE_P_TAX_3 = "XP";   //3 ใบภาษีหัก ณ ที่จ่าย บริษัท หักบุคคลธรรมดา ยื่นภงด3           N
        public static string gc_REFTYPE_P_TAX_5 = "XL";   //5 ใบภาษีหัก ณ ที่จ่าย บริษัท หักนิติบุคคล ยื่นภงด.53           N

        public static string gc_REFTYPE_ISSUE_TRI = "SP";   //ใบเบิกเงินทดรองจ่าย
        public static string gc_REFTYPE_CLEAR_TRI = "RP";   //ใบเคลียร์เงินทดรองจ่าย
        public static string gc_REFTYPE_EMPL_BORROW = "EB";   //ใบยืมเงินรองจ่าย
        public static string gc_REFTYPE_EMPL_RETURN = "ER";   //ใบคืนเงินรองจ่าย

        public static string gc_REFTYPE_PLEDGE_1 = "G1";   //ใบรับเงินมัดจำ แบบ1
        public static string gc_REFTYPE_PLEDGE_2 = "G2";   //ใบรับเงินมัดจำ แบบ2

        public static string gc_REFTYPE_DO_TAX = "D1";   //ใบ DO แบบ1
        public static string gc_REFTYPE_DO_TAX_CR = "DC";   //ใบ ลดหนี้ DO แบบ1
        public static string gc_REFTYPE_DO_TAX_DR = "DD";   //ใบ เพิ่มหนี้ DO แบบ1
        public static string gc_REFTYPE_DO_2 = "D2";   //ใบ DO แบบ2


        public static string gc_REFTYPE_P_TAX_1 = "X1";   //1 ใบภาษีหัก ณ ที่จ่าย ยื่นภงด.1
        public static string gc_REFTYPE_P_TAX_2 = "X2";   //2 ใบภาษีหัก ณ ที่จ่าย ยื่นภงด.2
        public static string gc_REFTYPE_P_TAX_X = "TP";   //7 ใบภาษีหัก ณ ที่จ่าย (กรณีเราเป็นผู้จ่าย/ผู้หักภาษี)	WITHOLDING TAX NOTE (PAY)
        public static string gc_REFTYPE_S_TAX_X = "TS";   //8 ใบภาษีหัก ณ ที่จ่าย (กรณีเราเป็นผู้รับ/ถูกหักภาษี)	WITHOLDING TAX NOTE (NOT-SUBMIT)


        public static string gc_RFTYPE_ADJUST = "A"; //ใบปรับปรุงยอด
        public static string gc_RFTYPE_DR_BUY = "D"; //ใบเพิ่มหนี้การซื้อ
        public static string gc_RFTYPE_DR_SELL = "E"; //ใบเพิ่มหนี้การขาย
        public static string gc_RFTYPE_CR_BUY = "C"; //ใบลดหนี้การซื้อ
        public static string gc_RFTYPE_CR_SELL = "F"; //ใบลดหนี้การขาย
        public static string gc_RFTYPE_PAYMENT = "P"; //ชำระเงิน ทั่วไป
        public static string gc_RFTYPE_RECEIVE = "R"; //รับชำระเงิน ทั่วไป
        public static string gc_RFTYPE_INV_BUY = "B"; //ซื้อ
        public static string gc_RFTYPE_INV_SELL = "S"; //ขาย
        public static string gc_RFTYPE_TAX_3 = "3"; //ภาษีหัก ณ ที่จ่าย (หัก บุคคลธรรมดา )
        public static string gc_RFTYPE_TAX_3I = "4"; //ภาษีหัก ณ ที่จ่าย (บุคคลธรรมดา ถูกหัก )
        public static string gc_RFTYPE_TAX_53 = "5"; //ภาษีหัก ณ ที่จ่าย (หัก นิติบุคคล)
        public static string gc_RFTYPE_TAX_53I = "6"; //ภาษีหัก ณ ที่จ่าย (บริษัท ถูกหัก)
        public static string gc_RFTYPE_PAY_VAT = "J"; //ใบเสร็จรับเงินพร้อมใบกำกับภาษี (ออกรายงานภาษีซื้อ)
        public static string gc_RFTYPE_RCV_VAT = "K"; //ใบเสร็จรับเงินพร้อมใบกำกับภาษี (ออกรายงานภาษีขาย)
        public static string gc_RFTYPE_LAY_BUY = "L"; //ใบวางบิล (ระบบซื้อ)
        public static string gc_RFTYPE_LAY_SELL = "M"; //ใบวางบิล (ระบบขาย)
        public static string gc_RFTYPE_LAY_PD_SELL = "H"; //ใบยืนยันการขาย (ระบบขาย)
        public static string gc_RFTYPE_ORDER_BUY = "N"; //ใบสั่งซื้อ (Order ระบบซื้อ)
        public static string gc_RFTYPE_ORDER_SELL = "O"; //ใบสั่งซื้อ (Order ระบบขาย)
        public static string gc_RFTYPE_TRAN_PD = "T"; //ใบโอนสินค้า
        public static string gc_RFTYPE_ISSUE_PD = "W"; //ใบเบิกวัตถุดิบ
        public static string gc_RFTYPE_PRODUCE_PD = "G"; //ใบรับสินค้าสำเร็จที่ผลิตได้
        public static string gc_RFTYPE_BRANCH_TRAN = "X"; //ใบโอนสินค้าระหว่างสาขา
        public static string gc_RFTYPE_WORK_ORDER = "Q"; //ใบสั่งผลิต
        public static string gc_RFTYPE_WORK_END = "V"; //ใบปิดการผลิต
        public static string gc_RFTYPE_GIVE_SELL = "Y"; //ใบให้ไม่คิดมูลค่า (ระบบขาย)
        public static string gc_RFTYPE_COL_SELL = "Z"; //ใบเก็บเงิน (ระบบขาย)
        public static string gc_RFTYPE_IN_PD = "j"; //ใบเพิ่มสินค้าเข้าสต็อก (ยังไม่ใช้) && Fm4
        public static string gc_RFTYPE_OUT_PD = "o"; //ใบตัดสินค้าออกจากสต็อก (ยังไม่ใช้) && Fm4
        public static string gc_RFTYPE_REP_SELL = "f"; //ใบรับรู้การขาย (ระบบขาย)
        public static string gc_RFTYPE_LEND_SELL = "I"; //ใบยืมสินค้า (ระบบขาย)
        public static string gc_RFTYPE_RETURN_SELL = "U"; //ใบคืนสินค้า (ระบบขาย)
        public static string gc_RFTYPE_LEND_BUY = "a"; //ใบยืมสินค้า (ระบบซื้อ)
        public static string gc_RFTYPE_RETURN_BUY = "b"; //ใบคืนสินค้า (ระบบซื้อ)
        public static string gc_RFTYPE_RETURN_ISSUE = "d"; //ใบรับคืนจากการเบิก (วัตถุดิบ,วัสดุสิ้นเปลือง)
        public static string gc_RFTYPE_ISSUE_TRI = "t"; //ใบเบิกเงินทดลองจ่าย
        public static string gc_RFTYPE_CLEAR_TRI = "k"; //ใบเคลียร์เงินทดลองจ่าย



        public static string gc_GRP_REFTYPE_P_INV_PRODUCT = gc_REFTYPE_P_INV_R + "," + gc_REFTYPE_P_INV_I + "," + gc_REFTYPE_P_INV_Q + ",";
        public static string gc_GRP_REFTYPE_P_INV_SERVICE = gc_REFTYPE_P_INV_U + "," + gc_REFTYPE_P_INV_V + "," + gc_REFTYPE_P_INV_Y + ",";
        public static string gc_GRP_REFTYPE_S_INV_PRODUCT = gc_REFTYPE_S_INV_R + "," + gc_REFTYPE_S_INV_I + "," + gc_REFTYPE_S_INV_Q + "," + gc_REFTYPE_S_INV_G + ",";
        public static string gc_GRP_REFTYPE_S_INV_SERVICE = gc_REFTYPE_S_INV_U + "," + gc_REFTYPE_S_INV_V + "," + gc_REFTYPE_S_INV_Y + ",";
        public static string gc_GRP_REFTYPE_P_CR_PRODUCT = gc_REFTYPE_P_CR_C + "," + gc_REFTYPE_P_CR_M + ",";
        public static string gc_GRP_REFTYPE_P_CR_SERVICE = gc_REFTYPE_P_CR_A + "," + gc_REFTYPE_P_CR_N + ",";
        public static string gc_GRP_REFTYPE_S_CR_PRODUCT = gc_REFTYPE_S_CR_C + "," + gc_REFTYPE_S_CR_M + ",";
        public static string gc_GRP_REFTYPE_S_CR_SERVICE = gc_REFTYPE_S_CR_A + "," + gc_REFTYPE_S_CR_N + ",";
        public static string gc_GRP_REFTYPE_P_DR_PRODUCT = gc_REFTYPE_P_DR_D + "," + gc_REFTYPE_P_DR_X + ",";
        public static string gc_GRP_REFTYPE_P_DR_SERVICE = gc_REFTYPE_P_DR_B + "," + gc_REFTYPE_P_DR_Z + ",";
        public static string gc_GRP_REFTYPE_S_DR_PRODUCT = gc_REFTYPE_S_DR_D + "," + gc_REFTYPE_S_DR_X + ",";
        public static string gc_GRP_REFTYPE_S_DR_SERVICE = gc_REFTYPE_S_DR_B + "," + gc_REFTYPE_S_DR_Z + ",";

        //Prod.Type & Formula.ProdType && ข้อมูลใน Prodtype.DBF
        public static string gc_PROD_TYPE_FINISH = "1";   // สินค้าสำเร็จ     
        public static string gc_PROD_TYPE_RAW_MAT = "2";   // วัตถุดิบ            
        public static string gc_PROD_TYPE_EXPENSE = "3";   // บริการ              
        public static string gc_PROD_TYPE_OFFICE_SUPPLY = "4";   // วัสดุสิ้นเปลือง
        public static string gc_PROD_TYPE_SEMI = "5";   // สินค้ากึ่งสำเร็จ        
        public static string gc_PROD_TYPE_ASSET = "6";   // สินทรัพย์
        public static string gc_PROD_TYPE_INCOME = "7";   // รายได้อื่น ๆ
        public static string gc_PROD_TYPE_OTHERS = "8";   // ค่าใช้จ่ายอื่น ๆ
        public static string gc_PROD_TYPE_COMPO = "9";   // วัสดุประกอบ
        public static string gc_PROD_TYPE_SPARE = "A";   // อะไหล่
        public static string gc_PROD_TYPE_LABEL = "B";   // วัสดุปะผิว
        public static string gc_PROD_TYPE_PACKAGE = "C";   // วัสดุหีบห่อ
        public static string gc_PROD_TYPE_LAND = "D";   // ที่ดิน
        public static string gc_PROD_TYPE_BUILDING = "E";   // อาคาร
        public static string gc_PROD_TYPE_OFFICE_EQUIP = "F";   // อุปกรณ์สำนักงาน
        public static string gc_PROD_TYPE_MACHINE = "G";   // เครื่องจักร
        public static string gc_PROD_TYPE_DIRECT_LABOR = "H";   // ค่าแรงทางตรง
        public static string gc_PROD_TYPE_INDIRECT_LABOR = "I";   // ค่าแรงทางอ้อม
        public static string gc_PROD_TYPE_DIRECT_EXPENSE = "J";   // ค่าใช้จ่ายทางตรง
        public static string gc_PROD_TYPE_INDIRECT_EXPENSE = "K";   // ค่าใช้จ่ายทางอ้อม
        public static string gc_PROD_TYPE_SUBCONTRAC = "L";   // วัสดุประกอบภายนอกโรงงาน
        public static string gc_PROD_TYPE_PLEDGE = "M";   // เงินมัดจำ
        public static string gc_PROD_TYPE_TOOLS = "N";   // เครื่องมือ
        public static string gc_PROD_TYPE_REMARK = "R";   // หมายเหตุ
        public static string gc_PROD_TYPE_INSURANCE = "O";   //เงินประกันผลงาน

        //public static string gc_PROD_TYPE_SET_GOODS	 = "14";   // Prod.Type ที่คิดต้นทุนแบบ Finish Goods ใช้ _SGoodsType , _SGoodsCost
        public static string gc_PROD_TYPE_SET_GOODS = gc_PROD_TYPE_FINISH + gc_PROD_TYPE_OFFICE_SUPPLY;
        public static string gc_PROD_TYPE_SET_RAW = gc_PROD_TYPE_RAW_MAT + gc_PROD_TYPE_COMPO + gc_PROD_TYPE_LABEL + gc_PROD_TYPE_PACKAGE + gc_PROD_TYPE_SUBCONTRAC;
        public static string gc_PROD_TYPE_SET_SEMI = gc_PROD_TYPE_SEMI;

        public static string gc_REFTYPE_BUDALLOC = "AL";   // ปันส่วนงบประมาณ

        public static string gc_ACCHART_CATEG_GROUP = "G";
        public static string gc_ACCHART_CATEG_DETAIL = "D";

        public static string gc_ACCHART_LCATEG_GROUP = "GROUP";
        public static string gc_ACCHART_LCATEG_DETAIL = "DETAIL";

        public static string gc_REFTYPE_BGTRAN1 = "B1";   // ตั้งงบประมาณ
        public static string gc_REFTYPE_BGTRAN2 = "B2";   // ตั้งงบประมาณเหลื่อมปี
        public static string gc_REFTYPE_BGTRAN3 = "B3";   // ตั้งงบประมาณเหลือจ่าย

        public static string gc_REFTYPE_RECV1 = "R1";   // จองงบประมาณ
        public static string gc_REFTYPE_RECV2 = "R2";   // จองงบประมาณ เหลื่อมปี
        public static string gc_REFTYPE_RECV3 = "R3";   // จองงบประมาณ เหลือจ่าย

        public static string gc_REFTYPE_ADV1 = "R4";   // เงินยืมทดรองจ่าย
        public static string gc_REFTYPE_ADV2 = "R5";   // เงินยืมทดรองจ่าย เหลื่อมปี
        public static string gc_REFTYPE_ADV3 = "R6";   // เงินยืมทดรองจ่าย เหลือจ่าย

        public static string gc_REFTYPE_ADJ1 = "A1";   // แก้ไขการจองงบประมาณ
        public static string gc_REFTYPE_ADJ2 = "A2";   // แก้ไขการจองงบประมาณ เหลื่อมปี
        public static string gc_REFTYPE_ADJ3 = "A3";   // แก้ไขการจองงบประมาณ เหลือจ่าย
        public static string gc_REFTYPE_ADJ4 = "A4";   // แก้ไขเงินยืมทดรองจ่าย
        public static string gc_REFTYPE_ADJ5 = "A5";   // แก้ไขเงินยืมทดรองจ่าย เหลื่อมปี
        public static string gc_REFTYPE_ADJ6 = "A6";   // แก้ไขเงินยืมทดรองจ่าย เหลือจ่าย

        public static string gc_REFTYPE_ADJ7 = "A7";   // แก้ไขเบิกจ่ายงบประมาณ
        public static string gc_REFTYPE_ADJ8 = "A8";   // แก้ไขเบิกจ่ายงบประมาณ เหลื่อมปี
        public static string gc_REFTYPE_ADJ9 = "A9";   // แก้ไขเบิกจ่ายงบประมาณ เหลือจ่าย
        public static string gc_REFTYPE_ADJA = "AA";   // แก้ไขเคลียร์เงินยืมทดรองจ่าย
        public static string gc_REFTYPE_ADJB = "AB";   // แก้ไขเคลียร์เงินยืมทดรองจ่าย เหลื่อมปี
        public static string gc_REFTYPE_ADJC = "AC";   // แก้ไขเคลียร์เงินยืมทดรองจ่าย เหลือจ่าย

        public static string gc_REFTYPE_PAY1 = "P1";   // เบิกจ่ายประมาณ
        public static string gc_REFTYPE_PAY2 = "P2";   // เบิกจ่ายประมาณ เหลื่อมปี
        public static string gc_REFTYPE_PAY3 = "P3";   // เบิกจ่ายประมาณ เหลือจ่าย
        public static string gc_REFTYPE_ADC1 = "P4";   // เคลียร์เงินยืมทดรองจ่าย
        public static string gc_REFTYPE_ADC2 = "P5";   // เคลียร์เงินยืมทดรองจ่าย เหลื่อมปี
        public static string gc_REFTYPE_ADC3 = "P6";   // เคลียร์เงินยืมทดรองจ่าย เหลือจ่าย

        public static string gc_BGTRAN_STEP_PREPARE = "1";   //บันทึกงบประมาณก่อนการอนุมัติ
        public static string gc_BGTRAN_STEP_APPROVE = "2";   //พิจารณางบประมาณ
        public static string gc_BGTRAN_STEP_POST = "3";   //Post งบประมาณ
        public static string gc_BGTRAN_STEP_REVISE = "E";   //แก้ไขงบประมาณ

        public static string gc_APPROVE_STEP_WAIT = " ";           //ตั้งงบประมาณยังไม่ผ่านขั้นตอนใด ๆ
        public static string gc_APPROVE_STEP_APPROVE = "A";    //อนุมัติงบประมาณ
        public static string gc_APPROVE_STEP_PASS = "P";          //งบประมาณผ่านการการอนุมัติจากหน่วยงานภายนอกแล้ว
        public static string gc_APPROVE_STEP_REJECT = "R";       //งบประมาณไม่ผ่านการอนุมัติ
        public static string gc_APPROVE_STEP_POST = "T";          //งบประมาณ POST รายการแล้วแก้ไขไม่ได้
        public static string gc_APPROVE_STEP_REVISE = "E";         //แก้ไขงบประมาณ

        public static string gc_APPROVE_TEXT_WAIT = "WAIT";           //ตั้งงบประมาณยังไม่ผ่านขั้นตอนใด ๆ
        public static string gc_APPROVE_TEXT_APPROVE = "APPROVE";    //อนุมัติงบประมาณ
        public static string gc_APPROVE_TEXT_PASS = "PASS";          //งบประมาณผ่านการการอนุมัติจากหน่วยงานภายนอกแล้ว
        public static string gc_APPROVE_TEXT_REJECT = "REJECT";       //งบประมาณไม่ผ่านการอนุมัติ
        public static string gc_APPROVE_TEXT_POST = "POST";          //งบประมาณ POST รายการแล้วแก้ไขไม่ได้

        public static string gc_BGCHART_CATEG_HR = "1";           //งบบุคลากร
        public static string gc_BGCHART_CATEG_OPERATE = "2";  //งบดำเนินงาน
        public static string gc_BGCHART_CATEG_INVEST = "3";    //งบลงทุน
        public static string gc_BGCHART_CATEG_SUPPORT = "4"; //งบอุดหนุน
        public static string gc_BGCHART_CATEG_OTHER = "5";    //งบรายจ่ายอื่น

        public static string gc_RESOURCE_TYPE_MACHINE = "M";    //เครื่องจักร
        public static string gc_RESOURCE_TYPE_TOOL = "T";    //เครื่องมือ

        public static string gc_PROCURE_TYPE_PURCHASE = "P";	//จากการซื้อ
        public static string gc_PROCURE_TYPE_MANUFACURING = "M";	//จากการผลิตเอง
        public static string gc_PROCURE_TYPE_SUBCONTRACT = "S";	//จากการจ้างทำ
        public static string gc_PROCURE_TYPE_NON_PURCHASE = "N";	//ไว้คิด Cost

        public static string gc_BOM_TYPE_PACKAGING = "P";	//Packaging BOM
        public static string gc_BOM_TYPE_MANUFACURING = "M";	//Manufacturing BOM

        public static string gc_ROUND_TYPE_NORMAL = " ";	//ปัดขึ้นลง ปรกติ
        public static string gc_ROUND_TYPE_UP = "1";	//มีเศษจะปัดขึ้นเสมอ เช่น 3.1 = 4, 3.6 = 4
        public static string gc_ROUND_TYPE_DOWN = "2";	//ปัดลงเสมอ  เช่น 3.1 = 3, 3.6 = 3

        public static string gc_REF_OPSTEP_CREATE = " ";
        public static string gc_REF_OPSTEP_START = "5";
        public static string gc_REF_OPSTEP_FINISH = "9";

        public static string gc_REF_OP_COSTTYPE_FIX = "FIX";
        
        public static string gc_REF_OP_COSTTYPE_VARM1 = "VM1";
        public static string gc_REF_OP_COSTTYPE_VARM2 = "VM2";
        public static string gc_REF_OP_COSTTYPE_VARM3 = "VM3";
        public static string gc_REF_OP_COSTTYPE_VARM4 = "VM4";
        public static string gc_REF_OP_COSTTYPE_VARM5 = "VM5";

        public static string gc_REF_OP_COSTTYPE_VARP1 = "VP1";
        public static string gc_REF_OP_COSTTYPE_VARP2 = "VP2";
        public static string gc_REF_OP_COSTTYPE_VARP3 = "VP3";
        public static string gc_REF_OP_COSTTYPE_VARP4 = "VP4";
        public static string gc_REF_OP_COSTTYPE_VARP5 = "VP5";

        public static string gc_COST_UNIT_NONE = "-";
        public static string gc_COST_UNIT_SEC = "SEC";
        public static string gc_COST_UNIT_MIN = "MIN";
        public static string gc_COST_UNIT_HOUR = "HOUR";
        public static string gc_COST_UNIT_DAY = "DAY";

        public static string gc_AUTH_RIG_CHGTRANDATE = "CHGTRANDATE";

    }

}