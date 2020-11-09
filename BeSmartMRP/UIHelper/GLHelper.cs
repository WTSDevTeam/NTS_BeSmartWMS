
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Windows.Forms;

using BeSmartMRP.Report.Agents;

namespace BeSmartMRP.UIHelper
{
    public class GLHelper
    {

        public GLHelper() { }

        public static DateTime xd_MIN_CAL_DATE = new DateTime(1990, 1, 1);

        private static string mstrAcChartSQLStr = "";
        private static string mstrAcChartCode = "";

        public static DataSet dtsDataEnv = new DataSet();

        private static string mstrTemBFFIFO = "TemBFFIFO";
        private static string mstrTemBalStock = "TemBalStock";
        private static string mstrTemPd = "TemPd";
        private static string mstrTemFIFOAmt = "TemFIFOAmt";

        public static decimal SumRngGL(string inBegAcChart, string inEndAcChart, DateTime inBegdate, DateTime inEnddate, string inBranch, string inSect, string inQcSect, bool inCalBF, DateTime inCorpCalDate, ref decimal[] ioMthVal)
        {
            decimal[] decVal = new decimal[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            decimal decSumAmt = 0;
            OleDbConnection conn = new OleDbConnection(App.ERPConnectionString);
            OleDbCommand cmd = new OleDbCommand();

            string strSQLStr = "SELECT ACCHART.FCCODE FROM ACCHART WHERE ACCHART.FCCORPCHAR = ? and ACCHART.FCCODE between ? and ? and ACCHART.FCCATEG2 = 'DETAIL' order by ACCHART.FCCODE";

            cmd.Parameters.Add(ReportHelper.AddParameter(App.ActiveCorp.CorpChar, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(inBegAcChart, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(inEndAcChart, DbType.String));

            cmd.Connection = conn;
            cmd.CommandText = strSQLStr;
            cmd.CommandType = CommandType.Text;
            decimal decSumAcc = 0;

            try
            {
                conn.Open();
                cmd.CommandTimeout = 2000;
                string strQcAcChart = "";
                System.Data.OleDb.OleDbDataReader dtrAcChart = cmd.ExecuteReader();
                if (dtrAcChart.HasRows)
                {
                    int intDiff = Convert.ToInt32(DateTimeUtil.DateHelper.xDateDiff(inBegdate, inEnddate, "MONTH"));
                    DateTime dttBegDate = inBegdate;
                    DateTime dttEndDate = inEnddate;
                    while (dtrAcChart.Read())
                    {
                        strQcAcChart = dtrAcChart["fcCode"].ToString().TrimEnd();
                        GLHelper.mstrAcChartSQLStr = "( SELECT FCSKID FROM ACCHART WHERE ACCHART.FCCORPCHAR = ? AND ACCHART.FCCODE LIKE ? )";
                        GLHelper.mstrAcChartCode = strQcAcChart.Trim() + "%";
                        decSumAcc += pmSumRngGL(inBegdate, inEnddate, inBranch, inSect, inQcSect, ref decVal);
                    }
                }
                dtrAcChart.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            ioMthVal = decVal;
            return decSumAcc;
        }

        public static decimal SumGL(string inAcCode, DateTime inBegdate, DateTime inEnddate, string inBranch, string inSect, string inQcSect, bool inCalBF, DateTime inCorpCalDate)
        {
            decimal decSumAmt = 0;
            GLHelper.mstrAcChartSQLStr = "( SELECT FCSKID FROM ACCHART WHERE ACCHART.FCCORPCHAR = ? AND ACCHART.FCCODE LIKE ? )";
            GLHelper.mstrAcChartCode = inAcCode.Trim() + "%";

            bool bllIsCalBF = (inCorpCalDate.Date.CompareTo(inBegdate.Date) >= 0);
            if (inCalBF || bllIsCalBF)
            {
                decSumAmt += GLHelper.pmSumAGL(inCorpCalDate, inBranch, inSect, inQcSect);
            }
            decSumAmt += GLHelper.pmSumGL(inBegdate, inEnddate, inBranch, inSect, inQcSect);
            return decSumAmt;
        }

        public static decimal SumAcBudget(string inAcCode, DateTime inBegdate, DateTime inEnddate, string inBranch, string inSect, string inQcSect)
        {
            decimal decSumAmt = 0;
            GLHelper.mstrAcChartSQLStr = "( SELECT FCSKID FROM ACCHART WHERE ACCHART.FCCORPCHAR = ? AND ACCHART.FCCODE LIKE ? )";
            GLHelper.mstrAcChartCode = inAcCode.Trim() + "%";

            decSumAmt = GLHelper.pmSumAcBudget(inBegdate, inEnddate, inBranch, inSect, inQcSect);
            //bool bllIsCalBF = (inCorpCalDate.Date.CompareTo(inBegdate.Date) >= 0);
            //if (inCalBF || bllIsCalBF)
            //{
            //    decSumAmt += GLHelper.pmSumAGL(inCorpCalDate, inBranch, inSect);
            //}
            return decSumAmt;
        }

        private static bool mbllAddClose = true;
        public static bool IsAddCloseGL
        {
            get { return mbllAddClose; }
            set { mbllAddClose = value; }
        }

        public static decimal SumGL2(string inAcCode, DateTime inBegdate, DateTime inEnddate, string inBranch, string inSect, bool inCalBF, DateTime inCorpCalDate, bool inAddClose)
        {
            decimal decSumAmt = 0;
            GLHelper.mstrAcChartSQLStr = "( SELECT FCSKID FROM ACCHART WHERE ACCHART.FCCORPCHAR = ? AND ACCHART.FCCODE = ? )";
            GLHelper.mstrAcChartCode = inAcCode.Trim();
            GLHelper.mbllAddClose = inAddClose;

            bool bllIsCalBF = (inCorpCalDate.Date.CompareTo(inBegdate.Date) > 0);
            if (inCalBF || bllIsCalBF)
            {
                decSumAmt += GLHelper.pmSumAGL(inCorpCalDate, inBranch, inSect, "");
            }
            decSumAmt += GLHelper.pmSumGL(inBegdate, inEnddate, inBranch, inSect, "");
            return decSumAmt;
        }

        private static decimal pmSumAcBudget(DateTime inBegDate, DateTime inEndDate, string inBranch, string inSect, string inQcSect)
        {
            decimal decSumAmt = 0;
            OleDbConnection conn = new OleDbConnection(App.ERPConnectionString);
            OleDbCommand cmd = new OleDbCommand();

            if (inEndDate.Year < inBegDate.Year)
                return 0;

            string strSumFld = "";
            strSumFld += "SUM( ACBUDGET.FNAMT01 ) AS AMT01";
            strSumFld += ",SUM( ACBUDGET.FNAMT02 ) AS AMT02";
            strSumFld += ",SUM( ACBUDGET.FNAMT03 ) AS AMT03";
            strSumFld += ",SUM( ACBUDGET.FNAMT04 ) AS AMT04";
            strSumFld += ",SUM( ACBUDGET.FNAMT05 ) AS AMT05";
            strSumFld += ",SUM( ACBUDGET.FNAMT06 ) AS AMT06";
            strSumFld += ",SUM( ACBUDGET.FNAMT07 ) AS AMT07";
            strSumFld += ",SUM( ACBUDGET.FNAMT08 ) AS AMT08";
            strSumFld += ",SUM( ACBUDGET.FNAMT09 ) AS AMT09";
            strSumFld += ",SUM( ACBUDGET.FNAMT10 ) AS AMT10";
            strSumFld += ",SUM( ACBUDGET.FNAMT11 ) AS AMT11";
            strSumFld += ",SUM( ACBUDGET.FNAMT12 ) AS AMT12";

            string strSQLStr = "SELECT " + strSumFld + " FROM ACBUDGET WHERE ACBUDGET.FCCORP = ? AND ACBUDGET.FCYEAR = ? ";

            cmd.Parameters.Add(ReportHelper.AddParameter(App.ActiveCorp.RowID, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(inBegDate.Year.ToString("0000"), DbType.String));
            if (inBranch != string.Empty)
            {
                strSQLStr += " AND ACBUDGET.FCBRANCH = ? ";
                cmd.Parameters.Add(ReportHelper.AddParameter(inBranch, DbType.String));
            }

            if (inQcSect != string.Empty)
            {
                strSQLStr += " AND ACBUDGET.FCSECT in (select FCSKID from SECT where FCCODE like ?) ";
                cmd.Parameters.Add(ReportHelper.AddParameter(inQcSect + "%", DbType.String));
            }
            else if (inSect != string.Empty)
            {
                strSQLStr += " AND ACBUDGET.FCSECT = ? ";
                cmd.Parameters.Add(ReportHelper.AddParameter(inSect, DbType.String));
            }

            strSQLStr += " AND ACBUDGET.FCACCHART in " + GLHelper.mstrAcChartSQLStr;
            cmd.Parameters.Add(ReportHelper.AddParameter(App.ActiveCorp.CorpChar, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(GLHelper.mstrAcChartCode, DbType.String));

            cmd.Connection = conn;
            cmd.CommandText = strSQLStr;
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                cmd.CommandTimeout = 2000;

                System.Data.OleDb.OleDbDataReader dtrGL = cmd.ExecuteReader();
                if (dtrGL.HasRows)
                {
                    int intMDiff = 1 + Convert.ToInt32(DateTimeUtil.DateHelper.xDateDiff(inBegDate, inEndDate, "MONTH"));
                    int intMStart = inBegDate.Month;
                    int intMEnd = inEndDate.Month + 1;
                    while (dtrGL.Read())
                    {
                        string strSumFld2 = "";
                        for (int intCnt = intMStart; intCnt < intMEnd; intCnt++)
                        {
                            strSumFld2 = "AMT" + intCnt.ToString("00");
                            decSumAmt += (Convert.IsDBNull(dtrGL[strSumFld2]) ? 0 : Convert.ToDecimal(dtrGL[strSumFld2]));
                        }
                    }
                }
                dtrGL.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return decSumAmt;
        }

        private static decimal pmSumAGL(DateTime inCalAtDate, string inBranch, string inSect, string inQcSect)
        {
            decimal decSumAmt = 0;
            OleDbConnection conn = new OleDbConnection(App.ERPConnectionString);
            OleDbCommand cmd = new OleDbCommand();

            string strSQLStr = "SELECT SUM( AGL.FNAMT0 ) AS FNSUMAMT0 FROM AGL WHERE AGL.FCCORP = ? AND AGL.FDDATE = ? ";

            cmd.Parameters.Add(ReportHelper.AddParameter(App.ActiveCorp.RowID, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(inCalAtDate, DbType.DateTime));
            if (inBranch != string.Empty)
            {
                strSQLStr += " AND AGL.FCBRANCH = ? ";
                cmd.Parameters.Add(ReportHelper.AddParameter(inBranch, DbType.String));
            }

            if (inQcSect != string.Empty)
            {
                strSQLStr += " AND AGL.FCSECT in (select FCSKID from SECT where FCCODE like ?) ";
                cmd.Parameters.Add(ReportHelper.AddParameter(inQcSect + "%", DbType.String));
            }
            else if (inSect != string.Empty)
            {
                strSQLStr += " AND AGL.FCSECT = ? ";
                cmd.Parameters.Add(ReportHelper.AddParameter(inSect, DbType.String));
            }

            strSQLStr += " AND AGL.FCACCHART in " + GLHelper.mstrAcChartSQLStr;
            cmd.Parameters.Add(ReportHelper.AddParameter(App.ActiveCorp.CorpChar, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(GLHelper.mstrAcChartCode, DbType.String));

            cmd.Connection = conn;
            cmd.CommandText = strSQLStr;
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                cmd.CommandTimeout = 2000;

                System.Data.OleDb.OleDbDataReader dtrGL = cmd.ExecuteReader();
                if (dtrGL.HasRows)
                {
                    while (dtrGL.Read())
                    {
                        decSumAmt = (Convert.IsDBNull(dtrGL["fnSumAmt0"]) ? 0 : Convert.ToDecimal(dtrGL["fnSumAmt0"]));
                    }
                }
                dtrGL.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return decSumAmt;
        }

        private static decimal pmSumGL(DateTime inBegDate, DateTime inEndDate, string inBranch, string inSect, string inQcSect)
        {
            decimal decSumAmt = 0;
            OleDbConnection conn = new OleDbConnection(App.ERPConnectionString);
            OleDbCommand cmd = new OleDbCommand();

            string strSQLStr = "SELECT SUM( GL.FNAMT ) AS FNSUMAMT FROM GL WHERE GL.FCCORP = ? AND GL.FCACCHART IN " + GLHelper.mstrAcChartSQLStr;
            if (GLHelper.mbllAddClose)
                strSQLStr += " AND GL.FDDATE BETWEEN ? AND ? ";
            //strSQLStr += " AND GL.FDDATE > ? AND GL.FDDATE < ? ";
            else
                //strSQLStr += " AND GL.FDDATE > ? AND GL.FDDATE < ? AND FCCLOSED <> 'C' ";
                strSQLStr += " AND GL.FDDATE BETWEEN ? AND ? AND FCCLOSED <> 'C' ";

            //cmd.Parameters.Add(ReportHelper.AddParameter(App.ActiveCorp.RowID, DbType.String));
            //cmd.Parameters.Add(ReportHelper.AddParameter(App.ActiveCorp.RowID, DbType.String));

            cmd.Parameters.Add(ReportHelper.AddParameter(App.ActiveCorp.RowID, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(App.ActiveCorp.CorpChar, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(GLHelper.mstrAcChartCode, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(inBegDate, DbType.DateTime));
            cmd.Parameters.Add(ReportHelper.AddParameter(inEndDate, DbType.DateTime));

            //cmd.Parameters.Add(ReportHelper.AddParameter(inBegDate.AddDays(-1), DbType.DateTime));
            //cmd.Parameters.Add(ReportHelper.AddParameter(inEndDate.AddDays(1), DbType.DateTime));

            if (inBranch != string.Empty)
            {
                strSQLStr += " AND GL.FCBRANCH = ? ";
                cmd.Parameters.Add(ReportHelper.AddParameter(inBranch, DbType.String));
            }

            if (inQcSect != string.Empty)
            {
                strSQLStr += " AND GL.FCSECT in (select FCSKID from SECT where FCCODE like ?) ";
                cmd.Parameters.Add(ReportHelper.AddParameter(inQcSect + "%", DbType.String));
            }
            else if (inSect != string.Empty)
            {
                strSQLStr += " AND GL.FCSECT = ? ";
                cmd.Parameters.Add(ReportHelper.AddParameter(inSect, DbType.String));
            }

            cmd.Connection = conn;
            cmd.CommandText = strSQLStr;
            cmd.CommandType = CommandType.Text;

            try
            {
                conn.Open();
                cmd.CommandTimeout = 2000;

                System.Data.OleDb.OleDbDataReader dtrGL = cmd.ExecuteReader();
                if (dtrGL.HasRows)
                {
                    while (dtrGL.Read())
                    {
                        decSumAmt = (Convert.IsDBNull(dtrGL["fnSumAmt"]) ? 0 : Convert.ToDecimal(dtrGL["fnSumAmt"]));
                    }
                }
                dtrGL.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return decSumAmt;
        }

        private static decimal pmSumRngGL(DateTime inBegDate, DateTime inEndDate, string inBranch, string inSect, string inQcSect, ref decimal[] ioRngVal)
        {
            decimal decSumAmt = 0;
            OleDbConnection conn = new OleDbConnection(App.ERPConnectionString);
            OleDbCommand cmd = new OleDbCommand();

            string strSQLStr = "SELECT month(GL.FDDATE) as nMonth, SUM( GL.FNAMT ) AS FNSUMAMT FROM GL WHERE GL.FCCORP = ? AND GL.FCACCHART IN " + GLHelper.mstrAcChartSQLStr;
            if (GLHelper.mbllAddClose)
                strSQLStr += " AND GL.FDDATE BETWEEN ? AND ? ";
            //strSQLStr += " AND GL.FDDATE > ? AND GL.FDDATE < ? ";
            else
                //strSQLStr += " AND GL.FDDATE > ? AND GL.FDDATE < ? AND FCCLOSED <> 'C' ";
                strSQLStr += " AND GL.FDDATE BETWEEN ? AND ? AND FCCLOSED <> 'C' ";

            //cmd.Parameters.Add(ReportHelper.AddParameter(App.ActiveCorp.RowID, DbType.String));
            //cmd.Parameters.Add(ReportHelper.AddParameter(App.ActiveCorp.RowID, DbType.String));

            cmd.Parameters.Add(ReportHelper.AddParameter(App.ActiveCorp.RowID, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(App.ActiveCorp.CorpChar, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(GLHelper.mstrAcChartCode, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(inBegDate, DbType.DateTime));
            cmd.Parameters.Add(ReportHelper.AddParameter(inEndDate, DbType.DateTime));

            //cmd.Parameters.Add(ReportHelper.AddParameter(inBegDate.AddDays(-1), DbType.DateTime));
            //cmd.Parameters.Add(ReportHelper.AddParameter(inEndDate.AddDays(1), DbType.DateTime));

            if (inBranch != string.Empty)
            {
                strSQLStr += " AND GL.FCBRANCH = ? ";
                cmd.Parameters.Add(ReportHelper.AddParameter(inBranch, DbType.String));
            }

            //if (inSect != string.Empty)
            //{
            //    strSQLStr += " AND GL.FCSECT = ? ";
            //    cmd.Parameters.Add(ReportHelper.AddParameter(inSect, DbType.String));
            //}

            if (inQcSect != string.Empty)
            {
                strSQLStr += " AND GL.FCSECT in (select FCSKID from SECT where FCCODE like ?) ";
                cmd.Parameters.Add(ReportHelper.AddParameter(inQcSect + "%", DbType.String));
            }
            else if (inSect != string.Empty)
            {
                strSQLStr += " AND GL.FCSECT = ? ";
                cmd.Parameters.Add(ReportHelper.AddParameter(inSect, DbType.String));
            }

            strSQLStr += " group by month(GL.FDDATE) ";

            cmd.Connection = conn;
            cmd.CommandText = strSQLStr;
            cmd.CommandType = CommandType.Text;

            try
            {
                conn.Open();
                cmd.CommandTimeout = 2000;

                System.Data.OleDb.OleDbDataReader dtrGL = cmd.ExecuteReader();
                if (dtrGL.HasRows)
                {
                    while (dtrGL.Read())
                    {
                        int intGLMonth = Convert.ToInt32(dtrGL["nMonth"]);
                        decSumAmt = (Convert.IsDBNull(dtrGL["fnSumAmt"]) ? 0 : Convert.ToDecimal(dtrGL["fnSumAmt"]));
                        ioRngVal[intGLMonth - 1] = Convert.ToDecimal(ioRngVal[intGLMonth - 1]) + decSumAmt;
                    }
                }
                dtrGL.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return decSumAmt;
        }



        public static void CalDepr(OleDbDataReader inAsset, DateTime inBegDate, DateTime inEndDate, ref bool ioToPrint, ref decimal ioDeprAmt, ref decimal ioAsBuyVal, ref decimal ioAsValBeg, ref decimal ioAsValEnd, ref decimal ioAccmDepr, ref bool ioHasChgAsInRngDate, int inBegMth, ref decimal[] ioMthAmt)
        {
            GLHelper.pmCalDepr(inAsset, inBegDate, inEndDate, ref ioToPrint, ref ioDeprAmt, ref ioAsBuyVal, ref ioAsValBeg, ref ioAsValEnd, ref ioAccmDepr, ref ioHasChgAsInRngDate, inBegMth, ref ioMthAmt);
        }

        private static void pmCalDepr(OleDbDataReader inAsset, DateTime inBegDate, DateTime inEndDate, ref bool ioToPrint, ref decimal ioDeprAmt, ref decimal ioAsBuyVal, ref decimal ioAsValBeg, ref decimal ioAsValEnd, ref decimal ioAccmDepr, ref bool ioHasChgAsInRngDate, int inBegMth, ref decimal[] ioMthAmt)
        {

            ioToPrint = true;
            OleDbConnection conn = new OleDbConnection(App.ERPConnectionString);
            OleDbCommand cmd = new OleDbCommand();
            OleDbCommand cmd2 = new OleDbCommand();

            //string strEndYrMth = inEndDate.AddMonths(-1).ToString("yyyyMM");
            //string strEndYrMth = inEndDate.AddMonths(-1).Year.ToString("0000") + inEndDate.AddMonths(-1).Month.ToString("00");
            string strEndYrMth = inEndDate.Year.ToString("0000") + inEndDate.Month.ToString("00");

            string strSQLStr = "select fcAsset,fcYr,fcMth,fnDepramt  from AssetDpr where AssetDpr.fcAsset = ? and AssetDpr.fcYr+AssetDpr.fcMth >= ? order by AssetDpr.fcAsset , AssetDpr.fcYr , AssetDpr.fcMth";

            cmd.Parameters.Add(ReportHelper.AddParameter(inAsset["fcSkid"].ToString(), DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(inBegDate.Year.ToString("0000") + inBegDate.Month.ToString("00"), DbType.String));
            //cmd.Parameters.Add(ReportHelper.AddParameter(inBegDate.ToString("yyyyMM"), DbType.String));

            cmd.Connection = conn;
            cmd.CommandText = strSQLStr.ToUpper();
            cmd.CommandType = CommandType.Text;

            try
            {
                conn.Open();
                cmd.CommandTimeout = 2000;

                //tNextDeprAmt = ค่าเสื่อมที่หักหลังจากเดือนสุดท้ายที่สั่งพิมพ์
                decimal tNextDeprAmt = 0;
                System.Data.OleDb.OleDbDataReader dtrAsDpr = cmd.ExecuteReader();
                if (dtrAsDpr.HasRows)
                {
                    while (dtrAsDpr.Read())
                    {
                        string strAssYr = dtrAsDpr["fcYr"].ToString() + dtrAsDpr["fcMth"].ToString();
                        if (strAssYr.CompareTo(strEndYrMth) <= 0)
                        {
                            ioDeprAmt += Convert.ToDecimal(dtrAsDpr["fnDeprAmt"]);
                            int intMth = Convert.ToInt32(dtrAsDpr["fcMth"]) - inBegMth;
                            if (intMth <= ioMthAmt.Length)
                            {
                                ioMthAmt[intMth] += Convert.ToDecimal(dtrAsDpr["fnDeprAmt"]);
                            }
                        }
                        else
                        {
                            tNextDeprAmt += Convert.ToDecimal(dtrAsDpr["fnDeprAmt"]);
                        }
                    }
                }
                dtrAsDpr.Close();

                //20/02/39 คุณสุวิมล บ. เชโก้ บอกว่าสินทรัพย์ที่ขายไปในช่วงวันที่ที่สั่งพิมพ์รายงานต้องแสดงด้วย
                if (ioDeprAmt == 0
                    && Convert.ToDateTime(inAsset["fdBuyDate"]).CompareTo(inEndDate) > 0
                    || (Convert.IsDBNull(inAsset["fdSaleDate"]) == false
                    && Convert.ToDateTime(inAsset["fdSaleDate"]).CompareTo(inBegDate) < 0))
                {
                    ioToPrint = false;
                }

                ioAsBuyVal = Convert.ToDecimal(inAsset["fnGrossPri"]);
                ioHasChgAsInRngDate = false;

                string strSQLStr2 = "select * from ChgAsAmt where ChgAsAmt.fcAsset = ? and ChgAsAmt.fdDate <= ? order by ChgAsAmt.fcAsset , ChgAsAmt.fdDate";
                cmd2.Parameters.Add(ReportHelper.AddParameter(inAsset["fcSkid"].ToString(), DbType.String));
                cmd2.Parameters.Add(ReportHelper.AddParameter(inEndDate, DbType.DateTime));

                cmd2.Connection = conn;
                cmd2.CommandText = strSQLStr2.ToUpper();
                cmd2.CommandType = CommandType.Text;
                cmd2.CommandTimeout = 2000;

                System.Data.OleDb.OleDbDataReader dtrChgAsAmt = cmd2.ExecuteReader();
                if (dtrChgAsAmt.HasRows)
                {
                    while (dtrChgAsAmt.Read())
                    {
                        ioAsBuyVal += Convert.ToDecimal(dtrChgAsAmt["fnAmt"]);
                        if (Convert.ToDateTime(dtrChgAsAmt["fdDate"]).CompareTo(inBegDate) > 0)
                        {
                            ioHasChgAsInRngDate = true;
                        }
                    }
                }
                dtrChgAsAmt.Close();

                //Asset.fnPreYrDep = ค่าเสื่อมสะสมก่อนวันเริ่มใช้ระบบ
                //Asset.fnAccmDepr = ค่าเสื่อมสะสมตั้งแต่เริ่มใช้ระบบ จนถึงเดือนสุดท้ายที่สั่งคำนวณค่าเสื่อม
                //ioAsValBeg = มูลค่าต้นทุน ในวันสุดท้ายของรอบระยะบัญชีก่อน (มูลค่า ณ วันที่เริ่มพิมพ์)
                ioAsValBeg = ioAsBuyVal - Convert.ToDecimal(inAsset["fnPreYrDep"]) - (Convert.ToDecimal(inAsset["fnAccmDepr"]) - ioDeprAmt - tNextDeprAmt);

                //ioAsValEnd = มูลค่าต้นทุนหลังจากหักค่าเสื่อมราคาแล้วจนถึงปัจจุบัน
                if (Convert.IsDBNull(inAsset["fdSaleDate"]) == false
                    && Convert.ToDateTime(inAsset["fdSaleDate"]).CompareTo(inBegDate) >= 0
                    && Convert.ToDateTime(inAsset["fdSaleDate"]).CompareTo(inEndDate) < 0)
                {
                    ioAsValEnd = 0;
                }
                else
                {
                    ioAsValEnd = ioAsValBeg - ioDeprAmt;
                }

                //ioAccmDepr = ค่าเสื่อมราคาสะสมจนถึงปัจจุบัน
                ioAccmDepr = ioAsBuyVal - ioAsValBeg + ioDeprAmt;

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

        }

        public static DateTime GetCloseDate(string inCorp, string inBranch, DateTime inCalAtDate)
        {
            DateTime dttCloseDate = App.ActiveCorp.StartAppDate;
            OleDbConnection conn = new OleDbConnection(App.ERPConnectionString);
            OleDbCommand cmd = new OleDbCommand();

            string strSQLStr = "select GLHead.fcCorp , GLHead.fcBranch , GLHead.fcClosed , GLHead.fdDate from GLHead where GLHead.fcCorp = ? and GLHead.fcBranch = ? and GLHead.fcClosed = 'C' and GLHead.fdDate <= ?";
            strSQLStr += " order by GLHead.fcCorp,GLHead.fcBranch,GLHead.fcClosed,GLHead.fdDate desc";

            cmd.Parameters.Add(ReportHelper.AddParameter(inCorp, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(inBranch, DbType.String));
            cmd.Parameters.Add(ReportHelper.AddParameter(inCalAtDate.AddDays(-1), DbType.DateTime));

            cmd.Connection = conn;
            cmd.CommandText = strSQLStr.ToUpper();
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                cmd.CommandTimeout = 2000;

                System.Data.OleDb.OleDbDataReader dtrGL = cmd.ExecuteReader();
                if (dtrGL.HasRows)
                {
                    while (dtrGL.Read())
                    {
                        //จริง ๆ FORMA ตั้งใจเป็นวันที่ + 1 แต่ BUG เลยต้องยึดตามเลข FORMA ชั่วคราว
                        //dttCloseDate = Convert.ToDateTime(dtrGL["fdDate"]).AddDays(1);
                        dttCloseDate = Convert.ToDateTime(dtrGL["fdDate"]);
                    }
                }
                dtrGL.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
            return dttCloseDate;
        }

        private static void pmCreateTem()
        {

            DataTable dtbTemBFFIFO = new DataTable(mstrTemBFFIFO);
            dtbTemBFFIFO.Columns.Add("IsBF", System.Type.GetType("System.String"));
            dtbTemBFFIFO.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
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
            dtbTemBFFIFO.Columns.Add("fnBalQty", System.Type.GetType("System.Decimal"));

            dtbTemBFFIFO.Columns["IsBF"].DefaultValue = "";
            dtbTemBFFIFO.Columns["SedRefNo"].DefaultValue = "";
            dtbTemBFFIFO.Columns["PriceSeq"].DefaultValue = "";
            dtbTemBFFIFO.Columns["fnPrice"].DefaultValue = 0;
            dtbTemBFFIFO.Columns["fnCostAmt"].DefaultValue = 0;
            dtbTemBFFIFO.Columns["fnCostAdj"].DefaultValue = 0;
            dtbTemBFFIFO.Columns["fnCostAmt1"].DefaultValue = 0;

            dtsDataEnv.Tables.Add(dtbTemBFFIFO);

            dtbTemBFFIFO.TableNewRow += new DataTableNewRowEventHandler(TemRefTo_TableNewRow);

            DataTable dtbTemBalStock = new DataTable(mstrTemBalStock);
            dtbTemBalStock.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemBalStock.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtbTemBalStock.Columns.Add("IOType", System.Type.GetType("System.String"));
            dtbTemBalStock.Columns.Add("Qty", System.Type.GetType("System.String"));

            dtbTemBalStock.Columns["cProd"].DefaultValue = "";
            dtbTemBalStock.Columns["QcProd"].DefaultValue = "";
            dtbTemBalStock.Columns["IOType"].DefaultValue = "";
            dtbTemBalStock.Columns["Qty"].DefaultValue = 0;

            dtsDataEnv.Tables.Add(dtbTemBalStock);

            DataTable dtbTemPd = new DataTable(mstrTemPd);
            dtbTemPd.Columns.Add("cProd", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("QcProd", System.Type.GetType("System.String"));
            dtsDataEnv.Tables.Add(dtbTemPd);

            DataTable dtbTemFIFOAmt = new DataTable(mstrTemFIFOAmt);
            dtbTemFIFOAmt.Columns.Add("QcPdGrp", System.Type.GetType("System.String"));
            dtbTemFIFOAmt.Columns.Add("YrMth", System.Type.GetType("System.String"));
            dtbTemFIFOAmt.Columns.Add("CostAmt", System.Type.GetType("System.Decimal"));
            dtsDataEnv.Tables.Add(dtbTemFIFOAmt);

        }

        private static void TemRefTo_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
        }

        public static void PrintSumFIFOGroupAmtAtDate(string inBranch, string inPdGrp, DateTime inDate, ref decimal ioSumStock, ref decimal ioSumCostAmt)
        {

            if (dtsDataEnv.Tables[mstrTemBFFIFO] == null)
            {
                pmCreateTem();
            }

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            //string strSQLStr = "select Prod.fcSkid from Prod where Prod.fcCorp = ? and Prod.fcPdGrp in (select PdGrp.fcSkid from PdGrp where PdGrp.fcCorp = ? and PdGrp.fcCode = ? ) order by Prod.fcCode";

            ioSumStock = 0;
            ioSumCostAmt = 0;

            decimal ioAmt = 0;
            decimal ioSedQty = 0;
            int ioSedRecn = 0;
            decimal ioSedCost = 0;
            decimal ioSedQty2 = 0;
            string ioSedRefNo = "";
            bool inChkWhouse = true;

            if (inBranch == "")
            {
                objSQLHelper.SetPara(new object[] { App.gcCorp });
                objSQLHelper.SQLExec(ref dtsDataEnv, "QBranch", "BRANCH", "select Branch.fcSkid, Branch.fcCode, Branch.fcName from Branch where fcCorp = ? order by fcCode", ref strErrorMsg);
            }
            else
            {
                objSQLHelper.SetPara(new object[] { inBranch });
                objSQLHelper.SQLExec(ref dtsDataEnv, "QBranch", "BRANCH", "select Branch.fcSkid, Branch.fcCode, Branch.fcName from Branch where fcSkid = ? order by fcCode", ref strErrorMsg);
            }

            foreach (DataRow dtrBranch in dtsDataEnv.Tables["QBranch"].Rows)
            {

                dtsDataEnv.Tables[mstrTemPd].Rows.Clear();
                dtsDataEnv.Tables[mstrTemBalStock].Rows.Clear();

                pmPrintStock(dtrBranch["fcSkid"].ToString(), inPdGrp, inPdGrp, inDate);

                foreach (DataRow dtrTemPd in dtsDataEnv.Tables[mstrTemPd].Rows)
                {
                    decimal decBFQty = pmGetStockQty(dtrTemPd["QcProd"].ToString());
                    App.AppMessage = "กำลังพิมพ์กลุ่มสินค้า : " + inPdGrp + "\r\n" + "สินค้า : " + dtrTemPd["QcProd"].ToString();
                    PrintBFQTYLine(dtrTemPd["cProd"].ToString(), dtrBranch["fcSkid"].ToString(), inDate, inDate, decBFQty, ref ioAmt, ref ioSedQty, ref ioSedRecn, ref ioSedCost, ref ioSedQty2, ref ioSedRefNo, inChkWhouse);

                    ioSumStock += decBFQty;
                    ioSumCostAmt += ioAmt;

                }
            }

        }

        public static void PrintSumFIFOGroupAmt(string inBranch, string inPdGrp, string inRFTypeFilter, DateTime inBegDate, DateTime inEndDate, ref decimal ioSumStock, ref decimal ioSumCostAmt)
        {

            if (dtsDataEnv.Tables[mstrTemBFFIFO] == null)
            {
                pmCreateTem();
            }

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            ioSumStock = 0;
            ioSumCostAmt = 0;
            decimal decSumCostAmt = 0;

            decimal ioAmt = 0;
            decimal ioSedQty = 0;
            int ioSedRecn = 0;
            decimal ioSedCost = 0;
            decimal ioSedQty2 = 0;
            string ioSedRefNo = "";
            bool inChkWhouse = true;
            string strRfTypeFilter = inRFTypeFilter;

            if (inBranch == "")
            {
                objSQLHelper.SetPara(new object[] { App.gcCorp });
                objSQLHelper.SQLExec(ref dtsDataEnv, "QBranch", "BRANCH", "select Branch.fcSkid, Branch.fcCode, Branch.fcName from Branch where fcCorp = ? order by fcCode", ref strErrorMsg);
            }
            else
            {
                objSQLHelper.SetPara(new object[] { inBranch });
                objSQLHelper.SQLExec(ref dtsDataEnv, "QBranch", "BRANCH", "select Branch.fcSkid, Branch.fcCode, Branch.fcName from Branch where fcSkid = ? order by fcCode", ref strErrorMsg);
            }

            foreach (DataRow dtrBranch in dtsDataEnv.Tables["QBranch"].Rows)
            {

                dtsDataEnv.Tables[mstrTemPd].Rows.Clear();
                dtsDataEnv.Tables[mstrTemBalStock].Rows.Clear();
                dtsDataEnv.Tables[mstrTemFIFOAmt].Rows.Clear();

                pmPrintStock(dtrBranch["fcSkid"].ToString(), inPdGrp, inPdGrp, inBegDate);

                foreach (DataRow dtrTemPd in dtsDataEnv.Tables[mstrTemPd].Rows)
                {
                    decimal decBFQty = pmGetStockQty(dtrTemPd["QcProd"].ToString());

                    App.AppMessage = "กำลังพิมพ์กลุ่มสินค้า : " + inPdGrp + "\r\n" + "สินค้า : " + dtrTemPd["QcProd"].ToString();
                    PrintFIFO(dtrTemPd["cProd"].ToString(), dtrBranch["fcSkid"].ToString(), inBegDate, inEndDate, decBFQty, ref ioAmt, ref ioSedQty, ref ioSedRecn, ref ioSedCost, ref ioSedQty2, ref ioSedRefNo, inChkWhouse);

                    ioSumStock += decBFQty;
                    ioSumCostAmt += ioAmt;

                }

                decimal decSign = 1;
                DataRow[] dtrSel = dtsDataEnv.Tables[mstrTemBFFIFO].Select("IsBF <> 'Y'");
                for (int i = 0; i < dtrSel.Length; i++)
                {

                    DateTime dttDate = Convert.ToDateTime(dtrSel[i]["fdDate"]);
                    string strYrMth = dttDate.Year.ToString("0000") + dttDate.ToString("MM");
                    if (strRfTypeFilter != string.Empty)
                    {
                        if (strRfTypeFilter.IndexOf(dtrSel[i]["fcRfType"].ToString()) > -1)
                        {
                            decSign = (dtrSel[i]["fcIOType"].ToString() == "I" ? 1 : -1);
                            decSumCostAmt += (Convert.ToDecimal(dtrSel[i]["fnCostAmt"]) * decSign);
                            pmAddTemFIFOAmt(inPdGrp, strYrMth, Convert.ToDecimal(dtrSel[i]["fnCostAmt"]) * decSign);
                        }
                    }
                    else
                    {
                        decSign = (dtrSel[i]["fcIOType"].ToString() == "I" ? 1 : -1);
                        decSumCostAmt += (Convert.ToDecimal(dtrSel[i]["fnCostAmt"]) * decSign);
                        pmAddTemFIFOAmt(inPdGrp, strYrMth, Convert.ToDecimal(dtrSel[i]["fnCostAmt"]) * decSign);
                    }
                }

            }
            ioSumCostAmt = decSumCostAmt;
        }

        private static void pmAddTemFIFOAmt(string inQcPdGrp, string inYrMth, decimal inCostAmt)
        {
            string strFilter = string.Format("QcPdGrp = '{0}' and YrMth = '{1}' ", new string[] { inQcPdGrp, inYrMth });
            DataRow[] strSel = dtsDataEnv.Tables[mstrTemFIFOAmt].Select(strFilter);
            if (strSel.Length == 0)
            {
                DataRow dtrPreview = dtsDataEnv.Tables[mstrTemFIFOAmt].NewRow();
                dtrPreview["QcPdGrp"] = inQcPdGrp;
                dtrPreview["YrMth"] = inYrMth;
                dtrPreview["CostAmt"] = inCostAmt;
                dtsDataEnv.Tables[mstrTemFIFOAmt].Rows.Add(dtrPreview);
            }
            else
            {
                strSel[0]["CostAmt"] = Convert.ToDecimal(strSel[0]["CostAmt"]) + inCostAmt;
            }
        }

        private static void pmPrintStock(string inBranch, string inBegQcPdGrp, string inEndQcPdGrp, DateTime inDate)
        {
            string strSQLStr = "select ";
            strSQLStr = strSQLStr + " REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " , (select PROD.FCCODE from PROD where PROD.FCSKID = REFPROD.FCPROD) as QcProd";
            strSQLStr = strSQLStr + " , (select PROD.FCNAME from PROD where PROD.FCSKID = REFPROD.FCPROD) as QnProd";
            strSQLStr = strSQLStr + " , (select UM.FCNAME from UM where UM.FCSKID = REFPROD.FCUM) as QnUM";
            strSQLStr = strSQLStr + " , REFPROD.FCIOTYPE as IOType , sum(REFPROD.FNQTY*REFPROD.FNUMQTY) as SumQty ";
            strSQLStr = strSQLStr + " from REFPROD ";
            strSQLStr = strSQLStr + " left join PROD on PROD.FCSKID = REFPROD.FCPROD ";
            strSQLStr = strSQLStr + " left join PDGRP on PDGRP.FCSKID = PROD.FCPDGRP ";
            strSQLStr = strSQLStr + " left join UM on UM.FCSKID = PROD.FCUM ";
            strSQLStr = strSQLStr + " left join WHOUSE on WHOUSE.FCSKID = REFPROD.FCWHOUSE ";
            strSQLStr = strSQLStr + " where REFPROD.FCCORP = @XACORP and REFPROD.FCBRANCH = @XABRANCH ";
            strSQLStr = strSQLStr + " and REFPROD.FDDATE <= @XADATE and REFPROD.FCSTAT <> 'C'  ";
            //strSQLStr = strSQLStr + " and PROD.FCCODE between @xaBegQcProd and @xaEndQcProd ";
            strSQLStr = strSQLStr + " and PDGRP.FCCODE between @XABEGQCPDGRP and @XAENDQCPDGRP ";
            ////strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' and WHOUSE.FCCODE = '01' ";
            strSQLStr = strSQLStr + " and WHOUSE.FCTYPE = ' ' ";
            strSQLStr = strSQLStr + " group by REFPROD.FCPROD, REFPROD.FCIOTYPE, REFPROD.FCUM ";
            //strSQLStr = strSQLStr + " group by PROD.FCCODE , PROD.FCNAME , PROD.FNPRICE, REFPROD.FCIOTYPE, PROD.FCUM ";
            strSQLStr = strSQLStr + " order by QcProd ";

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(App.ERPConnectionString2);
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(strSQLStr, conn);
            cmd.CommandTimeout = 3000;

            cmd.Parameters.AddWithValue("@XACORP", App.gcCorp);
            cmd.Parameters.AddWithValue("@XABRANCH", inBranch);
            cmd.Parameters.AddWithValue("@XADATE", inDate);
            cmd.Parameters.AddWithValue("@XABEGQCPDGRP", inBegQcPdGrp);
            cmd.Parameters.AddWithValue("@XAENDQCPDGRP", inEndQcPdGrp);
            //cmd.Parameters.AddWithValue("@xaBegQcProd", inBegQcProd);
            //cmd.Parameters.AddWithValue("@xaEndQcProd", inEndQcProd);

            try
            {

                conn.Open();
                System.Data.SqlClient.SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        DataRow dtrPreview = dtsDataEnv.Tables[mstrTemBalStock].NewRow();
                        dtrPreview["QcProd"] = dr["QcProd"].ToString();
                        dtrPreview["IOType"] = dr["IOType"].ToString();
                        dtrPreview["Qty"] = Convert.ToDecimal(dr["SumQty"]);
                        dtsDataEnv.Tables[mstrTemBalStock].Rows.Add(dtrPreview);

                        pmAddProd(dr["fcProd"].ToString(), dr["QcProd"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show(ex.Message);
#endif
                App.WriteEventsLog(ex);
            }
            finally
            {
                conn.Close();
            }

        }

        private static void pmAddProd(string inProd, string inQcProd)
        {
            string strFilter = string.Format("QcProd = '{0}' ", new string[] { inQcProd });
            DataRow[] strSel = dtsDataEnv.Tables[mstrTemPd].Select(strFilter);
            if (strSel.Length == 0)
            {
                DataRow dtrPreview = dtsDataEnv.Tables[mstrTemPd].NewRow();
                dtrPreview["cProd"] = inProd;
                dtrPreview["QcProd"] = inQcProd;
                dtsDataEnv.Tables[mstrTemPd].Rows.Add(dtrPreview);
            }
        }

        private static decimal pmGetStockQty(string inQcProd)
        {
            string strFilter = string.Format("QcProd = '{0}' ", new string[] { inQcProd });
            DataRow[] strSel = dtsDataEnv.Tables[mstrTemBalStock].Select(strFilter);
            decimal decQty = 0;
            if (strSel.Length > 0)
            {
                for (int intCnt = 0; intCnt < strSel.Length; intCnt++)
                {
                    decQty = decQty + (Convert.ToDecimal(strSel[intCnt]["Qty"]) * (strSel[intCnt]["IOType"].ToString() == "I" ? 1 : -1));
                }
            }
            return decQty;
        }

        public static void PrintFIFO(string inProd, string inBranch, DateTime inBegDate, DateTime inEndDate, decimal inYokMaQty, ref decimal ioAmt, ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo, bool inChkWhouse)
        {
            pmPrintBFQTYLine(inProd, inBranch, inBegDate, inEndDate, inYokMaQty, ref ioAmt, ref ioSedQty, ref ioSedRecn, ref ioSedCost, ref ioSedQty2, ref ioSedRefNo, inChkWhouse);
            pmPrint1Prod(inProd, inBranch, inBegDate, inEndDate, inYokMaQty, ref ioAmt, ref ioSedQty, ref ioSedRecn, ref ioSedCost, ref ioSedQty2, ref ioSedRefNo, inChkWhouse);
        }

        private static void pmPrint1Prod(string inProd, string inBranch, DateTime inBegDate, DateTime inEndDate, decimal inYokMaQty, ref decimal ioAmt, ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo, bool inChkWhouse)
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

            SqlConnection conn = new SqlConnection(App.ERPConnectionString2);
            SqlCommand cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
            SqlDataReader dtrRefProd = null;

            //try
            //{
            conn.Open();
            cmd.CommandTimeout = 2000;

            cmd.Parameters.AddWithValue("@XAPROD", inProd);
            cmd.Parameters.AddWithValue("@XABRANCH", inBranch);
            cmd.Parameters.AddWithValue("@XABEGDATE", inBegDate.Date);
            cmd.Parameters.AddWithValue("@XAENDDATE", inEndDate.Date);

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

        public static void PrintBFQTYLine(string inProd, string inBranch, DateTime inBegDate, DateTime inEndDate, decimal inYokMaQty, ref decimal ioAmt, ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo, bool inChkWhouse)
        {
            pmPrintBFQTYLine(inProd, inBranch, inBegDate, inEndDate, inYokMaQty, ref ioAmt, ref ioSedQty, ref ioSedRecn, ref ioSedCost, ref ioSedQty2, ref ioSedRefNo, inChkWhouse);
        }

        private static void pmPrintBFQTYLine(string inProd, string inBranch, DateTime inBegDate, DateTime inEndDate, decimal inYokMaQty, ref decimal ioAmt, ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo, bool inChkWhouse)
        {

            if (dtsDataEnv.Tables[mstrTemBFFIFO] == null)
            {
                pmCreateTem();
            }
            dtsDataEnv.Tables[mstrTemBFFIFO].Rows.Clear();
            //dtsDataEnv.Tables[mstrTemBFFIFO_BF].Rows.Clear();

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
            //เปลี่ยนมาคำนวณต้นทุนระดับบริษัท
            //lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @XAPROD and RefProd.fcBranch = @XABRANCH and RefProd.fdDate <= @XADATE ";
            lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @XAPROD and RefProd.fdDate <= @XADATE ";
            //if (this.txtTagWHouse.Text.Trim() != string.Empty)
            //{
            //    lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcCode in ( " + this.txtTagWHouse.Text.Trim() + " ) ";
            //}
            lcFOXSQLStr = lcFOXSQLStr + " and RefProd.fcStat <> 'C' ";
            lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fcProd desc,RefProd.fcBranch desc,RefProd.fdDate desc , RefProd.fcTimeStam desc , RefProd.fcIOType desc , RefProd.fcGLRef desc , RefProd.fcSkid desc";

            SqlConnection conn = new SqlConnection(App.ERPConnectionString2);
            SqlCommand cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
            SqlDataReader dtrRefProd = null;

            //try
            //{
            conn.Open();
            cmd.CommandTimeout = 2000;

            cmd.Parameters.AddWithValue("@XAPROD", inProd);
            //cmd.Parameters.AddWithValue("@XABRANCH", inBranch);
            cmd.Parameters.AddWithValue("@XADATE", inBegDate.AddDays(-1));

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
                            dtrTemFIFO["fnBalQty"] = tQty1;
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
                            dtrTemFIFO["fnBalQty"] = ioSedQty;
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

                #region "Logic มันแปลก ๆ ขอเอาออกก่อน"

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
                //lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @xaProd and RefProd.fcBranch = @xaBranch and RefProd.fdDate between @xaBegDate and @xaEndDate ";
                lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @xaProd and RefProd.fdDate between @xaBegDate and @xaEndDate ";
                //if (this.txtTagWHouse.Text.Trim() != string.Empty)
                //{
                //    lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcCode in ( " + this.txtTagWHouse.Text.Trim() + " ) ";
                //}
                lcFOXSQLStr = lcFOXSQLStr + " and RefProd.fcStat <> 'C' ";
                lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fcProd ,RefProd.fcBranch ,RefProd.fdDate , RefProd.fcTimeStam , RefProd.fcIOType , RefProd.fcGLRef , RefProd.fcSkid ";

                cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
                cmd.CommandTimeout = 2000;

                int cmpDate = ldDate.CompareTo(inEndDate);
                while (llHasData == false
                    && cmpDate < 1)
                {

                    cmpDate = ldDate.CompareTo(inEndDate);

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@XAPROD", inProd);
                    //cmd.Parameters.AddWithValue("@xaBranch", inBranch);
                    cmd.Parameters.AddWithValue("@XABEGDATE", ldDate);
                    cmd.Parameters.AddWithValue("@XAENDDATE", ldDate);

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

                #endregion

                if (inYokMaQty != 0)
                {
                    //    lcSeleFieldStr = " UM.fcName as QnUM";
                    //    lcSeleFieldStr += " , Prod.fcCode as QcProd , Prod.fcName as QnProd , UM.fcName as QnUM";
                    //    lcSeleFieldStr += " , PdGrp.fcCode as QcPdGrp , PdGrp.fcName as QnPdGrp";

                    //    lcFOXSQLStr = " select " + lcSeleFieldStr + " from Prod ";
                    //    lcFOXSQLStr = lcFOXSQLStr + " left join PdGrp on PdGrp.fcSkid = Prod.fcPdGrp ";
                    //    lcFOXSQLStr = lcFOXSQLStr + " left join UM on UM.fcSkid = Prod.fcUM ";
                    //    lcFOXSQLStr = lcFOXSQLStr + " where Prod.fcSkid = ? ";

                    //    string strErrorMsg = "";
                    //    WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

                    //    DateTime dttNegDate = inBegDate.AddDays(-1);
                    //    string strCDate = dttNegDate.Year.ToString("0000") + dttNegDate.ToString("MMdd");

                    //    objSQLHelper.SetPara(new object[] { inProd });
                    //    if (objSQLHelper.SQLExec(ref dtsDataEnv, "QProd2", "PROD", lcFOXSQLStr, ref strErrorMsg))
                    //    {

                    //        DataRow dtrProd2 = dtsDataEnv.Tables["QProd2"].Rows[0];
                    //        DataRow dtrTemFIFO = dtsDataEnv.Tables[mstrTemBFFIFO].NewRow();
                    //        dtrTemFIFO["IsBF"] = "Y";
                    //        dtrTemFIFO["QcBook"] = "";
                    //        dtrTemFIFO["fcRfType"] = "";
                    //        dtrTemFIFO["fcRefType"] = "";
                    //        dtrTemFIFO["fcRefNo"] = "";
                    //        dtrTemFIFO["fdDate"] = inBegDate.AddDays(-1);
                    //        dtrTemFIFO["cDate"] = strCDate;
                    //        dtrTemFIFO["QcPdGrp"] = dtrProd2["QcPdGrp"].ToString();
                    //        dtrTemFIFO["QnPdGrp"] = dtrProd2["QnPdGrp"].ToString();
                    //        dtrTemFIFO["QcProd"] = dtrProd2["QcProd"].ToString();
                    //        dtrTemFIFO["QnProd"] = dtrProd2["QnProd"].ToString();
                    //        dtrTemFIFO["QnUM"] = dtrProd2["QnUM"].ToString();
                    //        dtrTemFIFO["fcIOType"] = "O";
                    //        //dtrTemFIFO["fnQty"] = Math.Abs(inYokMaQty);
                    //        dtrTemFIFO["fnQty"] = inYokMaQty;
                    //        dtrTemFIFO["fnPrice"] = 0;
                    //        dtrTemFIFO["fnCostAmt"] = 0;
                    //        dtsDataEnv.Tables[mstrTemBFFIFO].Rows.Add(dtrTemFIFO);

                    //    }
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

        private static void SumSed(ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo)
        {
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables["XRefProd"].Select("fcIOType = 'I' ");
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables[this.mstrTemBFFIFO].Select("fcIOType = 'I' ", "cDate");
            DataRow[] dtrTemBFFIFO = dtsDataEnv.Tables[mstrTemBFFIFO].Select("fcIOType = 'I' ", "nRecNo");
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
            //By Yod : 7/6/54 
            //if (ioSedQty == 0) ioSedRecn = 0;

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

        private static void SumSedNegative(ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo)
        {
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables["XRefProd"].Select("fcIOType = 'I' ");
            //DataRow[] dtrTemBFFIFO = this.dtsDataEnv.Tables[this.mstrTemBFFIFO].Select("fcIOType = 'I' ", "cDate");
            DataRow[] dtrTemBFFIFO = dtsDataEnv.Tables[mstrTemBFFIFO].Select("fcIOType = 'I' ", "nRecNo");
            //ioSedQty = 0;
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
            //By Yod : 7/6/54 
            if (ioSedQty == 0) intCurrRecn = intCurrRecn - 1;

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

        private static void P1Line(SqlDataReader inSource, decimal inQty, decimal inCost, decimal inAmt, string inSedRefNo)
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

        private static void XXX2pmPrintBFQTYLine(string inProd, string inBranch, DateTime inBegDate, DateTime inEndDate, decimal inYokMaQty, ref decimal ioAmt, ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo, bool inChkWhouse)
        {

            if (dtsDataEnv.Tables[mstrTemBFFIFO] == null)
            {
                pmCreateTem();
            }
            dtsDataEnv.Tables[mstrTemBFFIFO].Rows.Clear();
            //dtsDataEnv.Tables[mstrTemBFFIFO_BF].Rows.Clear();

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
            //เปลี่ยนมาคำนวณต้นทุนระดับบริษัท
            //lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @XAPROD and RefProd.fcBranch = @XABRANCH and RefProd.fdDate <= @XADATE ";
            lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @XAPROD and RefProd.fdDate <= @XADATE ";
            //if (this.txtTagWHouse.Text.Trim() != string.Empty)
            //{
            //    lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcCode in ( " + this.txtTagWHouse.Text.Trim() + " ) ";
            //}
            lcFOXSQLStr = lcFOXSQLStr + " and RefProd.fcStat <> 'C' ";
            lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fcProd desc,RefProd.fcBranch desc,RefProd.fdDate desc , RefProd.fcTimeStam desc , RefProd.fcIOType desc , RefProd.fcGLRef desc , RefProd.fcSkid desc";

            SqlConnection conn = new SqlConnection(App.ERPConnectionString2);
            SqlCommand cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
            SqlDataReader dtrRefProd = null;

            //try
            //{
            conn.Open();
            cmd.CommandTimeout = 2000;

            cmd.Parameters.AddWithValue("@XAPROD", inProd);
            //cmd.Parameters.AddWithValue("@XABRANCH", inBranch);
            cmd.Parameters.AddWithValue("@XADATE", inBegDate.AddDays(-1));

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

                #region "Logic มันแปลก ๆ ขอเอาออกก่อน"

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
                //lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @xaProd and RefProd.fcBranch = @xaBranch and RefProd.fdDate between @xaBegDate and @xaEndDate ";
                lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @xaProd and RefProd.fdDate between @xaBegDate and @xaEndDate ";
                //if (this.txtTagWHouse.Text.Trim() != string.Empty)
                //{
                //    lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcCode in ( " + this.txtTagWHouse.Text.Trim() + " ) ";
                //}
                lcFOXSQLStr = lcFOXSQLStr + " and RefProd.fcStat <> 'C' ";
                lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fcProd ,RefProd.fcBranch ,RefProd.fdDate , RefProd.fcTimeStam , RefProd.fcIOType , RefProd.fcGLRef , RefProd.fcSkid ";

                cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
                cmd.CommandTimeout = 2000;

                int cmpDate = ldDate.CompareTo(inEndDate);
                while (llHasData == false
                    && cmpDate < 1)
                {

                    cmpDate = ldDate.CompareTo(inEndDate);

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@XAPROD", inProd);
                    //cmd.Parameters.AddWithValue("@xaBranch", inBranch);
                    cmd.Parameters.AddWithValue("@XABEGDATE", ldDate);
                    cmd.Parameters.AddWithValue("@XAENDDATE", ldDate);

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

                #endregion

                if (inYokMaQty != 0)
                {
                    //    lcSeleFieldStr = " UM.fcName as QnUM";
                    //    lcSeleFieldStr += " , Prod.fcCode as QcProd , Prod.fcName as QnProd , UM.fcName as QnUM";
                    //    lcSeleFieldStr += " , PdGrp.fcCode as QcPdGrp , PdGrp.fcName as QnPdGrp";

                    //    lcFOXSQLStr = " select " + lcSeleFieldStr + " from Prod ";
                    //    lcFOXSQLStr = lcFOXSQLStr + " left join PdGrp on PdGrp.fcSkid = Prod.fcPdGrp ";
                    //    lcFOXSQLStr = lcFOXSQLStr + " left join UM on UM.fcSkid = Prod.fcUM ";
                    //    lcFOXSQLStr = lcFOXSQLStr + " where Prod.fcSkid = ? ";

                    //    string strErrorMsg = "";
                    //    WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

                    //    DateTime dttNegDate = inBegDate.AddDays(-1);
                    //    string strCDate = dttNegDate.Year.ToString("0000") + dttNegDate.ToString("MMdd");

                    //    objSQLHelper.SetPara(new object[] { inProd });
                    //    if (objSQLHelper.SQLExec(ref dtsDataEnv, "QProd2", "PROD", lcFOXSQLStr, ref strErrorMsg))
                    //    {

                    //        DataRow dtrProd2 = dtsDataEnv.Tables["QProd2"].Rows[0];
                    //        DataRow dtrTemFIFO = dtsDataEnv.Tables[mstrTemBFFIFO].NewRow();
                    //        dtrTemFIFO["IsBF"] = "Y";
                    //        dtrTemFIFO["QcBook"] = "";
                    //        dtrTemFIFO["fcRfType"] = "";
                    //        dtrTemFIFO["fcRefType"] = "";
                    //        dtrTemFIFO["fcRefNo"] = "";
                    //        dtrTemFIFO["fdDate"] = inBegDate.AddDays(-1);
                    //        dtrTemFIFO["cDate"] = strCDate;
                    //        dtrTemFIFO["QcPdGrp"] = dtrProd2["QcPdGrp"].ToString();
                    //        dtrTemFIFO["QnPdGrp"] = dtrProd2["QnPdGrp"].ToString();
                    //        dtrTemFIFO["QcProd"] = dtrProd2["QcProd"].ToString();
                    //        dtrTemFIFO["QnProd"] = dtrProd2["QnProd"].ToString();
                    //        dtrTemFIFO["QnUM"] = dtrProd2["QnUM"].ToString();
                    //        dtrTemFIFO["fcIOType"] = "O";
                    //        //dtrTemFIFO["fnQty"] = Math.Abs(inYokMaQty);
                    //        dtrTemFIFO["fnQty"] = inYokMaQty;
                    //        dtrTemFIFO["fnPrice"] = 0;
                    //        dtrTemFIFO["fnCostAmt"] = 0;
                    //        dtsDataEnv.Tables[mstrTemBFFIFO].Rows.Add(dtrTemFIFO);

                    //    }
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

        public static void XXXPrintBFQTYLine(string inProd, string inBranch, DateTime inBegDate, DateTime inEndDate, decimal inYokMaQty, ref decimal ioAmt, ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo, bool inChkWhouse)
        {
            pmPrintBFQTYLine(inProd, inBranch, inBegDate, inEndDate, inYokMaQty, ref ioAmt, ref ioSedQty, ref ioSedRecn, ref ioSedCost, ref ioSedQty2, ref ioSedRefNo, inChkWhouse);
        }

        private static void XXXpmPrintBFQTYLine(string inProd, string inBranch, DateTime inBegDate, DateTime inEndDate, decimal inYokMaQty, ref decimal ioAmt, ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo, bool inChkWhouse)
        {

            if (dtsDataEnv.Tables[mstrTemBFFIFO] == null)
            {
                pmCreateTem();
            }
            dtsDataEnv.Tables[mstrTemBFFIFO].Rows.Clear();

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
            lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @XAPROD and RefProd.fcBranch = @XABRANCH and RefProd.fdDate <= @XADATE ";
            //if (this.txtTagWHouse.Text.Trim() != string.Empty)
            //{
            //    lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcCode in ( " + this.txtTagWHouse.Text.Trim() + " ) ";
            //}
            lcFOXSQLStr = lcFOXSQLStr + " and RefProd.fcStat <> 'C' ";
            lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fcProd desc,RefProd.fcBranch desc,RefProd.fdDate desc , RefProd.fcTimeStam desc , RefProd.fcIOType desc , RefProd.fcGLRef desc , RefProd.fcSkid desc";

            SqlConnection conn = new SqlConnection(App.ERPConnectionString2);
            SqlCommand cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
            SqlDataReader dtrRefProd = null;

            //try
            //{
            conn.Open();
            cmd.CommandTimeout = 2000;

            cmd.Parameters.AddWithValue("@XAPROD", inProd);
            cmd.Parameters.AddWithValue("@XABRANCH", inBranch);
            cmd.Parameters.AddWithValue("@XADATE", inBegDate.AddDays(-1));

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

                #region "Logic มันแปลก ๆ ขอเอาออกก่อน"

                //bool llHasData = false;
                //DateTime ldDate = inBegDate;
                //lcFOXSQLStr = lcSeleFieldStr;
                ////lcFOXSQLStr = lcFOXSQLStr + " from RefProd where RefProd.fcProd = @xaProd and RefProd.fcBranch = @xaBranch and RefProd.fdDate between @xaBegDate and @xaEndDate ";
                //lcFOXSQLStr = lcFOXSQLStr + " from RefProd ";
                //lcFOXSQLStr = lcFOXSQLStr + " left join GLRef on GLRef.fcSkid = RefProd.fcGLRef ";
                //lcFOXSQLStr = lcFOXSQLStr + " left join Prod on Prod.fcSkid = RefProd.fcProd ";
                //lcFOXSQLStr = lcFOXSQLStr + " left join PdGrp on PdGrp.fcSkid = Prod.fcPdGrp ";
                //lcFOXSQLStr = lcFOXSQLStr + " left join UM on UM.fcSkid = Prod.fcUM ";
                //lcFOXSQLStr = lcFOXSQLStr + " left join Sect on Sect.fcSkid = RefProd.fcSect ";
                //lcFOXSQLStr = lcFOXSQLStr + " left join Job on Job.fcSkid = RefProd.fcJob ";
                //lcFOXSQLStr = lcFOXSQLStr + " left join Book on Book.fcSkid = GLRef.fcBook ";
                //lcFOXSQLStr = lcFOXSQLStr + " left join WHOUSE on WHOUSE.fcSkid = RefProd.fcWHouse ";
                //lcFOXSQLStr = lcFOXSQLStr + " where RefProd.fcProd = @xaProd and RefProd.fcBranch = @xaBranch and RefProd.fdDate between @xaBegDate and @xaEndDate ";
                ////if (this.txtTagWHouse.Text.Trim() != string.Empty)
                ////{
                ////    lcFOXSQLStr = lcFOXSQLStr + " and WHouse.fcCode in ( " + this.txtTagWHouse.Text.Trim() + " ) ";
                ////}
                //lcFOXSQLStr = lcFOXSQLStr + " and RefProd.fcStat <> 'C' ";
                //lcFOXSQLStr = lcFOXSQLStr + " order by RefProd.fcProd ,RefProd.fcBranch ,RefProd.fdDate , RefProd.fcTimeStam , RefProd.fcIOType , RefProd.fcGLRef , RefProd.fcSkid ";

                //cmd = new SqlCommand(lcFOXSQLStr.ToUpper(), conn);
                //cmd.CommandTimeout = 2000;

                //int cmpDate = ldDate.CompareTo(inEndDate);
                //while (llHasData == false
                //    && cmpDate < 1)
                //{

                //    cmpDate = ldDate.CompareTo(inEndDate);

                //    cmd.Parameters.Clear();
                //    cmd.Parameters.AddWithValue("@xaProd", inProd);
                //    cmd.Parameters.AddWithValue("@xaBranch", inBranch);
                //    cmd.Parameters.AddWithValue("@xaBegDate", ldDate);
                //    cmd.Parameters.AddWithValue("@xaEndDate", ldDate);

                //    dtrRefProd = cmd.ExecuteReader();
                //    if (dtrRefProd.HasRows)
                //    {
                //        while (dtrRefProd.Read())
                //        {
                //            if (VRefProd2(dtrRefProd["fcRfType"].ToString(), dtrRefProd["fcIOType"].ToString(), dtrRefProd["fcStat"].ToString(), Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"])))
                //            {
                //                decimal tAbsQty = Math.Abs(Convert.ToDecimal(dtrRefProd["fnQty"]) * Convert.ToDecimal(dtrRefProd["fnUmQty"]));
                //                decimal tCostAmt = Math.Abs(Convert.ToDecimal(dtrRefProd["fnCostAmt"]) + Convert.ToDecimal(dtrRefProd["fnCostAdj"]));
                //                tFstYokMaDate = Convert.ToDateTime(dtrRefProd["fdDate"]);
                //                tCrSedSkid = dtrRefProd["fcSkid"].ToString();
                //                ioSedQty = QtySeq1(tCostAmt, tAbsQty);
                //                ioSedQty2 = tAbsQty - ioSedQty;
                //                ioSedCost = CostSeq1(tCostAmt, tAbsQty);
                //                llHasData = true;

                //                string strCDate = Convert.ToDateTime(dtrRefProd["fdDate"]).Year.ToString("0000") + Convert.ToDateTime(dtrRefProd["fdDate"]).ToString("MMdd") + dtrRefProd["fcRefNo"].ToString() + dtrRefProd["fcSkid"].ToString();
                //                DataRow dtrTemFIFO = dtsDataEnv.Tables[mstrTemBFFIFO].NewRow();
                //                dtrTemFIFO["IsBF"] = "Y";
                //                dtrTemFIFO["QcBook"] = dtrRefProd["QcBook"].ToString();
                //                dtrTemFIFO["fcRfType"] = dtrRefProd["fcRfType"].ToString();
                //                dtrTemFIFO["fcRefType"] = dtrRefProd["fcRefType"].ToString();
                //                dtrTemFIFO["fcRefNo"] = dtrRefProd["fcRefNo"].ToString();
                //                dtrTemFIFO["fdDate"] = Convert.ToDateTime(dtrRefProd["fdDate"]);
                //                dtrTemFIFO["cDate"] = strCDate;
                //                dtrTemFIFO["QcPdGrp"] = dtrRefProd["QcPdGrp"].ToString();
                //                dtrTemFIFO["QnPdGrp"] = dtrRefProd["QnPdGrp"].ToString();
                //                dtrTemFIFO["QcProd"] = dtrRefProd["QcProd"].ToString();
                //                dtrTemFIFO["QnProd"] = dtrRefProd["QnProd"].ToString();
                //                dtrTemFIFO["QnUM"] = dtrRefProd["QnUM"].ToString();
                //                dtrTemFIFO["fcIOType"] = dtrRefProd["fcIOType"].ToString();
                //                dtrTemFIFO["fnQty"] = ioSedQty;
                //                dtrTemFIFO["fnPrice"] = ioSedCost;
                //                dtrTemFIFO["fnCostAmt"] = (ioSedQty * ioSedCost);

                //                //dtsDataEnv.Tables[mstrTemBFFIFO].Rows.Add(dtrTemFIFO);

                //                break;
                //            }
                //        }
                //    }
                //    ldDate = ldDate.AddDays(1);
                //    dtrRefProd.Close();
                //}

                #endregion

                if (inYokMaQty != 0)
                {
                    lcSeleFieldStr = " UM.fcName as QnUM";
                    lcSeleFieldStr += " , Prod.fcCode as QcProd , Prod.fcName as QnProd , UM.fcName as QnUM";
                    lcSeleFieldStr += " , PdGrp.fcCode as QcPdGrp , PdGrp.fcName as QnPdGrp";

                    lcFOXSQLStr = " select " + lcSeleFieldStr + " from Prod ";
                    lcFOXSQLStr = lcFOXSQLStr + " left join PdGrp on PdGrp.fcSkid = Prod.fcPdGrp ";
                    lcFOXSQLStr = lcFOXSQLStr + " left join UM on UM.fcSkid = Prod.fcUM ";
                    lcFOXSQLStr = lcFOXSQLStr + " where Prod.fcSkid = ? ";

                    string strErrorMsg = "";
                    WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

                    DateTime dttNegDate = inBegDate.AddDays(-1);
                    string strCDate = dttNegDate.Year.ToString("0000") + dttNegDate.ToString("MMdd");

                    objSQLHelper.SetPara(new object[] { inProd });
                    if (objSQLHelper.SQLExec(ref dtsDataEnv, "QProd2", "PROD", lcFOXSQLStr, ref strErrorMsg))
                    {

                        DataRow dtrProd2 = dtsDataEnv.Tables["QProd2"].Rows[0];
                        DataRow dtrTemFIFO = dtsDataEnv.Tables[mstrTemBFFIFO].NewRow();
                        dtrTemFIFO["IsBF"] = "Y";
                        dtrTemFIFO["QcBook"] = "";
                        dtrTemFIFO["fcRfType"] = "";
                        dtrTemFIFO["fcRefType"] = "";
                        dtrTemFIFO["fcRefNo"] = "";
                        dtrTemFIFO["fdDate"] = inBegDate.AddDays(-1);
                        dtrTemFIFO["cDate"] = strCDate;
                        dtrTemFIFO["QcPdGrp"] = dtrProd2["QcPdGrp"].ToString();
                        dtrTemFIFO["QnPdGrp"] = dtrProd2["QnPdGrp"].ToString();
                        dtrTemFIFO["QcProd"] = dtrProd2["QcProd"].ToString();
                        dtrTemFIFO["QnProd"] = dtrProd2["QnProd"].ToString();
                        dtrTemFIFO["QnUM"] = dtrProd2["QnUM"].ToString();
                        dtrTemFIFO["fcIOType"] = "O";
                        //dtrTemFIFO["fnQty"] = Math.Abs(inYokMaQty);
                        dtrTemFIFO["fnQty"] = inYokMaQty;
                        dtrTemFIFO["fnPrice"] = 0;
                        dtrTemFIFO["fnCostAmt"] = 0;
                        dtsDataEnv.Tables[mstrTemBFFIFO].Rows.Add(dtrTemFIFO);

                    }
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

        private static void XXXSumSed(ref decimal ioSedQty, ref int ioSedRecn, ref decimal ioSedCost, ref decimal ioSedQty2, ref string ioSedRefNo)
        {
            DataRow[] dtrTemBFFIFO = dtsDataEnv.Tables[mstrTemBFFIFO].Select("fcIOType = 'I' ", "cDate");
            //DataRow[] dtrTemBFFIFO = dtsDataEnv.Tables["XRefProd"].Select("fcIOType = 'I' ");
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

        private static void XXXP1Line(SqlDataReader inSource, decimal inQty, decimal inCost, decimal inAmt, string inSedRefNo)
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

        //private static string xaXtraType = SysDef.gc_RFTYPE_TRAN_PD + "," + SysDef.gc_RFTYPE_ISSUE_PD + "," + SysDef.gc_RFTYPE_PRODUCE_PD + "," + SysDef.gc_RFTYPE_RETURN_ISSUE;
        //private static string xaXtraType2 = SysDef.gc_RFTYPE_TRAN_PD + "," + SysDef.gc_RFTYPE_ISSUE_PD;

        private static string xaXtraType = SysDef.gc_RFTYPE_ISSUE_PD + "," + SysDef.gc_RFTYPE_PRODUCE_PD + "," + SysDef.gc_RFTYPE_RETURN_ISSUE;
        private static string xaXtraType2 = SysDef.gc_RFTYPE_ISSUE_PD;

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
                || (inRfType == SysDef.gc_RFTYPE_ISSUE_PD && inIOType == "O")
                || (inRfType == SysDef.gc_RFTYPE_PRODUCE_PD && inIOType == "I")
                || (inRfType == SysDef.gc_RFTYPE_RETURN_ISSUE && inIOType == "I"));
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
