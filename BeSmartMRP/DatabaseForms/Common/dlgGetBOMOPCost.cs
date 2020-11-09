using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BeSmartMRP.UIHelper;

namespace BeSmartMRP.DatabaseForms.Common
{
    public partial class dlgGetBOMOPCost : UIHelper.frmBase
    {
        public dlgGetBOMOPCost()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        private DataSet dtsDataEnv = null;
        private string mstrAlias = "";
        private string mstrSortKey = "";

        private string mstrTemCost = "TemCost";
        private string mstrTemCostW = "TemCostW";
        private string mstrTemCostT = "TemCostT";

        private string mstrTem1Cost = "Tem1Cost";
        private string mstrTem1CostW = "Tem1CostW";
        private string mstrTem1CostT = "Tem1CostT";

        private string mstrParent = "";

        private void grdTemPd_EmbeddedNavigator_ButtonClick(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {

        }

        private void pmInitForm()
        {
        }

        private void pmCreateTemCost(string inAlias)
        {

            DataTable dtbTemPd = new DataTable(inAlias);
            dtbTemPd.Columns.Add("cParent", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cRowID", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nRecNo", System.Type.GetType("System.Int32"));
            dtbTemPd.Columns.Add("cCostType", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("cCostName", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nAmt", System.Type.GetType("System.Decimal"));
            dtbTemPd.Columns.Add("cCostBy", System.Type.GetType("System.String"));
            dtbTemPd.Columns.Add("nDummyFld", System.Type.GetType("System.Decimal"));

            dtbTemPd.Columns["cParent"].DefaultValue = "";
            dtbTemPd.Columns["cRowID"].DefaultValue = "";
            dtbTemPd.Columns["cCostName"].DefaultValue = "";
            dtbTemPd.Columns["nAmt"].DefaultValue = 0;
            dtbTemPd.Columns["cCostBy"].DefaultValue = "HOUR";
            dtbTemPd.Columns["nDummyFld"].DefaultValue = 0;

            dtbTemPd.TableNewRow += new DataTableNewRowEventHandler(dtbTemCost_TableNewRow);
            this.dtsDataEnv.Tables.Add(dtbTemPd);
        }

        private void dtbTemCost_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["nRecNo"] = e.Row.Table.Rows.Count + 1;
        }

        public void BindData(DataSet inDataEnv, string inParent)
        {
            this.mstrParent = inParent;
            this.dtsDataEnv = inDataEnv;
            this.pmBindData();
        }

        private void pmBindData()
        {

            this.dtsDataEnv.Tables[this.mstrTem1Cost].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTem1CostW].Rows.Clear();
            this.dtsDataEnv.Tables[this.mstrTem1CostT].Rows.Clear();

            this.pmInsertTemCost(this.mstrTem1Cost);
            this.pmInsertTemCost(this.mstrTem1CostW);
            this.pmInsertTemCost(this.mstrTem1CostT);

            this.pmLoadFormData(this.mstrTemCost, this.mstrTem1Cost);
            this.pmLoadFormData(this.mstrTemCostW, this.mstrTem1CostW);
            this.pmLoadFormData(this.mstrTemCostT, this.mstrTem1CostT);

            this.pmInitGridProp_TemCost();
            this.pmInitGridProp_TemCostW();
            this.pmInitGridProp_TemCostT();
        }

        private void pmLoadFormData(string inSource, string inDest)
        {
            DataRow[] dtrSel = this.dtsDataEnv.Tables[inSource].Select("cParent = '" + this.mstrParent + "'");
            for (int i = 0; i < dtrSel.Length; i++)
            {
                DataRow dtrSource = dtrSel[i];
                this.pmUpdateTemCost(inDest, dtrSource["cCostType"].ToString(), "", dtrSource["cRowID"].ToString(), Convert.ToDecimal(dtrSource["nAmt"]), dtrSource["cCostBy"].ToString());
            }
        }

        private void pmInitGridProp_TemCost()
        {

            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTem1Cost].DefaultView;

            this.grdTemCost.DataSource = this.dtsDataEnv.Tables[this.mstrTem1Cost];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView1.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView1.Columns["cCostType"].VisibleIndex = i++;
            this.gridView1.Columns["cCostName"].VisibleIndex = i++;
            this.gridView1.Columns["nAmt"].VisibleIndex = i++;
            this.gridView1.Columns["cCostBy"].VisibleIndex = i++;

            this.gridView1.Columns["nRecNo"].Caption = "No.";
            this.gridView1.Columns["cCostType"].Caption = "Cost Type";
            this.gridView1.Columns["cCostName"].Caption = "Cost Name";
            this.gridView1.Columns["nAmt"].Caption = "Amt";
            this.gridView1.Columns["cCostBy"].Caption = "Amt. Per";

            this.gridView1.Columns["nRecNo"].Width = 30;
            this.gridView1.Columns["cCostType"].Width = 80;
            this.gridView1.Columns["cCostName"].Width = 200;
            this.gridView1.Columns["nAmt"].Width = 80;
            this.gridView1.Columns["cCostBy"].Width = 80;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //this.gridView2.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView1.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView1.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView1.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView1.Columns["cCostType"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView1.Columns["cCostType"].OptionsColumn.AllowFocus = false;
            this.gridView1.Columns["cCostType"].OptionsColumn.ReadOnly = true;

            this.gridView1.Columns["cCostName"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView1.Columns["cCostName"].OptionsColumn.AllowFocus = false;
            this.gridView1.Columns["cCostName"].OptionsColumn.ReadOnly = true;

            this.gridView1.Columns["cCostBy"].ColumnEdit = this.grcCostBy;
            this.gridView1.Columns["nAmt"].ColumnEdit = this.grcAmt;

            //this.pmCalcColWidth();
        }

        private void pmInitGridProp_TemCostW()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTem1CostW].DefaultView;

            this.grdTemCostW.DataSource = this.dtsDataEnv.Tables[this.mstrTem1CostW];

            for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
            {
                this.gridView2.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView2.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView2.Columns["cCostType"].VisibleIndex = i++;
            this.gridView2.Columns["cCostName"].VisibleIndex = i++;
            this.gridView2.Columns["nAmt"].VisibleIndex = i++;
            this.gridView2.Columns["cCostBy"].VisibleIndex = i++;

            this.gridView2.Columns["nRecNo"].Caption = "No.";
            this.gridView2.Columns["cCostType"].Caption = "Cost Type";
            this.gridView2.Columns["cCostName"].Caption = "Cost Name";
            this.gridView2.Columns["nAmt"].Caption = "Amt";
            this.gridView2.Columns["cCostBy"].Caption = "Amt. Per";

            this.gridView2.Columns["nRecNo"].Width = 30;
            this.gridView2.Columns["cCostType"].Width = 80;
            this.gridView2.Columns["cCostName"].Width = 200;
            this.gridView2.Columns["nAmt"].Width = 80;
            this.gridView2.Columns["cCostBy"].Width = 80;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //this.gridView2.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView2.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["cCostType"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["cCostType"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["cCostType"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["cCostName"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Columns["cCostName"].OptionsColumn.AllowFocus = false;
            this.gridView2.Columns["cCostName"].OptionsColumn.ReadOnly = true;

            this.gridView2.Columns["cCostBy"].ColumnEdit = this.grcCostBy;
            this.gridView2.Columns["nAmt"].ColumnEdit = this.grcAmt;

            //this.pmCalcColWidth();
        }

        private void pmInitGridProp_TemCostT()
        {
            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTem1CostT].DefaultView;

            this.grdTemCostT.DataSource = this.dtsDataEnv.Tables[this.mstrTem1CostT];

            for (int intCnt = 0; intCnt < this.gridView3.Columns.Count; intCnt++)
            {
                this.gridView3.Columns[intCnt].Visible = false;
            }

            int i = 0;
            this.gridView3.Columns["nRecNo"].VisibleIndex = i++;
            this.gridView3.Columns["cCostType"].VisibleIndex = i++;
            this.gridView3.Columns["cCostName"].VisibleIndex = i++;
            this.gridView3.Columns["nAmt"].VisibleIndex = i++;
            this.gridView3.Columns["cCostBy"].VisibleIndex = i++;

            this.gridView3.Columns["nRecNo"].Caption = "No.";
            this.gridView3.Columns["cCostType"].Caption = "Cost Type";
            this.gridView3.Columns["cCostName"].Caption = "Cost Name";
            this.gridView3.Columns["nAmt"].Caption = "Amt";
            this.gridView3.Columns["cCostBy"].Caption = "Amt. Per";

            this.gridView3.Columns["nRecNo"].Width = 30;
            this.gridView3.Columns["cCostType"].Width = 80;
            this.gridView3.Columns["cCostName"].Width = 200;
            this.gridView3.Columns["nAmt"].Width = 80;
            this.gridView3.Columns["cCostBy"].Width = 80;

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

            //this.gridView3.Columns["nRecNo"].AppearanceCell.Options. = DevExpress.Utils.HorzAlignment.Far;
            this.gridView3.Columns["nRecNo"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["nRecNo"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["nRecNo"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["cCostType"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["cCostType"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["cCostType"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["cCostName"].AppearanceCell.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView3.Columns["cCostName"].OptionsColumn.AllowFocus = false;
            this.gridView3.Columns["cCostName"].OptionsColumn.ReadOnly = true;

            this.gridView3.Columns["cCostBy"].ColumnEdit = this.grcCostBy;
            this.gridView3.Columns["nAmt"].ColumnEdit = this.grcAmt;

            //this.pmCalcColWidth();
        }

        private void pmInsertTemCost(string inAlias)
        {
            string strCostName_FIX = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_FIX) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_FIX) : "Fix Cost");
            string strCostName_VAR1 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM1) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM1) : "Variable Cost per Man-hour 1");
            string strCostName_VAR2 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM2) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM2) : "Variable Cost per Man-hour 2");
            string strCostName_VAR3 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM3) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM3) : "Variable Cost per Man-hour 3");
            string strCostName_VAR4 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM4) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM4) : "Variable Cost per Man-hour 4");
            string strCostName_VAR5 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM5) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARM5) : "Variable Cost per Man-hour 5");
            string strCostName_VARP1 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP1) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP1) : "Variable Cost per Product output 1");
            string strCostName_VARP2 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP2) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP2) : "Variable Cost per Product output 2");
            string strCostName_VARP3 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP3) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP3) : "Variable Cost per Product output 3");
            string strCostName_VARP4 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP4) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP4) : "Variable Cost per Product output 4");
            string strCostName_VARP5 = (UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP5) != string.Empty ? UIBase.GetCostText(SysDef.gc_REF_OP_COSTTYPE_VARP5) : "Variable Cost per Product output 5");

            this.pmUpdateTemCost(inAlias, SysDef.gc_REF_OP_COSTTYPE_FIX, strCostName_FIX, "", 0, SysDef.gc_COST_UNIT_NONE);
            this.pmUpdateTemCost(inAlias, SysDef.gc_REF_OP_COSTTYPE_VARM1, strCostName_VAR1, "", 0, SysDef.gc_COST_UNIT_HOUR);
            this.pmUpdateTemCost(inAlias, SysDef.gc_REF_OP_COSTTYPE_VARM2, strCostName_VAR2, "", 0, SysDef.gc_COST_UNIT_HOUR);
            this.pmUpdateTemCost(inAlias, SysDef.gc_REF_OP_COSTTYPE_VARM3, strCostName_VAR3, "", 0, SysDef.gc_COST_UNIT_HOUR);
            this.pmUpdateTemCost(inAlias, SysDef.gc_REF_OP_COSTTYPE_VARM4, strCostName_VAR4, "", 0, SysDef.gc_COST_UNIT_HOUR);
            this.pmUpdateTemCost(inAlias, SysDef.gc_REF_OP_COSTTYPE_VARM5, strCostName_VAR5, "", 0, SysDef.gc_COST_UNIT_HOUR);

            this.pmUpdateTemCost(inAlias, SysDef.gc_REF_OP_COSTTYPE_VARP1, strCostName_VARP1, "", 0, SysDef.gc_COST_UNIT_NONE);
            this.pmUpdateTemCost(inAlias, SysDef.gc_REF_OP_COSTTYPE_VARP2, strCostName_VARP2, "", 0, SysDef.gc_COST_UNIT_NONE);
            this.pmUpdateTemCost(inAlias, SysDef.gc_REF_OP_COSTTYPE_VARP3, strCostName_VARP3, "", 0, SysDef.gc_COST_UNIT_NONE);
            this.pmUpdateTemCost(inAlias, SysDef.gc_REF_OP_COSTTYPE_VARP4, strCostName_VARP4, "", 0, SysDef.gc_COST_UNIT_NONE);
            this.pmUpdateTemCost(inAlias, SysDef.gc_REF_OP_COSTTYPE_VARP5, strCostName_VARP5, "", 0, SysDef.gc_COST_UNIT_NONE);

        }

        private void pmUpdateTemCost(string inAlias, string inType, string inCostName, string inRowID, decimal inAmt, string inCostPer)
        {
            DataRow[] dtrSel = this.dtsDataEnv.Tables[inAlias].Select("cCostType='" + inType + "'");
            DataRow dtrNewRow = null;
            if (dtrSel.Length == 0)
            {
                dtrNewRow = this.dtsDataEnv.Tables[inAlias].NewRow();
                this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrNewRow);
            }
            else
            {
                dtrNewRow = dtrSel[0];
            }
            dtrNewRow["cParent"] = this.mstrParent;
            dtrNewRow["cRowID"] = inRowID;
            dtrNewRow["cCostType"] = inType;
            if (inCostName.Trim() != string.Empty)
            {
                dtrNewRow["cCostName"] = inCostName;
            }
            dtrNewRow["nAmt"] = inAmt;
            dtrNewRow["cCostBy"] = inCostPer;
        }

        private void pmSaveToTemCost()
        {
            this.pmSaveToTem1Cost(this.mstrTem1Cost, this.mstrTemCost);
            this.pmSaveToTem1Cost(this.mstrTem1CostW, this.mstrTemCostW);
            this.pmSaveToTem1Cost(this.mstrTem1CostT, this.mstrTemCostT);
        }

        private void pmSaveToTem1Cost(string inSource, string inDest)
        {
            foreach (DataRow dtrSource in this.dtsDataEnv.Tables[inSource].Rows)
            {
                this.pmUpdateToTemCost(inDest, dtrSource);
            }
        }

        private void pmUpdateToTemCost(string inAlias, DataRow inSource)
        {
            DataRow[] dtrSel = this.dtsDataEnv.Tables[inAlias].Select("cParent = '" + this.mstrParent + "' and cCostType = '" + inSource["cCostType"].ToString() + "'");
            DataRow dtrNewRow = null;
            if (dtrSel.Length == 0)
            {
                dtrNewRow = this.dtsDataEnv.Tables[inAlias].NewRow();
                this.dtsDataEnv.Tables[inAlias].Rows.Add(dtrNewRow);
            }
            else
            {
                dtrNewRow = dtrSel[0];
            }
            dtrNewRow["cParent"] = this.mstrParent;
            dtrNewRow["cRowID"] = inSource["cRowID"].ToString();
            dtrNewRow["cCostType"] = inSource["cCostType"].ToString();
            dtrNewRow["cCostName"] = inSource["cCostType"].ToString();
            dtrNewRow["nAmt"] = Convert.ToDecimal(inSource["nAmt"]);
            dtrNewRow["cCostBy"] = inSource["cCostBy"].ToString();
        }

        private void barManager1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (e.Item.Tag.ToString())
            {
                case "ENTER":
                    this.pmSaveToTemCost();
                    this.DialogResult = DialogResult.OK;
                    break;
                case "EXIT":
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }

        }

        private void dlgGetBOMOPCost_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel;
                    break;
            }
        }
    
    }
}
