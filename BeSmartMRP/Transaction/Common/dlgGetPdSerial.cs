using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AppUtil;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Entity;

namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgGetPdSerial : UIHelper.frmBase
    {
        public dlgGetPdSerial()
        {
            InitializeComponent();
        }

		//private frmWHLoca pofrmGetWHLoca = null;

		private DataSet dtsDataEnv = null;
		private string mstrParentID = "";
		private string mstrSaleOrBuy = "S";
		private int intActiveRow = 0;
		private bool mbllCanSave = false;
		private string mstrCtrlStock = "";

		private string mstrBranch = "";
		private string mstrProd = "";
		private string mstrWHouse = "";
		private string mstrWHLoca = "";
		private string mstrLot = "";
		DataRowView mdtvActiveRow = null;

		private string mstrSeekVal = "";

		private bool mbllIsCRNote = false;
		public bool IsCRNote
		{
			set { this.mbllIsCRNote = value; }
			get { return this.mbllIsCRNote; }
		}

		public string WHLocaID
		{
			get { return this.mstrWHLoca; }
			set { this.mstrWHLoca = value; }
		}

		public bool IsQuickInsert
		{
			set
			{
				//this.uiCommandBar1.Commands["QInsert"].Visible = ((bool)value == true ? Janus.Windows.UI.InheritableBoolean.True : Janus.Windows.UI.InheritableBoolean.False);
			}
		}

		public string SaleOrBuy
		{
			set
			{
				this.mstrSaleOrBuy = value;
				if (this.mstrSaleOrBuy == "S")
				{
					//this.uiCommandBar1.Commands["RUN"].Visible = Janus.Windows.UI.InheritableBoolean.False;
				}
			}
		}

		public void BindData(DataSet inDataEnv, int inParent, string inProd, string inWHouse, string inWHLoca, string inLot)
		{

			WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
			WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

			this.mstrSeekVal = "";

			string strErrorMsg = "";
			this.mstrParentID = inParent.ToString("00000");
			this.dtsDataEnv = inDataEnv;
			this.mstrProd = inProd;
			this.mstrWHouse = inWHouse;
			this.mstrWHLoca = inWHLoca;

			pobjSQLUtil.SetPara(new object[] { this.mstrWHouse });
			if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QWHouse", "WHOUSE", "select fcBranch from WHOUSE where fcSkid = ?", ref strErrorMsg))
			{
				this.mstrBranch = this.dtsDataEnv.Tables["QWHouse"].Rows[0]["fcBranch"].ToString();
			}

			//pobjSQLUtil2.SetPara(new object[] { inWHLoca });
			//if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QWHLoca", "WHLOCA", "select cCode from WHLoca where cRowID = ?", ref strErrorMsg))
			//{
			//	this.txtQcWHLoca.RowID = inWHLoca;
			//	this.txtQcWHLoca.Text = this.dtsDataEnv.Tables["QWHLoca"].Rows[0]["cCode"].ToString();
			//}

			//this.mstrCtrlStock = BizRule.GetProdCtrlStock(pobjSQLUtil, this.mstrProd, App.ActiveCorp.SCtrlStock);

			this.mstrLot = inLot;
			this.mbllCanSave = true;
			this.pmQueryRefToPdSer();
			this.pmBindData();
		}

		public int CountTRefPdX3(DataSet inDataEnv, int inParent)
		{
			this.mstrParentID = inParent.ToString("00000");
			this.dtsDataEnv = inDataEnv;
			DataRow[] dtrTemPdSer = this.dtsDataEnv.Tables[PdSer.xd_Alias_TemPdSer].Select("cParentID = '" + this.mstrParentID + "' and cIsDelete <> 'Y' ");
			return dtrTemPdSer.Length;
		}

		public bool CheckDupPdSer(DataSet inDataEnv, int inParent, string inProd, ref string ioErrorMsg)
		{
			return this.pmChkDupPdSer(inDataEnv, inParent, inProd, ref ioErrorMsg);
		}

		private bool pmChkDupPdSer(DataSet inDataEnv, int inParent, string inProd, ref string ioErrorMsg)
		{
			bool bllResult = true;
			if (this.mstrSaleOrBuy == "P")
			{

				this.mstrProd = inProd;
				this.mstrParentID = inParent.ToString("00000");
				this.dtsDataEnv = inDataEnv;
				DataRow[] dtrTemPdSer = this.dtsDataEnv.Tables[PdSer.xd_Alias_TemPdSer].Select("cParentID = '" + this.mstrParentID + "' and cIsDelete <> 'Y' ");
				for (int intCnt = 0; intCnt < dtrTemPdSer.Length; intCnt++)
				{

					DataRow dtrPdSerRow = dtrTemPdSer[intCnt];
					if (dtrPdSerRow["cIsDelete"].ToString() != "Y")
					{
						bllResult = !this.pmIsDupPdSer(this.mstrProd, dtrPdSerRow["cQcPdSer"].ToString(), dtrPdSerRow["cPdSer"].ToString(), ref ioErrorMsg);
						break;
					}

				}

			}
			return bllResult;
		}

		private bool pmIsDupPdSer(string inProd, string inQcPdSer, string inPdSer, ref string ioErrorMsg)
		{

			bool bllResult = false;
			string strErrorMsg = "";
			WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

			string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
			string strProdTab = strFMDBName + ".dbo.PROD";

			string strSQLStr = "";
			strSQLStr = "select XAPDSER.CROWID , PROD.FCCODE AS QCPROD , PROD.FCNAME AS QNPROD FROM XAPDSER ";
			strSQLStr += " left join " + strProdTab + " PROD ON XAPDSER.CPROD = PROD.FCSKID ";
			strSQLStr += " where XAPDSER.CCORP = ? AND XAPDSER.CPROD = ? AND XAPDSER.CCODE = ? ";
			strSQLStr += " and XAPDSER.CSSTEP = ? ";
			strSQLStr += " and XAPDSER.CPSTEP <> ? ";

			pobjSQLUtil.NotUpperSQLExecString = true;
			pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, inProd, inQcPdSer, BusinessEnum.gc_PDSER_SSTEP_FREE, BusinessEnum.gc_PDSER_PSTEP_RETURN });
			if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdSer", MapTable.Table.PdSer, strSQLStr, ref strErrorMsg))
			{
				DataRow dtrQPdSer = this.dtsDataEnv.Tables["QPdSer"].Rows[0];
				if (inPdSer != dtrQPdSer["cRowID"].ToString())
				{
					ioErrorMsg = "สินค้า : (" + dtrQPdSer["QcProd"].ToString().TrimEnd() + ") " + dtrQPdSer["QnProd"].ToString().TrimEnd() + "\r\n Serial : " + inQcPdSer + "\r\n ข้อมูลซ้ำ !";
					bllResult = true;
				}
			}
			return bllResult;

		}

		private bool XXXpmIsDupPdSer(string inProd, string inQcPdSer, string inPdSer, ref string ioErrorMsg)
		{

			bool bllResult = false;
			string strErrorMsg = "";
			WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

			string strSQLStr = "";
			strSQLStr = "select PdSer.fcSkid , Prod.fcCode as QcProd , Prod.fcName as QnProd from PdSer ";
			strSQLStr += " left join Prod on PdSer.fcProd = Prod.fcSkid ";
			strSQLStr += " where PdSer.fcCorp = ? and PdSer.fcProd = ? and PdSer.fcCode = ?";
			strSQLStr += " and PdSer.fcSStep = ? ";
			strSQLStr += " and PdSer.fcPStep <> ? ";

			pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, inProd, inQcPdSer, BusinessEnum.gc_PDSER_SSTEP_FREE, BusinessEnum.gc_PDSER_PSTEP_RETURN });
			if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdSer", MapTable.Table.PdSer, strSQLStr, ref strErrorMsg))
			{
				DataRow dtrQPdSer = this.dtsDataEnv.Tables["QPdSer"].Rows[0];
				if (inPdSer != dtrQPdSer["fcSkid"].ToString())
				{
					ioErrorMsg = "สินค้า : (" + dtrQPdSer["QcProd"].ToString().TrimEnd() + ") " + dtrQPdSer["QnProd"].ToString().TrimEnd() + "\r\n Serial : " + inQcPdSer + "\r\n ข้อมูลซ้ำ !";
					bllResult = true;
				}
			}
			return bllResult;

		}

		//28/6/51 สำหรับใบลดหนี้
		private bool pmIsDupPdSer2(string inProd, string inQcPdSer, string inPdSer, ref string ioErrorMsg)
		{

			bool bllResult = false;
			string strErrorMsg = "";
			WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

			string strSQLStr = "";
			strSQLStr = "select PdSer.fcSkid , Prod.fcCode as QcProd , Prod.fcName as QnProd from PdSer ";
			strSQLStr += " left join Prod on PdSer.fcProd = Prod.fcSkid ";
			strSQLStr += " where PdSer.fcCorp = ? and PdSer.fcProd = ? and PdSer.fcCode = ?";
			strSQLStr += " and PdSer.fcSStep <> ? ";
			strSQLStr += " and PdSer.fcPStep <> ? ";

			pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, inProd, inQcPdSer, BusinessEnum.gc_PDSER_SSTEP_INV, BusinessEnum.gc_PDSER_PSTEP_RETURN });
			if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdSer", MapTable.Table.PdSer, strSQLStr, ref strErrorMsg))
			{
				DataRow dtrQPdSer = this.dtsDataEnv.Tables["QPdSer"].Rows[0];
				if (inPdSer != dtrQPdSer["fcSkid"].ToString())
				{
					ioErrorMsg = "สินค้า : (" + dtrQPdSer["QcProd"].ToString().TrimEnd() + ") " + dtrQPdSer["QnProd"].ToString().TrimEnd() + "\r\n Serial : " + inQcPdSer + "\r\n ข้อมูลซ้ำ !";
					bllResult = true;
				}
			}
			return bllResult;

		}

		private void pmBindData()
		{
			DataRow[] dtrTemPdSer = this.dtsDataEnv.Tables[PdSer.xd_Alias_TemPdSer].Select("cParentID = '" + this.mstrParentID + "'");
			this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].Rows.Clear();
			if (dtrTemPdSer.Length > 0)
			{
				foreach (DataRow dtrTem1PdSer in dtrTemPdSer)
				{
					DataRow dtrNewRow = this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].NewRow();
					DataSetHelper.CopyDataRow(dtrTem1PdSer, ref dtrNewRow);
					dtrNewRow["cLastQcPdSer"] = dtrNewRow["cQcPdSer"].ToString();
					this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].Rows.Add(dtrNewRow);

					foreach (DataRow dtrChk in this.dtsDataEnv.Tables[PdSer.xd_Alias_QXaPdSer].Rows)
					{
						if (dtrChk["cIsDelete"].ToString() != "Y"
							&& dtrChk["cCode"].ToString().Trim() == dtrNewRow["cQcPdSer"].ToString().Trim()
							&& dtrNewRow["cIsDelete"].ToString() != "Y")
						{
							dtrChk["IsCheck"] = true;
							dtrChk["cIsDelete"] = "Y";
							break;
						}
					}


				}
			}
			this.pmInitGridProp();
		}

		private void pmInitGridProp()
		{

			////this.dataGrid1.DataSource = this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer];
			//this.grdTemPdSer.SetDataBinding(this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer], "");
			//this.grdTemPdSer.RetrieveStructure();

			//for (int intCnt = 0; intCnt < this.grdTemPdSer.RootTable.Columns.Count; intCnt++)
			//{
			//	this.grdTemPdSer.RootTable.Columns[intCnt].Visible = false;
			//}

			//this.grdTemPdSer.RootTable.HeaderFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			//this.grdTemPdSer.RootTable.Columns["cQcPdSer"].Visible = true;

			//this.grdTemPdSer.RootTable.Columns["cQcPdSer"].Caption = "Serial No.";
			//this.grdTemPdSer.ColumnAutoResize = true;

			//this.grdTemPdSer.RootTable.SortKeys.Add(new Janus.Windows.GridEX.GridEXSortKey(this.grdTemPdSer.RootTable.Columns["cQcPdSer"], Janus.Windows.GridEX.SortOrder.Ascending));
			//this.grdTemPdSer.RootTable.Columns["cQcPdSer"].CellStyle.BackColor = Color.WhiteSmoke;

			if (this.mstrSaleOrBuy == "S")
			{
				//this.grdTemPdSer.RootTable.Columns["cQcPdSer"].ButtonStyle = Janus.Windows.GridEX.ButtonStyle.Image;
				//this.grdTemPdSer.RootTable.Columns["cQcPdSer"].ButtonImage = this.imgGridPic.Images[0];
				//this.grdTemPdSer.RootTable.Columns["cQcPdSer"].ButtonDisplayMode = Janus.Windows.GridEX.CellButtonDisplayMode.CurrentCell;
			}

			this.pmFilterRow();

			this.grdTemPdSer.Focus();
		}

		private void pmRetrievePopUpVal(string inPopupForm)
		{

			switch (inPopupForm.TrimEnd().ToUpper())
			{
				case "WHLOCA":
					//ERPSys.Business.Entity.Info.WHLocaInfo mWHLocaInfo = this.pofrmGetWHLoca.RetrieveValue();
					//if (mWHLocaInfo != null)
					//{
					//	if (this.txtQcWHLoca.RowID != mWHLocaInfo.RowID)
					//	{
					//		this.txtQcWHLoca.RowID = mWHLocaInfo.RowID;
					//		this.mstrWHLoca = mWHLocaInfo.RowID;
					//		this.pmClrItem();
					//		this.pmQueryRefToPdSer();
					//	}
					//	this.txtQcWHLoca.Text = mWHLocaInfo.Code.TrimEnd();
					//}
					//else
					//{
					//	this.txtQcWHLoca.RowID = "";
					//	this.txtQcWHLoca.Text = "";
					//	this.mstrWHLoca = "";
					//	this.pmClrItem();
					//	this.pmQueryRefToPdSer();
					//}
					break;
			}
		}

		private void txtPopup_ButtonClick(object sender, System.EventArgs e)
		{
			//WealthTech.Windows.Forms.WsEditBox txtPopup = (WealthTech.Windows.Forms.WsEditBox)sender;
			//this.pmPopUpButtonClick(txtPopup.Name.ToUpper());
		}

		private void pmPopUpButtonClick(string inTextbox)
		{
			//string strOrderBy = "";
			//switch (inTextbox)
			//{
			//	case "TXTQCWHLOCA":
			//	case "TXTQNWHLOCA":
			//		strOrderBy = (inTextbox.ToUpper() == "TXTQCWHLOCA" ? WHLoca.Field.Code : WHLoca.Field.Name);
			//		this.pmInitPopUpDialog("WHLOCA");
			//		this.pofrmGetWHLoca.ValidateField(this.mstrBranch, this.mstrWHouse, "", strOrderBy, true);
			//		if (this.pofrmGetWHLoca.PopUpResult)
			//			this.pmRetrievePopUpVal("WHLOCA");
			//		break;
			//}
		}

		private void txtPopup_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			//if (e.Alt | e.Control | e.Shift)
			//	return;

			//switch (e.KeyCode)
			//{
			//	case Keys.F3:
			//		WealthTech.Windows.Forms.WsEditBox txtPopup = (WealthTech.Windows.Forms.WsEditBox)sender;
			//		this.pmPopUpButtonClick(txtPopup.Name.ToUpper());
			//		break;
			//}
		}

		private void pmClrItem()
		{
			foreach (DataRow dtrTem1PdSer in this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].Rows)
			{

				//App.DebugEventsLog("PDSER_CLRITEM", App.Debug_Doc_RefType , App.Debug_Doc_QcBook , App.Debug_Doc_RefNo , App.Debug_Doc_Date , App.Debug_Doc_QcProd , App.Debug_Doc_QcWHouse ,  App.Debug_Doc_Lot , dtrTem1PdSer["cQcPdSer"].ToString() );

				dtrTem1PdSer["cQcPdSer"] = "";
				dtrTem1PdSer["cIsDelete"] = "Y";
			}
			//this.grdTemPdSer.Refetch();
			//this.grdTemPdSer.Refresh();
		}

		private bool pmHasDupPdSerial()
		{
			bool bllResult = false;
			foreach (DataRow dtrTem1PdSer in this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].Rows)
			{
				string strCode = dtrTem1PdSer["cQcPdSer"].ToString();
				if (strCode.TrimEnd() == string.Empty)
					continue;

				DataRow[] dtrTemPdSer = this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].Select("cQcPdSer = '" + strCode + "' and cIsDelete <> 'Y' ");
				if (dtrTemPdSer.Length > 1)
				{
					bllResult = true;

					//App.DebugEventsLog("PDSER_DUPITEM", App.Debug_Doc_RefType, App.Debug_Doc_QcBook, App.Debug_Doc_RefNo, App.Debug_Doc_Date, App.Debug_Doc_QcProd, App.Debug_Doc_QcWHouse, App.Debug_Doc_Lot, dtrTemPdSer[0]["cQcPdSer"].ToString());

					MessageBox.Show("SERAIL ซ้ำกรุณาตรวจสอบข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					break;
				}
			}
			return bllResult;
		}

		private bool pmHasDupPdSer()
		{
			bool bllResult = false;
			foreach (DataRow dtrTem1PdSer in this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].Rows)
			{
				string strCode = dtrTem1PdSer["cQcPdSer"].ToString();
				if (strCode.TrimEnd() == string.Empty)
					continue;

				DataRow[] dtrTemPdSer = this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].Select("cQcPdSer = '" + strCode + "' and cIsDelete <> 'Y' ");
				if (dtrTemPdSer.Length > 1)
				{
					bllResult = true;

					//App.DebugEventsLog("PDSER_HASDUPITEM", App.Debug_Doc_RefType, App.Debug_Doc_QcBook, App.Debug_Doc_RefNo, App.Debug_Doc_Date, App.Debug_Doc_QcProd, App.Debug_Doc_QcWHouse, App.Debug_Doc_Lot, dtrTemPdSer[0]["cQcPdSer"].ToString());

					MessageBox.Show("SERAIL ซ้ำกรุณาตรวจสอบข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					break;
				}
			}
			return bllResult;
		}

		private void dlgGetPdSerial_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
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

		private bool pmSaveData()
		{
			string strRowID = "";
			bool bllAddNew = false;
			DataRow dtrPdSerRow = null;

			//Loop จาก Tem1PdSer เพื่อนำไปเก็บใน TemPdSer
			foreach (DataRow dtrTem1PdSer in this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].Rows)
			{
				if (dtrTem1PdSer["cRowID"].ToString() == string.Empty)
				{
					bllAddNew = true;
					strRowID = Convert.ToInt32(dtrTem1PdSer["nRunRow"]).ToString("000000");
					dtrPdSerRow = this.dtsDataEnv.Tables[PdSer.xd_Alias_TemPdSer].NewRow();
					dtrPdSerRow["cRowID"] = strRowID;
				}
				else
				{
					bllAddNew = false;
					DataRow[] dtrTemPdX = this.dtsDataEnv.Tables[PdSer.xd_Alias_TemPdSer].Select("cRowID = '" + dtrTem1PdSer["cRowID"].ToString() + "'");
					dtrPdSerRow = dtrTemPdX[0];
				}

				dtrPdSerRow["cQcPdSer"] = dtrTem1PdSer["cQcPdSer"].ToString();
				dtrPdSerRow["cParentID"] = this.mstrParentID;
				dtrPdSerRow["cProd"] = dtrTem1PdSer["cProd"].ToString();
				dtrPdSerRow["cPdSer"] = dtrTem1PdSer["cPdSer"].ToString();
				dtrPdSerRow["cQc2PdSer"] = dtrTem1PdSer["cQc2PdSer"].ToString();
				dtrPdSerRow["cRefPdX3"] = dtrTem1PdSer["cRefPdX3"].ToString();
				dtrPdSerRow["cPStep"] = dtrTem1PdSer["cPStep"].ToString();
				dtrPdSerRow["cSStep"] = dtrTem1PdSer["cSStep"].ToString();
				dtrPdSerRow["cFrWhouse"] = dtrTem1PdSer["cFrWhouse"].ToString();
				dtrPdSerRow["cIsDelete"] = dtrTem1PdSer["cIsDelete"].ToString();
				dtrPdSerRow["nQty"] = Convert.ToDecimal(dtrTem1PdSer["nQty"]);
				dtrPdSerRow["nUmQty"] = Convert.ToDecimal(dtrTem1PdSer["nUmQty"]);

				if (dtrTem1PdSer["cQcPdSer"].ToString().TrimEnd() == "")
				{
					dtrPdSerRow["cIsDelete"] = "Y";
				}

				if (bllAddNew)
				{
					this.dtsDataEnv.Tables[PdSer.xd_Alias_TemPdSer].Rows.Add(dtrPdSerRow);
				}
			}
			return true;
		}

		private void pmDelItem()
		{
			//if (this.grdTemPdSer.SelectedItems.Count > 0)
			//{
			//	foreach (Janus.Windows.GridEX.GridEXSelectedItem grdPdSer in this.grdTemPdSer.SelectedItems)
			//	{
			//		if (grdPdSer != null)
			//		{
			//			Janus.Windows.GridEX.GridEXRow mRow = grdPdSer.GetRow();
			//			DataRowView dtrPdSer = (DataRowView)mRow.DataRow;

			//			if (dtrPdSer == null)
			//				continue;

			//			dtrPdSer["cIsDelete"] = "Y";
			//			this.grdTemPdSer.UpdateData();

			//			if (dtrPdSer["cQcPdSer"] != null)
			//			{
			//				//App.DebugEventsLog("PDSER_DELITEM", App.Debug_Doc_RefType , App.Debug_Doc_QcBook , App.Debug_Doc_RefNo , App.Debug_Doc_Date , App.Debug_Doc_QcProd , App.Debug_Doc_QcWHouse ,  App.Debug_Doc_Lot , dtrPdSer["cQcPdSer"].ToString() );
			//			}

			//			foreach (DataRow dtrUpdPdSer in this.dtsDataEnv.Tables[PdSer.xd_Alias_QXaPdSer].Rows)
			//			{
			//				if (dtrUpdPdSer["cCode"].ToString().TrimEnd() == dtrPdSer["cQcPdSer"].ToString().TrimEnd())
			//				{
			//					dtrUpdPdSer["cIsDelete"] = "N";
			//					dtrUpdPdSer["IsCheck"] = false;

			//					break;
			//				}
			//			}
			//		}

			//	}
			//}
			//this.grdTemPdSer.Refetch();
		}

		private bool mbllIsFilter = false;
		private void pmFilterRow()
		{
			//Janus.Windows.GridEX.GridEXFilterCondition filter;
			//filter = new Janus.Windows.GridEX.GridEXFilterCondition(this.grdTemPdSer.RootTable.Columns["cIsDelete"], Janus.Windows.GridEX.ConditionOperator.NotEqual, "Y");
			////filter = new Janus.Windows.GridEX.GridEXFilterCondition(this.grdTemPdSer.RootTable.Columns["cQcPdSer"], Janus.Windows.GridEX.ConditionOperator.NotEqual, "");
			//this.grdTemPdSer.RootTable.FilterCondition = filter;
			//this.mbllIsFilter = true;
		}

		private bool mbllPopUpResult = false;

		private void pmInitPopUpDialog(string inDialogName)
		{
			switch (inDialogName.TrimEnd().ToUpper())
			{
				case "WHLOCA":
					//if (this.pofrmGetWHLoca == null)
					//{
					//	this.pofrmGetWHLoca = new frmWHLoca(UIHelper.FormActiveMode.PopUp);
					//	this.pofrmGetWHLoca.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetWHLoca.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
					//}
					break;
				case "PDSER_ITEM":
					//using (TransactionForms.Common.dlgSelPdSerial dlg = new TransactionForms.Common.dlgSelPdSerial())
					//{
					//	//DataRow dtrTemPd = this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].Rows[this.grdTemPdSer.Row];

					//	this.mbllPopUpResult = false;
					//	//this.pmQueryRefToPdSer();
					//	dlg.BindData(this.dtsDataEnv);
					//	dlg.ShowDialog();
					//	if (dlg.DialogResult == DialogResult.OK)
					//	{
					//		if (this.grdTemPdSer.SelectedItems.Count > 0)
					//		{
					//			//foreach (Janus.Windows.GridEX.GridEXSelectedItem grdPdSer in dlg.SelectedItems)
					//			//{
					//			//	Janus.Windows.GridEX.GridEXRow mRow = grdPdSer.GetRow();
					//			//	DataRowView dtrPdSer = (DataRowView)mRow.DataRow;
					//			//	DataRow dtrNewTemPd = this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].NewRow();
					//			//	dtrNewTemPd["cPdSer"] = dtrPdSer["cPdSer"].ToString();
					//			//	dtrNewTemPd["cQcPdSer"] = dtrPdSer["cCode"].ToString();
					//			//	dtrNewTemPd["cLastQcPdSer"] = dtrPdSer["cCode"].ToString();
					//			//	this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].Rows.Add(dtrNewTemPd);
					//			//}
					//			this.mbllPopUpResult = dlg.PopUpResult;
					//			bool bllIsFrist = false;
					//			bool bllIsNewRow = false;
					//			foreach (DataRow dtrPdSer in this.dtsDataEnv.Tables[PdSer.xd_Alias_QXaPdSer].Rows)
					//			{
					//				DataRow dtrNewTemPd = null;
					//				if (dtrPdSer["cIsDelete"].ToString() != "Y" && Convert.ToBoolean(dtrPdSer["IsCheck"]) == true)
					//				{
					//					if (this.mdtvActiveRow != null && bllIsFrist == false)
					//					{
					//						dtrNewTemPd = this.mdtvActiveRow.Row;
					//						bllIsFrist = true;
					//					}
					//					else
					//					{
					//						dtrNewTemPd = this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].NewRow();
					//						bllIsNewRow = true;
					//					}

					//					dtrPdSer["cIsDelete"] = "Y";    //Hide Row

					//					if (this.mstrWHLoca.TrimEnd() == string.Empty)
					//					{
					//						//this.mstrWHLoca = dtrPdSer["cWHLoca"].ToString();
					//						//this.txtQcWHLoca.RowID = dtrPdSer["cWHLoca"].ToString();
					//						//this.txtQcWHLoca.Text = dtrPdSer["cQcWHLoca"].ToString();
					//					}

					//					dtrNewTemPd["cPdSer"] = dtrPdSer["cPdSer"].ToString();
					//					dtrNewTemPd["cQcPdSer"] = dtrPdSer["cCode"].ToString();
					//					dtrNewTemPd["cLastQcPdSer"] = dtrPdSer["cCode"].ToString();
					//					if (bllIsNewRow)
					//					{
					//						this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].Rows.Add(dtrNewTemPd);

					//						//App.DebugEventsLog("PDSER_PDSER_ITEM_ADD", App.Debug_Doc_RefType , App.Debug_Doc_QcBook , App.Debug_Doc_RefNo , App.Debug_Doc_Date , App.Debug_Doc_QcProd , App.Debug_Doc_QcWHouse ,  App.Debug_Doc_Lot , dtrPdSer["cCode"].ToString() );

					//					}
					//				}
					//			}

					//		}
					//	}
					//}
					break;
			}
		}

		private void grdTemPdSer_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.Alt | e.Control | e.Shift)
			{
				if (e.Shift && e.KeyCode == Keys.Enter)
				{
				}
				else
					return;
			}

			switch (e.KeyCode)
			{
				case Keys.F3:
					string strSeekVal = "";
					//this.grdTemPdSer.SetValue("nDummyFld", 0);  //Force to Update in TemPd
					//this.grdTemPdSer.UpdateData();
					//this.intActiveRow = this.grdTemPdSer.Row;
					//string strDataMember = this.grdTemPdSer.RootTable.Columns[this.grdTemPdSer.Col].DataMember.ToUpper();
					string strDataMember = "CQCPDSER";

					this.pmGetGridActiveRow();
					if (this.mdtvActiveRow == null)
					{
						//this.grdTemPdSer.SetValue("nDummyFld", 0);  //Force to Update in TemPd
						//this.grdTemPdSer.UpdateData();
						this.pmGetGridActiveRow();
					}

					//DataRow dtrTemPd = this.dtsDataEnv.Tables[PdSer.xd_Alias_Tem1PdSer].Rows[this.grdTemPdSer.Row];

					switch (strDataMember)
					{
						case "CQCPDSER":
							this.pmInitPopUpDialog("PDSER_ITEM");
							break;
					}
					break;
			}

		}

		private void pmGetGridActiveRow()
		{
			this.mdtvActiveRow = null;
			//if (this.grdTemPdSer.SelectedItems.Count > 0)
			//{
			//	Janus.Windows.GridEX.GridEXSelectedItem grdPdSer = this.grdTemPdSer.SelectedItems[0];
			//	Janus.Windows.GridEX.GridEXRow mRow = grdPdSer.GetRow();
			//	this.mdtvActiveRow = (DataRowView)mRow.DataRow;
			//}
		}

		private bool pmValidPdSerCol(string inQcPdSer)
		{
			//DataRow dtrTemPd = this.dtsDataEnv.Tables[PdSer.xd_Alias_TemPdSer].Rows[this.intActiveRow];
			//dtrTemPd["cLastQcPdSer"] = inQcPdSer;
			bool bllResult = false;
			//if (this.grdTemPdSer.SelectedItems.Count > 0)
			//{
			//	Janus.Windows.GridEX.GridEXSelectedItem grdPdSer = this.grdTemPdSer.SelectedItems[0];
			//	Janus.Windows.GridEX.GridEXRow mRow = grdPdSer.GetRow();
			//	DataRowView dtrPdSer = (DataRowView)mRow.DataRow;

			//	foreach (DataRow dtrUpdPdSer in this.dtsDataEnv.Tables[PdSer.xd_Alias_QXaPdSer].Rows)
			//	{
			//		if (dtrUpdPdSer["cCode"].ToString().TrimEnd() == dtrPdSer["cLastQcPdSer"].ToString().TrimEnd())
			//		{
			//			dtrUpdPdSer["cIsDelete"] = "N";
			//			dtrUpdPdSer["IsCheck"] = false;
			//			break;
			//		}
			//	}

			//	bool bllIsPopUp = true;
			//	if (inQcPdSer.Trim() == string.Empty)
			//	{
			//		dtrPdSer["cLastQcPdSer"] = "";
			//	}
			//	else
			//	{
			//		foreach (DataRow dtrTemPdSer in this.dtsDataEnv.Tables[PdSer.xd_Alias_QXaPdSer].Rows)
			//		{
			//			if (dtrTemPdSer["cIsDelete"].ToString() != "Y"
			//				&& dtrTemPdSer["cCode"].ToString().Trim() == inQcPdSer.Trim())
			//			{

			//				dtrTemPdSer["IsCheck"] = true;
			//				dtrTemPdSer["cIsDelete"] = "Y";
			//				dtrPdSer["cPdSer"] = dtrTemPdSer["cPdSer"].ToString();
			//				dtrPdSer["cQcPdSer"] = dtrTemPdSer["cCode"].ToString();
			//				dtrPdSer["cLastQcPdSer"] = dtrTemPdSer["cCode"].ToString();

			//				bllIsPopUp = false;
			//				bllResult = true;
			//				break;
			//			}
			//		}
			//		if (bllIsPopUp)
			//		{
			//			//if (this.mstrSeekVal != inQcPdSer)
			//			//{
			//			//	this.mstrSeekVal = inQcPdSer;
			//			//	this.pmQueryRefToPdSer();
			//			//}
			//			this.pmInitPopUpDialog("PDSER_ITEM");
			//			bllResult = this.mbllPopUpResult;
			//		}
			//	}
			//}
			return bllResult;
		}

		private bool pmValidPdSerColForCRNote(string inQcPdSer)
		{
			bool bllResult = true;
			//if (this.grdTemPdSer.SelectedItems.Count > 0)
			//{
			//	Janus.Windows.GridEX.GridEXSelectedItem grdPdSer = this.grdTemPdSer.SelectedItems[0];
			//	Janus.Windows.GridEX.GridEXRow mRow = grdPdSer.GetRow();
			//	DataRowView dtrPdSer = (DataRowView)mRow.DataRow;

			//	string strErrorMsg = "";
			//	if (this.pmIsDupPdSer2(this.mstrProd, inQcPdSer, dtrPdSer["cPdSer"].ToString(), ref strErrorMsg))
			//	{
			//		bllResult = false;
			//		MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			//	}
			//}
			return bllResult;
		}

		private void pmQueryRefToPdSer()
		{
			string strErrorMsg = "";
			WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

			string strSQLStr = "";
			strSQLStr = "select XAPDSER.CROWID , XAPDSER.CROWID AS CPDSER , XAPDSER.CPSTEP , XAPDSER.CSSTEP , XAPDSER.CCODE , XAPDSER.CCODE2 , XAPDSER.CWHOUSE , XAPDSER.CWHLOCA,WHLOCA.CCODE AS CQCWHLOCA ";
			strSQLStr += " from XAPDSER ";
			strSQLStr += " left join WHLOCA ON WHLOCA.CROWID = XAPDSER.CWHLOCA ";
			strSQLStr += " where XAPDSER.CCORP = ?";
			strSQLStr += " and XAPDSER.CPROD = ? ";
			strSQLStr += " and XAPDSER.CWHOUSE = ? ";
			if (this.mstrWHLoca.TrimEnd() != string.Empty)
			{
				strSQLStr += " and XAPDSER.CWHLOCA = ? ";
			}
			strSQLStr += " and XAPDSER.CLOT = ? ";

			if (this.mstrWHLoca.TrimEnd() != string.Empty)
			{
				if (this.mstrSaleOrBuy == "P")
				{
					strSQLStr += " and XAPDSER.CPSTEP = ?";
					pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrProd, this.mstrWHouse, this.mstrWHLoca, this.mstrLot, BusinessEnum.gc_PDSER_PSTEP_FREE });
				}
				else
				{
					strSQLStr += " and XAPDSER.CSSTEP = ?";
					strSQLStr += " and XAPDSER.CPSTEP <> ?";
					pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrProd, this.mstrWHouse, this.mstrWHLoca, this.mstrLot, BusinessEnum.gc_PDSER_SSTEP_FREE, BusinessEnum.gc_PDSER_PSTEP_RETURN });
				}
			}
			else
			{
				if (this.mstrSaleOrBuy == "P")
				{
					strSQLStr += " and XAPDSER.CPSTEP = ?";
					pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrProd, this.mstrWHouse, this.mstrLot, BusinessEnum.gc_PDSER_PSTEP_FREE });
				}
				else
				{
					strSQLStr += " and XAPDSER.CSSTEP = ?";
					strSQLStr += " and XAPDSER.CPSTEP <> ?";
					pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, this.mstrProd, this.mstrWHouse, this.mstrLot, BusinessEnum.gc_PDSER_SSTEP_FREE, BusinessEnum.gc_PDSER_PSTEP_RETURN });
				}
			}

			strSQLStr += (this.mstrSeekVal.Trim() != string.Empty ? " and XAPDSER.CCODE like '%" + this.mstrSeekVal.Trim() + "%' " : "");
			strSQLStr += " order by XAPDSER.CCODE ";

			pobjSQLUtil.NotUpperSQLExecString = true;
			pobjSQLUtil.SQLExec(ref this.dtsDataEnv, PdSer.xd_Alias_QXaPdSer, MapTable.Table.xaPdSer, strSQLStr, ref strErrorMsg);
			DataColumn dtcCheck = new DataColumn("IsCheck", System.Type.GetType("System.Boolean"));
			dtcCheck.DefaultValue = false;
			this.dtsDataEnv.Tables[PdSer.xd_Alias_QXaPdSer].Columns.Add(dtcCheck);

			DataColumn dtcIsDel = new DataColumn("cIsDelete", System.Type.GetType("System.String"));
			dtcIsDel.DefaultValue = "N";
			this.dtsDataEnv.Tables[PdSer.xd_Alias_QXaPdSer].Columns.Add(dtcIsDel);

		}

	}
}
