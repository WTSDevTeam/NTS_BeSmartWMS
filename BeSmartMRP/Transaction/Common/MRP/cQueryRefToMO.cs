
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace BeSmartMRP.Transaction.Common.MRP
{
    
    public class cQueryRefToMO
    {

        public static string xd_TemRef2MO = "TemRef2MO";

        public cQueryRefToMO()
        {
            this.pmCreateTem();
        }

        private DataSet dtsDataEnv = new DataSet();

        private string mstrEditRowID = "";

        public string EditRowID
        {
            get { return this.mstrEditRowID; }
            set { this.mstrEditRowID = value; }
        }

        private void pmCreateTem()
        {
            DataTable dtbTemRef2MO = new DataTable(xd_TemRef2MO);

            dtbTemRef2MO.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("cRefType", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtbTemRef2MO.Columns.Add("cCode", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("cRefNo", System.Type.GetType("System.String"));

            dtbTemRef2MO.Columns.Add("cCoor", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("cFrWHouse", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("cToWHouse", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("cSect", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("cJob", System.Type.GetType("System.String"));

            dtbTemRef2MO.Columns.Add("cQcCoor", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("cQnCoor", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("cQcFrWHouse", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("cQnFrWHouse", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("cQcToWHouse", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("cQnToWHouse", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("cQcSect", System.Type.GetType("System.String"));
            dtbTemRef2MO.Columns.Add("cQcJob", System.Type.GetType("System.String"));
            //dtbTemRef2MO.Columns.Add("nAmt", System.Type.GetType("System.Decimal"));
            this.dtsDataEnv.Tables.Add(dtbTemRef2MO);
        
        }

        public void ShowDetail(string inEditRowID)
        {
            this.mstrEditRowID = inEditRowID;
            this.pmQueryPaymentDetail();
        }

        private void pmQueryPaymentDetail()
        {
            if (this.dtsDataEnv.Tables[xd_TemRef2MO].Rows.Count > 0)
            {
                this.dtsDataEnv.Tables[xd_TemRef2MO].Rows.Clear();
            }

            this.pmQueryRefTo();
            
            //if (this.dtsDataEnv.Tables[xd_TemRef2MO].Rows.Count > 0)
            //{
                MRP.dlgViewRefToMO ofrmHistory = new MRP.dlgViewRefToMO();
                ofrmHistory.BindData(this.dtsDataEnv, xd_TemRef2MO);
                ofrmHistory.ShowDialog();
            //}
        
        }

        private void pmQueryRefTo()
        {
            WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            string strFldList = " GLRef.fcRfType, GLRef.fcRefType , GLRef.fcCoor , year(GLREF.FDDATE) as Yr , month(GLREF.FDDATE) as Mth";
            strFldList = strFldList + ", sum(GLRef.fnAmt) as Amt";
            strFldList = strFldList + ", sum(GLRef.fnVatAmt) as VatAmt";
            strFldList = strFldList + ", sum(GLRef.fnPayAmt) as PaidAmt";
            strFldList = strFldList + ", sum(GLRef.fnBPayInv) as PayAmt";
            //strFldList = strFldList + " , (select Book.fcCode from Book where Book.fcSkid = GLRef.fcBook) as QcBook";
            strFldList = strFldList + " , (select Coor.fcCode from Coor where Coor.fcSkid = GLRef.fcCoor) as QcCoor";
            strFldList = strFldList + " , (select Coor.fcName from Coor where Coor.fcSkid = GLRef.fcCoor) as QnCoor";
            strFldList = strFldList + " , (select Branch.fcCode from Branch where Branch.fcSkid = GLRef.fcBranch) as QcBranch";

            string strSQLStr = "select " + strFldList + " from GLRef ";
            strSQLStr = strSQLStr + " left join Branch on Branch.fcSkid = GLRef.fcBranch ";
            strSQLStr = strSQLStr + " left join Coor on Coor.fcSkid = GLRef.fcCoor ";
            strSQLStr = strSQLStr + " where GLRef.fcCorp = ? ";
            //strSQLStr = strSQLStr + " and GLRef.fcCoor in (select fcSkid from Coor where Coor.fcCorp = ? and Coor.fcIsCust = 'Y' and Coor.fcCode between ? and ?) ";
            strSQLStr = strSQLStr + " and GLRef.fcStat <> 'C' ";
            strSQLStr = strSQLStr + " group by GLRef.fcBranch, GLRef.fcCoor, GLRef.fcRFType, GLRef.fcREFType , year(GLREF.FDDATE) , month(GLREF.FDDATE) ";
            strSQLStr = strSQLStr + " order by year(GLREF.FDDATE) , month(GLREF.FDDATE) , QcBranch , QcCoor , GLRef.fcRFType, GLRef.fcREFType ";

            System.Data.IDbConnection conn = objSQLHelper.GetDBConnection();
            System.Data.IDbCommand cmd = objSQLHelper.GetDBCommand(strSQLStr.ToUpper(), conn);
            try
            {
                conn.Open();
                cmd.CommandTimeout = 2000;

                cmd.Parameters.Add(AppUtil.ReportHelper.AddParameter(App.ActiveCorp.RowID, DbType.String));

                bool bllIsPYokma = false;
                decimal decAmt = 0;
                decimal decVatAmt = 0;
                decimal decPayAmt = 0;

                string strLastQcCoor = "";

                string strDRRefType = "SB,SD,SI,SQ,SR,SU,SV,SW,SX,SY,SZ,";
                string strCRRefType = "SA,SC,SM,SN";
                string strPayRefType = "RI,RV";
                string strCashRefType = "SR,SU,";

                string strRefType = "";
                string strRfType = "";
                string strAddr11 = "";
                string strAddr21 = "";
                string strAddr31 = "";
                string strCoorTel = "";
                string strCoorFax = "";
                string strMemdata = "";

                System.Data.OleDb.OleDbDataReader dtrGLRef = (System.Data.OleDb.OleDbDataReader)cmd.ExecuteReader();
                if (dtrGLRef.HasRows)
                {
                    //DataRow dtrBFRow = dtsPrintPreview.XRAR01.NewRow();
                    DataRow dtrPreview = null;
                    DataRow dtrBFRow = null;
                    string strCurrPrefix = "";
                    while (dtrGLRef.Read())
                    {

                        strRefType = dtrGLRef["fcRefType"].ToString();
                        strRfType = dtrGLRef["fcRfType"].ToString();
                        dtrPreview["cYrMth"] = Convert.ToInt32(dtrGLRef["Yr"]).ToString("0000") + "/" + Convert.ToInt32(dtrGLRef["Mth"]).ToString("00");
                        dtrPreview["CQCCOOR"] = dtrGLRef["QcCoor"].ToString();
                        dtrPreview["CQNCOOR"] = dtrGLRef["QnCoor"].ToString();
                        decAmt = Convert.ToDecimal(dtrGLRef["Amt"]) + Convert.ToDecimal(dtrGLRef["VatAmt"]);
                        decPayAmt = Convert.ToDecimal(dtrGLRef["PayAmt"]);

                        if ((SysDef.gc_RFTYPE_RECEIVE + SysDef.gc_RFTYPE_RCV_VAT + SysDef.gc_RFTYPE_PAYMENT + SysDef.gc_RFTYPE_PAY_VAT).IndexOf(strRfType) > -1)
                        {
                            dtrPreview["NPAYAMT"] = Convert.ToDecimal(dtrPreview["NPAYAMT"]) + decPayAmt;
                        }
                        else if ((SysDef.gc_RFTYPE_INV_SELL + SysDef.gc_RFTYPE_INV_BUY).IndexOf(strRfType) > -1)
                        {
                            dtrPreview["NBUYSELL"] = Convert.ToDecimal(dtrPreview["NBUYSELL"]) + decAmt + decVatAmt;

                            if (strCashRefType.IndexOf(strRefType) > -1)
                            {
                                dtrPreview["NPAYAMT"] = Convert.ToDecimal(dtrPreview["NPAYAMT"]) + decAmt + decVatAmt;
                            }
                        }
                        else if ((SysDef.gc_RFTYPE_CR_SELL + SysDef.gc_RFTYPE_CR_BUY).IndexOf(strRfType) > -1)
                        {
                            dtrPreview["NCRNOTE"] = Convert.ToDecimal(dtrPreview["NCRNOTE"]) + decAmt + decVatAmt;
                        }
                        else if ((SysDef.gc_RFTYPE_DR_SELL + SysDef.gc_RFTYPE_DR_BUY).IndexOf(strRfType) > -1)
                        {
                            dtrPreview["NDRNOTE"] = Convert.ToDecimal(dtrPreview["NDRNOTE"]) + decAmt + decVatAmt;
                        }

                    }
                }
                dtrGLRef.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

        }

    }
}
