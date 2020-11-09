using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Entity;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Component;
using DevExpress.XtraGrid.Views.Base;

namespace BeSmartMRP.DatabaseForms.Common
{
    public partial class dlgSetWorkHour : UIHelper.frmBase
    {

        public dlgSetWorkHour(DataSet inSource, string inRowID, string inCurrDate, DateTime inDate)
        {
            InitializeComponent();
            BeSmartMRP.UIHelper.UIBase.SetDefaultChildAppreance(this);

            this.pmMapEvent();

            this.pmCreateTem();
            this.pmInitGridProp_TemWkHour();
            this.pmInitGridProp_TemOTHour();

            this.mstrEditRowID = inRowID;
            this.mstrCurrDate = inCurrDate;
            this.mdttDate = inDate;

            this.dtsSource = inSource;
            if (this.mstrEditRowID.Trim() != string.Empty)
            {
                this.pmCreateTemWorkHour(true);
            }
            else
            {
                this.pmCreateTemWorkHour(false);
            }
        }


        private string mstrTemWkHour = "TemWkHour";
        private string mstrTemOTHour = "TemOTHour";
        private string mstrEditRowID = "";
        private string mstrCurrDate = "";
        private DateTime mdttDate = DateTime.Now;

        private string mstrRefTable = MapTable.Table.EMWorkCalendar;
        private string mstrITable = MapTable.Table.EMWorkCalendarItem_Range;

        private DataSet dtsDataEnv = new DataSet();
        private DataSet dtsSource = new DataSet();

        private void pmMapEvent()
        {
            //this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBase_KeyDown);

            this.gridView2.CellValueChanged += new CellValueChangedEventHandler(gridView2_CellValueChanged);
            this.gridView3.CellValueChanged += new CellValueChangedEventHandler(gridView3_CellValueChanged);

        }

        private void pmCreateTem()
        {

            DataTable dtbTemPd = new DataTable(this.mstrTemWkHour);
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cDay", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("dBegTime", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("dEndTime", System.Type.GetType("System.DateTime"));
            dtbTemPd.Columns.Add("nTotHour", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["nTotHour"].DefaultValue = 0;

            //dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemWkHour_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemPd);

            DataTable dtbTemOT = new DataTable(this.mstrTemOTHour);
            dtbTemOT.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemOT.Columns.Add("cDay", System.Type.GetType("System.String"));
            dtbTemOT.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemOT.Columns.Add("dBegTime", System.Type.GetType("System.DateTime"));
            dtbTemOT.Columns.Add("dEndTime", System.Type.GetType("System.DateTime"));
            dtbTemOT.Columns.Add("nTotHour", System.Type.GetType("System.Decimal"));

            dtbTemOT.Columns["cRowID"].DefaultValue = "";
            dtbTemOT.Columns["nTotHour"].DefaultValue = 0;

            //dtbTemOT.TableNewRow += new DataTableNewRowEventHandler(dtbTemWkHour_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemOT);

        }

        private void pmInitGridProp_TemWkHour()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemWkHour].DefaultView;

            this.grdTemWorkHour.DataSource = this.dtsDataEnv.Tables[this.mstrTemWkHour];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView2.Columns["nRecNo"].VisibleIndex = i++;
            //this.gridView2.Columns["cDay"].VisibleIndex = i++;
            this.gridView2.Columns["dBegTime"].VisibleIndex = i++;
            this.gridView2.Columns["dEndTime"].VisibleIndex = i++;
            this.gridView2.Columns["nTotHour"].VisibleIndex = i++;

            this.gridView2.Columns["nRecNo"].Caption = "No.";
            this.gridView2.Columns["dBegTime"].Caption = UIBase.GetAppUIText(new string[] { "ตั้งแต่", "Start" });
            this.gridView2.Columns["dEndTime"].Caption = UIBase.GetAppUIText(new string[] { "ถึง", "End" });
            this.gridView2.Columns["nTotHour"].Caption = UIBase.GetAppUIText(new string[] { "ชม.", "Hour" });

            this.gridView2.Columns["nRecNo"].Width = 30;
            this.gridView2.Columns["dBegTime"].Width = 80;
            this.gridView2.Columns["dEndTime"].Width = 80;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //this.gridView2.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView2.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["nTotHour"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["nTotHour"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["nTotHour"].OptionsColumn.ReadOnly = true;
            this.gridView2.Columns["nTotHour"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView2.Columns["nTotHour"].DisplayFormat.FormatString = "{0:#,###,##0.0;-#,###,##0.0; }";

            this.gridView2.Columns["dBegTime"].ColumnEdit = this.grcBegTime;
            this.gridView2.Columns["dEndTime"].ColumnEdit = this.grcEndTime;

            this.gridView2.Columns["nTotHour"].SummaryItem.DisplayFormat = "{0:#,###,##0.0;-#,###,##0.0; }";
            this.gridView2.Columns["nTotHour"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            //this.pmCalcColWidth();

        }

        private void pmInitGridProp_TemOTHour()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemOTHour].DefaultView;

            this.grdTemOTHour.DataSource = this.dtsDataEnv.Tables[this.mstrTemOTHour];

            for (int intCnt = 0; intCnt < this.gridView3.Columns.Count; intCnt++)
            {
                this.gridView3.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView3.Columns["nRecNo"].VisibleIndex = i++;
            //this.gridView3.Columns["cDay"].VisibleIndex = i++;
            this.gridView3.Columns["dBegTime"].VisibleIndex = i++;
            this.gridView3.Columns["dEndTime"].VisibleIndex = i++;
            this.gridView3.Columns["nTotHour"].VisibleIndex = i++;

            this.gridView3.Columns["nRecNo"].Caption = "No.";
            this.gridView3.Columns["dBegTime"].Caption = UIBase.GetAppUIText(new string[] { "ตั้งแต่", "Start" });
            this.gridView3.Columns["dEndTime"].Caption = UIBase.GetAppUIText(new string[] { "ถึง", "End" });
            this.gridView3.Columns["nTotHour"].Caption = UIBase.GetAppUIText(new string[] { "ชม.", "Hour" });

            this.gridView3.Columns["nRecNo"].Width = 30;
            this.gridView3.Columns["dBegTime"].Width = 80;
            this.gridView3.Columns["dEndTime"].Width = 80;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //this.gridView3.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView3.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["nTotHour"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["nTotHour"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["nTotHour"].OptionsColumn.ReadOnly = true;
            this.gridView3.Columns["nTotHour"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView3.Columns["nTotHour"].DisplayFormat.FormatString = "{0:#,###,##0.0;-#,###,##0.0; }";

            this.gridView3.Columns["dBegTime"].ColumnEdit = this.grcBegTime;
            this.gridView3.Columns["dEndTime"].ColumnEdit = this.grcEndTime;

            this.gridView3.Columns["nTotHour"].SummaryItem.DisplayFormat = "{0:#,###,##0.0000;-#,###,##0.0000; }";
            this.gridView3.Columns["nTotHour"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;

            //this.pmCalcColWidth();

        }

        private void pmCreateTemWorkHour(bool inLoadVal)
        {

            string strErrorMsg = "";
            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            this.dtsDataEnv.Tables[this.mstrTemWkHour].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTemOTHour].Rows.Clear();

            for (int j = 0; j < 5; j++)
            {
                DataRow dtrRngDate = this.dtsDataEnv.Tables[this.mstrTemWkHour].NewRow();
                dtrRngDate["nRecNo"] = j + 1;
                dtrRngDate["cDay"] = this.mstrCurrDate;

                if (inLoadVal)
                {

                    pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrEditRowID, this.mstrCurrDate, StringHelper.ConvertToBase64(Convert.ToInt32(dtrRngDate["nRecNo"]), 2) });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWKHourIT", "", "select * from " + this.mstrITable + " where cCorp = ? and cWorkCalIT = ? and cType = '' and cDay = ? and cSeq = ? ", ref strErrorMsg))
                    {
                        DataRow dtrWKHourIT = this.dtsDataEnv.Tables["QWKHourIT"].Rows[0];
                        dtrRngDate["cRowID"] = dtrWKHourIT["cRowID"].ToString();

                        DateTime dttBegTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrWKHourIT["dBegTime"]).Hour, Convert.ToDateTime(dtrWKHourIT["dBegTime"]).Minute, 0);
                        DateTime dttEndTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrWKHourIT["dEndTime"]).Hour, Convert.ToDateTime(dtrWKHourIT["dEndTime"]).Minute, 0);
                        TimeSpan tsTotHour = dttEndTime - dttBegTime;

                        dtrRngDate["dBegTime"] = dttBegTime;
                        dtrRngDate["dEndTime"] = dttEndTime;
                        dtrRngDate["nTotHour"] = tsTotHour.TotalHours;

                        var ps = from p1 in this.dtsSource.Tables[this.mstrTemWorkHr_Range].AsEnumerable()
                                 where p1["cRowID"].ToString() == dtrWKHourIT["cRowID"].ToString()
                                 select new { cRowID = p1["CROWID"] };

                        DataRow dtrAllWorkHour = null;
                        if (ps.ToList().Count > 0)
                        {

                            foreach (var psWorkRng in ps.ToList())
                            {
                                dtrAllWorkHour = pmGetDataRow(this.dtsSource, psWorkRng.cRowID.ToString());

                                dtrRngDate["dBegTime"] = Convert.ToDateTime(dtrAllWorkHour["dBegTime"]);
                                dtrRngDate["dEndTime"] = Convert.ToDateTime(dtrAllWorkHour["dEndTime"]);
                                dtrRngDate["nTotHour"] = Convert.ToDecimal(dtrAllWorkHour["nTotHour"]);

                            }

                        }

                    }

                }

                this.dtsDataEnv.Tables[this.mstrTemWkHour].Rows.Add(dtrRngDate);
            }

            for (int j = 0; j < 2; j++)
            {
                DataRow dtrRngDate = this.dtsDataEnv.Tables[this.mstrTemOTHour].NewRow();
                dtrRngDate["nRecNo"] = j + 1;
                dtrRngDate["cDay"] = this.mstrCurrDate;

                if (inLoadVal)
                {
                    pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrEditRowID, this.mstrCurrDate, StringHelper.ConvertToBase64(Convert.ToInt32(dtrRngDate["nRecNo"]), 2) });
                    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWKHourIT", "", "select * from " + this.mstrITable + " where cCorp = ? and cWorkCalIT = ? and cType = 'O' and cDay = ? and cSeq = ? ", ref strErrorMsg))
                    {
                        DataRow dtrWKHourIT = this.dtsDataEnv.Tables["QWKHourIT"].Rows[0];
                        dtrRngDate["cRowID"] = dtrWKHourIT["cRowID"].ToString();

                        DateTime dttBegTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrWKHourIT["dBegTime"]).Hour, Convert.ToDateTime(dtrWKHourIT["dBegTime"]).Minute, 0);
                        DateTime dttEndTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrWKHourIT["dEndTime"]).Hour, Convert.ToDateTime(dtrWKHourIT["dEndTime"]).Minute, 0);
                        TimeSpan tsTotHour = dttEndTime - dttBegTime;

                        dtrRngDate["dBegTime"] = dttBegTime;
                        dtrRngDate["dEndTime"] = dttEndTime;
                        dtrRngDate["nTotHour"] = tsTotHour.TotalHours;

                    }

                }

                this.dtsDataEnv.Tables[this.mstrTemOTHour].Rows.Add(dtrRngDate);
            }

            this.pmSumWorkHour();

        }

        private void pmSumWorkHour()
        {
            decimal decSumWorkHr = (from p1 in this.dtsDataEnv.Tables[this.mstrTemWkHour].AsEnumerable()
                                    select new { TotHour = p1["nTotHour"] }).Sum(p1 => Convert.ToDecimal(p1.TotHour));

            decimal decSumOTHr = (from p1 in this.dtsDataEnv.Tables[this.mstrTemOTHour].AsEnumerable()
                                  select new { TotHour = p1["nTotHour"] }).Sum(p1 => Convert.ToDecimal(p1.TotHour));

            this.txtSumWkHour.Value = decSumWorkHr;
            this.txtSumOTWkHour.Value = decSumOTHr;
        }

        private void gridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            DataRow dtrTemPd = this.gridView2.GetDataRow(e.RowHandle);
            if (e.RowHandle > -1 && dtrTemPd != null)
            {
                if (!Convert.IsDBNull(dtrTemPd["dBegTime"]) && !Convert.IsDBNull(dtrTemPd["dEndTime"]))
                {
                    DateTime dttBegTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrTemPd["dBegTime"]).Hour, Convert.ToDateTime(dtrTemPd["dBegTime"]).Minute, 0);
                    DateTime dttEndTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrTemPd["dEndTime"]).Hour, Convert.ToDateTime(dtrTemPd["dEndTime"]).Minute, 0);
                    TimeSpan tsTotHour = dttEndTime - dttBegTime;
                    dtrTemPd["nTotHour"] = tsTotHour.TotalHours;

                }
                else
                {
                    dtrTemPd["nTotHour"] = 0;
                }
                this.gridView2.UpdateSummary();
                this.pmSumWorkHour();

            }
        }

        private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            DataRow dtrTemPd = this.gridView3.GetDataRow(e.RowHandle);
            if (e.RowHandle > -1 && dtrTemPd != null)
            {
                if (!Convert.IsDBNull(dtrTemPd["dBegTime"]) && !Convert.IsDBNull(dtrTemPd["dEndTime"]))
                {
                    DateTime dttBegTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrTemPd["dBegTime"]).Hour, Convert.ToDateTime(dtrTemPd["dBegTime"]).Minute, 0);
                    DateTime dttEndTime = new DateTime(DateTime.Now.Year, 1, 1, Convert.ToDateTime(dtrTemPd["dEndTime"]).Hour, Convert.ToDateTime(dtrTemPd["dEndTime"]).Minute, 0);
                    TimeSpan tsTotHour = dttEndTime - dttBegTime;
                    dtrTemPd["nTotHour"] = tsTotHour.TotalHours;

                }
                else
                {
                    dtrTemPd["nTotHour"] = 0;
                }
                this.gridView3.UpdateSummary();
                this.pmSumWorkHour();

            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        public void SaveData(ref DataSet inSource, ref decimal ioWorkHr, ref decimal ioOTHr)
        {
            ioWorkHr = this.txtSumWkHour.Value;
            ioOTHr = this.txtSumOTWkHour.Value;
            pmSaveData(ref inSource);
        }

        private string mstrTemWorkHr_Range = "TemWorkHr_Range";
        private void pmSaveData(ref DataSet inSource)
        {

            foreach (DataRow dtrTemWkHour in this.dtsDataEnv.Tables[this.mstrTemWkHour].Rows)
            {

                #region "Process Update WorkHour"
                var ps = from p1 in inSource.Tables[this.mstrTemWorkHr_Range].AsEnumerable()
                         where p1["cRowID"].ToString() == dtrTemWkHour["cRowID"].ToString()
                         select new { cRowID = p1["CROWID"] };

                DataRow dtrAllWorkHour = null;
                bool bllIsNew = false;
                if (ps.ToList().Count > 0)
                {

                    foreach (var psWorkRng in ps.ToList())
                    {
                        if (dtrTemWkHour["cRowID"].ToString().Trim() == "")
                        {
                            dtrAllWorkHour = inSource.Tables[this.mstrTemWorkHr_Range].NewRow();
                            bllIsNew = true;
                        }
                        else
                        {
                            dtrAllWorkHour = pmGetDataRow(inSource, psWorkRng.cRowID.ToString());
                            bllIsNew = false;
                        }
                    }

                }
                else
                {
                    dtrAllWorkHour = inSource.Tables[this.mstrTemWorkHr_Range].NewRow();
                    bllIsNew = true;
                }

                if (Convert.IsDBNull(dtrTemWkHour["dBegTime"]) || Convert.IsDBNull(dtrTemWkHour["dEndTime"]))
                {
                    //TODO: To Delete Row
                    if (dtrTemWkHour["cRowID"].ToString().Trim() != string.Empty)
                    {
                        dtrAllWorkHour["cIsDel"] = "Y";
                    }
                }
                else
                {
                    dtrAllWorkHour["cIsDel"] = "";
                    dtrAllWorkHour["dDate"] = this.mdttDate;
                    dtrAllWorkHour["cRowID"] = dtrTemWkHour["cRowID"].ToString();
                    dtrAllWorkHour["cType"] = "";
                    dtrAllWorkHour["cWkHourRowID"] = this.mstrEditRowID;
                    dtrAllWorkHour["cDay"] = dtrTemWkHour["cDay"].ToString();
                    dtrAllWorkHour["nRecNo"] = Convert.ToInt32(dtrTemWkHour["nRecNo"]);
                    dtrAllWorkHour["dBegTime"] = Convert.ToDateTime(dtrTemWkHour["dBegTime"]);
                    dtrAllWorkHour["dEndTime"] = Convert.ToDateTime(dtrTemWkHour["dEndTime"]);
                    dtrAllWorkHour["nTotHour"] = Convert.ToDecimal(dtrTemWkHour["nTotHour"]);
                
                    if (bllIsNew)
                    {
                        inSource.Tables[this.mstrTemWorkHr_Range].Rows.Add(dtrAllWorkHour);
                    }

                }

                #endregion
            
            }

            foreach (DataRow dtrTemWkHour in this.dtsDataEnv.Tables[this.mstrTemOTHour].Rows)
            {

                #region "Process Update WorkHour"
                var ps = from p1 in inSource.Tables[this.mstrTemWorkHr_Range].AsEnumerable()
                         where p1["cRowID"].ToString() == dtrTemWkHour["cRowID"].ToString()
                         select new { cRowID = p1["CROWID"] };

                DataRow dtrAllWorkHour = null;
                bool bllIsNew = false;
                if (ps.ToList().Count > 0)
                {

                    foreach (var psWorkRng in ps.ToList())
                    {
                        //dtrAllWorkHour = pmGetDataRow(inSource, psWorkRng.cRowID.ToString());
                        //bllIsNew = false;
                        if (dtrTemWkHour["cRowID"].ToString().Trim() == "")
                        {
                            dtrAllWorkHour = inSource.Tables[this.mstrTemWorkHr_Range].NewRow();
                            bllIsNew = true;
                        }
                        else
                        {
                            dtrAllWorkHour = pmGetDataRow(inSource, psWorkRng.cRowID.ToString());
                            bllIsNew = false;
                        }
                    }

                }
                else
                {
                    dtrAllWorkHour = inSource.Tables[this.mstrTemWorkHr_Range].NewRow();
                    bllIsNew = true;
                }

                if (Convert.IsDBNull(dtrTemWkHour["dBegTime"]) || Convert.IsDBNull(dtrTemWkHour["dEndTime"]))
                {
                    //TODO: To Delete Row
                    if (dtrTemWkHour["cRowID"].ToString().Trim() != string.Empty)
                    {
                        dtrAllWorkHour["cIsDel"] = "Y";
                    }
                }
                else
                {
                    dtrAllWorkHour["cIsDel"] = "";
                    dtrAllWorkHour["cRowID"] = dtrTemWkHour["cRowID"].ToString();
                    dtrAllWorkHour["dDate"] = this.mdttDate;
                    dtrAllWorkHour["cType"] = "O";
                    dtrAllWorkHour["cWkHourRowID"] = this.mstrEditRowID;
                    dtrAllWorkHour["cDay"] = dtrTemWkHour["cDay"].ToString();
                    dtrAllWorkHour["nRecNo"] = Convert.ToInt32(dtrTemWkHour["nRecNo"]);
                    dtrAllWorkHour["dBegTime"] = Convert.ToDateTime(dtrTemWkHour["dBegTime"]);
                    dtrAllWorkHour["dEndTime"] = Convert.ToDateTime(dtrTemWkHour["dEndTime"]);
                    dtrAllWorkHour["nTotHour"] = Convert.ToDecimal(dtrTemWkHour["nTotHour"]);

                    if (bllIsNew)
                    {
                        inSource.Tables[this.mstrTemWorkHr_Range].Rows.Add(dtrAllWorkHour);
                    }

                }

                #endregion
            
            }


        }

        private DataRow pmGetDataRow(DataSet inSource,string inRowID)
        {
            DataRow dtrGetRow = null;
            foreach (DataRow dtrGetRow2 in inSource.Tables[this.mstrTemWorkHr_Range].Rows)
            {
                if (dtrGetRow2["cRowID"].ToString() == inRowID)
                {
                    dtrGetRow = dtrGetRow2;
                }
            }
            return dtrGetRow;
        }

    }
}
