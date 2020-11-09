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

        public static string gc_VAT_NOT_PRN = "4";	// ����� vat

        public static string gc_PAYTYPE_ATS = "ATS";	// ����¡�úѭ���ѵ��ѵ�
        public static string gc_PAYTYPE_CD = "CD ";	// �ѵ��ôԵ
        public static string gc_PAYTYPE_CG = "CG ";	// �͹�Թ��ҹ�������
        public static string gc_PAYTYPE_CW = "CW ";	// �͹����
        public static string gc_PAYTYPE_BB = "BB ";	// �ҡ���� �Թ��ҵ��.
        public static string gc_PAYTYPE_CL = "CL ";	// �ҡ���礸�Ҥ�����
        public static string gc_PAYTYPE_HC = "HC ";	// �ҡ���礸�Ҥ��
        public static string gc_PAYTYPE_CM = "CM ";	// ��Ҹ�������
        public static string gc_PAYTYPE_CR = "CR ";	// �礤׹
        public static string gc_PAYTYPE_CS = "CS ";	// �͹�Թʴ
        public static string gc_PAYTYPE_DD = "DD ";	// �ҡ�´�ҿ��
        public static string gc_PAYTYPE_INW = "INW";	// �͡����
        public static string gc_PAYTYPE_LC = "LC ";	// ��������Ϳ�ôԵ
        public static string gc_PAYTYPE_PC = "PC ";	// �ҡ���Թʴ
        public static string gc_PAYTYPE_TRD = "TRD";	// �ҡ�¡���͹
        public static string gc_PAYTYPE_TRW = "TRW";	// �͹�¡���͹
        public static string gc_PAYTYPE_CHI = "CHI";	// �Թʴ�Ѻ
        public static string gc_PAYTYPE_CHO = "CHO";	// �Թʴ����
        public static string gc_PAYTYPE_RF = "RF ";	// Refer 仴ٷ�� payment �ͧ�͡������� (��������稢ͧ �ǹ�Ե�)
        public static string gc_PAYTYPE_OC = "OV ";	// ��� OV
        public static string gc_PAYTYPE_OCI = "OCI";	// �Թ�Թ
        public static string gc_PAYTYPE_COU = "COU";	// Coupon
        public static string gc_PAYTYPE_MO = "MO ";	// ��ҳѵ� Money Order
        public static string gc_PAYTYPE_CC = "CC ";	// �ҡ�������¡��
        public static string gc_PAYTYPE_RP = "RP"; 	// ������Ѻ�Թ�ҡ��ѡ�Թ��Сѹ�ŧҹ (����)
        public static string gc_PAYTYPE_RS = "RS"; 	// ������Ѻ�Թ�ҡ��ѡ�Թ��Сѹ�ŧҹ (���)

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

        public static string gc_REF_STEP2_NORMAL = " ";	// ���� ����ͧ���͡�������ҵѴ
        public static string gc_REF_STEP2_WAIT = "1";	// �ͷ� PR
        public static string gc_REF_STEP2_PR = "3";	// �� PR �ú����
        public static string gc_REF_STEP2_CLOSED = "5";	// Closed ����

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
        public static string gc_WHOUSE_TYPE_DAMAGE = "D";		//��ѧ�ͧ����
        public static string gc_WHOUSE_TYPE_STORAGE = "S";		//��ѧ⡴ѧ

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

        public static string gc_STEPX1_COLLECT = "1";	//�Ǻ���������
        public static string gc_STEPX1_CREATED = "3";	//��鹨Ѵ��
        public static string gc_STEPX1_WAITAPPR = "5";	//��͹��ѵ�
        public static string gc_STEPX1_WAITORD = "7";	//���Ѻ�����
        public static string gc_STEPX1_COORAPPR = "9";	//�١���͹��ѵ�

        public static string gc_REFTYPE_P_ORDER = "PO";   // N ���觫��� (Order �к�����)                                  N
        public static string gc_REFTYPE_S_ORDER = "SO";   // O ��Ѻ����觫��� (Order �к����)                              N
        public static string gc_REFTYPE_P_REQUEST = "PR";   // w 㺢ͫ��� (�к�����)
        public static string gc_REFTYPE_P_HIRE = "PH";   // w 㺢ͨ�ҧ (�к�����)
        public static string gc_REFTYPE_P_CONTACT = "PJ";   // �ѭ�Ҩ�ҧ
        public static string gc_REFTYPE_S_REQUEST = "SH";   // x 㺢ͫ��� (�к����)
        public static string gc_REFTYPE_W_ORDER = "WO";   // Q ���觼�Ե (Work Order)                              N
        public static string gc_REFTYPE_P_LEND = "BS";   // I �����Թ��Ҩҡ����˹���                               N
        public static string gc_REFTYPE_P_RET = "BT";   // I 㺤׹�Թ��Ҩҡ������������˹���
        public static string gc_REFTYPE_S_LEND = "SS";   // I �����Թ��Ңͧ�١���                              N
        public static string gc_REFTYPE_S_RET = "ST";   // I 㺤׹�Թ��Ҩҡ�������ͧ�١���
        public static string gc_REFTYPE_S_REP_F = "SF";   // ��Ѻ����â�� (�к����)

        public static string gc_REFTYPE_S_QUATATION = "QS";   // q ��ʹ��Ҥ� (�к����)

        public static string gc_REFTYPE_P_CR_C = "BC";   //F �Ŵ˹�� �Թʴ (����)                                               N
        public static string gc_REFTYPE_P_CR_A = "BA";   //F �Ŵ˹�� �Թʴ ��ԡ�� (����)                                               N
        public static string gc_REFTYPE_P_CR_M = "BM";   //F �Ŵ˹�� �Թ���� (����)                                               N
        public static string gc_REFTYPE_P_CR_N = "BN";   //F �Ŵ˹�� �Թ���� ��ԡ�� (����)                                               N
        public static string gc_REFTYPE_P_DR_D = "BD";   //E �����˹�� �Թʴ (����)                                            N
        public static string gc_REFTYPE_P_DR_B = "BB";   //E �����˹�� �Թʴ ��ԡ�� (����)                                            N
        public static string gc_REFTYPE_P_DR_X = "BX";   //E �����˹�� �Թ���� (����)                                            N
        public static string gc_REFTYPE_P_DR_Z = "BZ";   //E �����˹�� �Թ���� ��ԡ�� (����)                                            N
        public static string gc_REFTYPE_P_INV_R = "BR";   //B ��觢ͧ/㺡ӡѺ����/������Ѻ�Թ(����Թʴ) (����)        Y Y
        public static string gc_REFTYPE_P_INV_I = "BI";   //B ��觢ͧ/㺡ӡѺ����/���˹�� (����)                       N Y
        public static string gc_REFTYPE_P_INV_Q = "BQ";   //B ��觢ͧ/㺡ӡѺ���� (����)                                  N Y
        public static string gc_REFTYPE_P_INV_U = "BU";   //B ��觢ͧ/㺡ӡѺ����/������Ѻ�Թ(����Թʴ ��ԡ��) (����)        Y Y
        public static string gc_REFTYPE_P_INV_V = "BV";   //B ��觢ͧ/���˹��(�ҹ��ԡ��,VAT ���֧��˹�) (����)        N N
        public static string gc_REFTYPE_P_INV_Y = "BY";   //B ��觢ͧ/��������˹���¡ (�ҹ��ԡ��,VAT ���֧��˹�) (����) N N
        public static string gc_REFTYPE_S_CR_A = "SA";   //F �Ŵ˹�� �Թʴ ��ԡ�� (���)                                               N
        public static string gc_REFTYPE_S_CR_M = "SM";   //F �Ŵ˹�� �Թ���� (���)                                               N
        public static string gc_REFTYPE_S_CR_N = "SN";   //F �Ŵ˹�� �Թ���� ��ԡ�� (���)                                               N
        public static string gc_REFTYPE_S_CR_C = "SC";   //F �Ŵ˹�� �Թʴ (���)                                               N
        public static string gc_REFTYPE_S_DR_B = "SB";   //E �����˹�� �Թʴ ��ԡ�� (���)                                            N
        public static string gc_REFTYPE_S_DR_D = "SD";   //E �����˹�� �Թʴ (���)                                            N
        public static string gc_REFTYPE_S_DR_X = "SX";   //E �����˹�� �Թ���� (���)                                            N
        public static string gc_REFTYPE_S_DR_Z = "SZ";   //E �����˹�� �Թ���� ��ԡ�� (���)                                            N
        public static string gc_REFTYPE_S_INV_G = "SG";   //Y ��觢ͧ/㺡ӡѺ����/������Դ��Ť�� (���)                    N Y
        public static string gc_REFTYPE_S_INV_I = "SI";   //S ��觢ͧ/㺡ӡѺ����/���˹�� (���)                        N Y
        public static string gc_REFTYPE_S_INV_Q = "SQ";   //S ��觢ͧ/㺡ӡѺ���� (���)                                   N Y
        public static string gc_REFTYPE_S_INV_R = "SR";   //S ��觢ͧ/㺡ӡѺ����/������Ѻ�Թ(����Թʴ) (���)         Y Y
        public static string gc_REFTYPE_S_INV_U = "SU";   //S ��觢ͧ/㺡ӡѺ����/������Ѻ�Թ(����Թʴ ��ԡ��) (���)         Y Y
        public static string gc_REFTYPE_S_INV_V = "SV";   //S ��觢ͧ/���˹��(�ҹ��ԡ��,VAT ���֧��˹�) (���)         N N
        public static string gc_REFTYPE_S_INV_W = "SW";   //Y SLIP POS (���)                    N Y
        public static string gc_REFTYPE_S_INV_Y = "SY";   //S ��觢ͧ/��������˹���¡ (�ҹ��ԡ��,VAT ���֧��˹�) (���) N N

        public static string gc_REFTYPE_P_BILL = "PI";   //P ������Ѻ�Թ (����)                                        N
        public static string gc_REFTYPE_P_BILL_VAT = "PV";   //J ������Ѻ�Թ/㺡ӡѺ���� (����) (�ҹ��ԡ��)                N Y
        public static string gc_REFTYPE_S_BILL = "RI";   //R ������Ѻ�Թ (���)                                         N
        public static string gc_REFTYPE_S_BILL_VAT = "RV";   //K ������Ѻ�Թ/㺡ӡѺ���� (���) (�ҹ��ԡ��)                 N Y

        public static string gc_REFTYPE_ADJ = "AJ";   //A 㺻�Ѻ��ا�ʹ                                                N
        public static string gc_REFTYPE_ADJ_LOCATION = "AL";   //A 㺻�Ѻ�ʹ (Location)                                               N
        public static string gc_REFTYPE_COUNT_STOCK = "CS";   //V 㺵�Ǩ�Ѻ�Թ���                                              N
        public static string gc_REFTYPE_TRANSFER = "TR";   //T ��͹�Թ���                                                  N
        public static string gc_REFTYPE_BR_TRANSFER = "TB";   //X ��͹�Թ��������ҧ�Ң�                                       N
        public static string gc_REFTYPE_RAW_WITH = "WR";   //W ��ԡ�ѵ�شԺ                                               N
        public static string gc_REFTYPE_FINISH_RCV = "FR";   //G ��Ѻ�Թ��������                                            N
        public static string gc_REFTYPE_WITHDRAW = "WX";   //W ��ԡ��ʴ�������ͧ                                        N
        public static string gc_REFTYPE_WORK_END = "WE";   //V 㺻Դ��ü�Ե                                                 N
        public static string gc_REFTYPE_RET_RAW_WITH = "RW";   //d ��Ѻ�׹�ҡ����ԡ�ѵ�شԺ
        public static string gc_REFTYPE_RET_WITHDRAW = "RX";   //d ��Ѻ�׹�ҡ����ԡ��ʴ�

        public static string gc_REFTYPE_S_TAX_3 = "WP";   //4 ������ѡ � ������ �ؤ�Ÿ����Ҷ١�ѡ ����ͧ����           N
        public static string gc_REFTYPE_S_TAX_5 = "WL";   //6 ������ѡ � ������ ����ѷ�١�ѡ ������������Ǩ�ͺ��¡�� N
        public static string gc_REFTYPE_P_TAX_3 = "XP";   //3 ������ѡ � ������ ����ѷ �ѡ�ؤ�Ÿ����� ������3           N
        public static string gc_REFTYPE_P_TAX_5 = "XL";   //5 ������ѡ � ������ ����ѷ �ѡ�ԵԺؤ�� ������.53           N

        public static string gc_REFTYPE_ISSUE_TRI = "SP";   //��ԡ�Թ���ͧ����
        public static string gc_REFTYPE_CLEAR_TRI = "RP";   //��������Թ���ͧ����
        public static string gc_REFTYPE_EMPL_BORROW = "EB";   //�����Թ�ͧ����
        public static string gc_REFTYPE_EMPL_RETURN = "ER";   //㺤׹�Թ�ͧ����

        public static string gc_REFTYPE_PLEDGE_1 = "G1";   //��Ѻ�Թ�Ѵ�� Ẻ1
        public static string gc_REFTYPE_PLEDGE_2 = "G2";   //��Ѻ�Թ�Ѵ�� Ẻ2

        public static string gc_REFTYPE_DO_TAX = "D1";   //� DO Ẻ1
        public static string gc_REFTYPE_DO_TAX_CR = "DC";   //� Ŵ˹�� DO Ẻ1
        public static string gc_REFTYPE_DO_TAX_DR = "DD";   //� ����˹�� DO Ẻ1
        public static string gc_REFTYPE_DO_2 = "D2";   //� DO Ẻ2


        public static string gc_REFTYPE_P_TAX_1 = "X1";   //1 ������ѡ � ������ ������.1
        public static string gc_REFTYPE_P_TAX_2 = "X2";   //2 ������ѡ � ������ ������.2
        public static string gc_REFTYPE_P_TAX_X = "TP";   //7 ������ѡ � ������ (�ó�����繼�����/����ѡ����)	WITHOLDING TAX NOTE (PAY)
        public static string gc_REFTYPE_S_TAX_X = "TS";   //8 ������ѡ � ������ (�ó�����繼���Ѻ/�١�ѡ����)	WITHOLDING TAX NOTE (NOT-SUBMIT)


        public static string gc_RFTYPE_ADJUST = "A"; //㺻�Ѻ��ا�ʹ
        public static string gc_RFTYPE_DR_BUY = "D"; //�����˹���ë���
        public static string gc_RFTYPE_DR_SELL = "E"; //�����˹���â��
        public static string gc_RFTYPE_CR_BUY = "C"; //�Ŵ˹���ë���
        public static string gc_RFTYPE_CR_SELL = "F"; //�Ŵ˹���â��
        public static string gc_RFTYPE_PAYMENT = "P"; //�����Թ �����
        public static string gc_RFTYPE_RECEIVE = "R"; //�Ѻ�����Թ �����
        public static string gc_RFTYPE_INV_BUY = "B"; //����
        public static string gc_RFTYPE_INV_SELL = "S"; //���
        public static string gc_RFTYPE_TAX_3 = "3"; //�����ѡ � ������ (�ѡ �ؤ�Ÿ����� )
        public static string gc_RFTYPE_TAX_3I = "4"; //�����ѡ � ������ (�ؤ�Ÿ����� �١�ѡ )
        public static string gc_RFTYPE_TAX_53 = "5"; //�����ѡ � ������ (�ѡ �ԵԺؤ��)
        public static string gc_RFTYPE_TAX_53I = "6"; //�����ѡ � ������ (����ѷ �١�ѡ)
        public static string gc_RFTYPE_PAY_VAT = "J"; //������Ѻ�Թ�����㺡ӡѺ���� (�͡��§ҹ���ի���)
        public static string gc_RFTYPE_RCV_VAT = "K"; //������Ѻ�Թ�����㺡ӡѺ���� (�͡��§ҹ���բ��)
        public static string gc_RFTYPE_LAY_BUY = "L"; //��ҧ��� (�к�����)
        public static string gc_RFTYPE_LAY_SELL = "M"; //��ҧ��� (�к����)
        public static string gc_RFTYPE_LAY_PD_SELL = "H"; //��׹�ѹ��â�� (�к����)
        public static string gc_RFTYPE_ORDER_BUY = "N"; //���觫��� (Order �к�����)
        public static string gc_RFTYPE_ORDER_SELL = "O"; //���觫��� (Order �к����)
        public static string gc_RFTYPE_TRAN_PD = "T"; //��͹�Թ���
        public static string gc_RFTYPE_ISSUE_PD = "W"; //��ԡ�ѵ�شԺ
        public static string gc_RFTYPE_PRODUCE_PD = "G"; //��Ѻ�Թ�������稷���Ե��
        public static string gc_RFTYPE_BRANCH_TRAN = "X"; //��͹�Թ��������ҧ�Ң�
        public static string gc_RFTYPE_WORK_ORDER = "Q"; //���觼�Ե
        public static string gc_RFTYPE_WORK_END = "V"; //㺻Դ��ü�Ե
        public static string gc_RFTYPE_GIVE_SELL = "Y"; //�������Դ��Ť�� (�к����)
        public static string gc_RFTYPE_COL_SELL = "Z"; //����Թ (�к����)
        public static string gc_RFTYPE_IN_PD = "j"; //������Թ������ʵ�͡ (�ѧ�����) && Fm4
        public static string gc_RFTYPE_OUT_PD = "o"; //㺵Ѵ�Թ����͡�ҡʵ�͡ (�ѧ�����) && Fm4
        public static string gc_RFTYPE_REP_SELL = "f"; //��Ѻ����â�� (�к����)
        public static string gc_RFTYPE_LEND_SELL = "I"; //�����Թ��� (�к����)
        public static string gc_RFTYPE_RETURN_SELL = "U"; //㺤׹�Թ��� (�к����)
        public static string gc_RFTYPE_LEND_BUY = "a"; //�����Թ��� (�к�����)
        public static string gc_RFTYPE_RETURN_BUY = "b"; //㺤׹�Թ��� (�к�����)
        public static string gc_RFTYPE_RETURN_ISSUE = "d"; //��Ѻ�׹�ҡ����ԡ (�ѵ�شԺ,��ʴ�������ͧ)
        public static string gc_RFTYPE_ISSUE_TRI = "t"; //��ԡ�Թ���ͧ����
        public static string gc_RFTYPE_CLEAR_TRI = "k"; //��������Թ���ͧ����



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

        //Prod.Type & Formula.ProdType && ������� Prodtype.DBF
        public static string gc_PROD_TYPE_FINISH = "1";   // �Թ��������     
        public static string gc_PROD_TYPE_RAW_MAT = "2";   // �ѵ�شԺ            
        public static string gc_PROD_TYPE_EXPENSE = "3";   // ��ԡ��              
        public static string gc_PROD_TYPE_OFFICE_SUPPLY = "4";   // ��ʴ�������ͧ
        public static string gc_PROD_TYPE_SEMI = "5";   // �Թ��ҡ�������        
        public static string gc_PROD_TYPE_ASSET = "6";   // �Թ��Ѿ��
        public static string gc_PROD_TYPE_INCOME = "7";   // �������� �
        public static string gc_PROD_TYPE_OTHERS = "8";   // ����������� �
        public static string gc_PROD_TYPE_COMPO = "9";   // ��ʴػ�Сͺ
        public static string gc_PROD_TYPE_SPARE = "A";   // ������
        public static string gc_PROD_TYPE_LABEL = "B";   // ��ʴػм��
        public static string gc_PROD_TYPE_PACKAGE = "C";   // ��ʴ��պ���
        public static string gc_PROD_TYPE_LAND = "D";   // ���Թ
        public static string gc_PROD_TYPE_BUILDING = "E";   // �Ҥ��
        public static string gc_PROD_TYPE_OFFICE_EQUIP = "F";   // �ػ�ó��ӹѡ�ҹ
        public static string gc_PROD_TYPE_MACHINE = "G";   // ����ͧ�ѡ�
        public static string gc_PROD_TYPE_DIRECT_LABOR = "H";   // ����ç�ҧ�ç
        public static string gc_PROD_TYPE_INDIRECT_LABOR = "I";   // ����ç�ҧ����
        public static string gc_PROD_TYPE_DIRECT_EXPENSE = "J";   // �������·ҧ�ç
        public static string gc_PROD_TYPE_INDIRECT_EXPENSE = "K";   // �������·ҧ����
        public static string gc_PROD_TYPE_SUBCONTRAC = "L";   // ��ʴػ�Сͺ��¹͡�ç�ҹ
        public static string gc_PROD_TYPE_PLEDGE = "M";   // �Թ�Ѵ��
        public static string gc_PROD_TYPE_TOOLS = "N";   // ����ͧ���
        public static string gc_PROD_TYPE_REMARK = "R";   // �����˵�
        public static string gc_PROD_TYPE_INSURANCE = "O";   //�Թ��Сѹ�ŧҹ

        //public static string gc_PROD_TYPE_SET_GOODS	 = "14";   // Prod.Type ���Դ�鹷عẺ Finish Goods �� _SGoodsType , _SGoodsCost
        public static string gc_PROD_TYPE_SET_GOODS = gc_PROD_TYPE_FINISH + gc_PROD_TYPE_OFFICE_SUPPLY;
        public static string gc_PROD_TYPE_SET_RAW = gc_PROD_TYPE_RAW_MAT + gc_PROD_TYPE_COMPO + gc_PROD_TYPE_LABEL + gc_PROD_TYPE_PACKAGE + gc_PROD_TYPE_SUBCONTRAC;
        public static string gc_PROD_TYPE_SET_SEMI = gc_PROD_TYPE_SEMI;

        public static string gc_REFTYPE_BUDALLOC = "AL";   // �ѹ��ǹ������ҳ

        public static string gc_ACCHART_CATEG_GROUP = "G";
        public static string gc_ACCHART_CATEG_DETAIL = "D";

        public static string gc_ACCHART_LCATEG_GROUP = "GROUP";
        public static string gc_ACCHART_LCATEG_DETAIL = "DETAIL";

        public static string gc_REFTYPE_BGTRAN1 = "B1";   // ��駧�����ҳ
        public static string gc_REFTYPE_BGTRAN2 = "B2";   // ��駧�����ҳ���������
        public static string gc_REFTYPE_BGTRAN3 = "B3";   // ��駧�����ҳ����ͨ���

        public static string gc_REFTYPE_RECV1 = "R1";   // �ͧ������ҳ
        public static string gc_REFTYPE_RECV2 = "R2";   // �ͧ������ҳ ���������
        public static string gc_REFTYPE_RECV3 = "R3";   // �ͧ������ҳ ����ͨ���

        public static string gc_REFTYPE_ADV1 = "R4";   // �Թ������ͧ����
        public static string gc_REFTYPE_ADV2 = "R5";   // �Թ������ͧ���� ���������
        public static string gc_REFTYPE_ADV3 = "R6";   // �Թ������ͧ���� ����ͨ���

        public static string gc_REFTYPE_ADJ1 = "A1";   // ��䢡�èͧ������ҳ
        public static string gc_REFTYPE_ADJ2 = "A2";   // ��䢡�èͧ������ҳ ���������
        public static string gc_REFTYPE_ADJ3 = "A3";   // ��䢡�èͧ������ҳ ����ͨ���
        public static string gc_REFTYPE_ADJ4 = "A4";   // ����Թ������ͧ����
        public static string gc_REFTYPE_ADJ5 = "A5";   // ����Թ������ͧ���� ���������
        public static string gc_REFTYPE_ADJ6 = "A6";   // ����Թ������ͧ���� ����ͨ���

        public static string gc_REFTYPE_ADJ7 = "A7";   // ����ԡ���§�����ҳ
        public static string gc_REFTYPE_ADJ8 = "A8";   // ����ԡ���§�����ҳ ���������
        public static string gc_REFTYPE_ADJ9 = "A9";   // ����ԡ���§�����ҳ ����ͨ���
        public static string gc_REFTYPE_ADJA = "AA";   // ����������Թ������ͧ����
        public static string gc_REFTYPE_ADJB = "AB";   // ����������Թ������ͧ���� ���������
        public static string gc_REFTYPE_ADJC = "AC";   // ����������Թ������ͧ���� ����ͨ���

        public static string gc_REFTYPE_PAY1 = "P1";   // �ԡ���»���ҳ
        public static string gc_REFTYPE_PAY2 = "P2";   // �ԡ���»���ҳ ���������
        public static string gc_REFTYPE_PAY3 = "P3";   // �ԡ���»���ҳ ����ͨ���
        public static string gc_REFTYPE_ADC1 = "P4";   // �������Թ������ͧ����
        public static string gc_REFTYPE_ADC2 = "P5";   // �������Թ������ͧ���� ���������
        public static string gc_REFTYPE_ADC3 = "P6";   // �������Թ������ͧ���� ����ͨ���

        public static string gc_BGTRAN_STEP_PREPARE = "1";   //�ѹ�֡������ҳ��͹���͹��ѵ�
        public static string gc_BGTRAN_STEP_APPROVE = "2";   //�Ԩ�óҧ�����ҳ
        public static string gc_BGTRAN_STEP_POST = "3";   //Post ������ҳ
        public static string gc_BGTRAN_STEP_REVISE = "E";   //��䢧�����ҳ

        public static string gc_APPROVE_STEP_WAIT = " ";           //��駧�����ҳ�ѧ����ҹ��鹵͹� �
        public static string gc_APPROVE_STEP_APPROVE = "A";    //͹��ѵԧ�����ҳ
        public static string gc_APPROVE_STEP_PASS = "P";          //������ҳ��ҹ��á��͹��ѵԨҡ˹��§ҹ��¹͡����
        public static string gc_APPROVE_STEP_REJECT = "R";       //������ҳ����ҹ���͹��ѵ�
        public static string gc_APPROVE_STEP_POST = "T";          //������ҳ POST ��¡��������������
        public static string gc_APPROVE_STEP_REVISE = "E";         //��䢧�����ҳ

        public static string gc_APPROVE_TEXT_WAIT = "WAIT";           //��駧�����ҳ�ѧ����ҹ��鹵͹� �
        public static string gc_APPROVE_TEXT_APPROVE = "APPROVE";    //͹��ѵԧ�����ҳ
        public static string gc_APPROVE_TEXT_PASS = "PASS";          //������ҳ��ҹ��á��͹��ѵԨҡ˹��§ҹ��¹͡����
        public static string gc_APPROVE_TEXT_REJECT = "REJECT";       //������ҳ����ҹ���͹��ѵ�
        public static string gc_APPROVE_TEXT_POST = "POST";          //������ҳ POST ��¡��������������

        public static string gc_BGCHART_CATEG_HR = "1";           //���ؤ�ҡ�
        public static string gc_BGCHART_CATEG_OPERATE = "2";  //�����Թ�ҹ
        public static string gc_BGCHART_CATEG_INVEST = "3";    //��ŧ�ع
        public static string gc_BGCHART_CATEG_SUPPORT = "4"; //���ش˹ع
        public static string gc_BGCHART_CATEG_OTHER = "5";    //����¨������

        public static string gc_RESOURCE_TYPE_MACHINE = "M";    //����ͧ�ѡ�
        public static string gc_RESOURCE_TYPE_TOOL = "T";    //����ͧ���

        public static string gc_PROCURE_TYPE_PURCHASE = "P";	//�ҡ��ë���
        public static string gc_PROCURE_TYPE_MANUFACURING = "M";	//�ҡ��ü�Ե�ͧ
        public static string gc_PROCURE_TYPE_SUBCONTRACT = "S";	//�ҡ��è�ҧ��
        public static string gc_PROCURE_TYPE_NON_PURCHASE = "N";	//���Դ Cost

        public static string gc_BOM_TYPE_PACKAGING = "P";	//Packaging BOM
        public static string gc_BOM_TYPE_MANUFACURING = "M";	//Manufacturing BOM

        public static string gc_ROUND_TYPE_NORMAL = " ";	//�Ѵ���ŧ �á��
        public static string gc_ROUND_TYPE_UP = "1";	//����ɨлѴ������� �� 3.1 = 4, 3.6 = 4
        public static string gc_ROUND_TYPE_DOWN = "2";	//�Ѵŧ����  �� 3.1 = 3, 3.6 = 3

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