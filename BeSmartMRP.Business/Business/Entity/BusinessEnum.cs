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

        //PdSer.SStep        && ��ҹ����
        public static string gc_PDSER_PSTEP_FREE = "*";	//������͡��ë�����ҧ�֧
        public static string gc_PDSER_PSTEP_ORDER = "0";	//Order ���� �ѧ������Թ�������
        public static string gc_PDSER_PSTEP_INV = "5";	//�� Inv ���� ���Թ�������
        public static string gc_PDSER_PSTEP_RETURN = "7";	//�׹�ͧ Supplier ����
        //PdSer.SStep        && ��ҹ���
        public static string gc_PDSER_SSTEP_FREE = ":";	//������͡��â����ҧ�֧
        public static string gc_PDSER_SSTEP_ORDER = "?";	//Order ��� �ͧ���
        public static string gc_PDSER_SSTEP_INV = "D";	//�� Inv ��� ��������

        //Prod.CtrlStock
        public static string gc_PROD_CTRL_STOCK_CORP = "0";					// �������ѷ��˹�
        public static string gc_PROD_CTRL_STOCK_NOT_NEGATIVE = "1";		//�Ѻ�ʹ������Դź
        public static string gc_PROD_CTRL_STOCK_NEGATIVE_OK = "2";		//�Դź��
        public static string gc_PROD_CTRL_STOCK_NO_COUNT = "3";			//���Ѻ�ʹ

        public static string gc_PROD_TYPE_FINISH = "1";  //�Թ��������        
        public static string gc_PROD_TYPE_RAW_MAT = "2";  //�ѵ�شԺ            
        public static string gc_PROD_TYPE_EXPENSE = "3";  //��ԡ��              
        public static string gc_PROD_TYPE_OFFICE_SUPPLY = "4";  //��ʴ�������ͧ
        public static string gc_PROD_TYPE_SEMI = "5";  //�Թ��ҡ�������        
        public static string gc_PROD_TYPE_ASSET = "6";  //�Թ��Ѿ��
        public static string gc_PROD_TYPE_INCOME = "7";  //�������� �
        public static string gc_PROD_TYPE_OTHERS = "8";  //����������� �
        public static string gc_PROD_TYPE_COMPO = "9";  //��ʴػ�Сͺ
        public static string gc_PROD_TYPE_SPARE = "A";  //������
        public static string gc_PROD_TYPE_LABEL = "B";  //��ʴػм��
        public static string gc_PROD_TYPE_PACKAGE = "C";  //��ʴ��պ���
        public static string gc_PROD_TYPE_LAND = "D";  //���Թ
        public static string gc_PROD_TYPE_BUILDING = "E";  //�Ҥ��
        public static string gc_PROD_TYPE_OFFICE_EQUIP = "F";  //�ػ�ó��ӹѡ�ҹ
        public static string gc_PROD_TYPE_MACHINE = "G";  //����ͧ�ѡ�
        public static string gc_PROD_TYPE_DIRECT_LABOR = "H";  //����ç�ҧ�ç
        public static string gc_PROD_TYPE_INDIRECT_LABOR = "I";  //����ç�ҧ����
        public static string gc_PROD_TYPE_DIRECT_EXPENSE = "J";  //�������·ҧ�ç
        public static string gc_PROD_TYPE_INDIRECT_EXPENSE = "K";  //�������·ҧ����
        public static string gc_PROD_TYPE_SUBCONTRAC = "L";  //��ʴػ�Сͺ��¹͡�ç�ҹ
        public static string gc_PROD_TYPE_PLEDGE = "M";  //�Թ�Ѵ��
        public static string gc_PROD_TYPE_TOOLS = "N";  //����ͧ���
        public static string gc_PROD_TYPE_REMARK = "R";  //�����˵�
        public static string gc_PROD_TYPE_INSURANCE = "O";  //�Թ��Сѹ�ŧҹ

        public static string gc_PROD_TYPE_SET_GOODS = gc_PROD_TYPE_FINISH + gc_PROD_TYPE_OFFICE_SUPPLY;
        public static string gc_PROD_TYPE_SET_RAW = gc_PROD_TYPE_RAW_MAT + gc_PROD_TYPE_COMPO + gc_PROD_TYPE_LABEL + gc_PROD_TYPE_PACKAGE + gc_PROD_TYPE_SUBCONTRAC;
        public static string gc_PROD_TYPE_SET_SEMI = gc_PROD_TYPE_SEMI;

        public static string gc_RFTYPE_ADJUST = "A";   //㺻�Ѻ��ا�ʹ
        public static string gc_RFTYPE_DR_BUY = "D";   //�����˹���ë���
        public static string gc_RFTYPE_DR_SELL = "E";   //�����˹���â��
        public static string gc_RFTYPE_CR_BUY = "C";   //�Ŵ˹���ë���
        public static string gc_RFTYPE_CR_SELL = "F";   //�Ŵ˹���â��
        public static string gc_RFTYPE_PAYMENT = "P";   //�����Թ �����
        public static string gc_RFTYPE_RECEIVE = "R";   //�Ѻ�����Թ �����
        public static string gc_RFTYPE_INV_BUY = "B";   //����
        public static string gc_RFTYPE_INV_SELL = "S";   //���
        public static string gc_RFTYPE_TAX_3 = "3";   //�����ѡ � ������ (�ѡ �ؤ�Ÿ����� )
        public static string gc_RFTYPE_TAX_3I = "4";   //�����ѡ � ������ (�ؤ�Ÿ����� �١�ѡ )
        public static string gc_RFTYPE_TAX_53 = "5";   //�����ѡ � ������ (�ѡ �ԵԺؤ��)
        public static string gc_RFTYPE_TAX_53I = "6";   //�����ѡ � ������ (����ѷ �١�ѡ)
        public static string gc_RFTYPE_PAY_VAT = "J";   //������Ѻ�Թ�����㺡ӡѺ���� (�͡��§ҹ���ի���)
        public static string gc_RFTYPE_RCV_VAT = "K";   //������Ѻ�Թ�����㺡ӡѺ���� (�͡��§ҹ���բ��)
        public static string gc_RFTYPE_LAY_BUY = "L";   //��ҧ��� (�к�����)
        public static string gc_RFTYPE_LAY_SELL = "M";   //��ҧ��� (�к����)
        public static string gc_RFTYPE_LAY_PD_SELL = "H";   //��׹�ѹ��â�� (�к����)
        public static string gc_RFTYPE_ORDER_BUY = "N";   // ���觫��� (Order �к�����)
        public static string gc_RFTYPE_ORDER_SELL = "O";   //���觫��� (Order �к����)
        public static string gc_RFTYPE_TRAN_PD = "T";   //��͹�Թ���
        public static string gc_RFTYPE_ISSUE_PD = "W";   //��ԡ�ѵ�شԺ
        public static string gc_RFTYPE_PRODUCE_PD = "G";   //��Ѻ�Թ�������稷���Ե��
        public static string gc_RFTYPE_BRANCH_TRAN = "X";   //��͹�Թ��������ҧ�Ң�
        public static string gc_RFTYPE_WORK_ORDER = "Q";   //���觼�Ե
        public static string gc_RFTYPE_WORK_END = "V";   // 㺻Դ��ü�Ե
        public static string gc_RFTYPE_GIVE_SELL = "Y";   //�������Դ��Ť�� (�к����)
        public static string gc_RFTYPE_COL_SELL = "Z";   //����Թ (�к����)
        public static string gc_RFTYPE_IN_PD = "j";   //������Թ������ʵ�͡ (�ѧ�����) && Fm4
        public static string gc_RFTYPE_OUT_PD = "o";   //㺵Ѵ�Թ����͡�ҡʵ�͡ (�ѧ�����) && Fm4
        public static string gc_RFTYPE_REP_SELL = "f";   //��Ѻ����â�� (�к����)
        public static string gc_RFTYPE_LEND_SELL = "I";   //�����Թ��� (�к����)
        public static string gc_RFTYPE_RETURN_SELL = "U";   //㺤׹�Թ��� (�к����)
        public static string gc_RFTYPE_LEND_BUY = "a";   //�����Թ��� (�к�����)
        public static string gc_RFTYPE_RETURN_BUY = "b";   //㺤׹�Թ��� (�к�����)
        public static string gc_RFTYPE_RETURN_ISSUE = "d";   //��Ѻ�׹�ҡ����ԡ (�ѵ�شԺ,��ʴ�������ͧ)
        public static string gc_RFTYPE_ISSUE_TRI = "t";   //��ԡ�Թ���ͧ����
        public static string gc_RFTYPE_CLEAR_TRI = "k";   //��������Թ���ͧ����

        public static string gc_RFTYPE_TAX_1 = "1";   //�����ѡ � ������ ���.1
        public static string gc_RFTYPE_TAX_2 = "2";   //�����ѡ � ������ ���.2
        public static string gc_RFTYPE_P_TAX_X = "7";   //������ѡ � ������ (�ó�����繼�����/����ѡ����)
        public static string gc_RFTYPE_S_TAX_X = "8";   //������ѡ � ������ (�ó�����繼���Ѻ/�١�ѡ����)

        public static string gc_RFTYPE_CHEQUE = "c";   //��
        public static string gc_RFTYPE_VOUCHER = "v";   //Voucher
        public static string gc_RFTYPE_REQUEST_BUY = "w";   // 㺢ͫ��� , �ͨ�ҧ (�к�����)
        public static string gc_RFTYPE_REQUEST_SELL = "x";   // 㺢ͫ��� (�к����)
        public static string gc_RFTYPE_EMPL_BORROW = "y";   //�����Թ�ͧ����
        public static string gc_RFTYPE_EMPL_RETURN = "z";   //㺤׹�Թ�ͧ����
        public static string gc_RFTYPE_QUOTATION_SELL = "q";   //��ʹ��Ҥ� (�к����)
        public static string gc_RFTYPE_SALENOTE_SELL = "n";   //Sale note (�к����)
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
