using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BeSmartMRP.Business.Agents;


namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgSelPdSerial2 : UIHelper.frmBase
    {
        public dlgSelPdSerial2()
        {
            InitializeComponent();
        }

		public dlgSelPdSerial2(string inBranch, string inWHouse, string inProd)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.mstrBranchID = inBranch;
			this.mstrProdID = inProd;
			this.mstrWHouse = inWHouse;

			this.pmInitForm();
		}

		private DataSet dtsDataEnv = new DataSet();
		private bool mbllPopUpResult = false;

		private string mstrOrderBy = "cCode";
		private string mstrBrowViewAlias = PdSer.xd_Alias_QPdSer;

		private string mstrEditRowID = "";
		private string mstrBranchID = "";
		private string mstrProdID = "";

		private string mstrLot = "";
		private string mstrWHLoca = "";
		private string mstrWHouse = "";

		public string BranchID
		{
			get { return this.mstrBranchID; }
			set { this.mstrBranchID = value; }
		}

		public string ProdID
		{
			get { return this.mstrProdID; }
			set { this.mstrProdID = value; }
		}

		public string Lot
		{
			get { return this.mstrLot; }
			set { this.mstrLot = value; }
		}

		public string WHouseID
		{
			get { return this.mstrWHouse; }
			set { this.mstrWHouse = value; }
		}

		public string WHLocaID
		{
			get { return this.mstrWHLoca; }
			set { this.mstrWHLoca = value; }
		}

		public bool PopUpResult
		{
			get { return this.mbllPopUpResult; }
		}

		private void pmInitForm()
		{
			//this.imgSmallIcon.ImageStream = UIBase.GetToolBarImageStream2();
			//UIHelper.UIBase.CreatePopUpToolBar(this.WsLocale, ref this.uiStandardBar);
			//this.uiCmd01.Commands[WsToolBar.Search.ToString()].Visible = Janus.Windows.UI.InheritableBoolean.False;
			this.pmSetBrowView();
			this.pmInitGridProp();
		}

		private void grdTemPdSer_DoubleClick(object sender, System.EventArgs e)
		{
			this.mbllPopUpResult = true;
			this.Hide();
		}

		public void BindData(DataSet inDataEnv)
		{
			this.dtsDataEnv = inDataEnv;
			this.pmBindData();
		}

		//public Janus.Windows.GridEX.GridEXSelectedItemCollection SelectedItems
		//{
		//	get { return this.grdTemPdSer.SelectedItems; }
		//}

		private void pmBindData()
		{
			this.pmInitGridProp();
		}

		private void pmInitGridProp()
		{
			//this.grdTemPdSer.SetDataBinding(this.dtsDataEnv.Tables[PdSer.xd_Alias_QPdSer], "");
			//this.grdTemPdSer.RetrieveStructure();

			//for (int intCnt = 0; intCnt < this.grdTemPdSer.RootTable.Columns.Count; intCnt++)
			//{
			//	this.grdTemPdSer.RootTable.Columns[intCnt].Visible = false;
			//}

			//this.grdTemPdSer.RootTable.HeaderFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			//this.grdTemPdSer.RootTable.Columns["cCode"].Visible = true;
			////this.grdTemPdSer.RootTable.Columns["cQcWHLoca"].Visible = true;
			//this.grdTemPdSer.RootTable.Columns["cQcWHouse"].Visible = true;
			//this.grdTemPdSer.RootTable.Columns["cLot"].Visible = true;

			//this.grdTemPdSer.RootTable.Columns["cCode"].Caption = "Serial No.";
			//this.grdTemPdSer.RootTable.Columns["cLot"].Caption = "ล๊อต";
			//this.grdTemPdSer.RootTable.Columns["cQcWHLoca"].Caption = "ที่เก็บสินค้า";
			//this.grdTemPdSer.RootTable.Columns["cQcWHouse"].Caption = "คลังสินค้า";
			//this.grdTemPdSer.ColumnAutoResize = true;

			//this.grdTemPdSer.RootTable.Columns["cCode"].Width = 70;
			//this.grdTemPdSer.RootTable.Columns["cQcWHLoca"].Width = 70;
			//this.grdTemPdSer.RootTable.Columns["cQcWHouse"].Width = 70;

			//this.pmFilterRow();
			//this.grdTemPdSer.Focus();
		}

		private bool mbllIsFilter = false;
		private void pmFilterRow()
		{
			//Janus.Windows.GridEX.GridEXFilterCondition filter;
			//filter = new Janus.Windows.GridEX.GridEXFilterCondition(this.grdTemPdSer.RootTable.Columns["cIsDelete"], Janus.Windows.GridEX.ConditionOperator.NotEqual, "Y");
			//this.grdTemPdSer.RootTable.FilterCondition = filter;
			//this.mbllIsFilter = true;
		}

		private void dlgSelPdSerial_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.Alt | e.Control | e.Shift)
				return;

			switch (e.KeyCode)
			{
				case Keys.Enter:
					this.mbllPopUpResult = true;
					this.DialogResult = DialogResult.OK;
					break;
				case Keys.Escape:
					this.mbllPopUpResult = false;
					this.DialogResult = DialogResult.Cancel;
					break;
			}
		}

		private void pmSetBrowView()
		{
			WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
			WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

			DataTable dtrBrowView = new DataTable();

			dtrBrowView = PdSer.GetPdSerTable(pobjSQLUtil, pobjSQLUtil2, App.ActiveCorp.RowID, this.mstrBranchID, this.mstrProdID, this.mstrWHouse, "");

			if (this.dtsDataEnv.Tables[this.mstrBrowViewAlias] != null)
				this.dtsDataEnv.Tables.Remove(this.mstrBrowViewAlias);

			this.dtsDataEnv.Tables.Add(dtrBrowView);

		}

		private void pmRefreshBrowView()
		{
			this.pmSetBrowView();
			//this.grdTemPdSer.SetDataBinding(this.dtsDataEnv.Tables[this.mstrBrowViewAlias], "");

		}

		public bool ValidateField(string inSearchStr, bool inForcePopUp)
		{
		//	bool bllIsVField = true;
		//	this.mbllPopUpResult = false;

		//	this.pmRefreshBrowView();

		//	DataView dv = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
		//	dv.Sort = this.mstrOrderBy;
		//	DataRowView[] adv = dv.FindRows(new object[] { inSearchStr });
		//	if (adv.Length > 0)
		//	{
		//		this.grdTemPdSer.Find(this.grdTemPdSer.RootTable.Columns[this.mstrOrderBy], Janus.Windows.GridEX.ConditionOperator.Equal, adv[0][this.mstrOrderBy].ToString(), 0, 1);
		//		bllIsVField = false;
		//		this.mbllPopUpResult = true;
		//	}

		//	if (inForcePopUp || bllIsVField)
		//	{
		//		this.ShowDialog();
		//	}
			return this.mbllPopUpResult;
		}

		public DataRow RetrieveValue()
		{
			//if (this.grdTemPdSer.GetValue("cPdSer") == null)
			//	return null;

			//this.mstrEditRowID = this.grdTemPdSer.GetValue("cPdSer").ToString();
			//string strErrorMsg = "";
			//WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
			//object[] pAPara = new object[1] { this.mstrEditRowID };
			//pobjSQLUtil.SetPara(pAPara);
			//if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVFLD_Table", "XAPDSER", "select * from XAPDSER where cPdSer = ? ", ref strErrorMsg))
			//{
			//	return this.dtsDataEnv.Tables["QVFLD_Table"].Rows[0];
			//}
			return null;
		}


	}
}
