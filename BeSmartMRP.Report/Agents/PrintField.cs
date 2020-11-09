using System;
using System.Data;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.Business.Component;
using AppUtil;

using WS.Data;
using WS.Data.Agents;

namespace BeSmartMRP.Report.Agents
{
    /// <summary>
    /// Summary description for PrintField.
    /// </summary>
    public class PrintField
    {

        public const string xd_PRN_FORM_H = "Q_PRN_FORM_H";
        public const string xd_PRN_FORM_I = "Q_PRN_FORM_I";

        private WS.Data.Agents.cDBMSAgent poSQLHelper = null;
        private WS.Data.Agents.cDBMSAgent poSQLHelper2 = null;

        private DataSet dtsDataEnv = new DataSet();
        private string mstrLastProd = "";
        private string mstrLastFormula = "";
        private string mstrLastUM = "";
        private string mstrLastCoor = "";
        private string mstrLastEmpl = "";
        private string mstrLastSect = "";
        private string mstrLastJob = "";
        private string mstrLastWHouse = "";

        private DataRow mdtrLastProd = null;
        private DataRow mdtrLastFormula = null;
        private DataRow mdtrLastUM = null;
        private DataRow mdtrLastCoor = null;
        private DataRow mdtrLastEmpl = null;
        private DataRow mdtrLastSect = null;
        private DataRow mdtrLastWHouse = null;
        private DataRow mdtrLastJob = null;
        private Business.Entity.CorpInfo poCorp = null;

        public PrintField(WS.Data.Agents.cDBMSAgent inSQLHelper, Business.Entity.CorpInfo inCorp)
        {
            this.poSQLHelper = inSQLHelper;
            this.poCorp = inCorp;
        }

        public PrintField(WS.Data.Agents.cDBMSAgent inSQLHelper, WS.Data.Agents.cDBMSAgent inSQLHelper2, Business.Entity.CorpInfo inCorp)
        {
            this.poSQLHelper = inSQLHelper;
            this.poSQLHelper2 = inSQLHelper2;
            this.poCorp = inCorp;
        }

        public void LoadFieldValue(Report.PrintFieldType inType, DataRow inSource, ref DataRow ioLoadValue)
        {
            switch (inType)
            {
                case Report.PrintFieldType.GLRefField:
                    this.ppfGLRefField(inSource, ref ioLoadValue);
                    break;
                case Report.PrintFieldType.RefProdField:
                    this.ppfRefProdField(inSource, ref ioLoadValue);
                    break;
            }
        }

        private void ppfGLRefField(DataRow inSource, ref DataRow ioLoadValue)
        {

            string strErrorMsg = "";
            string strRemark = (Convert.IsDBNull(inSource["fmMemData"]) ? "" : inSource["fmMemData"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(inSource["fmMemData2"]) ? "" : inSource["fmMemData2"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(inSource["fmMemData3"]) ? "" : inSource["fmMemData3"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(inSource["fmMemData4"]) ? "" : inSource["fmMemData4"].ToString().TrimEnd());
            if (inSource["fmMemData5"] != null)
                strRemark += (Convert.IsDBNull(inSource["fmMemData5"]) ? "" : inSource["fmMemData5"].ToString().TrimEnd());

            string strRemark1 = BizRule.GetMemData(strRemark, "Rem");
            string strRemark2 = BizRule.GetMemData(strRemark, "Rm2");
            string strRemark3 = BizRule.GetMemData(strRemark, "Rm3");
            string strRemark4 = BizRule.GetMemData(strRemark, "Rm4");
            string strRemark5 = BizRule.GetMemData(strRemark, "Rm5");
            string strRemark6 = BizRule.GetMemData(strRemark, "Rm6");
            string strRemark7 = BizRule.GetMemData(strRemark, "Rm7");
            string strRemark8 = BizRule.GetMemData(strRemark, "Rm8");
            string strRemark9 = BizRule.GetMemData(strRemark, "Rm9");
            string strRemark10 = BizRule.GetMemData(strRemark, "RmA");

            //����ѷ : ���ͺ���ѷ (Old version)
            ioLoadValue["F1010"] = this.lpfCorpField("F1010");
            //����ѷ : ����������ѷ (�óվ�����÷Ѵ����) (Old version)
            ioLoadValue["F1011"] = this.lpfCorpField("F1011");
            //����ѷ : ����������ѷ ��÷Ѵ��� 1 (Old version)
            ioLoadValue["F1012"] = this.lpfCorpField("F1012");
            //����ѷ : ����������ѷ ��÷Ѵ��� 2 (Old version)
            ioLoadValue["F1013"] = this.lpfCorpField("F1013");
            //����ѷ : �������Ѿ�� ����ѷ (Old version)
            ioLoadValue["F1014"] = this.lpfCorpField("F1014");
            //����ѷ : ��������� ����ѷ (Old version)
            ioLoadValue["F1015"] = this.lpfCorpField("F1015");
            //����ѷ : �Ţ����������բͧ����ѷ (Old version)
            ioLoadValue["F1016"] = this.lpfCorpField("F1016");
            //�١��� : �����١��� �ҡ�͡�����ҧ�ԧ��á
            ioLoadValue["F1020"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1020");
            //�١��� : �����١��� �ҡ�͡�����ҧ�ԧ��á
            ioLoadValue["F1021"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1021");
            //�١��� : ��������١��� ��÷Ѵ��� 1 (�óվ���� 3 ��÷Ѵ)
            ioLoadValue["F1023"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1023");
            //�١��� : ��������١��� ��÷Ѵ��� 2 (�óվ���� 3 ��÷Ѵ)
            ioLoadValue["F1024"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1024");
            //�١��� : �������Ѿ�� �ͧ�١���
            ioLoadValue["F1025"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1025");
            //�١��� : ��������� �ͧ�١���
            ioLoadValue["F1026"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1026");
            //�١��� : ��������١��� ��÷Ѵ��� 3 (�óվ���� 3 ��÷Ѵ)
            ioLoadValue["F1027"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1027");
            //����� : ���ͼ���� �ҡ�͡�����ҧ�ԧ��á
            ioLoadValue["F1030"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1030");
            //����� : ���ʼ���� �ҡ�͡�����ҧ�ԧ��á
            ioLoadValue["F1031"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1031");
            //����� : ����������º�÷Ѵ��� 1(�óվ���������� 3��÷Ѵ)
            ioLoadValue["F1033"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1033");
            //����� : ����������º�÷Ѵ��� 2(�óվ���������� 3��÷Ѵ)
            ioLoadValue["F1034"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1034");
            //����� : �������Ѿ�� �����
            ioLoadValue["F1035"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1035");
            //����� : ��������� �����
            ioLoadValue["F1036"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1036");
            //����� : ����������º�÷Ѵ��� 3(�óվ���������� 3��÷Ѵ)
            ioLoadValue["F1037"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1037");
            //��������´�͡��� : �ѹ����͡���
            ioLoadValue["F1040"] = Convert.ToDateTime(inSource["fdDate"]);

            //��������´�͡��� : ������� �ͧ�͡���
            ioLoadValue["F1042"] = "";
            this.poSQLHelper.SetPara(new object[] { inSource["fcBook"].ToString() });
            if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_BOOK", "BOOK", "select fcCode from BOOK where fcSkid = ?", ref strErrorMsg))
            {
                ioLoadValue["F1042"] = this.dtsDataEnv.Tables["Q1_PrintForm_BOOK"].Rows[0]["fcCode"].ToString();
            }

            //��������´�͡��� : �Ţ��� �ͧ�͡���
            ioLoadValue["F1043"] = inSource["fcCode"].ToString();
            //��������´�͡��� : ��ҧ�ԧ�Ţ������觫���/㺢��͹�Թ���
            ioLoadValue["F1044"] = this.pmGetRefToCodeH(inSource["fcRefType"].ToString(), inSource["fcSkid"].ToString());
            //if ((SysDef.gc_RFTYPE_INV_BUY+SysDef.gc_RFTYPE_INV_SELL).IndexOf(inSource["fcRfType"].ToString()) > -1)
            if (("B" + "S").IndexOf(inSource["fcRfType"].ToString()) > -1)
            {
            }

            //if ((SysDef.gc_RFTYPE_INV_BUY+SysDef.gc_RFTYPE_INV_SELL).IndexOf(inSource["fcRfType"].ToString()) > -1)
            if (("GD").IndexOf(inSource["fcRefType"].ToString()) > -1)
            {
                ioLoadValue["F1044"] = this.pmGetRefToCodeH2(inSource["fcRefType"].ToString(), inSource["fcSkid"].ToString());
            }
            
            //��������´�͡��� : ���͹䢡�ê����Թ
            ioLoadValue["F1045"] = inSource["fnCredTerm"].ToString();
            //��������´�͡��� : �ѹ���ú��˹�����
            ioLoadValue["F1046"] = (Convert.IsDBNull(inSource["fdDueDate"]) ? DateTime.MinValue : Convert.ToDateTime(inSource["fdDueDate"]));
            //��������´�͡��� : �����˵� 1 , ��������´�������͡���
            ioLoadValue["F1047"] = this.pmPTextField2(strRemark1);
            //�ѵ������
            ioLoadValue["F1048"] = Convert.ToDecimal(inSource["fnVatRate"]);
            //��������´�͡��� : �ѹ���Ѵ�觢ͧ (ORDER)
            //ioLoadValue["F1049"] =  DateTime.Now;
            ioLoadValue["F1049"] = (Convert.IsDBNull(inSource["fdReceDate"]) ? DateTime.MinValue : Convert.ToDateTime(inSource["fdReceDate"]));

            //��������´�͡��� : �Ţ�����ҧ�ԧ , �Ţ���㺡ӡѺ
            ioLoadValue["F1050"] = inSource["fcRefNo"].ToString().TrimEnd();
            //��ѡ�ҹ��� : ���;�ѡ�ҹ���
            ioLoadValue["F1052"] = this.lpfEmplField(inSource["fcEmpl"].ToString(), "F1052");
            //��ѧ�Թ��� : ���ʤ�ѧ�鹷ҧ �������͡��� (�ó��͹�Թ��Ң���
            ioLoadValue["F1061"] = this.lpfWHouseField(inSource["fcFRWHouse"].ToString(), "F1061");
            //��ѧ�Թ��� : ���ͤ�ѧ�鹷ҧ (�ó��͹�Թ��Ң�����ѧ)
            ioLoadValue["F1062"] = this.lpfWHouseField(inSource["fcFRWHouse"].ToString(), "F1062");
            //��ѧ�Թ��� : ���ͤ�ѧ�鹷ҧ (�ó��͹�Թ��Ң�����ѧ)  (�ѧ��
            ioLoadValue["F1066"] = this.lpfWHouseField(inSource["fcFRWHouse"].ToString(), "F1066");
            //��ѧ�Թ��� : ���ʤ�ѧ���·ҧ �������͡��� (�ó��͹�Թ��Ң��
            ioLoadValue["F1071"] = this.lpfWHouseField(inSource["fcToWHouse"].ToString(), "F1071");
            //��ѧ�Թ��� : ���ͤ�ѧ���·ҧ (�ó��͹�Թ��Ң�����ѧ)
            ioLoadValue["F1072"] = this.lpfWHouseField(inSource["fcToWHouse"].ToString(), "F1072");
            //��ѧ�Թ��� : ���ͤ�ѧ���·ҧ (�ó��͹�Թ��Ң�����ѧ) (�ѧ��
            ioLoadValue["F1075"] = this.lpfWHouseField(inSource["fcToWHouse"].ToString(), "F1075");
            //Ἱ� : ����Ἱ�
            ioLoadValue["F1082"] = this.lpfSectField(inSource["fcSect"].ToString(), "F1082");
            //Ἱ� :���� Job
            ioLoadValue["F1301"] = this.lpfJobField(inSource["fcJob"].ToString(), "F1301");
            //Ἱ� : ���� Job
            ioLoadValue["F1302"] = this.lpfJobField(inSource["fcJob"].ToString(), "F1302");
            //��������´�͡��� : �����Ţ�蹷��
            ioLoadValue["F1090"] = 0;
            //��������´�͡��� : ���ҷ������
            ioLoadValue["F1091"] = DateTime.Now;
            //��������´�͡��� : �ѹ�������觾����
            ioLoadValue["F1092"] = DateTime.Now;
            //��������´�͡��� : �ѹ���ҷ�����ҧ�͡���
            ioLoadValue["F1093"] = "";
            //��������´�͡��� : Login �����ҷӧҹ
            ioLoadValue["F1094"] = "";
            //��������´�͡��� : ���ͼ���ͧ����Թ���
            ioLoadValue["F1097"] = "";
            //�١��� : �����١��� (�ѧ���)
            ioLoadValue["F1120"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1120");
            //�١��� : ������������Թ (�óվ�����÷Ѵ����)
            ioLoadValue["F1148"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1148");
            //�١��� : ��������١�����Դ��� ��÷Ѵ 1 (����� 3 ��÷Ѵ)
            ioLoadValue["F1160"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1160");
            //�١��� : ��������١�����Դ��� ��÷Ѵ 2 (����� 3 ��÷Ѵ)
            ioLoadValue["F1161"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1161");
            //�١��� : ��������١�����Դ��� ��÷Ѵ 3 (����� 3 ��÷Ѵ)
            ioLoadValue["F1162"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1162");
            //�١���,����� : ���ͼ��Դ���
            ioLoadValue["F1167"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1167");
            //�ӹǹ�Թ : �ӹǹ�Թ�ʹ�����͹�ѡ��ǹŴ
            decimal decSumAmt = 0;
            switch (inSource["fcRfType"].ToString())
            {
                case "XXXX":
                    break;
                default:
                    if (inSource["fcVatIsOut"].ToString() == "Y")
                    {
                        decSumAmt = Convert.ToDecimal(inSource["fnAmt"]) + Convert.ToDecimal(inSource["fnDiscAmt1"]);
                    }
                    else
                    {
                        decSumAmt = Convert.ToDecimal(inSource["fnAmt"]) + Convert.ToDecimal(inSource["fnVatAmt"]) + Convert.ToDecimal(inSource["fnDiscAmt1"]);
                    }
                    break;
            }
            ioLoadValue["F1210"] = decSumAmt;
            //��ǹŴ : �ӹǹ�Թ ��ǹŴ�������
            ioLoadValue["F1220"] = Convert.ToDecimal(inSource["fnDiscAmt1"]);

            //�ӹǹ�Թ : �ӹǹ�Թ��Ť���Թ��� �ʹ����ѡ��ǹŴ����������
            decimal decSumAmt_F1230 = 0;
            switch (inSource["fcRfType"].ToString())
            {
                case "XXXX":
                    break;
                default:
                    if (inSource["fcVatIsOut"].ToString() == "Y")
                    {
                        decSumAmt_F1230 = Convert.ToDecimal(inSource["fnAmt"]);
                    }
                    else
                    {
                        decSumAmt_F1230 = Convert.ToDecimal(inSource["fnAmt"]);
                    }
                    break;
            }
            ioLoadValue["F1230"] = decSumAmt_F1230;

            //�ӹǹ�Թ : �ӹǹ�Թ�����͹�ѡ��ǹŴ�����¡���Թ���
            //TODO: �Ѵ�͡���ͤ�������㹡���͡��§ҹ
            //ioLoadValue["F1235"] = this.lpfSumRefA9("INVOICE", inSource, "F1235");
            //�ӹǹ�Թ : �ӹǹ�Թ����
            ioLoadValue["F1240"] = Convert.ToDecimal(inSource["fnVatAmt"]);
            //�Ţ�����ҧ���
            ioLoadValue["F1242"] = "";
            //�ӹǹ�Թ : �ӹǹ�Թ�ط�� (�ʹ����ѡ��ǹŴ���� +��Ť������
            decimal decSumAmt_F1250 = 0;
            switch (inSource["fcRfType"].ToString())
            {
                case "XXXX":
                    break;
                default:
                    decSumAmt_F1250 = Convert.ToDecimal(inSource["fnAmt"]) + Convert.ToDecimal(inSource["fnVATAmt"]);
                    break;
            }
            ioLoadValue["F1250"] = decSumAmt_F1250;
            //�ӹǹ�Թ : �ӹǹ�Թ�ط�������� �� ����Ժ�ҷ
            string strAmtText = "";
            try
            {
                strAmtText = N2Alpha.ConvNumberToText(Convert.ToDouble(decSumAmt_F1250));
            }
            catch (Exception ex)
            {
                strAmtText = "";
            }
            ioLoadValue["F1251"] = strAmtText;

            //�ӹǹ�Թ : �ӹǹ�Թ�ط�������ѧ��� �� NINETY BAHT
            ioLoadValue["F1252"] = "";
            //�ӹǹ�Թ(�����) : �ʹ�Թ���INVOICE������������͡�Ҫ���
            ioLoadValue["F1260"] = 0;
            //��ǹŴ��� 0 ��������� : �ӹǹ�Թ ��ǹŴ�������
            ioLoadValue["F1280"] = Convert.ToDecimal(inSource["fnDiscAmt1"]);
            //�ç��� : �����ç���
            //ioLoadValue["F1302"] = "";

            //�ӹǹ�Թ(CURRENCY) : �ӹǹ�Թ�ʹ�����͹�ѡ��ǹŴ
            decimal decSumAmt_F1610 = 0;
            switch (inSource["fcRfType"].ToString())
            {
                case "XXXX":
                    break;
                default:
                    if (inSource["fcVatIsOut"].ToString() == "Y")
                    {
                        decSumAmt_F1610 = Convert.ToDecimal(inSource["fnAmtKe"]) + Convert.ToDecimal(inSource["fnDiscAmtK"]);
                    }
                    else
                    {
                        decSumAmt_F1610 = Convert.ToDecimal(inSource["fnAmtKe"]) + Convert.ToDecimal(inSource["fnVatAmtKe"]) + Convert.ToDecimal(inSource["fnDiscAmtK"]);
                    }
                    break;
            }
            ioLoadValue["F1610"] = decSumAmt_F1610;

            //�ӹǹ�Թ(CURRENCY) : �ط�������ѧ��� �� NINETY BAHT
            ioLoadValue["F1652"] = "";
            //����ѷ : ���ͺ���ѷ (�ѧ���) (Old version)
            ioLoadValue["F1710"] = this.lpfCorpField("F1710");
            //����ѷ : ����������ѷ (�ѧ���) (�óվ�����÷Ѵ����) (Old
            ioLoadValue["F1711"] = this.lpfCorpField("F1711");
            //����ѷ : ����������ѷ (�ѧ���) ��÷Ѵ��� 1 (Old version)
            ioLoadValue["F1712"] = this.lpfCorpField("F1712");
            //����ѷ : ����������ѷ (�ѧ���) ��÷Ѵ��� 2 (Old version)
            ioLoadValue["F1713"] = this.lpfCorpField("F1713");
            //�١��� : ��������١�����Դ��� ��÷Ѵ 1 (������������ɳ��
            ioLoadValue["F1850"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1850");
            //�١��� : ��������١�����Դ��� ��÷Ѵ 2 (������������ɳ��
            ioLoadValue["F1851"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1851");
            //�١��� : EMAIL
            ioLoadValue["F1863"] = this.lpfCoorField(inSource["fcCoor"].ToString(), "F1863");
            //�Թ��� : �����Թ���(੾����¡���Թ��ҵ���á)
            ioLoadValue["F1870"] = "";
            //�Թ��� : �����Թ����� + Remark(੾����¡���Թ��ҵ���á)
            ioLoadValue["F1871"] = "";
            //�Թ��� : �����Թ�����(੾����¡���Թ��ҵ���á)
            ioLoadValue["F1872"] = "";
            //��������´�͡��� : ��ҧ�ԧ�Ţ㺡ӡѺ� ���(��Ŵ˹��-�����
            ioLoadValue["F3100"] = "";
            //�ӹǹ�Թ : ��Ť���Թ��ҵ��㺡ӡѺ���� (��Ŵ˹��-����˹�
            ioLoadValue["F3102"] = 0;
            //�ӹǹ�Թ : �ӹǹ�Թ��Ť�ҷ��١��ͧ����Ѻ�Ŵ˹��-����˹
            ioLoadValue["F3103"] = 0;
            //��������´�͡��� : ���˹�ҶѴ�
            ioLoadValue["F3107"] = "";
            //�ӹǹ�Թ��� : �ӹǹ����Թ��ҷء��Ǣͧ���� INVOICE
            ioLoadValue["F3109"] = this.lpfSumRefQty("F3109", inSource["fcSkid"].ToString());
            //��������´�͡��� : �����˵� 2 , ��������´�������͡���
            ioLoadValue["F3113"] = this.pmPTextField2(strRemark2);
            //��������´�͡��� : �����˵� 3 , ��������´�������͡���
            ioLoadValue["F3114"] = this.pmPTextField2(strRemark3);
            //��������´�͡��� : �����˵� 4 , ��������´�������͡���
            ioLoadValue["F3115"] = this.pmPTextField2(strRemark4);
            //��������´�͡��� : �����˵� 5 , ��������´�������͡���
            ioLoadValue["F3116"] = this.pmPTextField2(strRemark5);
            //��������´�͡��� : �����˵� 6 , ��������´�������͡���
            ioLoadValue["F3117"] = this.pmPTextField2(strRemark6);
            //��������´�͡��� : �����˵� 7 , ��������´�������͡���
            ioLoadValue["F3118"] = this.pmPTextField2(strRemark7);
            //��������´�͡��� : �����˵� 8 , ��������´�������͡���
            ioLoadValue["F3119"] = this.pmPTextField2(strRemark8);
            //��������´�͡��� : �����˵� 9 , ��������´�������͡���
            ioLoadValue["F3120"] = this.pmPTextField2(strRemark9);
            //��������´�͡��� : �����˵�10 , ��������´�������͡���
            ioLoadValue["F3121"] = this.pmPTextField2(strRemark10);

            //��������´�͡��� : �������� �ͧ�͡���
            this.poSQLHelper.SetPara(new object[] { inSource["fcBook"].ToString() });
            if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_BOOK", "BOOK", "select fcCode, fcName, fcName2 from BOOK where fcSkid = ?", ref strErrorMsg))
            {
                ioLoadValue["F3123"] = this.dtsDataEnv.Tables["Q1_PrintForm_BOOK"].Rows[0]["fcName"].ToString();
            }

            //˹����Թ : ����˹����Թ (�����ѧ���)
            ioLoadValue["F3127"] = "";
            //��������´�͡��� : ��ҧ�ԧ�Ţ�����ҧ�ԧ㺢ͫ���
            ioLoadValue["F3134"] = "";

            this.pmLoadRefTo2MMOField(ref ioLoadValue, inSource["fcRefType"].ToString(), inSource["fcSkid"].ToString(), "M1001");
            this.pmLoadRefTo2MMOField(ref ioLoadValue, inSource["fcRefType"].ToString(), inSource["fcSkid"].ToString(), "M1002");
            this.pmLoadRefTo2MMOField(ref ioLoadValue, inSource["fcRefType"].ToString(), inSource["fcSkid"].ToString(), "M1010");
            this.pmLoadRefTo2MMOField(ref ioLoadValue, inSource["fcRefType"].ToString(), inSource["fcSkid"].ToString(), "M1011");
            this.pmLoadRefTo2MMOField(ref ioLoadValue, inSource["fcRefType"].ToString(), inSource["fcSkid"].ToString(), "M1012");
            this.pmLoadRefTo2MMOField(ref ioLoadValue, inSource["fcRefType"].ToString(), inSource["fcSkid"].ToString(), "M1013");
            this.pmLoadRefTo2MMOField(ref ioLoadValue, inSource["fcRefType"].ToString(), inSource["fcSkid"].ToString(), "M1014");
            this.pmLoadRefTo2MMOField(ref ioLoadValue, inSource["fcRefType"].ToString(), inSource["fcSkid"].ToString(), "M1015");
            this.pmLoadRefTo2MMOField(ref ioLoadValue, inSource["fcRefType"].ToString(), inSource["fcSkid"].ToString(), "M1016");
            this.pmLoadRefTo2MMOField(ref ioLoadValue, inSource["fcRefType"].ToString(), inSource["fcSkid"].ToString(), "M1017");

        }

        private void ppfRefProdField(DataRow inSource, ref DataRow ioLoadValue)
        {

            bool llFormula = false;
            bool llProd = false;
            if (inSource["fcRefPdTyp"].ToString() != "F")
            {
                llProd = true;
            }
            if (inSource["fcRefPdTyp"].ToString() == "F"
                || (inSource["fcRefPdTyp"].ToString() == "P" && inSource["fcPFormula"].ToString().TrimEnd() == ""))
            {
                llFormula = true;
            }

            //�����˵� : �����˵� (Remark) �����¡��
            //			string strAllRemark = this.pmPTextField(inSource, "fmReMark")+Convert.ToChar(13).ToString();
            //			strAllRemark += this.pmPTextField(inSource, "fmReMark2")+Convert.ToChar(13).ToString();
            //			strAllRemark += this.pmPTextField(inSource, "fmReMark3")+Convert.ToChar(13).ToString();
            //			strAllRemark += this.pmPTextField(inSource, "fmReMark4")+Convert.ToChar(13).ToString();
            //			strAllRemark += this.pmPTextField(inSource, "fmReMark5")+Convert.ToChar(13).ToString();
            //			strAllRemark += this.pmPTextField(inSource, "fmReMark6")+Convert.ToChar(13).ToString();
            //			strAllRemark += this.pmPTextField(inSource, "fmReMark7")+Convert.ToChar(13).ToString();
            //			strAllRemark += this.pmPTextField(inSource, "fmReMark8")+Convert.ToChar(13).ToString();
            //			strAllRemark += this.pmPTextField(inSource, "fmReMark9")+Convert.ToChar(13).ToString();
            //			strAllRemark += this.pmPTextField(inSource, "fmReMark10");

            string strAllRemark = (this.pmPTextField(inSource, "fmReMark").Trim() != string.Empty ? this.pmPTextField(inSource, "fmReMark") + Convert.ToChar(13).ToString() : "");
            strAllRemark += (this.pmPTextField(inSource, "fmReMark2").Trim() != string.Empty ? this.pmPTextField(inSource, "fmReMark2") + Convert.ToChar(13).ToString() : "");
            strAllRemark += (this.pmPTextField(inSource, "fmReMark3").Trim() != string.Empty ? this.pmPTextField(inSource, "fmReMark3") + Convert.ToChar(13).ToString() : "");
            strAllRemark += (this.pmPTextField(inSource, "fmReMark4").Trim() != string.Empty ? this.pmPTextField(inSource, "fmReMark4") + Convert.ToChar(13).ToString() : "");
            strAllRemark += (this.pmPTextField(inSource, "fmReMark5").Trim() != string.Empty ? this.pmPTextField(inSource, "fmReMark5") + Convert.ToChar(13).ToString() : "");
            strAllRemark += (this.pmPTextField(inSource, "fmReMark6").Trim() != string.Empty ? this.pmPTextField(inSource, "fmReMark6") + Convert.ToChar(13).ToString() : "");
            strAllRemark += (this.pmPTextField(inSource, "fmReMark7").Trim() != string.Empty ? this.pmPTextField(inSource, "fmReMark7") + Convert.ToChar(13).ToString() : "");
            strAllRemark += (this.pmPTextField(inSource, "fmReMark8").Trim() != string.Empty ? this.pmPTextField(inSource, "fmReMark8") + Convert.ToChar(13).ToString() : "");
            strAllRemark += (this.pmPTextField(inSource, "fmReMark9").Trim() != string.Empty ? this.pmPTextField(inSource, "fmReMark9") + Convert.ToChar(13).ToString() : "");
            strAllRemark += this.pmPTextField(inSource, "fmReMark10");

            string strAllRemark2 = (this.pmPTextField(inSource, "fmReMark").Trim() != string.Empty ? Convert.ToChar(13).ToString() + this.pmPTextField(inSource, "fmReMark") : "");
            strAllRemark2 += (this.pmPTextField(inSource, "fmReMark2").Trim() != string.Empty ? Convert.ToChar(13).ToString() + this.pmPTextField(inSource, "fmReMark2") : "");
            strAllRemark2 += (this.pmPTextField(inSource, "fmReMark3").Trim() != string.Empty ? Convert.ToChar(13).ToString() + this.pmPTextField(inSource, "fmReMark3") : "");
            strAllRemark2 += (this.pmPTextField(inSource, "fmReMark4").Trim() != string.Empty ? Convert.ToChar(13).ToString() + this.pmPTextField(inSource, "fmReMark4") : "");
            strAllRemark2 += (this.pmPTextField(inSource, "fmReMark5").Trim() != string.Empty ? Convert.ToChar(13).ToString() + this.pmPTextField(inSource, "fmReMark5") : "");
            strAllRemark2 += (this.pmPTextField(inSource, "fmReMark6").Trim() != string.Empty ? Convert.ToChar(13).ToString() + this.pmPTextField(inSource, "fmReMark6") : "");
            strAllRemark2 += (this.pmPTextField(inSource, "fmReMark7").Trim() != string.Empty ? Convert.ToChar(13).ToString() + this.pmPTextField(inSource, "fmReMark7") : "");
            strAllRemark2 += (this.pmPTextField(inSource, "fmReMark8").Trim() != string.Empty ? Convert.ToChar(13).ToString() + this.pmPTextField(inSource, "fmReMark8") : "");
            strAllRemark2 += (this.pmPTextField(inSource, "fmReMark9").Trim() != string.Empty ? Convert.ToChar(13).ToString() + this.pmPTextField(inSource, "fmReMark9") : "");
            strAllRemark2 += (this.pmPTextField(inSource, "fmReMark10").Trim() != string.Empty ? Convert.ToChar(13).ToString() + this.pmPTextField(inSource, "fmReMark10") : "");

            if (inSource["fcRefPdTyp"].ToString() == "P")
            {
                //�Թ��� : �����Թ���
                ioLoadValue["I2021"] = this.lpfProdField(inSource["fcProd"].ToString(), "I2021", "");
                //�Թ��� : �����Թ����� + Remark
                ioLoadValue["I2022"] = this.lpfProdField(inSource["fcProd"].ToString(), "I2022", strAllRemark2);
                //�Թ��� : �����Թ�����
                ioLoadValue["I2023"] = this.lpfProdField(inSource["fcProd"].ToString(), "I2023", "");
                //�Թ��� : ��������Թ�����
                ioLoadValue["I2026"] = this.lpfProdField(inSource["fcProd"].ToString(), "I2026", "");
                //�Թ��� : ��������Թ����ѧ���
                ioLoadValue["I2027"] = this.lpfProdField(inSource["fcProd"].ToString(), "I2027", "");
            }
            else
            {
                //�Թ��� : �����Թ���
                ioLoadValue["I2021"] = this.lpfFormulaField(inSource["fcFormulas"].ToString(), "I2021", "");
                //�Թ��� : �����Թ����� + Remark
                ioLoadValue["I2022"] = this.lpfFormulaField(inSource["fcFormulas"].ToString(), "I2022", "");
                //�Թ��� : �����Թ�����
                ioLoadValue["I2023"] = this.lpfFormulaField(inSource["fcFormulas"].ToString(), "I2023", "");
                //�Թ��� : ��������Թ�����
                ioLoadValue["I2026"] = this.lpfFormulaField(inSource["fcFormulas"].ToString(), "I2026", "");
                //�Թ��� : ��������Թ����ѧ���
                ioLoadValue["I2027"] = this.lpfFormulaField(inSource["fcFormulas"].ToString(), "I2027", "");
            }

            ioLoadValue["I2029"] = strAllRemark;
            //�Թ��� : ����˹��¹Ѻ
            ioLoadValue["I2031"] = this.lpfUMField(inSource["fcUM"].ToString(), "I2031");
            //�Թ��� : ����˹��¹Ѻ��
            ioLoadValue["I2032"] = this.lpfUMField(inSource["fcUM"].ToString(), "I2032");
            //˹��¹Ѻ : ����˹��¹Ѻ�ѧ���
            ioLoadValue["I2033"] = this.lpfUMField(inSource["fcUM"].ToString(), "I2033");
            //�Թ��� : ��������Թ����� + Remark
            //ioLoadValue["I2034"] = "";

            decimal decQty = 0;
            switch (inSource["fcRefType"].ToString())
            {
                case "CS":
                    decQty = Convert.ToDecimal(inSource["fnQtyAtDat"]);
                    break;
                case "AJ":
                    decQty = Convert.ToDecimal(inSource["fnQty"]) * (inSource["fcIOType"].ToString() == "I" ? 1 : -1);
                    break;
                default:
                    decQty = Convert.ToDecimal(inSource["fnQty"]);
                    break;
            }

            //�ӹǹ : �ӹǹ�Թ���
            ioLoadValue["I2041"] = decQty;

            //�Ҥ� : �Ҥ��Թ��ҵ��˹���
            ioLoadValue["I2042"] = 0;
            decimal decPrice = 0;
            decPrice = Convert.ToDecimal(inSource["fnPriceKe"]) * Convert.ToDecimal(inSource["fnXRate"]);

            if (llProd || llFormula)
            {
                ioLoadValue["I2042"] = decPrice;
            }

            //��ǹŴ : ��ǹŴ % ������¡���Թ���
            ioLoadValue["I2050"] = "";
            string strDiscStr = inSource["fcDiscStr"].ToString().Trim();
            if ("%".IndexOf(strDiscStr) > -1 && "+".IndexOf(strDiscStr) < 0)
            {
                ioLoadValue["I2050"] = strDiscStr;
            }
            //��ǹŴ : ��ǹŴ �������͹ ������¡��
            ioLoadValue["I2052"] = strDiscStr;

            //��Ť�� : ��Ť�� (�Ҥ� X �ӹǹ)
            decimal decVal_2060 = Math.Round(Convert.ToDecimal(inSource["fnQty"]) * Convert.ToDecimal(inSource["fnPriceKe"]) * Convert.ToDecimal(inSource["fnXRate"]) - Convert.ToDecimal(inSource["fnDiscAmt"]), 2, MidpointRounding.AwayFromZero);
            if (inSource["fcRefType"].ToString() == "AJ")
            {
                decVal_2060 = decVal_2060 * (inSource["fcIOType"].ToString() == "I" ? 1 : -1);
            }
            ioLoadValue["I2060"] = decVal_2060;

            //��Ť�� : ��Ť�ҡ�͹ VAT ������¡���Թ���
            decimal decVal_2062 = decVal_2060;
            if (inSource["fcVatIsOut"].ToString() == "N")
            {
                decVal_2062 = decVal_2062 - Convert.ToDecimal(inSource["fnVatAmt"]);
            }
            ioLoadValue["I2062"] = decVal_2062;

            ioLoadValue["I2422"] = decVal_2062;

            //��ѧ�Թ��� : ���ʤ�ѧ�Թ��ҷ�����������͢���͡
            ioLoadValue["I2070"] = this.lpfWHouseField(inSource["fcWhouse"].ToString(), "I2070");

            //Location �Թ���
            ioLoadValue["I2034"] = this.lpfWHLocaField(inSource["fcWhLoca"].ToString(), "I2034");
            ioLoadValue["I2036"] = this.lpfWHLocaField(inSource["fcWhLoca"].ToString(), "I2036");
            
            //��͵�Թ���
            ioLoadValue["I2080"] = inSource["fcLot"].ToString().Trim();
            //Serial #
            ioLoadValue["I2081"] = this.lpfSerialStr("I2081",
                inSource["fcCorp"].ToString()
                , inSource["fcBranch"].ToString()
                , inSource["fcRefType"].ToString()
                , inSource["fcSkid"].ToString()
                , inSource["fcProd"].ToString());

            //˹��¹Ѻ : ����˹��¹Ѻ�ҵðҹ��
            //ioLoadValue["I2092"] =  "";
            ioLoadValue["I2092"] = this.lpfUMField(inSource["fcUM"].ToString(), "I2092");
            //�Ҥ�(CURRENCY) : �Ҥ��Թ��ҵ��˹���
            ioLoadValue["I2242"] = Convert.ToDecimal(inSource["fnPriceKe"]);

            //��Ť��(CURRENCY) : ��Ť�� (�Ҥ� X �ӹǹ)
            decimal decVal_2260 = Math.Round(Convert.ToDecimal(inSource["fnQty"]) * Convert.ToDecimal(inSource["fnPriceKe"]) - Convert.ToDecimal(inSource["fnDiscAmtK"]), 2, MidpointRounding.AwayFromZero);
            if (inSource["fcRefType"].ToString() == "AJ")
            {
                decVal_2260 = decVal_2260 * (inSource["fcIOType"].ToString() == "I" ? 1 : -1);
            }
            ioLoadValue["I2260"] = decVal_2260;

            //�Ҥ� : �Ҥ��Թ��ҵ��˹��� (�ش�Թ���)
            ioLoadValue["I2410"] = decPrice;
            //�Ҥ� : �ҤҢ���ҵðҹ (�ش�Թ���)
            ioLoadValue["I2412"] = 0;
            //��Ť�� : ��Ť�� (�Ҥ� X �ӹǹ) (�ش�Թ���)
            ioLoadValue["I2420"] = 0;
            //���觫��� : �ѹ���ioLoadValue["I6821"] = DateTime.Now;

            ioLoadValue["I2037"] = this.lpfProdTypeField(inSource["fcProdType"].ToString(), "I2037");
            ioLoadValue["I2038"] = this.lpfProdTypeField(inSource["fcProdType"].ToString(), "I2038");
            ioLoadValue["I2043"] = Convert.ToDecimal(inSource["fnUMQty"]);

        }

        private string lpfCorpField(string inFieldNo)
        {
            string strRetVal = "";
            switch (inFieldNo)
            {
                case "F1010":
                    strRetVal = this.poCorp.Name;
                    break;
                case "F1011":
                    strRetVal = this.poCorp.Address1.TrimEnd() + ' ' + this.poCorp.Address2.TrimEnd();
                    break;
                case "F1012":
                    strRetVal = this.poCorp.Address1.TrimEnd();
                    break;
                case "F1013":
                    strRetVal = this.poCorp.Address2.TrimEnd();
                    break;
                case "F1014":
                    strRetVal = this.poCorp.TelNo;
                    break;
                case "F1015":
                    strRetVal = this.poCorp.FaxNo;
                    break;
                case "F1016":
                    strRetVal = this.poCorp.TaxID;
                    break;
                case "F1710":
                    strRetVal = this.poCorp.Name2;
                    break;
                case "F1711":
                    strRetVal = this.poCorp.Address12.TrimEnd() + " " + this.poCorp.Address22.TrimEnd();
                    break;
                case "F1712":
                    strRetVal = this.poCorp.Address12.TrimEnd();
                    break;
                case "F1713":
                    strRetVal = this.poCorp.Address22.TrimEnd();
                    break;
            }
            return strRetVal;
        }

        private string lpfUMField(string inUM, string inFieldNo)
        {
            string strErrorMsg = "";
            string strRetVal = "";
            DataRow dtrUM = null;
            if (this.mstrLastUM != inUM)
            {
                this.mstrLastUM = inUM;
                this.poSQLHelper.SetPara(new object[] { this.mstrLastUM });
                if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_UM", "UM", "select * from UM where fcSkid = ?", ref strErrorMsg))
                {
                    this.mdtrLastUM = this.dtsDataEnv.Tables["Q1_PrintForm_UM"].Rows[0];
                    dtrUM = this.mdtrLastUM;
                }
            }
            else
            {
                dtrUM = this.mdtrLastUM;
            }

            if (dtrUM == null)
                return "";

            switch (inFieldNo)
            {
                case "I2031":
                    strRetVal = dtrUM["fcCode"].ToString().Trim();
                    break;
                case "I2092":
                case "I2032":
                    strRetVal = dtrUM["fcName"].ToString().Trim();
                    break;
                case "F2033":
                    strRetVal = dtrUM["fcName2"].ToString().Trim();
                    break;
            }

            return strRetVal;
        }

        private string mstrLastPdType = "";
        private DataRow mdtrLastPdType;
        private string lpfProdTypeField(string inPdType, string inFieldNo)
        {
            string strErrorMsg = "";
            string strRetVal = "";
            DataRow dtrUM = null;
            if (this.mstrLastPdType != inPdType)
            {
                this.mstrLastPdType = inPdType;
                this.poSQLHelper.SetPara(new object[] { this.mstrLastPdType });
                if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_PdType", "PDTYPE", "select * from PRODTYPE where fcCode = ?", ref strErrorMsg))
                {
                    this.mdtrLastPdType = this.dtsDataEnv.Tables["Q1_PrintForm_PdType"].Rows[0];
                    dtrUM = this.mdtrLastPdType;
                }
            }
            else
            {
                dtrUM = this.mdtrLastPdType;
            }

            if (dtrUM == null)
                return "";

            switch (inFieldNo)
            {
                case "I2037":
                    strRetVal = dtrUM["fcCode"].ToString().Trim();
                    break;
                case "I2038":
                    strRetVal = dtrUM["fcName"].ToString().Trim();
                    break;
            }

            return strRetVal;
        }

        private string lpfWHLocaField(string inWHLoca, string inFieldNo)
        {
            string strErrorMsg = "";
            string strRetVal = "";
            DataRow dtrUM = null;
            this.poSQLHelper.SetPara(new object[] { inWHLoca });
            if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_WHLoca", "PDTYPE", "select * from WHLOCATION where FCSKID = ?", ref strErrorMsg))
            {
                dtrUM = this.dtsDataEnv.Tables["Q1_PrintForm_WHLoca"].Rows[0];
                switch (inFieldNo)
                {
                    case "I2034":
                        strRetVal = dtrUM["fcCode"].ToString().Trim();
                        break;
                    case "I2036":
                        strRetVal = dtrUM["fcName"].ToString().Trim();
                        break;
                }
            }


            return strRetVal;
        }

        private string lpfEmplField(string inEmpl, string inFieldNo)
        {
            string strErrorMsg = "";
            string strRetVal = "";
            DataRow dtrEmpl = null;
            if (this.mstrLastEmpl != inEmpl)
            {
                this.mstrLastEmpl = inEmpl;
                this.poSQLHelper.SetPara(new object[] { this.mstrLastEmpl });
                if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_Empl", "Empl", "select * from Empl where fcSkid = ?", ref strErrorMsg))
                {
                    this.mdtrLastEmpl = this.dtsDataEnv.Tables["Q1_PrintForm_Empl"].Rows[0];
                    dtrEmpl = this.mdtrLastEmpl;
                }
            }
            else
            {
                dtrEmpl = this.mdtrLastEmpl;
            }

            if (dtrEmpl == null)
                return "";

            switch (inFieldNo)
            {
                case "F1051":
                    strRetVal = dtrEmpl["fcCode"].ToString().Trim();
                    break;
                case "F1052":
                    strRetVal = dtrEmpl["fcName"].ToString().Trim();
                    break;
                case "F1053":
                    strRetVal = dtrEmpl["fcName2"].ToString().Trim();
                    break;
            }

            return strRetVal;
        }

        private string lpfSectField(string inSect, string inFieldNo)
        {
            string strErrorMsg = "";
            string strRetVal = "";
            DataRow dtrSect = null;
            if (this.mstrLastSect != inSect)
            {
                this.mstrLastSect = inSect;
                this.poSQLHelper.SetPara(new object[] { this.mstrLastSect });
                if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_Sect", "Sect", "select * from Sect where fcSkid = ?", ref strErrorMsg))
                {
                    this.mdtrLastSect = this.dtsDataEnv.Tables["Q1_PrintForm_Sect"].Rows[0];
                    dtrSect = this.mdtrLastSect;
                }
            }
            else
            {
                dtrSect = this.mdtrLastSect;
            }

            if (dtrSect == null)
                return "";

            switch (inFieldNo)
            {
                case "F1081":
                case "I2451":
                    strRetVal = dtrSect["fcCode"].ToString().Trim();
                    break;
                case "F1082":
                case "I2452":
                    strRetVal = dtrSect["fcName"].ToString().Trim();
                    break;
                case "F1083":
                case "I2453":
                    strRetVal = dtrSect["fcName2"].ToString().Trim();
                    break;
            }

            return strRetVal;
        }

        private string lpfJobField(string inJob, string inFieldNo)
        {
            string strErrorMsg = "";
            string strRetVal = "";
            DataRow dtrJob = null;
            if (this.mstrLastJob != inJob)
            {
                this.mstrLastJob = inJob;
                this.poSQLHelper.SetPara(new object[] { this.mstrLastJob });
                if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_Job", "Job", "select * from Job where fcSkid = ?", ref strErrorMsg))
                {
                    this.mdtrLastJob = this.dtsDataEnv.Tables["Q1_PrintForm_Job"].Rows[0];
                    dtrJob = this.mdtrLastJob;
                }
            }
            else
            {
                dtrJob = this.mdtrLastJob;
            }

            if (dtrJob == null)
                return "";

            switch (inFieldNo)
            {
                case "F1301":
                    strRetVal = dtrJob["fcCode"].ToString().Trim();
                    break;
                case "F1302":
                    strRetVal = dtrJob["fcName"].ToString().Trim();
                    break;
            }

            return strRetVal;
        }

        private string lpfWHouseField(string inWHouse, string inFieldNo)
        {
            string strErrorMsg = "";
            string strRetVal = "";
            DataRow dtrWHouse = null;
            if (this.mstrLastWHouse != inWHouse)
            {
                this.mstrLastWHouse = inWHouse;
                this.poSQLHelper.SetPara(new object[] { this.mstrLastWHouse });
                if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_WHouse", "WHouse", "select * from WHouse where fcSkid = ?", ref strErrorMsg))
                {
                    this.mdtrLastWHouse = this.dtsDataEnv.Tables["Q1_PrintForm_WHouse"].Rows[0];
                    dtrWHouse = this.mdtrLastWHouse;
                }
            }
            else
            {
                dtrWHouse = this.mdtrLastWHouse;
            }

            if (dtrWHouse == null)
                return "";

            switch (inFieldNo)
            {
                case "F1061":
                case "F1071":
                case "F2072":
                case "I2070":
                    strRetVal = dtrWHouse["fcCode"].ToString().Trim();
                    break;
                case "F1062":
                case "F1072":
                case "F2073":
                    strRetVal = dtrWHouse["fcName"].ToString().Trim();
                    break;
                case "F1066":
                case "F1075":
                    strRetVal = dtrWHouse["fcName2"].ToString().Trim();
                    break;
            }

            return strRetVal;
        }

        private string lpfCoorField(string inCoor, string inFieldNo)
        {
            string strErrorMsg = "";
            string strRetVal = "";
            string strRemark = "";
            DataRow dtrCoor = null;

            if (this.mstrLastCoor != inCoor)
            {
                this.mstrLastCoor = inCoor;
                this.poSQLHelper.SetPara(new object[] { this.mstrLastCoor });
                if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_Coor", "Coor", "select * from Coor where fcSkid = ?", ref strErrorMsg))
                {
                    this.mdtrLastCoor = this.dtsDataEnv.Tables["Q1_PrintForm_Coor"].Rows[0];
                    dtrCoor = this.mdtrLastCoor;
                }
            }
            else
            {
                dtrCoor = this.mdtrLastCoor;
            }

            if (dtrCoor == null)
                return "";

            strRemark = (Convert.IsDBNull(dtrCoor["fmMemData"]) ? "" : dtrCoor["fmMemData"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(dtrCoor["fmMemData2"]) ? "" : dtrCoor["fmMemData2"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(dtrCoor["fmMemData3"]) ? "" : dtrCoor["fmMemData3"].ToString().TrimEnd());
            strRemark += (Convert.IsDBNull(dtrCoor["fmMemData4"]) ? "" : dtrCoor["fmMemData4"].ToString().TrimEnd());
            if (dtrCoor["fmMemData5"] != null)
                strRemark += (Convert.IsDBNull(dtrCoor["fmMemData5"]) ? "" : dtrCoor["fmMemData5"].ToString().TrimEnd());

            switch (inFieldNo)
            {
                case "F1020":
                case "F1030":
                    strRetVal = dtrCoor["fcName"].ToString().Trim();
                    break;
                case "F1120":
                    strRetVal = dtrCoor["fcName2"].ToString().Trim();
                    break;
                case "F1021":
                case "F1031":
                    strRetVal = dtrCoor["fcCode"].ToString().Trim();
                    break;
                case "F1023":
                case "F1033":
                    strRetVal = BizRule.GetMemData(strRemark, "A11");
                    string stRemark2 = BizRule.GetMemData(strRemark, "A21");
                    if (dtrCoor["fcZip"].ToString().TrimEnd() != string.Empty && stRemark2.TrimEnd() == string.Empty)
                    {
                        strRetVal = strRetVal.Trim() + " " + dtrCoor["fcZip"].ToString().TrimEnd();
                    }
                    break;
                case "F1024":
                case "F1034":
                    strRetVal = BizRule.GetMemData(strRemark, "A21");
                    string stRemark3 = BizRule.GetMemData(strRemark, "A31");
                    if (dtrCoor["fcZip"].ToString().TrimEnd() != string.Empty && stRemark3.TrimEnd() == string.Empty)
                    {
                        strRetVal = strRetVal.Trim() + " " + dtrCoor["fcZip"].ToString().TrimEnd();
                    }
                    break;
                case "F1027":
                case "F1037":
                    strRetVal = BizRule.GetMemData(strRemark, "A31");
                    if (dtrCoor["fcZip"].ToString().TrimEnd() != string.Empty)
                    {
                        strRetVal = strRetVal.Trim() + " " + dtrCoor["fcZip"].ToString().TrimEnd();
                    }
                    break;
                case "F1025":
                case "F1035":
                    strRetVal = BizRule.GetMemData(strRemark, "Tel");
                    break;
                case "F1026":
                case "F1036":
                    strRetVal = BizRule.GetMemData(strRemark, "Fax");
                    break;
                case "F1148":
                    strRetVal = BizRule.GetMemData(strRemark, "A11") + " ";
                    strRetVal += BizRule.GetMemData(strRemark, "A21") + " ";
                    strRetVal += BizRule.GetMemData(strRemark, "A31");
                    if (dtrCoor["fcZip"].ToString().TrimEnd() != string.Empty)
                    {
                        strRetVal = strRetVal.Trim() + " " + dtrCoor["fcZip"].ToString().TrimEnd();
                    }
                    break;
                case "F1160":
                    strRetVal = BizRule.GetMemData(strRemark, "C11");
                    break;
                case "F1161":
                    strRetVal = BizRule.GetMemData(strRemark, "C21");
                    break;
                case "F1162":
                    strRetVal = BizRule.GetMemData(strRemark, "C31");
                    break;
                case "F1167":
                    strRetVal = dtrCoor["fcContactN"].ToString().TrimEnd();
                    break;
                case "F1850":
                    strRetVal = BizRule.GetMemData(strRemark, "A11");
                    break;
                case "F1851":
                    strRetVal = BizRule.GetMemData(strRemark, "A21");
                    break;
                case "F1863":
                    strRetVal = BizRule.GetMemData(strRemark, "Ema");
                    break;
            }

            return strRetVal;
        }

        private string lpfProdField(string inProd, string inFieldNo, string inRemark)
        {
            string strErrorMsg = "";
            string strRetVal = "";
            DataRow dtrProd = null;
            if (this.mstrLastProd != inProd)
            {
                this.mstrLastProd = inProd;
                this.poSQLHelper.SetPara(new object[] { this.mstrLastProd });
                if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_Prod", "PROD", "select * from PROD where fcSkid = ?", ref strErrorMsg))
                {
                    this.mdtrLastProd = this.dtsDataEnv.Tables["Q1_PrintForm_Prod"].Rows[0];
                    dtrProd = this.mdtrLastProd;
                }
            }
            else
            {
                dtrProd = this.mdtrLastProd;
            }

            if (dtrProd == null)
                return "";


            switch (inFieldNo)
            {
                case "I2021":
                    strRetVal = dtrProd["fcCode"].ToString().Trim();
                    break;
                case "I2022":
                    //strRetVal = dtrProd["fcName"].ToString().Trim() + inRemark.Trim();
                    strRetVal = dtrProd["fcName"].ToString().Trim() + inRemark.TrimEnd();
                    break;
                case "I2023":
                    strRetVal = dtrProd["fcName"].ToString().Trim();
                    break;
                case "I2024":
                    strRetVal = dtrProd["fcName2"].ToString().Trim() + inRemark.Trim();
                    break;
                case "I2025":
                    strRetVal = dtrProd["fcName2"].ToString().Trim();
                    break;
                case "I2026":
                    strRetVal = dtrProd["fcSName"].ToString().Trim();
                    break;
                case "I2027":
                    strRetVal = dtrProd["fcSName2"].ToString().Trim();
                    break;
            }
            return strRetVal;
        }

        private string lpfFormulaField(string inFormula, string inFieldNo, string inRemark)
        {
            string strErrorMsg = "";
            string strRetVal = "";
            DataRow dtrFormula = null;
            if (this.mstrLastFormula != inFormula)
            {
                this.mstrLastFormula = inFormula;
                this.poSQLHelper.SetPara(new object[] { this.mstrLastFormula });
                if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_Formula", "Formula", "select * from Formulas where fcSkid = ?", ref strErrorMsg))
                {
                    this.mdtrLastFormula = this.dtsDataEnv.Tables["Q1_PrintForm_Formula"].Rows[0];
                    dtrFormula = this.mdtrLastFormula;
                }
            }
            else
            {
                dtrFormula = this.mdtrLastFormula;
            }

            if (dtrFormula == null)
                return "";


            switch (inFieldNo)
            {
                case "I2021":
                    strRetVal = dtrFormula["fcCode"].ToString().Trim();
                    break;
                case "I2022":
                    strRetVal = dtrFormula["fcName"].ToString().Trim() + inRemark.Trim();
                    break;
                case "I2023":
                    strRetVal = dtrFormula["fcName"].ToString().Trim();
                    break;
                case "I2024":
                    strRetVal = dtrFormula["fcName2"].ToString().Trim() + inRemark.Trim();
                    break;
                case "I2025":
                    strRetVal = dtrFormula["fcName2"].ToString().Trim();
                    break;
                case "I2026":
                    strRetVal = dtrFormula["fcSName"].ToString().Trim();
                    break;
                case "I2027":
                    strRetVal = dtrFormula["fcSName2"].ToString().Trim();
                    break;
            }
            return strRetVal;
        }

        private decimal lpfSumRefA9(string inTab, DataRow inSource, string inFieldNo)
        {
            string strErrorMsg = "";
            decimal decSumAmt = 0;
            if (inTab == "INVOICE")
            {
                this.poSQLHelper.SetPara(new object[] { inSource["fcSkid"].ToString() });
                if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_RefProd", "REFPROD", "select * from REFPROD where FCGLREF = ? and FCREFPDTYP = 'P' order by FCSEQ", ref strErrorMsg))
                {
                    foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables["Q1_PrintForm_RefProd"].Rows)
                    {
                        switch (inFieldNo)
                        {
                            case "F1235":
                                decSumAmt += Math.Round(Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnPriceKe"]) * Convert.ToDecimal(dtrRefProd["fnXRate"]), 2, MidpointRounding.AwayFromZero);
                                break;
                            case "F1236":
                                decSumAmt += Math.Round(Convert.ToDecimal(dtrRefProd["fnDiscAmt"]), 2, MidpointRounding.AwayFromZero);
                                break;
                            case "F1237":
                                decSumAmt += Math.Round(Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnPriceKe"]), 2, MidpointRounding.AwayFromZero);
                                break;
                            case "F1238":
                                decSumAmt += Math.Round(Convert.ToDecimal(dtrRefProd["fnDiscAmtK"]), 2, MidpointRounding.AwayFromZero);
                                break;
                        }
                    }
                }

            }

            if (inTab == "ORDER")
            { }
            return decSumAmt;
        }

        private string pmPTextField(DataRow inRow, string inField)
        {
            if (!Convert.IsDBNull(inRow[inField]))
                return AppUtil.StringHelper.ChrTran(inRow[inField].ToString().Trim(), "|", Convert.ToChar(13).ToString());
            else
                return "";
        }

        private string pmPTextField2(string inText)
        {
            return AppUtil.StringHelper.ChrTran(inText, "|", Convert.ToChar(13).ToString());
        }

        private string lpfSerialStr(string inFieldNo, string tcinCorp, string tcinBranch, string tcinRefType, string tcinRefItem, string tcinProd)
        {
            string strErrorMsg = "";
            string strRetVal = "";
            DataRow dtrProd = null;
            this.poSQLHelper.SetPara(new object[] { tcinCorp, tcinBranch, tcinRefType, tcinRefItem });
            if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_RefPdX3", "REFPDX3", "select * from REFPDX3 where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcRefItem = ?", ref strErrorMsg))
            {
                foreach (DataRow dtrRefPdX3 in this.dtsDataEnv.Tables["Q1_PrintForm_RefPdX3"].Rows)
                {
                    this.poSQLHelper.SetPara(new object[] { dtrRefPdX3["fcPdSer"].ToString() });
                    if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_PrintForm_PdSer", "PDSER", "select FCCODE from PDSER where fcSkid = ? ", ref strErrorMsg))
                    {
                        strRetVal += this.dtsDataEnv.Tables["Q1_PrintForm_PdSer"].Rows[0]["fcCode"].ToString().TrimEnd() + "|";
                    }
                }
            }
            strRetVal = AppUtil.StringHelper.ChrTran(strRetVal, "|", Convert.ToChar(13).ToString());

            return strRetVal;
        }

        private string xxxxpmGetRefToCodeH(string inRefType, string inMasterH)
        {
            string strErrorMsg = "";

            string strRefToCode = "";
            string strNoteCut = "select fcChildH from NOTECUT where FCMASTERTY = ? and FCMASTERH = ? group by fcChildH";
            this.poSQLHelper.SetPara(new object[] { inRefType, inMasterH });
            if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "QNoteCut", "NoteCut", strNoteCut, ref strErrorMsg))
            {
                string strSQLRefToStr = "select * from ORDERH where ORDERH.FCSKID = ?";
                for (int intCnt = 0; intCnt < this.dtsDataEnv.Tables["QNoteCut"].Rows.Count; intCnt++)
                {
                    string strRefToHRowID = this.dtsDataEnv.Tables["QNoteCut"].Rows[intCnt]["fcChildH"].ToString();
                    if ((this.poSQLHelper.SetPara(new object[] { strRefToHRowID })
                        && this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "QRefTo", "ORDERH", strSQLRefToStr, ref strErrorMsg)))
                    {
                        DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                        strRefToCode += dtrRefTo["fcRefNo"].ToString() + (intCnt < this.dtsDataEnv.Tables["QNoteCut"].Rows.Count - 1 ? ", " : "");
                    }
                }
            }
            return strRefToCode;
        }

        private string pmGetRefToCodeH(string inRefType, string inMasterH)
        {
            string strErrorMsg = "";

            string strRefToCode = "";
            string strNoteCut = "select CCHILDH from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? group by CCHILDH";
            this.poSQLHelper.SetPara(new object[] { inRefType, inMasterH });
            if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "QNoteCut", "NoteCut", strNoteCut, ref strErrorMsg))
            {
                string strSQLRefToStr = "select * from ORDERH where ORDERH.FCSKID = ?";
                for (int intCnt = 0; intCnt < this.dtsDataEnv.Tables["QNoteCut"].Rows.Count; intCnt++)
                {
                    string strRefToHRowID = this.dtsDataEnv.Tables["QNoteCut"].Rows[intCnt]["CCHILDH"].ToString();
                    if ((this.poSQLHelper.SetPara(new object[] { strRefToHRowID })
                        && this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "QRefTo", "ORDERH", strSQLRefToStr, ref strErrorMsg)))
                    {
                        DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                        strRefToCode += dtrRefTo["fcRefNo"].ToString() + (intCnt < this.dtsDataEnv.Tables["QNoteCut"].Rows.Count - 1 ? ", " : "");
                    }
                }
            }
            return strRefToCode;
        }

        private string pmGetRefToCodeH2(string inRefType, string inMasterH)
        {
            string strErrorMsg = "";

            string strRefToCode = "";
            string strNoteCut = "select CCHILDH from REFDOC_STMOVE where CMASTERTYP = ? and CMASTERH = ? group by CCHILDH";
            this.poSQLHelper.SetPara(new object[] { inRefType, inMasterH });
            if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "QNoteCut", "NoteCut", strNoteCut, ref strErrorMsg))
            {
                string strSQLRefToStr = "select * from STMREQH where STMREQH.FCSKID = ?";
                for (int intCnt = 0; intCnt < this.dtsDataEnv.Tables["QNoteCut"].Rows.Count; intCnt++)
                {
                    string strRefToHRowID = this.dtsDataEnv.Tables["QNoteCut"].Rows[intCnt]["cChildH"].ToString();
                    if ((this.poSQLHelper.SetPara(new object[] { strRefToHRowID })
                        && this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "QRefTo", "ORDERH", strSQLRefToStr, ref strErrorMsg)))
                    {
                        DataRow dtrRefTo = this.dtsDataEnv.Tables["QRefTo"].Rows[0];
                        strRefToCode += dtrRefTo["fcRefNo"].ToString() + (intCnt < this.dtsDataEnv.Tables["QNoteCut"].Rows.Count - 1 ? ", " : "");
                        //if (!strRefToCode.Contains(dtrRefTo["fcRefNo"].ToString().Trim()))
                        //{
                        //    strRefToCode += dtrRefTo["fcRefNo"].ToString().Trim() + ",";
                        //}
                    }
                }
            }
            return strRefToCode;
        }

        private decimal lpfSumRefQty(string inFieldNo, string tcinRowID)
        {
            string strErrorMsg = "";
            decimal lnSumQty = 0;
            this.poSQLHelper.SetPara(new object[] { tcinRowID });
            if (this.poSQLHelper.SQLExec(ref this.dtsDataEnv, "Q1_SumRefQty", "REFPROD", "select * from REFPROD where fcGLRef = ? order by fcSeq", ref strErrorMsg))
            {
                foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables["Q1_SumRefQty"].Rows)
                {
                    if ("T,W,G".IndexOf(dtrRefProd["fcRfType"].ToString()) > -1)
                    {
                        if (dtrRefProd["fcRefPdTyp"].ToString() == "P"
                            && dtrRefProd["fcIOType"].ToString() == "O")
                        {
                            switch (inFieldNo)
                            {
                                case "F3139":
                                    lnSumQty = lnSumQty + Convert.ToDecimal(dtrRefProd["fnStQty"]);
                                    break;
                                default:
                                    lnSumQty = lnSumQty + Convert.ToDecimal(dtrRefProd["fnQty"]) * (inFieldNo == "F3138" ? 1 : Convert.ToDecimal(dtrRefProd["fnUmQty"]));
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (dtrRefProd["fcRefPdTyp"].ToString() == "P")
                        {
                            switch (inFieldNo)
                            {
                                case "F3139":
                                    lnSumQty = lnSumQty + Convert.ToDecimal(dtrRefProd["fnStQty"]) * (dtrRefProd["fcIOType"].ToString() == "O" ? -1 : 1);
                                    break;
                                default:
                                    lnSumQty = lnSumQty + Convert.ToDecimal(dtrRefProd["fnQty"]) * (inFieldNo == "F3138" ? 1 : Convert.ToDecimal(dtrRefProd["fnUmQty"])) * (dtrRefProd["fcIOType"].ToString() == "O" ? -1 : 1);
                                    break;
                            }
                        }
                    }
                }
            }
            return lnSumQty;
        }

        private string mstrLastMasterH = "";
        private DataRow mdtrLastMasterH = null;
        private DataRow mdtrLastOPSeq = null;

        private void pmLoadRefTo2MMOField(ref DataRow ioLoadValue, string inRefType, string inMasterH, string inFieldNo)
        {

            string strErrorMsg = "";

            string strRetVal = "";
            DataRow dtrWorderH = null;
            DataRow dtrOPSeq = null;
            if (this.mstrLastMasterH != inMasterH)
            {
                this.mstrLastMasterH = inMasterH;
                //Load Reference Document
                this.poSQLHelper2.SetPara(new object[] { inRefType, inMasterH });
                if (this.poSQLHelper2.SQLExec(ref this.dtsDataEnv, "QRefDoc", "REFDOC", "select * from REFDOC where cMasterTyp = ? and cMasterH = ?", ref strErrorMsg))
                {
                    DataRow dtrRefDoc = this.dtsDataEnv.Tables["QRefDoc"].Rows[0];
                    this.poSQLHelper2.SetPara(new object[1] { dtrRefDoc["cChildH"].ToString() });
                    if (this.poSQLHelper2.SQLExec(ref this.dtsDataEnv, "QRefToMO", QMFWOrderHDInfo.TableName, "select * from " + QMFWOrderHDInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {

                        this.mdtrLastMasterH = this.dtsDataEnv.Tables["QRefToMO"].Rows[0];

                        this.poSQLHelper2.SetPara(new object[1] { dtrRefDoc["cChildI"].ToString() });
                        if (this.poSQLHelper2.SQLExec(ref this.dtsDataEnv, "QRefToOP", QMFWOrderIT_OPInfo.TableName, "select * from " + QMFWOrderIT_OPInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                        {
                            this.mdtrLastOPSeq = this.dtsDataEnv.Tables["QRefToOP"].Rows[0];
                        }

                    }

                    dtrWorderH = this.mdtrLastMasterH;
                    dtrOPSeq = this.mdtrLastOPSeq;
                
                }
            }
            else
            {
                dtrWorderH = this.mdtrLastMasterH;
                dtrOPSeq = this.mdtrLastOPSeq;
            }

            if (dtrWorderH == null)
                return;

            DataRow dtrBOM = null;

            switch (inFieldNo)
            {
                case "M1001":
                    this.poSQLHelper2.SetPara(new object[1] { this.mdtrLastMasterH[QMFWOrderHDInfo.Field.BOMID].ToString() });
                    if (this.poSQLHelper2.SQLExec(ref this.dtsDataEnv, "QRefToBOM", QMFBOMInfo.TableName, "select * from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        dtrBOM = this.dtsDataEnv.Tables["QRefToBOM"].Rows[0];
                    }
                    ioLoadValue["M1001"] = dtrBOM[QMFBOMInfo.Field.Code].ToString().TrimEnd();
                    break;
                case "M1002":
                    this.poSQLHelper2.SetPara(new object[1] { this.mdtrLastMasterH[QMFWOrderHDInfo.Field.BOMID].ToString() });
                    if (this.poSQLHelper2.SQLExec(ref this.dtsDataEnv, "QRefToBOM", QMFBOMInfo.TableName, "select * from " + QMFBOMInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        dtrBOM = this.dtsDataEnv.Tables["QRefToBOM"].Rows[0];
                    }
                    ioLoadValue["M1002"] = dtrBOM[QMFBOMInfo.Field.Name].ToString().TrimEnd();
                    break;
                case "M1010":
                    ioLoadValue["M1010"] = dtrWorderH[QMFWOrderHDInfo.Field.Code].ToString().TrimEnd();
                    break;
                case "M1011":
                    ioLoadValue["M1011"] = dtrWorderH[QMFWOrderHDInfo.Field.RefNo].ToString().TrimEnd();
                    break;
                case "M1012":
                    ioLoadValue["M1012"] = Convert.ToDateTime(dtrWorderH[QMFWOrderHDInfo.Field.Date]);
                    break;
                case "M1013":
                    string strStartDate = "";
                    if (!Convert.IsDBNull(dtrWorderH[QMFWOrderHDInfo.Field.StartDate]))
                    {
                        DateTime dttDate = Convert.ToDateTime(dtrWorderH[QMFWOrderHDInfo.Field.StartDate]).Date;
                        strStartDate = dttDate.ToString("dd/MM/yy");
                    }
                    ioLoadValue["M1013"] = strStartDate;
                    break;
                case "M1014":
                    string strDueDate = "";
                    if (!Convert.IsDBNull(dtrWorderH[QMFWOrderHDInfo.Field.DueDate]))
                    {
                        DateTime dttDate = Convert.ToDateTime(dtrWorderH[QMFWOrderHDInfo.Field.DueDate]).Date;
                        strDueDate = dttDate.ToString("dd/MM/yy");
                    }
                    ioLoadValue["M1014"] = strDueDate;
                    break;
                case "M1015":
                    ioLoadValue["M1015"] = dtrOPSeq["cOPSeq"].ToString().TrimEnd();
                    break;
                case "M1016":

                    this.poSQLHelper2.SetPara(new object[1] { dtrOPSeq["cMOPR"].ToString() });
                    if (this.poSQLHelper2.SQLExec(ref this.dtsDataEnv, "QMOPR", "MOPR", "select cCode,cName from " + QMFStdOprInfo.TableName + " where cRowID = ?", ref strErrorMsg))
                    {
                        ioLoadValue["M1016"] = this.dtsDataEnv.Tables["QMOPR"].Rows[0]["cName"].ToString().TrimEnd();
                    }
                    break;
                case "M1017":
                    ioLoadValue["M1017"] = Convert.ToDecimal(dtrWorderH[QMFWOrderHDInfo.Field.MfgQty]);
                    break;
            }

        }

    }
}
