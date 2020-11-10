using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class BusinessEnum
    {

        public static string gc_KEEPLOG_TYPE_INSERT = "A";
        public static string gc_KEEPLOG_TYPE_UPDATE = "U";
        public static string gc_KEEPLOG_TYPE_DELETE = "D";
        public static string gc_KEEPLOG_TYPE_LOG_IN = "I";
        public static string gc_KEEPLOG_TYPE_LOG_OUT = "O";
        public static string gc_KEEPLOG_TYPE_PRINT = "P";

        public static DocumentType GetDocEnum(string inKey)
        {
            return (DocumentType)System.Enum.Parse(Type.GetType("BeSmartMRP.Business.Entity.DocumentType"), inKey);
        }

        public static string gc_PRICE_POLICY_SALE = "L";
        public static string gc_APP_FM3AP = "$-";	//Formula 3 A/P
        public static string gc_APP_FM3AR = "$,";	//Formula 3 A/R
        public static string gc_APP_FM3PA = "$/";	//Formula 3 PA
        public static string gc_APP_FM3SA = "$.";	//Formula 3 SA
        public static string gc_APP_FM3ST = "$0";	//Formula 3 ST
        public static string gc_APP_FM3CQ = "$W";	//Formula 3 CQ
        public static string gc_APP_FM3FA = "$Y";	//Formula 3 FA

        //PdSer.SStep        && ด้านซื้อ
        public static string gc_PDSER_PSTEP_FREE = "*";	//ไม่มีเอกสารซื้ออ้างถึง
        public static string gc_PDSER_PSTEP_ORDER = "0";	//Order ซื้อ ยังไม่มีสินค้าแล้ว
        public static string gc_PDSER_PSTEP_INV = "5";	//มี Inv ซื้อ มีสินค้าแล้ว
        public static string gc_PDSER_PSTEP_RETURN = "7";	//คืนของ Supplier แล้ว
        //PdSer.SStep        && ด้านขาย
        public static string gc_PDSER_SSTEP_FREE = ":";	//ไม่มีเอกสารขายอ้างถึง
        public static string gc_PDSER_SSTEP_ORDER = "?";	//Order ขาย จองไว้
        public static string gc_PDSER_SSTEP_INV = "D";	//มี Inv ขาย ขายไปแล้ว

        //Prod.CtrlStock
        public static string gc_PROD_CTRL_STOCK_CORP = "0";					// ตามบริษัทกำหนด
        public static string gc_PROD_CTRL_STOCK_NOT_NEGATIVE = "1";		//นับยอดไม่ให้ติดลบ
        public static string gc_PROD_CTRL_STOCK_NEGATIVE_OK = "2";		//ติดลบได้
        public static string gc_PROD_CTRL_STOCK_NO_COUNT = "3";			//ไม่นับยอด

        public static string gc_PROD_TYPE_FINISH = "1";  //สินค้าสำเร็จ        
        public static string gc_PROD_TYPE_RAW_MAT = "2";  //วัตถุดิบ            
        public static string gc_PROD_TYPE_EXPENSE = "3";  //บริการ              
        public static string gc_PROD_TYPE_OFFICE_SUPPLY = "4";  //วัสดุสิ้นเปลือง
        public static string gc_PROD_TYPE_SEMI = "5";  //สินค้ากึ่งสำเร็จ        
        public static string gc_PROD_TYPE_ASSET = "6";  //สินทรัพย์
        public static string gc_PROD_TYPE_INCOME = "7";  //รายได้อื่น ๆ
        public static string gc_PROD_TYPE_OTHERS = "8";  //ค่าใช้จ่ายอื่น ๆ
        public static string gc_PROD_TYPE_COMPO = "9";  //วัสดุประกอบ
        public static string gc_PROD_TYPE_SPARE = "A";  //อะไหล่
        public static string gc_PROD_TYPE_LABEL = "B";  //วัสดุปะผิว
        public static string gc_PROD_TYPE_PACKAGE = "C";  //วัสดุหีบห่อ
        public static string gc_PROD_TYPE_LAND = "D";  //ที่ดิน
        public static string gc_PROD_TYPE_BUILDING = "E";  //อาคาร
        public static string gc_PROD_TYPE_OFFICE_EQUIP = "F";  //อุปกรณ์สำนักงาน
        public static string gc_PROD_TYPE_MACHINE = "G";  //เครื่องจักร
        public static string gc_PROD_TYPE_DIRECT_LABOR = "H";  //ค่าแรงทางตรง
        public static string gc_PROD_TYPE_INDIRECT_LABOR = "I";  //ค่าแรงทางอ้อม
        public static string gc_PROD_TYPE_DIRECT_EXPENSE = "J";  //ค่าใช้จ่ายทางตรง
        public static string gc_PROD_TYPE_INDIRECT_EXPENSE = "K";  //ค่าใช้จ่ายทางอ้อม
        public static string gc_PROD_TYPE_SUBCONTRAC = "L";  //วัสดุประกอบภายนอกโรงงาน
        public static string gc_PROD_TYPE_PLEDGE = "M";  //เงินมัดจำ
        public static string gc_PROD_TYPE_TOOLS = "N";  //เครื่องมือ
        public static string gc_PROD_TYPE_REMARK = "R";  //หมายเหตุ
        public static string gc_PROD_TYPE_INSURANCE = "O";  //เงินประกันผลงาน

        public static string gc_PROD_TYPE_SET_GOODS = gc_PROD_TYPE_FINISH + gc_PROD_TYPE_OFFICE_SUPPLY;
        public static string gc_PROD_TYPE_SET_RAW = gc_PROD_TYPE_RAW_MAT + gc_PROD_TYPE_COMPO + gc_PROD_TYPE_LABEL + gc_PROD_TYPE_PACKAGE + gc_PROD_TYPE_SUBCONTRAC;
        public static string gc_PROD_TYPE_SET_SEMI = gc_PROD_TYPE_SEMI;

        public static string gc_RFTYPE_ADJUST = "A";   //ใบปรับปรุงยอด
        public static string gc_RFTYPE_DR_BUY = "D";   //ใบเพิ่มหนี้การซื้อ
        public static string gc_RFTYPE_DR_SELL = "E";   //ใบเพิ่มหนี้การขาย
        public static string gc_RFTYPE_CR_BUY = "C";   //ใบลดหนี้การซื้อ
        public static string gc_RFTYPE_CR_SELL = "F";   //ใบลดหนี้การขาย
        public static string gc_RFTYPE_PAYMENT = "P";   //ชำระเงิน ทั่วไป
        public static string gc_RFTYPE_RECEIVE = "R";   //รับชำระเงิน ทั่วไป
        public static string gc_RFTYPE_INV_BUY = "B";   //ซื้อ
        public static string gc_RFTYPE_INV_SELL = "S";   //ขาย
        public static string gc_RFTYPE_TAX_3 = "3";   //ภาษีหัก ณ ที่จ่าย (หัก บุคคลธรรมดา )
        public static string gc_RFTYPE_TAX_3I = "4";   //ภาษีหัก ณ ที่จ่าย (บุคคลธรรมดา ถูกหัก )
        public static string gc_RFTYPE_TAX_53 = "5";   //ภาษีหัก ณ ที่จ่าย (หัก นิติบุคคล)
        public static string gc_RFTYPE_TAX_53I = "6";   //ภาษีหัก ณ ที่จ่าย (บริษัท ถูกหัก)
        public static string gc_RFTYPE_PAY_VAT = "J";   //ใบเสร็จรับเงินพร้อมใบกำกับภาษี (ออกรายงานภาษีซื้อ)
        public static string gc_RFTYPE_RCV_VAT = "K";   //ใบเสร็จรับเงินพร้อมใบกำกับภาษี (ออกรายงานภาษีขาย)
        public static string gc_RFTYPE_LAY_BUY = "L";   //ใบวางบิล (ระบบซื้อ)
        public static string gc_RFTYPE_LAY_SELL = "M";   //ใบวางบิล (ระบบขาย)
        public static string gc_RFTYPE_LAY_PD_SELL = "H";   //ใบยืนยันการขาย (ระบบขาย)
        public static string gc_RFTYPE_ORDER_BUY = "N";   // ใบสั่งซื้อ (Order ระบบซื้อ)
        public static string gc_RFTYPE_ORDER_SELL = "O";   //ใบสั่งซื้อ (Order ระบบขาย)
        public static string gc_RFTYPE_TRAN_PD = "T";   //ใบโอนสินค้า
        public static string gc_RFTYPE_ISSUE_PD = "W";   //ใบเบิกวัตถุดิบ
        public static string gc_RFTYPE_PRODUCE_PD = "G";   //ใบรับสินค้าสำเร็จที่ผลิตได้
        public static string gc_RFTYPE_BRANCH_TRAN = "X";   //ใบโอนสินค้าระหว่างสาขา
        public static string gc_RFTYPE_WORK_ORDER = "Q";   //ใบสั่งผลิต
        public static string gc_RFTYPE_WORK_END = "V";   // ใบปิดการผลิต
        public static string gc_RFTYPE_GIVE_SELL = "Y";   //ใบให้ไม่คิดมูลค่า (ระบบขาย)
        public static string gc_RFTYPE_COL_SELL = "Z";   //ใบเก็บเงิน (ระบบขาย)
        public static string gc_RFTYPE_IN_PD = "j";   //ใบเพิ่มสินค้าเข้าสต็อก (ยังไม่ใช้) && Fm4
        public static string gc_RFTYPE_OUT_PD = "o";   //ใบตัดสินค้าออกจากสต็อก (ยังไม่ใช้) && Fm4
        public static string gc_RFTYPE_REP_SELL = "f";   //ใบรับรู้การขาย (ระบบขาย)
        public static string gc_RFTYPE_LEND_SELL = "I";   //ใบยืมสินค้า (ระบบขาย)
        public static string gc_RFTYPE_RETURN_SELL = "U";   //ใบคืนสินค้า (ระบบขาย)
        public static string gc_RFTYPE_LEND_BUY = "a";   //ใบยืมสินค้า (ระบบซื้อ)
        public static string gc_RFTYPE_RETURN_BUY = "b";   //ใบคืนสินค้า (ระบบซื้อ)
        public static string gc_RFTYPE_RETURN_ISSUE = "d";   //ใบรับคืนจากการเบิก (วัตถุดิบ,วัสดุสิ้นเปลือง)
        public static string gc_RFTYPE_ISSUE_TRI = "t";   //ใบเบิกเงินทดลองจ่าย
        public static string gc_RFTYPE_CLEAR_TRI = "k";   //ใบเคลียร์เงินทดลองจ่าย

        public static string gc_RFTYPE_TAX_1 = "1";   //ภาษีหัก ณ ที่จ่าย ภงด.1
        public static string gc_RFTYPE_TAX_2 = "2";   //ภาษีหัก ณ ที่จ่าย ภงด.2
        public static string gc_RFTYPE_P_TAX_X = "7";   //ใบภาษีหัก ณ ที่จ่าย (กรณีเราเป็นผู้จ่าย/ผู้หักภาษี)
        public static string gc_RFTYPE_S_TAX_X = "8";   //ใบภาษีหัก ณ ที่จ่าย (กรณีเราเป็นผู้รับ/ถูกหักภาษี)

        public static string gc_RFTYPE_CHEQUE = "c";   //เช็ค
        public static string gc_RFTYPE_VOUCHER = "v";   //Voucher
        public static string gc_RFTYPE_REQUEST_BUY = "w";   // ใบขอซื้อ , ขอจ้าง (ระบบซื้อ)
        public static string gc_RFTYPE_REQUEST_SELL = "x";   // ใบขอซื้อ (ระบบขาย)
        public static string gc_RFTYPE_EMPL_BORROW = "y";   //ใบยืมเงินรองจ่าย
        public static string gc_RFTYPE_EMPL_RETURN = "z";   //ใบคืนเงินรองจ่าย
        public static string gc_RFTYPE_QUOTATION_SELL = "q";   //ใบเสนอราคา (ระบบขาย)
        public static string gc_RFTYPE_SALENOTE_SELL = "n";   //Sale note (ระบบขาย)
    }

    public enum DocumentType
    {
        QS, SO, SS,
        SI, SQ, SR, SU, SV, SY, SG,
        SD, SX, SB, SZ,
        SC, SA, SM, SN,
        PR, PO,
        BI, BQ, BR, BU, BV, BY,
        BD, BX, BB, BZ,
        BC, BA, BM, BN,
        MO, MC, RJ, MQ,
        AJ, AL, CS, TR, TB, FR, WR, WX, RW, RX,IM,
        L1, L2,
        AC, A1, A2, A3, A4, A5, A6, A7, A8, A9, AA,
        AB, B1, B2,
        B3, P1, P2, P3, P4, P5,
        P6, R1, R2, R3, R4, R5,
        R6, T1, T2, T3,
        GD, Q1
    }

    public enum CoorType
    {
        Customer, Supplier
    }

    public enum WareHouseType
    {
        Normal, WIP, OfficeSupply
    }

    public enum WHLocaType
    {
        Normal
    }

    public enum OrganizationType
    {
        Detail, Group
    }

    public enum AppUserType
    {
        User, Group
    }

    public enum AuthenType
    {
        Visible, AskPassword, CanAccess, CanInsert, CanEdit, CanDelete, CanPrint, CanExport, CanApprove, CanApprove2,IsAllow
    }

    public enum AppCodeName
    {
        MF2, MF1, MCM, ERP, CRM, SCM
    }

    /// <summary>
    ///Application Module
    /// </summary>
    public enum AppModule
    {
        B1, B2, B3, B4, R5, SM
    }

    public enum AppObjType
    {
        Bar, Pad, Item
    }

    public enum AppObjMenuType
    {
        None, Input, Report, Process, Config
    }

    public enum KeepLogType
    {
        Insert, Update, Delete, Cancel, Close, LogIn, LogOut, Print
    }

    public enum KeepLogChagneType
    {
        None, ChgCode, ChgName, Cancel, Close
    }

    public enum PostLevel
    {
        Top, Middle, Operator
    }

    public enum BudgetStep
    {
        Prepare, Approve, Post, Revise
    }

    public enum ApproveStep
    {
        Wait, Approve, Reject, Post, Revise, Pass
    }

    public enum MfgResourceType
    {
        Machine, Tool, CostM1, CostM2, CostM3, CostM4, CostM5
    }

}
