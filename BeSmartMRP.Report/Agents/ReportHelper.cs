using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using AppUtil;
using WS.Data;
using WS.Data.Agents;

namespace BeSmartMRP.Report.Agents
{
	/// <summary>
	/// Summary description for ReportHelper.
	/// </summary>
	public class ReportHelper
	{

		public ReportHelper() {}

        private static DataSet dtsDataEnv = new DataSet();
        
        public static System.Data.OleDb.OleDbParameter AddParameter(object inValue, DbType inDBType)
		{
			System.Data.OleDb.OleDbParameter prm = new System.Data.OleDb.OleDbParameter();
			prm.DbType = inDBType;
			prm.Direction = ParameterDirection.Input;
			prm.Value = inValue;
			return prm;
		}

        private string[] pmAMthName_Thai = new string[] { "ม.ค.", "ก.พ.", "มี.ค.", "เม.ษ.", "พ.ค.", "มิ.ย.", "ก.ค.", "ส.ค.", "ก.ย.", "ต.ค.", "พ.ย.", "ธ.ค." };
        private string[] pmALongMthName_Thai = new string[] { "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม" };

        public static string mMonthName(int inMonth, bool inIslong)
		{
			string strMthName = "";
			switch (inMonth)
			{
				case 1 :
					strMthName = (inIslong ? "January" : "Jan");
					break;
				case 2 :
					strMthName = (inIslong ? "February" : "Feb");
					break;
				case 3 :
					strMthName = (inIslong ? "March" : "Mar");
					break;
				case 4 :
					strMthName = (inIslong ? "April" : "Apr");
					break;
				case 5 :
					strMthName = (inIslong ? "May" : "May");
					break;
				case 6 :
					strMthName = (inIslong ? "June" : "Jun");
					break;
				case 7 :
					strMthName = (inIslong ? "July" : "Jul");
					break;
				case 8 :
					strMthName = (inIslong ? "August" : "Aug");
					break;
				case 9 :
					strMthName = (inIslong ? "September" : "Sep");
					break;
				case 10 :
					strMthName = (inIslong ? "October" : "Oct");
					break;
				case 11 :
					strMthName = (inIslong ? "November" : "Nov");
					break;
				case 12 :
					strMthName = (inIslong ? "December" : "Dec");
					break;
			}
			return strMthName;
		}

        public static string ERPConnectionString = "";
        public static string ERPConnectionString2 = "";

        public static DataTable GetCostFIFO(string inCorp, string inBranch, string inProd, DateTime inDate)
        {

            DataTable dtRetVal = new DataTable();
            
            pmCreateTem();
            pmSumBFQty(inCorp, inBranch, inProd, inDate);

            return dtRetVal;
        }

        //private string xaXtraType = SysDef.gc_RFTYPE_TRAN_PD + "," + SysDef.gc_RFTYPE_ISSUE_PD + "," + SysDef.gc_RFTYPE_PRODUCE_PD + "," + SysDef.gc_RFTYPE_RETURN_ISSUE;
        //private string xaXtraType2 = SysDef.gc_RFTYPE_TRAN_PD + "," + SysDef.gc_RFTYPE_ISSUE_PD;
        private static string xaXtraType = "T,W,G,d";
        private static string xaXtraType2 = "T,W";

        private static void pmCreateTem()
        {
            DataTable dtbTemBFQty = new DataTable(mstrTemBFQty);
            dtbTemBFQty.Columns.Add("QcSect", System.Type.GetType("System.String"));
            dtbTemBFQty.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemBFQty.Columns.Add("Qty", System.Type.GetType("System.Decimal"));
            dtbTemBFQty.Columns.Add("Amt", System.Type.GetType("System.Decimal"));

            dtbTemBFQty.Columns["QcSect"].DefaultValue = "";
            dtbTemBFQty.Columns["QcProd"].DefaultValue = "";
            dtbTemBFQty.Columns["Qty"].DefaultValue = 0;
            dtbTemBFQty.Columns["Amt"].DefaultValue = 0;

            dtsDataEnv.Tables.Add(dtbTemBFQty);


            //string strFldLst = " PROD.FCCODE as QCPROD, FCIOTYPE, sum(FNQTY*FNUMQTY) as SUMQTY ";

            DataTable dtbTemBFQty2 = new DataTable(mstrTemAllBFQty);
            dtbTemBFQty2.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemBFQty2.Columns.Add("fcIOType", System.Type.GetType("System.String"));
            dtbTemBFQty2.Columns.Add("SumQty", System.Type.GetType("System.Decimal"));

            dtsDataEnv.Tables.Add(dtbTemBFQty2);

            DataTable dtbTemCalPrice = new DataTable(mstrTemCalPrice);
            dtbTemCalPrice.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemCalPrice.Columns.Add("Qty", System.Type.GetType("System.Decimal"));
            dtbTemCalPrice.Columns.Add("Amt", System.Type.GetType("System.Decimal"));

            dtbTemCalPrice.Columns["QcProd"].DefaultValue = "";
            dtbTemCalPrice.Columns["Qty"].DefaultValue = 0;
            dtbTemCalPrice.Columns["Amt"].DefaultValue = 0;

            dtsDataEnv.Tables.Add(dtbTemCalPrice);

            DataTable dtbTemCalPrice2 = new DataTable(mstrTemAllCalPrice);
            dtbTemCalPrice2.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemCalPrice2.Columns.Add("fcIOType", System.Type.GetType("System.String"));
            dtbTemCalPrice2.Columns.Add("fcRfType", System.Type.GetType("System.String"));
            dtbTemCalPrice2.Columns.Add("fnQty", System.Type.GetType("System.Decimal"));
            dtbTemCalPrice2.Columns.Add("fnUMQty", System.Type.GetType("System.Decimal"));
            dtbTemCalPrice2.Columns.Add("fnCostAmt", System.Type.GetType("System.Decimal"));
            dtbTemCalPrice2.Columns.Add("fnCostAdj", System.Type.GetType("System.Decimal"));

            DataTable dtbTemBFFIFO = new DataTable(mstrTemBFFIFO);
            dtbTemBFFIFO.Columns.Add("IsBF", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("SedRefNo", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("PriceSeq", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("QcBook", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("fcRfType", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("fcRefType", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("fcRefNo", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("fdDate", System.Type.GetType("System.DateTime"));
            dtbTemBFFIFO.Columns.Add("cDate", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("QcPdGrp", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("QnPdGrp", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("QcSect", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("QnSect", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("QcJob", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("QnJob", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("QnProd", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("QnUM", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("fcIOType", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("fnQty", System.Type.GetType("System.Decimal"));
            dtbTemBFFIFO.Columns.Add("fnUMQty", System.Type.GetType("System.Decimal"));
            dtbTemBFFIFO.Columns.Add("fnPrice", System.Type.GetType("System.Decimal"));
            dtbTemBFFIFO.Columns.Add("fnCostAmt", System.Type.GetType("System.Decimal"));
            dtbTemBFFIFO.Columns.Add("fnCostAdj", System.Type.GetType("System.Decimal"));
            dtbTemBFFIFO.Columns.Add("fnCostAmt1", System.Type.GetType("System.Decimal"));

            dtbTemBFFIFO.Columns["IsBF"].DefaultValue = "";
            dtbTemBFFIFO.Columns["SedRefNo"].DefaultValue = "";
            dtbTemBFFIFO.Columns["PriceSeq"].DefaultValue = "";
            dtbTemBFFIFO.Columns["fnPrice"].DefaultValue = 0;
            dtbTemBFFIFO.Columns["fnCostAmt"].DefaultValue = 0;
            dtbTemBFFIFO.Columns["fnCostAdj"].DefaultValue = 0;
            dtbTemBFFIFO.Columns["fnCostAmt1"].DefaultValue = 0;

            dtsDataEnv.Tables.Add(dtbTemBFFIFO);


            DataTable dtbTemPreview = new DataTable(mstrTemPreview);
            dtbTemPreview.Columns.Add("QnCorp", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("QcBook", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("Title1", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("Title2", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("Title3", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("Title4", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("Title5", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("PriceSeq", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("fcRefNo", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("fdDate", System.Type.GetType("System.DateTime"));
            dtbTemPreview.Columns.Add("QcPdGrp", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("QnPdGrp", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("QnProd", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("QnUM", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("QcSect", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("QnSect", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("QcJob", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("QnJob", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("fcIOType", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("fcRefType", System.Type.GetType("System.String"));
            dtbTemPreview.Columns.Add("fnQty", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("fnUMQty", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("fnPrice", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("fnCostAmt", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("fnCostAdj", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("fnCostAmt1", System.Type.GetType("System.Decimal"));

            //ยอดยกมา
            dtbTemPreview.Columns.Add("Qty_BF", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Cost_BF", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Amt_BF", System.Type.GetType("System.Decimal"));

            //ยอดยกไป
            dtbTemPreview.Columns.Add("Qty_CF", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Cost_CF", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Amt_CF", System.Type.GetType("System.Decimal"));

            //ยอดยกไป
            //RefType = 'B,C,D'
            dtbTemPreview.Columns.Add("Qty_Buy", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Cost_Buy", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Amt_Buy", System.Type.GetType("System.Decimal"));

            //ยอดขาย
            //RefType = 'S,E,F'
            dtbTemPreview.Columns.Add("Qty_Sell", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Cost_Sell", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Amt_Sell", System.Type.GetType("System.Decimal"));

            //ปรับยอดเพิ่ม (3.1)
            //QCBOOK="A"+รหัสแผนก and fcIOType = 'I'
            dtbTemPreview.Columns.Add("Qty_AJI", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Cost_AJI", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Amt_AJI", System.Type.GetType("System.Decimal"));

            //ปรับยอดลด (3.1)
            //QCBOOK="A"+รหัสแผนก and fcIOType = 'O'
            dtbTemPreview.Columns.Add("Qty_AJO", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Cost_AJO", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Amt_AJO", System.Type.GetType("System.Decimal"));

            //ยอดคืน (3.1)
            //QCBOOK="A"+รหัสแผนก and fcIOType = 'I'
            dtbTemPreview.Columns.Add("Qty_RT", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Cost_RT", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Amt_RT", System.Type.GetType("System.Decimal"));

            //ยอดเบิก (3.2)
            //QCBOOK="0"+รหัสแผนก
            dtbTemPreview.Columns.Add("Qty_IS", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Cost_IS", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Amt_IS", System.Type.GetType("System.Decimal"));

            //ยอดแจก (3.3)
            //QCBOOK="I"+รหัสแผนก
            dtbTemPreview.Columns.Add("Qty_Sell2", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Cost_Sell2", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Amt_Sell2", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Price_Sell", System.Type.GetType("System.Decimal"));

            //ยอดโอน (3.4)
            //รายงาน 5.2.4 a ใช้ QCBOOK="T"+รหัสแผนก and fcIOType = 'O'
            //รายงาน 5.2.4 b ใช้ QCBOOK=T0001 and fcIOType = 'I'
            dtbTemPreview.Columns.Add("Qty_TR", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Cost_TR", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Amt_TR", System.Type.GetType("System.Decimal"));

            //ยอดคืนคลังใหญ่ (3.5)
            //รายงาน 5.2.4 b ใช้ QCBOOK=R0001
            dtbTemPreview.Columns.Add("Qty_RT2", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Cost_RT2", System.Type.GetType("System.Decimal"));
            dtbTemPreview.Columns.Add("Amt_RT2", System.Type.GetType("System.Decimal"));

            dtbTemPreview.Columns["Title1"].DefaultValue = "";
            dtbTemPreview.Columns["Title2"].DefaultValue = "";
            dtbTemPreview.Columns["Title3"].DefaultValue = "";
            dtbTemPreview.Columns["Title4"].DefaultValue = "";
            dtbTemPreview.Columns["Title5"].DefaultValue = "";

            dtbTemPreview.Columns["fnQty"].DefaultValue = 0;
            dtbTemPreview.Columns["fnPrice"].DefaultValue = 0;
            dtbTemPreview.Columns["fnCostAmt"].DefaultValue = 0;
            dtbTemPreview.Columns["fnCostAmt1"].DefaultValue = 0;

            dtbTemPreview.Columns["Qty_BF"].DefaultValue = 0;
            dtbTemPreview.Columns["Cost_BF"].DefaultValue = 0;
            dtbTemPreview.Columns["Amt_BF"].DefaultValue = 0;
            dtbTemPreview.Columns["Qty_CF"].DefaultValue = 0;
            dtbTemPreview.Columns["Cost_CF"].DefaultValue = 0;
            dtbTemPreview.Columns["Amt_CF"].DefaultValue = 0;
            dtbTemPreview.Columns["Qty_Buy"].DefaultValue = 0;
            dtbTemPreview.Columns["Cost_Buy"].DefaultValue = 0;
            dtbTemPreview.Columns["Amt_Buy"].DefaultValue = 0;
            dtbTemPreview.Columns["Qty_Sell"].DefaultValue = 0;
            dtbTemPreview.Columns["Cost_Sell"].DefaultValue = 0;
            dtbTemPreview.Columns["Amt_Sell"].DefaultValue = 0;
            dtbTemPreview.Columns["Qty_AJI"].DefaultValue = 0;
            dtbTemPreview.Columns["Cost_AJI"].DefaultValue = 0;
            dtbTemPreview.Columns["Amt_AJI"].DefaultValue = 0;
            dtbTemPreview.Columns["Qty_AJO"].DefaultValue = 0;
            dtbTemPreview.Columns["Cost_AJO"].DefaultValue = 0;
            dtbTemPreview.Columns["Amt_AJO"].DefaultValue = 0;
            dtbTemPreview.Columns["Qty_RT"].DefaultValue = 0;
            dtbTemPreview.Columns["Cost_RT"].DefaultValue = 0;
            dtbTemPreview.Columns["Amt_RT"].DefaultValue = 0;
            dtbTemPreview.Columns["Qty_IS"].DefaultValue = 0;
            dtbTemPreview.Columns["Cost_IS"].DefaultValue = 0;
            dtbTemPreview.Columns["Amt_IS"].DefaultValue = 0;
            dtbTemPreview.Columns["Qty_Sell2"].DefaultValue = 0;
            dtbTemPreview.Columns["Cost_Sell2"].DefaultValue = 0;
            dtbTemPreview.Columns["Amt_Sell2"].DefaultValue = 0;
            dtbTemPreview.Columns["Price_Sell"].DefaultValue = 0;
            dtbTemPreview.Columns["Qty_TR"].DefaultValue = 0;
            dtbTemPreview.Columns["Cost_TR"].DefaultValue = 0;
            dtbTemPreview.Columns["Amt_TR"].DefaultValue = 0;
            dtbTemPreview.Columns["Qty_RT2"].DefaultValue = 0;
            dtbTemPreview.Columns["Cost_RT2"].DefaultValue = 0;
            dtbTemPreview.Columns["Amt_RT2"].DefaultValue = 0;

            dtsDataEnv.Tables.Add(dtbTemPreview);

        }

        private static WS.Data.Agents.cDBMSAgent poSQLHelper = null;
        private static WS.Data.Agents.cDBMSAgent poSQLHelper2 = null;

        private static string mstrTemBFQty = "TemQty";
        private static string mstrTemAllBFQty = "TemAllQty";
        private static string mstrTemBFFIFO = "TemBFFIFO";
        private static string mstrTemPreview = "TemPreview";

        private static string mstrTemCalPrice = "TemCalPrice";
        private static string mstrTemAllCalPrice = "TemAllCalPrice";

        private static void pmSumBFQty(string inCorp, string inBranch, string inProd, DateTime inDate)
        {

            string strFld = " (select Prod.fcCode from Prod where Prod.fcSkid = RefProd.fcProd) as QcProd ";
            strFld += " , (select Prod.fcName from Prod where Prod.fcSkid = RefProd.fcProd) as QnProd ";
            strFld += " , RefProd.fcIOType , sum(RefProd.fnQty * RefProd.fnUMQty) as SumQty ";

            string strSQLStr = "select " + strFld + " from RefProd";
            strSQLStr += " left join PROD on PROD.fcSkid = RefProd.fcProd ";
            strSQLStr += " left join WHOUSE on WHOUSE.fcSkid = RefProd.fcWHouse ";
            strSQLStr += " where RefProd.fcCorp = ? ";
            strSQLStr += " and RefProd.fcBranch = ? ";
            strSQLStr += " and RefProd.fcProd = ? ";
            strSQLStr += " and RefProd.fdDate < ? ";
            strSQLStr += " and RefProd.fcStat <> 'C' ";
            strSQLStr += " and WHouse.fcType = ' ' ";
            strSQLStr += " and WHouse.fcType = ' ' ";
            strSQLStr += " group by RefProd.fcProd, RefProd.fcIOType ";

            System.Data.IDbConnection conn = poSQLHelper.GetDBConnection();
            System.Data.IDbCommand cmd = poSQLHelper.GetDBCommand(strSQLStr.ToUpper(), conn);

            try
            {
                conn.Open();
                cmd.CommandTimeout = 2000;

                cmd.Parameters.Add(ReportHelper.AddParameter(inCorp, DbType.String));
                cmd.Parameters.Add(ReportHelper.AddParameter(inBranch, DbType.String));
                cmd.Parameters.Add(ReportHelper.AddParameter(inProd, DbType.String));
                cmd.Parameters.Add(ReportHelper.AddParameter(inDate, DbType.DateTime));

                System.Data.OleDb.OleDbDataReader dtrProd = (System.Data.OleDb.OleDbDataReader)cmd.ExecuteReader();
                if (dtrProd.HasRows)
                {
                    DateTime dttBegDate = inDate;
                    DateTime dttEndDate = dttBegDate;

                    while (dtrProd.Read())
                    {

                        DataRow dtrPreview = dtsDataEnv.Tables[mstrTemAllBFQty].NewRow();

                        dtrPreview["QcProd"] = dtrProd["QcProd"].ToString();
                        dtrPreview["fcIOType"] = dtrProd["fcIOType"].ToString();
                        dtrPreview["SumQty"] = Convert.IsDBNull(dtrProd["SumQty"]) ? 0 : Convert.ToDecimal(dtrProd["SumQty"]);
                        dtsDataEnv.Tables[mstrTemAllBFQty].Rows.Add(dtrPreview);

                    }
                }
                dtrProd.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        private static void pmGetBFQty(string inProd, ref decimal ioBFQty, ref decimal ioAmt)
        {
            decimal decBFQty = 0;
            decimal decAmt = 0;
            //DataRow[] drBFQty = dtsDataEnv.Tables[mstrTemBFQty].Select("QcSect = '" +inSect +"' and QcProd = '" +inProd+"'");
            DataRow[] drBFQty = dtsDataEnv.Tables[mstrTemBFQty].Select("QcProd = '" + inProd + "'");
            if (drBFQty.Length == 0)
            {
                DataRow dtrBFQty = dtsDataEnv.Tables[mstrTemBFQty].NewRow();
                //dtrBFQty["QcSect"] = inSect;
                dtrBFQty["QcProd"] = inProd;

                decBFQty = pmCalBFQty(inProd);
                //decAmt = this.pmCalPrice(inProd);

                dtrBFQty["Qty"] = decBFQty;
                dtrBFQty["Amt"] = decAmt;
                dtsDataEnv.Tables[mstrTemBFQty].Rows.Add(dtrBFQty);
            }

            ioBFQty = decBFQty;
            ioAmt = decAmt;
        }

        private static decimal pmCalBFQty(string inProd)
        {
            decimal decSumQty = 0;
            decimal tSign = 1;
            DataRow[] drBFQty = dtsDataEnv.Tables[mstrTemAllBFQty].Select("QCPROD = '" + inProd + "'");
            if (drBFQty.Length > 0)
            {
                for (int intCnt = 0; intCnt < drBFQty.Length; intCnt++)
                {
                    tSign = (drBFQty[intCnt]["fcIoType"].ToString() == "I" ? 1 : -1);
                    decSumQty += Convert.ToDecimal(drBFQty[intCnt]["SUMQTY"]) * tSign;
                }
            }
            return decSumQty;
        }

        private void pmPrint1Prod(string inProd, string inBranch, DateTime inBegDate, DateTime inEndDate, decimal inYokMaQty, ref decimal ioAmt, ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo, bool inChkWhouse)
        {

            decimal tYokMaAmt = 0;
            decimal tQty = inYokMaQty;
            decimal xaSedQty = ioSedQty;
            string xaSedRefNo = ioSedRefNo;
            decimal xaSedQty2 = ioSedQty2;
            decimal xaSedCost = ioSedCost;
            int xaSedRecn = ioSedRecn;
            bool tFstPSed = false;

            string lcSeleFieldStr = " select RefProd.fcProd,RefProd.fcBranch,RefProd.fcWhouse,RefProd.fdDate,RefProd.fcTimeStam,RefProd.fcIOType,RefProd.fcGLRef,RefProd.fcSkid,RefProd.fnQty,RefProd.fnUmQty,RefProd.fcRfType,RefProd.fcRefType,RefProd.fnCostAmt,RefProd.fnCostAdj,RefProd.fnPriceKe, RefProd.fcStat,RefProd.fcSect,RefProd.fcJob,RefProd.fcLot,RefProd.fcSeq ";
            lcSeleFieldStr += " , GLRef.fcRefNo";
            lcSeleFieldStr += " , Book.fcCode as QcBook";
            lcSeleFieldStr += " , UM.fcName as QnUM";
            lcSeleFieldStr += " , Prod.fcCode as QcProd , Prod.fcName as QnProd , UM.fcName as QnUM";
            lcSeleFieldStr += " , PdGrp.fcCode as QcPdGrp , PdGrp.fcName as QnPdGrp";
            lcSeleFieldStr += " , Sect.fcCode as QcSect , Sect.fcName as QnSect ";
            lcSeleFieldStr += " , Job.fcCode as QcJob , Job.fcName as QnJob ";

            string lcFOXSQLStr = lcSeleFieldStr;
            lcFOXSQLStr = lcFOXSQLStr + " from RefProd ";
            lcFOXSQLStr = lcFOXSQLStr + " left join GLRef on GLRef.fcSkid = RefProd.fcGLRef ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Prod on Prod.fcSkid = RefProd.fcProd ";
            lcFOXSQLStr = lcFOXSQLStr + " left join PdGrp on PdGrp.fcSkid = Prod.fcPdGrp ";
            lcFOXSQLStr = lcFOXSQLStr + " left join UM on UM.fcSkid = Prod.fcUM ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Sect on Sect.fcSkid = RefProd.fcSect ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Job on Job.fcSkid = RefProd.fcJob ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Book on Book.fcSkid = GLRef.fcBook ";
            lcFOXSQLStr = lcFOXSQLStr + " left join WHouse on WHouse.fcSkid = RefProd.fcWHouse ";
            lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @xaProd and RefProd.fcBranch = @xaBranch and RefProd.fdDate between @xaBegDate and @xaEndDate ";
            //if (this.txtTagWHouse.Text.TrimEnd() != string.Empty)
            //{
            //    lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcCode in ( " + this.txtTagWHouse.Text.TrimEnd() + " )";
            //}
            lcFOXSQLStr = lcFOXSQLStr + " and RefProd.fcStat <> 'C' ";
            lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fdDate , RefProd.fcTimeStam , RefProd.fcIOType , RefProd.fcGLRef  , RefProd.fcSkid ";

            SqlConnection conn = new SqlConnection(ERPConnectionString2);
            SqlCommand cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
            SqlDataReader dtrRefProd = null;

            //try
            //{
            conn.Open();
            cmd.CommandTimeout = 2000;

            cmd.Parameters.AddWithValue("@xaProd", inProd);
            cmd.Parameters.AddWithValue("@xaBranch", inBranch);
            cmd.Parameters.AddWithValue("@xaBegDate", inBegDate.Date);
            cmd.Parameters.AddWithValue("@xaEndDate", inEndDate.Date);

            dtrRefProd = cmd.ExecuteReader();
            if (dtrRefProd.HasRows)
            {
                //while (dtrRefProd.Read() && tQty > 0)
                while (dtrRefProd.Read())
                {

                    if (VRefProd3(dtrRefProd["fcRfType"].ToString(), dtrRefProd["fcIOType"].ToString()))
                    {

                        decimal tRefPdQty = Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"]) * (dtrRefProd["fcIOType"].ToString() == "I" ? 1 : -1);
                        decimal tAbsQty = Math.Abs(tRefPdQty);
                        decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrRefProd["fnCostAmt"]) + Convert.ToDecimal(dtrRefProd["fnCostAdj"]));
                        decimal tRefCost = 0;

                        decimal tAmt = 0;
                        decimal tAmt1 = 0;
                        decimal tQty1 = 0;
                        decimal tQty2 = 0;
                        decimal tCost1 = 0;
                        decimal tCostAdj = 0;
                        decimal tInAmount = 0;
                        if (tRefPdQty >= 0)
                        {
                            //รายการฝั่งรับ
                            tAmt = Math.Round(tCostAmt, 4, MidpointRounding.AwayFromZero);
                            tQty1 = QtySeq1(tCostAmt, tAbsQty);
                            tCost1 = CostSeq1(tCostAmt, tAbsQty);

                            tAmt1 = tCost1 * tQty1;
                            tCostAdj = tCostAmt;
                            tInAmount = tAmt1 + (tAmt - tAmt1);

                            //= P1Line( tDate , tRefNo , tQty1 , tCost1 , tAmt1 , 'I' , .T. , @xaYokMaQty , @xaYokMaAmt , RefProd.fcTimeStam , tCostAdj , tInAmount , RefProd.fcSect , RefProd.fcJob , RefProd.fcLot , tAbsQty )
                            P1Line(dtrRefProd, tQty1, tCost1, tAmt1, xaSedRefNo);
                            tQty2 = tAbsQty - tQty1;

                            if ((xaSedQty == 0) && (xaSedRecn != -1))
                            {
                                SumSed(ref xaSedQty, ref xaSedRecn, ref xaSedCost, ref xaSedQty2, ref xaSedRefNo);
                            }

                        }
                        else
                        {
                            //รายการฝั่งจ่าย
                            tCostAdj = tCostAmt;
                            tRefCost = xaSedCost;

                            //if RefProd.fcRfType $ gc_RFTYPE_CR_BUY .and. ! xdCrBuyCostAsSell $ xaToDisp
                            //    tRefCost = iif( RefProd.fnQty * RefProd.fnUmQty # 0 , roun( tCostAmt / tAbsQty , xaRoundPrcAt ) , 0 )
                            //else
                            //    tRefCost = xaSedCost
                            //endif
                            tFstPSed = true;
                            decimal tPCost = 0;
                            if (xaSedQty - tAbsQty >= 0)
                            {
                                tPCost = Math.Round(tRefCost, 4, MidpointRounding.AwayFromZero);
                                tAmt = tRefCost * tAbsQty;
                                //= P1Line( tDate , tRefNo , tAbsQty , tPCost , tAmt , 'O' , .T. , @xaYokMaQty , @xaYokMaAmt , RefProd.fcTimeStam , tCostAdj , tAmt , RefProd.fcSect , RefProd.fcJob , RefProd.fcLot , tAbsQty )
                                P1Line(dtrRefProd, tAbsQty, tPCost, tAmt, xaSedRefNo);
                                xaSedQty = xaSedQty - tAbsQty;
                                tFstPSed = false;
                            }
                            else
                            {

                                decimal tAdj = 0;
                                decimal tOutAmt = 0;
                                decimal tOutAmt1 = 0;
                                decimal tOutAmt2 = 0;
                                decimal tOQty = tAbsQty;
                                if (xaSedQty > 0)
                                {
                                    while ((xaSedQty - tOQty < 0) && (xaSedRecn > -1))
                                    {
                                        tOQty = tOQty - xaSedQty;
                                        tPCost = Math.Round(tRefCost, 4, MidpointRounding.AwayFromZero);
                                        tAmt = tRefCost * xaSedQty;
                                        tOutAmt1 = tAmt;
                                        tAdj = tCostAdj - tAmt;
                                        tOutAmt = tAdj + tAmt;
                                        //= P1Line( iif( tFstPSed , tDate , {//} ) , iif( tFstPSed , tRefNo , '' ) , xaSedQty , tPCost , tAmt , 'O' , .T. , @xaYokMaQty , @xaYokMaAmt , iif( tFstPSed , RefProd.fcTimeStam , '' ) , tCostAdj , tOutAmt , RefProd.fcSect , RefProd.fcJob , RefProd.fcLot , tAbsQty )
                                        P1Line(dtrRefProd, xaSedQty, tPCost, tAmt, xaSedRefNo);
                                        //if (tFstPSed)
                                        //{
                                        //    P1Line(dtrRefProd, xaSedQty, tPCost, tAmt);
                                        //}
                                        //else
                                        //{
                                        //    P1Line(null, xaSedQty, tPCost, tAmt);
                                        //}
                                        SumSed(ref xaSedQty, ref xaSedRecn, ref xaSedCost, ref xaSedQty2, ref xaSedRefNo);
                                        tFstPSed = false;
                                        tRefCost = xaSedCost;
                                        //if RefProd.fcRfType $ gc_RFTYPE_CR_BUY .and. ! xdCrBuyCostAsSell $ xaToDisp
                                        //  tRefCost = iif( RefProd.fnQty * RefProd.fnUmQty # 0 , roun( tCostAmt / tAbsQty , xaRoundPrcAt ) , 0 )
                                        //else
                                        //  tRefCost = xaSedCost
                                        //endif
                                    }
                                }
                                if (tOQty > 0)
                                {
                                    xaSedQty = xaSedQty - tOQty;
                                    xaSedQty = (xaSedQty > 0 ? xaSedQty : 0);
                                    //xaSedQty = max( xaSedQty , 0 )
                                    tPCost = Math.Round(tRefCost, 4, MidpointRounding.AwayFromZero);
                                    tAmt = tRefCost * tOQty;
                                    tOutAmt2 = tOutAmt1 + tAmt;
                                    //= P1Line( iif( tFstPSed , tDate , {//} ) , iif( tFstPSed , tRefNo , '' ) , tOQty , tPCost , tAmt , 'O' , .T. , @xaYokMaQty , @xaYokMaAmt , iif( tFstPSed , RefProd.fcTimeStam , '' ) , tCostAdj , tOutAmt2 , RefProd.fcSect , RefProd.fcJob , RefProd.fcLot , tAbsQty )
                                    P1Line(dtrRefProd, tOQty, tPCost, tAmt, xaSedRefNo);
                                    //if (tFstPSed)
                                    //{
                                    //    P1Line(dtrRefProd, tOQty, tPCost, tAmt);
                                    //}
                                    //else
                                    //{
                                    //    P1Line(null, tOQty, tPCost, tAmt);
                                    //}
                                }
                            }
                            if ((xaSedQty == 0) && (xaSedRecn != -1))
                            {
                                SumSed(ref xaSedQty, ref xaSedRecn, ref xaSedCost, ref xaSedQty2, ref xaSedRefNo);
                            }
                        }

                    }
                }
            }

            //}
            //catch (Exception ex)
            //{
            //    string strErrorMsg = ex.Message;
            //}
            //finally
            //{
            conn.Close();
            //}

        }

        private static void pmPrintBFQTYLine(string inProd, string inBranch, DateTime inBegDate, decimal inYokMaQty, ref decimal ioAmt, ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo)
        {

            DateTime tFstYokMaDate = DateTime.MinValue;
            string tCrSedSkid = "";
            ioSedQty = 0;
            ioSedQty2 = 0;
            ioSedRecn = -1;
            ioSedCost = 0;
            decimal tYokMaAmt = 0;
            decimal tQty = inYokMaQty;

            string lcSeleFieldStr = " select RefProd.fcProd,RefProd.fcBranch,RefProd.fcWhouse,RefProd.fdDate,RefProd.fcTimeStam,RefProd.fcIOType,RefProd.fcGLRef,RefProd.fcSkid,RefProd.fnQty,RefProd.fnUmQty,RefProd.fcRfType,RefProd.fcRefType,RefProd.fnCostAmt,RefProd.fnCostAdj,RefProd.fnPriceKe, RefProd.fcStat,RefProd.fcSect,RefProd.fcJob,RefProd.fcLot, RefProd.fcSeq ";
            lcSeleFieldStr += " , GLRef.fcRefNo";
            lcSeleFieldStr += " , Book.fcCode as QcBook";
            lcSeleFieldStr += " , UM.fcName as QnUM";
            lcSeleFieldStr += " , Prod.fcCode as QcProd , Prod.fcName as QnProd , UM.fcName as QnUM";
            lcSeleFieldStr += " , PdGrp.fcCode as QcPdGrp , PdGrp.fcName as QnPdGrp";
            lcSeleFieldStr += " , Sect.fcCode as QcSect , Sect.fcName as QnSect ";
            lcSeleFieldStr += " , Job.fcCode as QcJob , Job.fcName as QnJob ";

            string lcFOXSQLStr = lcSeleFieldStr;
            lcFOXSQLStr = lcFOXSQLStr + " from RefProd ";
            lcFOXSQLStr = lcFOXSQLStr + " left join GLRef on GLRef.fcSkid = RefProd.fcGLRef ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Prod on Prod.fcSkid = RefProd.fcProd ";
            lcFOXSQLStr = lcFOXSQLStr + " left join PdGrp on PdGrp.fcSkid = Prod.fcPdGrp ";
            lcFOXSQLStr = lcFOXSQLStr + " left join UM on UM.fcSkid = Prod.fcUM ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Sect on Sect.fcSkid = RefProd.fcSect ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Job on Job.fcSkid = RefProd.fcJob ";
            lcFOXSQLStr = lcFOXSQLStr + " left join Book on Book.fcSkid = GLRef.fcBook ";
            lcFOXSQLStr = lcFOXSQLStr + " left join WHOUSE on WHOUSE.fcSkid = RefProd.fcWHouse ";
            lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @xaProd and RefProd.fcBranch = @xaBranch and RefProd.fdDate <= @xaDate ";
            //if (this.txtTagWHouse.Text.Trim() != string.Empty)
            //{
            //    lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcCode in ( " + this.txtTagWHouse.Text.Trim() + " ) ";
            //}
            lcFOXSQLStr = lcFOXSQLStr + " and RefProd.fcStat <> 'C' ";
            lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fcProd desc,RefProd.fcBranch desc,RefProd.fdDate desc , RefProd.fcTimeStam desc , RefProd.fcIOType desc , RefProd.fcGLRef desc , RefProd.fcSkid desc";

            SqlConnection conn = new SqlConnection(ERPConnectionString2);
            SqlCommand cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
            SqlDataReader dtrRefProd = null;

            //try
            //{
            conn.Open();
            cmd.CommandTimeout = 2000;

            cmd.Parameters.AddWithValue("@xaProd", inProd);
            cmd.Parameters.AddWithValue("@xaBranch", inBranch);
            cmd.Parameters.AddWithValue("@xaDate", inBegDate.AddDays(-1));

            dtrRefProd = cmd.ExecuteReader();
            if (dtrRefProd.HasRows)
            {
                string strCDate = "";
                while (dtrRefProd.Read() && tQty > 0)
                {

                    if (VRefProd2(dtrRefProd["fcRfType"].ToString(), dtrRefProd["fcIOType"].ToString(), dtrRefProd["fcStat"].ToString(), Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"])))
                    {

                        DataRow dtrTemFIFO = dtsDataEnv.Tables[mstrTemBFFIFO].NewRow();
                        strCDate = Convert.ToDateTime(dtrRefProd["fdDate"]).Year.ToString("0000") + Convert.ToDateTime(dtrRefProd["fdDate"]).ToString("MMdd") + dtrRefProd["fcRefNo"].ToString() + dtrRefProd["fcSkid"].ToString();
                        dtrTemFIFO["IsBF"] = "Y";
                        dtrTemFIFO["QcBook"] = dtrRefProd["QcBook"].ToString();
                        dtrTemFIFO["fcRfType"] = dtrRefProd["fcRfType"].ToString();
                        dtrTemFIFO["fcRefType"] = dtrRefProd["fcRefType"].ToString();
                        dtrTemFIFO["fcRefNo"] = dtrRefProd["fcRefNo"].ToString();
                        dtrTemFIFO["fdDate"] = Convert.ToDateTime(dtrRefProd["fdDate"]);
                        dtrTemFIFO["cDate"] = strCDate;
                        dtrTemFIFO["QcPdGrp"] = dtrRefProd["QcPdGrp"].ToString();
                        dtrTemFIFO["QnPdGrp"] = dtrRefProd["QnPdGrp"].ToString();
                        dtrTemFIFO["QcProd"] = dtrRefProd["QcProd"].ToString();
                        dtrTemFIFO["QnProd"] = dtrRefProd["QnProd"].ToString();
                        dtrTemFIFO["QnUM"] = dtrRefProd["QnUM"].ToString();
                        dtrTemFIFO["fcIOType"] = dtrRefProd["fcIOType"].ToString();

                        dtsDataEnv.Tables[mstrTemBFFIFO].Rows.Add(dtrTemFIFO);

                        decimal tQty1 = 0;
                        decimal tQty2 = 0;
                        decimal tCost1 = 0;
                        decimal tCost2 = 0;

                        decimal tRefPdQty = Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"]) * (dtrRefProd["fcIOType"].ToString() == "I" ? 1 : -1);
                        decimal tAbsQty = Math.Abs(tRefPdQty);
                        decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrRefProd["fnCostAmt"]) + Convert.ToDecimal(dtrRefProd["fnCostAdj"]));
                        if (tRefPdQty < tQty)
                        {
                            tQty1 = QtySeq1(tCostAmt, tAbsQty);
                            tQty2 = tAbsQty - tQty1;
                            //tYokMaAmt = tYokMaAmt + Math.Round(tCostAmt, App.ActiveCorp.RoundAmtAt);
                            tYokMaAmt = tYokMaAmt + tCostAmt;
                            ioSedCost = CostSeq1(tCostAmt, tAbsQty);
                            ioSedQty = tQty - tQty2;  // เผื่อกรณีที่ไม่มี record ที่ตรงเงื่อนไขอีกแล้ว ก็ถือว่าจำนวนยกมาครั้งแรกมีราคาทุน = record แรก
                            ioSedQty2 = tQty2;         //เผื่อกรณีที่ไม่มี record ที่ตรงเงื่อนไขอีกแล้ว ก็ถือว่าจำนวนยกมาครั้งแรกมีราคาทุน = record แรก
                            tQty = tQty - tAbsQty;

                            dtrTemFIFO["fnQty"] = tQty1;
                            dtrTemFIFO["fnUMQty"] = 1;
                            dtrTemFIFO["fnPrice"] = ioSedCost;
                            dtrTemFIFO["fnCostAmt"] = tCostAmt;
                        }
                        else
                        {
                            tQty1 = QtySeq1(tCostAmt, tAbsQty);
                            tCost1 = CostSeq1(tCostAmt, tAbsQty);
                            tCost2 = CostSeq2(tCostAmt, tAbsQty);
                            tQty2 = tAbsQty - tQty1;
                            if (tQty2 >= tQty)
                            {
                                //tYokMaAmt = tYokMaAmt + tCostAmt - Math.Round(tCost1 * tQty1, App.ActiveCorp.RoundAmtAt) - Math.Round(tCost2 * (tQty2 - tQty), App.ActiveCorp.RoundAmtAt);
                                tYokMaAmt = tYokMaAmt + tCostAmt - (tCost1 * tQty1) - (tCost2 * (tQty2 - tQty));
                                ioSedCost = tCost2;
                                ioSedQty = tQty;
                                ioSedQty2 = 0;
                            }
                            else
                            {
                                //tYokMaAmt = tYokMaAmt + tCostAmt - Math.Round(tCost1 * (tAbsQty - tQty), App.ActiveCorp.RoundAmtAt);
                                tYokMaAmt = tYokMaAmt + tCostAmt - (tCost1 * (tAbsQty - tQty));
                                ioSedCost = tCost1;
                                ioSedQty = tQty - tQty2;
                                ioSedQty2 = tQty2;
                            }
                            dtrTemFIFO["fnQty"] = ioSedQty;
                            dtrTemFIFO["fnUMQty"] = 1;
                            dtrTemFIFO["fnPrice"] = ioSedCost;
                            dtrTemFIFO["fnCostAmt"] = (ioSedQty * ioSedCost);
                            tQty = 0;
                        }
                    }
                    tFstYokMaDate = Convert.ToDateTime(dtrRefProd["fdDate"]);
                    tCrSedSkid = dtrRefProd["fcSkid"].ToString();
                    ioSedRecn = 0;
                    //ioSedRefNo = dtrRefProd["fcRefNo"].ToString() + dtrRefProd["fcSeq"].ToString();
                    ioSedRefNo = strCDate;
                }
            }
            dtrRefProd.Close();

            if (tQty > 0)
            {
                //tYokMaAmt = tYokMaAmt + Math.Round(ioSedCost * tQty, App.ActiveCorp.RoundAmtAt);
                tYokMaAmt = tYokMaAmt + (ioSedCost * tQty);
            }

            if (tCrSedSkid == string.Empty) 	// ไม่มีรายการด้านรับเข้าก่อน inBegDate เลย
            {
                bool llHasData = false;
                DateTime ldDate = inBegDate;
                lcFOXSQLStr = lcSeleFieldStr;
                //lcFOXSQLStr = lcFOXSQLStr + " from RefProd where RefProd.fcProd = @xaProd and RefProd.fcBranch = @xaBranch and RefProd.fdDate between @xaBegDate and @xaEndDate ";
                lcFOXSQLStr = lcFOXSQLStr + " from RefProd ";
                lcFOXSQLStr = lcFOXSQLStr + " left join GLRef on GLRef.fcSkid = RefProd.fcGLRef ";
                lcFOXSQLStr = lcFOXSQLStr + " left join Prod on Prod.fcSkid = RefProd.fcProd ";
                lcFOXSQLStr = lcFOXSQLStr + " left join PdGrp on PdGrp.fcSkid = Prod.fcPdGrp ";
                lcFOXSQLStr = lcFOXSQLStr + " left join UM on UM.fcSkid = Prod.fcUM ";
                lcFOXSQLStr = lcFOXSQLStr + " left join Sect on Sect.fcSkid = RefProd.fcSect ";
                lcFOXSQLStr = lcFOXSQLStr + " left join Job on Job.fcSkid = RefProd.fcJob ";
                lcFOXSQLStr = lcFOXSQLStr + " left join Book on Book.fcSkid = GLRef.fcBook ";
                lcFOXSQLStr = lcFOXSQLStr + " left join WHOUSE on WHOUSE.fcSkid = RefProd.fcWHouse ";
                lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @xaProd and RefProd.fcBranch = @xaBranch and RefProd.fdDate between @xaBegDate and @xaEndDate ";
                //if (this.txtTagWHouse.Text.Trim() != string.Empty)
                //{
                //    lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcCode in ( " + this.txtTagWHouse.Text.Trim() + " ) ";
                //}
                lcFOXSQLStr = lcFOXSQLStr + " and RefProd.fcStat <> 'C' ";
                lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcType = ' ' ";
                lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fcProd ,RefProd.fcBranch ,RefProd.fdDate , RefProd.fcTimeStam , RefProd.fcIOType , RefProd.fcGLRef , RefProd.fcSkid ";

                cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
                cmd.CommandTimeout = 2000;

                int cmpDate = ldDate.CompareTo(inBegDate);
                while (llHasData == false
                    && cmpDate < 1)
                {

                    cmpDate = ldDate.CompareTo(inBegDate);

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@xaProd", inProd);
                    cmd.Parameters.AddWithValue("@xaBranch", inBranch);
                    cmd.Parameters.AddWithValue("@xaBegDate", ldDate);
                    cmd.Parameters.AddWithValue("@xaEndDate", ldDate);

                    dtrRefProd = cmd.ExecuteReader();
                    if (dtrRefProd.HasRows)
                    {
                        while (dtrRefProd.Read())
                        {
                            if (VRefProd2(dtrRefProd["fcRfType"].ToString(), dtrRefProd["fcIOType"].ToString(), dtrRefProd["fcStat"].ToString(), Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"])))
                            {
                                decimal tAbsQty = Math.Abs(Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"]));
                                decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrRefProd["fnCostAmt"]) + Convert.ToDecimal(dtrRefProd["fnCostAdj"]));
                                tFstYokMaDate = Convert.ToDateTime(dtrRefProd["fdDate"]);
                                tCrSedSkid = dtrRefProd["fcSkid"].ToString();
                                ioSedQty = QtySeq1(tCostAmt, tAbsQty);
                                ioSedQty2 = tAbsQty - ioSedQty;
                                ioSedCost = CostSeq1(tCostAmt, tAbsQty);
                                llHasData = true;

                                string strCDate = Convert.ToDateTime(dtrRefProd["fdDate"]).Year.ToString("0000") + Convert.ToDateTime(dtrRefProd["fdDate"]).ToString("MMdd") + dtrRefProd["fcRefNo"].ToString() + dtrRefProd["fcSkid"].ToString();
                                DataRow dtrTemFIFO = dtsDataEnv.Tables[mstrTemBFFIFO].NewRow();
                                dtrTemFIFO["IsBF"] = "Y";
                                dtrTemFIFO["QcBook"] = dtrRefProd["QcBook"].ToString();
                                dtrTemFIFO["fcRfType"] = dtrRefProd["fcRfType"].ToString();
                                dtrTemFIFO["fcRefType"] = dtrRefProd["fcRefType"].ToString();
                                dtrTemFIFO["fcRefNo"] = dtrRefProd["fcRefNo"].ToString();
                                dtrTemFIFO["fdDate"] = Convert.ToDateTime(dtrRefProd["fdDate"]);
                                dtrTemFIFO["cDate"] = strCDate;
                                dtrTemFIFO["QcPdGrp"] = dtrRefProd["QcPdGrp"].ToString();
                                dtrTemFIFO["QnPdGrp"] = dtrRefProd["QnPdGrp"].ToString();
                                dtrTemFIFO["QcProd"] = dtrRefProd["QcProd"].ToString();
                                dtrTemFIFO["QnProd"] = dtrRefProd["QnProd"].ToString();
                                dtrTemFIFO["QnUM"] = dtrRefProd["QnUM"].ToString();
                                dtrTemFIFO["fcIOType"] = dtrRefProd["fcIOType"].ToString();
                                dtrTemFIFO["fnQty"] = ioSedQty;
                                dtrTemFIFO["fnPrice"] = ioSedCost;
                                dtrTemFIFO["fnCostAmt"] = (ioSedQty * ioSedCost);

                                //dtsDataEnv.Tables[mstrTemBFFIFO].Rows.Add(dtrTemFIFO);

                                break;
                            }
                        }
                    }
                    ldDate = ldDate.AddDays(1);
                    dtrRefProd.Close();
                }
            }
            ioAmt = tYokMaAmt;

            //}
            //catch (Exception ex)
            //{
            //    string strErrorMsg = ex.Message;
            //}
            //finally
            //{
            conn.Close();
            //}

        }

        private void P1Line(SqlDataReader inSource, decimal inQty, decimal inCost, decimal inAmt, string inSedRefNo)
        {

            DataRow dtrTemFIFO = dtsDataEnv.Tables[mstrTemBFFIFO].NewRow();

            dtrTemFIFO["QcBook"] = inSource["QcBook"].ToString();
            dtrTemFIFO["SedRefNo"] = inSedRefNo;
            dtrTemFIFO["fcRfType"] = inSource["fcRfType"].ToString();
            dtrTemFIFO["fcRefType"] = inSource["fcRefType"].ToString();
            dtrTemFIFO["fcRefNo"] = inSource["fcRefNo"].ToString();
            dtrTemFIFO["fdDate"] = Convert.ToDateTime(inSource["fdDate"]);
            dtrTemFIFO["cDate"] = Convert.ToDateTime(inSource["fdDate"]).Year.ToString("0000") + Convert.ToDateTime(inSource["fdDate"]).ToString("MMdd") + inSource["fcRefNo"].ToString() + inSource["fcSkid"].ToString();
            dtrTemFIFO["QcPdGrp"] = inSource["QcPdGrp"].ToString();
            dtrTemFIFO["QnPdGrp"] = inSource["QnPdGrp"].ToString();
            dtrTemFIFO["QcProd"] = inSource["QcProd"].ToString();
            dtrTemFIFO["QnProd"] = inSource["QnProd"].ToString();
            dtrTemFIFO["QnUM"] = inSource["QnUM"].ToString();
            dtrTemFIFO["QcSect"] = inSource["QcSect"].ToString();
            dtrTemFIFO["QnSect"] = inSource["QnSect"].ToString();
            dtrTemFIFO["QcJob"] = inSource["QcJob"].ToString();
            dtrTemFIFO["QnJob"] = inSource["QnJob"].ToString();
            dtrTemFIFO["fcIOType"] = inSource["fcIOType"].ToString();
            dtrTemFIFO["fnQty"] = inQty;
            dtrTemFIFO["fnUMQty"] = 1;
            dtrTemFIFO["fnPrice"] = inCost;
            dtrTemFIFO["fnCostAmt"] = inAmt;

            dtsDataEnv.Tables[mstrTemBFFIFO].Rows.Add(dtrTemFIFO);

        }

        private void SumSed(ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo)
        {
            DataRow[] dtrTemBFFIFO = dtsDataEnv.Tables[mstrTemBFFIFO].Select("fcIOType = 'I' ", "cDate");
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables["XRefProd"].Select("fcIOType = 'I' ");
            ioSedQty = 0;
            ioSedCost = 0;
            bool tFstIn = true;
            int intCurrRecn = 0;
            if (ioSedRecn >= 0 && ioSedRecn < dtrTemBFFIFO.Length)
            {
                intCurrRecn = ioSedRecn;
                if (ioSedQty2 > 0)
                {
                    ioSedQty = ioSedQty2;
                    decimal decBFAmt = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAmt"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAdj"]));
                    decimal decBFQty = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"]));
                    ioSedCost = CostSeq2(decBFAmt, decBFQty);
                    tFstIn = false;
                }
                intCurrRecn++;
            }
            ioSedQty2 = 0;
            if (ioSedQty == 0) ioSedRecn = 0;

            while (intCurrRecn < dtrTemBFFIFO.Length)
            {
                decimal tAbsQty = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) * Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"]));
                decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAmt"]) + Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnCostAdj"]));
                decimal tQty1 = QtySeq1(tCostAmt, tAbsQty);
                decimal tCost1 = CostSeq1(tCostAmt, tAbsQty);
                decimal tCost2 = CostSeq2(tCostAmt, tAbsQty);
                if (tFstIn)
                {
                    tFstIn = false;
                    ioSedCost = tCost1;
                }
                if (ioSedCost == tCost1)
                {
                    ioSedQty = ioSedQty + tQty1;
                    ioSedRecn = intCurrRecn;
                    ioSedRefNo = dtrTemBFFIFO[intCurrRecn]["cDate"].ToString();
                    if ((tCost1 != tCost2) || (Math.Abs(tQty1) != tAbsQty))
                    {
                        ioSedQty2 = Math.Abs(Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnQty"]) * Convert.ToDecimal(dtrTemBFFIFO[intCurrRecn]["fnUmQty"])) - tQty1;
                        break;
                    }

                }
                else
                {
                    break;
                }
                intCurrRecn++;
            }
        }

        private static bool VRefProd2(string inRfType, string inIOType, string inStat, decimal inQty)
        {
            inQty = inQty * (inIOType == "I" ? 1 : -1);
            int intSearch = xaXtraType2.IndexOf(inRfType);
            return inQty >= 0
                && inStat != "C"
                && (!(intSearch > -1) || false);
        }

        private static bool VRefProd3(string inRfType, string inIOType)
        {
            int intSearch = xaXtraType.IndexOf(inRfType);
            return (!(intSearch > -1)
                || (inRfType == "W" && inIOType == "O")
                || (inRfType == "G" && inIOType == "I")
                || (inRfType == "d" && inIOType == "I"));
        }

        private static decimal QtySeq1(decimal inCostAmt, decimal inQty)
        {
            //decimal tAmt = Math.Round(inCostAmt, App.ActiveCorp.RoundAmtAt);
            decimal tAmt = (inCostAmt);
            decimal tCost = CostSeq1(inCostAmt, inQty);
            //decimal tQty = Math.Round(inQty, 4);
            decimal tQty = inQty;
            return tQty;
        }

        private static decimal CostSeq1(decimal inCostAmt, decimal inQty)
        {
            //return Math.Round(Math.Round(inCostAmt, App.ActiveCorp.RoundAmtAt) / inQty, App.ActiveCorp.RoundPriceAt);
            return (inQty != 0 ? inCostAmt / inQty : 0);
        }

        private static decimal CostSeq2(decimal inCostAmt, decimal inQty)
        {
            //decimal tAmt = Math.Round(inCostAmt, App.ActiveCorp.RoundAmtAt);
            //decimal tCost2 = Math.Round(tAmt / inQty, App.ActiveCorp.RoundPriceAt);
            decimal tAmt = inCostAmt;
            decimal tCost2 = (inQty != 0 ? tAmt / inQty : 0);
            return tCost2;
        }


	}
}
