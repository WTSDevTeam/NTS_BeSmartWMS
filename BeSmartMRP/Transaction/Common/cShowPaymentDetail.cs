
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace BeSmartMRP.Transaction.Common
{

    public class cShowPaymentDetail
    {

        public static string xd_TemPayment = "TemHisCust";
        public static string xd_TemCredit = "TemCredit";

        private string mstrSaleOrBuy = "S";

        public cShowPaymentDetail(string inSaleOrBuy)
        {
            this.mstrSaleOrBuy = inSaleOrBuy;
            this.pmCreateTem();
        }

        private DataSet dtsDataEnv = new DataSet();

        private string mstrBranchID = "";
        private string mstrProd = "";
        private string mstrCoor = "";
        private string mstrCoorTN = "";
        private string mstrBegQcProd = "";
        private string mstrEndQcProd = "";
        private DateTime mdttBegDate = DateTime.Now;
        private DateTime mdttEndDate = DateTime.Now;

        public string BranchID
        {
            get { return this.mstrBranchID; }
            set { this.mstrBranchID = value; }
        }

        private void pmCreateTem()
        {
            DataTable dtbTemPayment = new DataTable(xd_TemPayment);
            //dtbTemPayment.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPayment.Columns.Add("cType", System.Type.GetType("System.String"));
            dtbTemPayment.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
            dtbTemPayment.Columns.Add("cQcBank", System.Type.GetType("System.String"));
            dtbTemPayment.Columns.Add("cCode", System.Type.GetType("System.String"));
            dtbTemPayment.Columns.Add("nAmt", System.Type.GetType("System.Decimal"));
            this.dtsDataEnv.Tables.Add(dtbTemPayment);


            DataTable dtbTemCredit = new DataTable(xd_TemCredit);
            //dtbTemCredit.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemCredit.Columns.Add("cYrMth", System.Type.GetType("System.String"));
            dtbTemCredit.Columns.Add("cQcCoor", System.Type.GetType("System.String"));
            dtbTemCredit.Columns.Add("cQnCoor", System.Type.GetType("System.String"));
            dtbTemCredit.Columns.Add("NPAYAMT", System.Type.GetType("System.Decimal"));
            dtbTemCredit.Columns.Add("NBUYSELL", System.Type.GetType("System.Decimal"));
            dtbTemCredit.Columns.Add("NCRNOTE", System.Type.GetType("System.Decimal"));
            dtbTemCredit.Columns.Add("NDRNOTE", System.Type.GetType("System.Decimal"));
            dtbTemCredit.Columns.Add("NBALAMT", System.Type.GetType("System.Decimal"), "NBUYSELL-NPAYAMT-NCRNOTE+NDRNOTE");
            dtbTemCredit.Columns.Add("NBALAMT2", System.Type.GetType("System.Decimal"));
            
            dtbTemCredit.Columns["NPAYAMT"].DefaultValue = 0;
            dtbTemCredit.Columns["NBUYSELL"].DefaultValue = 0;
            dtbTemCredit.Columns["NCRNOTE"].DefaultValue = 0;
            dtbTemCredit.Columns["NDRNOTE"].DefaultValue = 0;
            dtbTemCredit.Columns["NBALAMT2"].DefaultValue = 0;

            this.dtsDataEnv.Tables.Add(dtbTemCredit);
        
        }

        public void ShowPaymentDetail(string inBranch, string inCoor, string inConTactTN)
        {
            this.mstrBranchID = inBranch;
            this.mstrCoor = inCoor;
            this.mstrCoorTN = inConTactTN;
            this.pmQueryPaymentDetail();
        }

        private void pmQueryPaymentDetail()
        {
            if (this.dtsDataEnv.Tables[xd_TemPayment].Rows.Count > 0)
            {
                this.dtsDataEnv.Tables[xd_TemPayment].Rows.Clear();
            }

            if (this.dtsDataEnv.Tables[xd_TemCredit].Rows.Count > 0)
            {
                this.dtsDataEnv.Tables[xd_TemCredit].Rows.Clear();
            }

            pmGetGLRefDetail();

            if (this.dtsDataEnv.Tables[xd_TemCredit].Rows.Count > 0)
            {
                decimal decBalAmt = 0;
                foreach (DataRow dtrCal in this.dtsDataEnv.Tables[xd_TemCredit].Rows)
                {
                    decBalAmt += Convert.ToDecimal(dtrCal["nBalAmt"]);
                    dtrCal["nBalAmt2"] = decBalAmt;
                }
            }
            
            pmGetPaymentDetail("1");
            pmGetPaymentDetail("2");
            pmGetPaymentDetail("3");
            pmGetPaymentDetail("4");

            //if (this.dtsDataEnv.Tables[xd_TemPayment].Rows.Count > 0)
            //{
                dlgShowPaymentDet ofrmHistory = new dlgShowPaymentDet();
                ofrmHistory.BindData(this.dtsDataEnv, xd_TemPayment);
                ofrmHistory.ShowDialog();
            //}
        
        }

        private void pmGetPaymentDetail(string inGetType)
        {
            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

            decimal decPayAmt = 0;

            string strCriteria1 = "";
            string strStep = "";
            string strStat = "";
            string strTypeName = "";
            switch (inGetType)
            {
                // Type 1 : เช็คในมือระบุใบเสร็จ
                case "1":
                    strStep = " ";
                    strStat = " ";
                    strTypeName = "3.1 เช็คในมือระบุใบเสร็จ";
                    strCriteria1 = " and PAYMENT.FCSKID in (select FCPAYMENT from BILPAY where FCCOOR = ?) ";
                    pobjSQLUtil.SetPara(new object[] { this.mstrCoor, strStep, strStat, this.mstrCoor });
                    break;
                // Type 2 : เช็คในมือไม่ระบุใบเสร็จ
                case "2":
                    strStep = " ";
                    strStat = " ";
                    strTypeName = "3.2 เช็คในมือไม่ระบุใบเสร็จ";
                    strCriteria1 = " and PAYMENT.FCSKID not in (select FCPAYMENT from BILPAY where FCCOOR = ?) ";
                    pobjSQLUtil.SetPara(new object[] { this.mstrCoor, strStep, strStat, this.mstrCoor });
                    break;
                // Type 3 : เช็คยังไม่ผ่าน
                case "3":
                    strStep = "B";
                    strStat = " ";
                    strTypeName = "4. เช็คยังไม่ผ่าน";
                    pobjSQLUtil.SetPara(new object[] { this.mstrCoor, strStep, strStat });
                    break;
                // Type 4 : เช็คคืน
                case "4":
                    strStep = "B";
                    strStat = "R";
                    strTypeName = "5. เช็คคืน";
                    pobjSQLUtil.SetPara(new object[] { this.mstrCoor, strStep, strStat });
                    break;
            }

            string strSQLStr = "select PAYMENT.* , BANK.FCCODE as QCBANK from PAYMENT ";
            strSQLStr += " left join BANK on BANK.FCSKID = PAYMENT.FCBANK ";
            strSQLStr += " where PAYMENT.FCCOOR = ? ";
            strSQLStr += " and PAYMENT.FCPAYTYPE = 'HC' ";
            strSQLStr += " and PAYMENT.FCSTEP = ? ";
            strSQLStr += " and PAYMENT.FCSTAT = ? ";
            strSQLStr += strCriteria1;
            strSQLStr += " order by PAYMENT.FDDATE ";

            pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPayment", "PAYMENT", strSQLStr, ref strErrorMsg);
            foreach (DataRow dtrRefProd in this.dtsDataEnv.Tables["QPayment"].Rows)
            {
                DataRow dtrTemHis = this.dtsDataEnv.Tables[xd_TemPayment].NewRow();
                dtrTemHis["cType"] = strTypeName;
                //dtrTemHis["dDate"] = Convert.ToDateTime(dtrRefProd["fdDate"]);
                dtrTemHis["dDate"] = Convert.ToDateTime(dtrRefProd["fdDueDate"]);
                dtrTemHis["cQcBank"] = dtrRefProd["QcBank"].ToString();
                dtrTemHis["cCode"] = dtrRefProd["fcCode"].ToString();
                dtrTemHis["nAmt"] = Convert.ToDecimal(dtrRefProd["fnAmt"]);
                this.dtsDataEnv.Tables[xd_TemPayment].Rows.Add(dtrTemHis);
            }

        }

        private void pmGetGLRefDetail()
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
            if (this.mstrCoorTN.Trim() != string.Empty)
            {
                strSQLStr = strSQLStr + " and GLRef.fcCoor in (select fcSkid from Coor where Coor.fcCorp = ? and Coor.fcIsCust = 'Y' and Coor.fcContactN = ? and Coor.fcContactN <> '') ";
            }
            else
            {
                strSQLStr = strSQLStr + " and GLRef.fcCoor = ? ";
            }
            //strSQLStr = strSQLStr + " and GLRef.fdDate between ? and ? ";
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
                //if (this.txtQcBranch.Text.Trim() != "")
                //{
                //    cmd.Parameters.Add(ReportHelper.AddParameter(this.txtQcBranch.Tag.ToString(), DbType.String));
                //}
                if (this.mstrCoorTN.Trim() != string.Empty)
                {
                    cmd.Parameters.Add(AppUtil.ReportHelper.AddParameter(App.ActiveCorp.RowID, DbType.String));
                    cmd.Parameters.Add(AppUtil.ReportHelper.AddParameter(this.mstrCoorTN, DbType.String));
                }
                else
                {
                    cmd.Parameters.Add(AppUtil.ReportHelper.AddParameter(this.mstrCoor, DbType.String));
                }

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

                        if (strCurrPrefix != dtrGLRef["QcCoor"].ToString() + Convert.ToInt32(dtrGLRef["Yr"]).ToString("0000") + Convert.ToInt32(dtrGLRef["Mth"]).ToString("00"))
                        {
                            strCurrPrefix = dtrGLRef["QcCoor"].ToString() + Convert.ToInt32(dtrGLRef["Yr"]).ToString("0000") + Convert.ToInt32(dtrGLRef["Mth"]).ToString("00");
                            dtrPreview = this.dtsDataEnv.Tables[xd_TemCredit].NewRow();
                            this.dtsDataEnv.Tables[xd_TemCredit].Rows.Add(dtrPreview);
                        }

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
