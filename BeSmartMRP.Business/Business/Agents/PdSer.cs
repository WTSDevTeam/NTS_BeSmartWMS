using System;
using System.Text;
using System.Data;

using AppUtil;
using WS.Data;
using WS.Data.Agents;

using BeSmartMRP.Business.Component;
using BeSmartMRP.Business.Entity;

namespace BeSmartMRP.Business.Agents
{
    public class PdSer : cEntityBase
	{

		public const string xd_Alias_TemPdSer = "TRefPdX3";
		public const string xd_Alias_QXaPdSer = "QXaPdSer";
		public const string xd_Alias_Tem1PdSer = "Tem1PdSer";
		public const string xd_Alias_QPdSer = "TemQPdSer";

		public PdSer() : base() { }
		/// <summary>
		///ฐานข้อมูล Serial Number
		/// </summary>
		/// <param name="inAlias">ระบุชื่อ Alias ที่จะใช้อ้างอิงใน DataSet</param>
		public PdSer(string inAlias) : base(inAlias) { }
		/// <summary>
		///ฐานข้อมูล Serial Number
		/// </summary>
		/// <param name="inConnectionString">ระบุ Connection String ในการ Access Database</param>
		/// <param name="inDataBaseReside">ระบุ Database Type</param>
		public PdSer(string inConnectionString, DBMSType inDataBaseReside, string inConnectionString2) : base(inConnectionString, inDataBaseReside)
		{
			this.mstrConnectionString2 = inConnectionString2;
		}

		/// <summary>
		///ฐานข้อมูล Serial Number
		/// </summary>
		/// <param name="inAlias">ระบุชื่อ Alias ที่จะใช้อ้างอิงใน DataSet</param>
		/// <param name="inConnectionString">ระบุ Connection String ในการ Access Database</param>
		/// <param name="inDataBaseReside">ระบุ Database Type</param>
		public PdSer(string inAlias, string inConnectionString, DBMSType inDataBaseReside) : base(inAlias, inConnectionString, inDataBaseReside) { }

		private string mstrConnectionString2 = "";
		private string xdRefPCrNote = DocumentType.BC.ToString() + "," + DocumentType.BA.ToString() + "," + DocumentType.BM.ToString() + "," + DocumentType.BN.ToString() + ",";

		WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
		System.Data.IDbConnection mdbConn = null;
		System.Data.IDbTransaction mdbTran = null;

		WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
		System.Data.IDbConnection mdbConn2 = null;
		System.Data.IDbTransaction mdbTran2 = null;

		private string mstrParentID = "";
		private DataSet dtsDataEnv = null;
		private string mstrErrorMsg = "";

		private string mstrFrWHouse = "";
		private string mstrFrWHLoca = "";

		public void SetSaveDBAgent2(WS.Data.Agents.cDBMSAgent inSaveDBAgent, IDbConnection inDbConn, IDbTransaction inDbTran)
		{
			this.mSaveDBAgent2 = inSaveDBAgent;
			this.mdbConn2 = inDbConn;
			this.mdbTran2 = inDbTran;
		}

		public void SetFromWHouse(string inFrWHouse, string inFrWHLoca)
		{
			this.mstrFrWHouse = inFrWHouse;
			this.mstrFrWHLoca = inFrWHLoca;
		}

		public DataTable CreateTemPdSer()
		{
			return this.pmCreateTemPdSer();
		}

		private DataTable pmCreateTemPdSer()
		{
			//รายการ Serial No. สินค้า
			DataTable dtbTemPdSer = new DataTable(xd_Alias_TemPdSer);

			dtbTemPdSer.Columns.Add("nDummyFld", System.Type.GetType("System.Int32"));
			dtbTemPdSer.Columns.Add("cRowID", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cQcPdSer", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cLastQcPdSer", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cParentID", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cProd", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cPdSer", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cQc2PdSer", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cRefPdX3", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cPStep", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cSStep", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cFrWhouse", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
			dtbTemPdSer.Columns.Add("nUmQty", System.Type.GetType("System.Decimal"));
			dtbTemPdSer.Columns.Add("nRunRow", System.Type.GetType("System.Int32"));
			dtbTemPdSer.Columns.Add("cIsDelete", System.Type.GetType("System.String"));

			dtbTemPdSer.Columns["cQcPdSer"].DefaultValue = "";
			dtbTemPdSer.Columns["cLastQcPdSer"].DefaultValue = "";
			dtbTemPdSer.Columns["cParentID"].DefaultValue = "";
			dtbTemPdSer.Columns["cProd"].DefaultValue = "";
			dtbTemPdSer.Columns["cPdSer"].DefaultValue = "";
			dtbTemPdSer.Columns["cQc2PdSer"].DefaultValue = "";
			dtbTemPdSer.Columns["cRefPdX3"].DefaultValue = "";
			dtbTemPdSer.Columns["cPStep"].DefaultValue = "";
			dtbTemPdSer.Columns["cSStep"].DefaultValue = "";
			dtbTemPdSer.Columns["cFrWhouse"].DefaultValue = "";
			dtbTemPdSer.Columns["nQty"].DefaultValue = 0;
			dtbTemPdSer.Columns["nUmQty"].DefaultValue = 1;
			dtbTemPdSer.Columns["cIsDelete"].DefaultValue = "N";
			dtbTemPdSer.Columns["nDummyFld"].DefaultValue = 0;

			dtbTemPdSer.Columns["cRowID"].DefaultValue = "";
			dtbTemPdSer.Columns["nRunRow"].AutoIncrement = true;

			return dtbTemPdSer.Copy();
		}

		public bool DelItemRefPdX3
			(
			WS.Data.Agents.cDBMSAgent inSaveDBAgent, IDbConnection inDbConn, IDbTransaction inDbTran, DataSet inDataEnv,
			string inCorp, string inBranch, string inRefType, string inForSaleOrBuy, string inProd, string inRefItem, string inWHouse, string inWHLoca, string inLot
			)
		{
			this.mSaveDBAgent = inSaveDBAgent;
			this.mdbConn = inDbConn;
			this.mdbTran = inDbTran;
			this.dtsDataEnv = inDataEnv;

			return this.pmDelItemRefPdX3(inCorp, inBranch, inRefType, inForSaleOrBuy, inProd, inRefItem, inWHouse, inWHLoca, inLot);
		}

		public bool LoadItemTRefPdX3
			(
			WS.Data.Agents.cDBMSAgent inSQLHelper, DataSet inDataEnv, int inParent,
			string inCorp, string inBranch, string inRefType, string inRefItem, string inProd, bool inRefLoad, string inMaxPStep, string inMaxSStep, string inFrWhouse
			)
		{

			string strErrorMsg = "";
			string strRowID = "";
			this.dtsDataEnv = inDataEnv;
			this.mstrParentID = inParent.ToString("00000");

			string strMaxPStep = (inMaxPStep == string.Empty ? Convert.ToChar(255).ToString() : inMaxPStep);
			string strMaxSStep = (inMaxSStep == string.Empty ? Convert.ToChar(255).ToString() : inMaxSStep);

			string strSQLStr = "select * from " + MapTable.Table.RefPdX3 + " where fcCorp = ? and fcBranch = ? and fcRefType = ? and fcRefItem = ?";
			inSQLHelper.SetPara(new object[] { inCorp, inBranch, inRefType, inRefItem });
			inSQLHelper.SQLExec(ref this.dtsDataEnv, "QRefPdX3", MapTable.Table.RefPdX3, strSQLStr, ref strErrorMsg);
			foreach (DataRow dtrRefPdX3 in this.dtsDataEnv.Tables["QRefPdX3"].Rows)
			{

				inSQLHelper.SetPara(new object[] { dtrRefPdX3["fcPdSer"].ToString() });
				if (inSQLHelper.SQLExec(ref this.dtsDataEnv, "QPdSer", MapTable.Table.PdSer, "select * from " + MapTable.Table.PdSer + " where fcSkid = ?", ref strErrorMsg))
				{
					DataRow dtrPdSer = this.dtsDataEnv.Tables["QPdSer"].Rows[0];
					int i = dtrPdSer["fcPstep"].ToString().CompareTo(strMaxPStep);
					int j = dtrPdSer["fcSstep"].ToString().CompareTo(strMaxSStep);

					if (i <= 0 && j <= 0)
					{
						DataRow dtrTRefPdX3 = this.dtsDataEnv.Tables[PdSer.xd_Alias_TemPdSer].NewRow();

						strRowID = Convert.ToInt32(dtrTRefPdX3["nRunRow"]).ToString("000000");
						dtrTRefPdX3["cRowID"] = strRowID;
						dtrTRefPdX3["cRefPdX3"] = dtrRefPdX3["fcSkid"].ToString();
						dtrTRefPdX3["cParentID"] = this.mstrParentID;
						dtrTRefPdX3["cProd"] = inProd;
						dtrTRefPdX3["cPdSer"] = dtrRefPdX3["fcPdSer"].ToString();
						dtrTRefPdX3["cQcPdSer"] = dtrPdSer["fcCode"].ToString().TrimEnd();
						dtrTRefPdX3["cPStep"] = dtrPdSer["fcPStep"].ToString();
						dtrTRefPdX3["cSStep"] = dtrPdSer["fcSStep"].ToString();
						dtrTRefPdX3["cFrWhouse"] = (inFrWhouse == string.Empty ? "" : inFrWhouse);
						dtrTRefPdX3["cQc2PdSer"] = dtrPdSer["fcCode2"].ToString();

						if (inRefLoad)
						{
							dtrTRefPdX3["cRefPdX3"] = "";
						}
						this.dtsDataEnv.Tables[PdSer.xd_Alias_TemPdSer].Rows.Add(dtrTRefPdX3);
					}


				}
			}//end for

			return true;
		}

		public bool DeleteItemPdSer(int inParent, DataSet inDataEnv, ref string ioErrorMsg)
		{
			this.dtsDataEnv = inDataEnv;
			this.mstrParentID = inParent.ToString("00000");
			return this.pmDel1TemRefPdX3(ref ioErrorMsg);
		}

		public bool SaveRefPdX3
			(
			WS.Data.Agents.cDBMSAgent inSaveDBAgent, System.Data.IDbConnection inDbConn, System.Data.IDbTransaction inDbTran,
			DataSet inDataEnv, int inParent,
			string inCorp, string inBranch, string inRefType, string inForSaleOrBuy, string inProd, string inRefItem, string inWHouse, string inWHLoca, string inLot
			)
		{
			this.mSaveDBAgent = inSaveDBAgent;
			this.mdbConn = inDbConn;
			this.mdbTran = inDbTran;
			this.dtsDataEnv = inDataEnv;
			this.mstrParentID = inParent.ToString("00000");

			return this.pmSaveRefPdX3(inCorp, inBranch, inRefType, inForSaleOrBuy, inProd, inRefItem, inWHouse, inWHLoca, inLot);
		}

		private bool pmSaveRefPdX3(string inCorp, string inBranch, string inRefType, string inForSaleOrBuy, string inProd, string inRefItem, string inWHouse, string inWHLoca, string inLot)
		{

			bool bllResult = true;
			string strErrorMsg = "";
			string strRowID = "";
			object[] pAPara = null;
			string strSQLUpdateStr = "";

			DataRow dtrRPdSer = null;
			DataRow dtrRXaPdSer = null;
			DataRow dtrRefPdX3 = null;

			DataRow[] dtrTemPdSer = this.dtsDataEnv.Tables[PdSer.xd_Alias_TemPdSer].Select("cParentID = '" + this.mstrParentID + "'");
			if (dtrTemPdSer.Length > 0)
			{

				//1/7/08 แก้ให้รองรับเรื่อง การคีย์ใบลดหนี้
				string lcCtrlStock = "1";
				if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QCorp", MapTable.Table.Corp, "select * from " + MapTable.Table.Corp + " where fcSkid = ?", new object[] { inCorp }, ref strErrorMsg, this.mdbConn, this.mdbTran))
				{
					string strCorpSCtrlStock = this.dtsDataEnv.Tables["QCorp"].Rows[0]["fcCtrlStoc"].ToString();
					lcCtrlStock = Component.BizRule.GetProdCtrlStock(this.mSaveDBAgent, inProd, strCorpSCtrlStock);
				}

				bool bllCanNegativeStock = (lcCtrlStock == "2");

				foreach (DataRow dtrTem1PdSer in dtrTemPdSer)
				{
					bool llNewPdSer = false;
					bool llNewXaPdSer = false;
					bool bllUpdWHouse = false;

					if (dtrTem1PdSer["cQcPdSer"].ToString() != string.Empty
						&& dtrTem1PdSer["cIsDelete"].ToString() != "Y")
					{

						if (inForSaleOrBuy == "P")
						{
							#region "Save Serial No. ฝั่งซื้อ"

							if (dtrTem1PdSer["cPdSer"].ToString() == string.Empty
								| !this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.PdSer, MapTable.Table.PdSer, "select * from " + MapTable.Table.PdSer + " where fcSkid = ?", new object[] { dtrTem1PdSer["cPdSer"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
							{
								WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
								strRowID = objConn.RunRowID(MapTable.Table.PdSer);
								dtrRPdSer = this.dtsDataEnv.Tables[MapTable.Table.PdSer].NewRow();
								llNewPdSer = true;
								dtrRPdSer["fcSkid"] = strRowID;
								dtrRPdSer["fcCorp"] = inCorp;
								dtrRPdSer["fcBranch"] = inBranch;
								dtrRPdSer["fcProd"] = inProd;
								dtrRPdSer["fcCreateAp"] = this.mSaveDBAgent.AppID;
								dtrRPdSer["fcPStep"] = BusinessEnum.gc_PDSER_PSTEP_ORDER;
								dtrRPdSer["fcSStep"] = BusinessEnum.gc_PDSER_SSTEP_FREE;
							}
							else
							{
								dtrRPdSer = this.dtsDataEnv.Tables[MapTable.Table.PdSer].Rows[0];
								llNewPdSer = false;
							}

							bllUpdWHouse = (this.pmCountRefItem(dtrRPdSer["fcSkid"].ToString()) <= 1);
							string strWHouse = (bllUpdWHouse ? inWHouse : dtrRPdSer["fcWhouse"].ToString());

							#region "Save xaPdSer"
							if (!this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.xaPdSer, MapTable.Table.xaPdSer, "select * from " + MapTable.Table.xaPdSer + " where cPdSer = ?", new object[] { dtrTem1PdSer["cPdSer"].ToString() }, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
							{
								WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
								strRowID = objConn.RunRowID(MapTable.Table.xaPdSer);
								dtrRXaPdSer = this.dtsDataEnv.Tables[MapTable.Table.xaPdSer].NewRow();
								llNewXaPdSer = true;
								dtrRXaPdSer["cRowID"] = strRowID;
								dtrRXaPdSer["cPdSer"] = dtrRPdSer["fcSkid"].ToString();
								dtrRXaPdSer["cCorp"] = inCorp;
								dtrRXaPdSer["cBranch"] = inBranch;
								dtrRXaPdSer["cProd"] = inProd;
								dtrRXaPdSer["cCreateAp"] = this.mSaveDBAgent2.AppID;
							}
							else
							{
								dtrRXaPdSer = this.dtsDataEnv.Tables[MapTable.Table.xaPdSer].Rows[0];
								llNewXaPdSer = false;
							}

							dtrRXaPdSer["cCode"] = dtrTem1PdSer["cQcPdSer"].ToString();
							dtrRXaPdSer["cCode2"] = dtrTem1PdSer["cQc2PdSer"].ToString();
							dtrRXaPdSer["cWhouse"] = strWHouse;
							dtrRXaPdSer["cWHLoca"] = (bllUpdWHouse ? inWHLoca : dtrRXaPdSer["cWHLoca"].ToString());
							dtrRXaPdSer["cLot"] = inLot;
							dtrRXaPdSer["cPStep"] = dtrRPdSer["fcPStep"].ToString();
							dtrRXaPdSer["cSStep"] = dtrRPdSer["fcSStep"].ToString();

							//Update xaPdSer
							strSQLUpdateStr = "";
							DataSetHelper.GenUpdateSQLString(dtrRXaPdSer, "CROWID", llNewXaPdSer, ref strSQLUpdateStr, ref pAPara);
							this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

							#endregion

							dtrRPdSer["fcCode"] = dtrTem1PdSer["cQcPdSer"].ToString();
							dtrRPdSer["fcCode2"] = dtrTem1PdSer["cQc2PdSer"].ToString();
							dtrRPdSer["fcWhouse"] = strWHouse;
							dtrRPdSer["fcLot"] = inLot;
							dtrRPdSer["fclUpdApp"] = this.mSaveDBAgent.AppID;
							dtrRPdSer["fcEAfterR"] = "E";

							//Update PdSer
							strSQLUpdateStr = "";
							DataSetHelper.GenUpdateSQLString(dtrRPdSer, "FCSKID", llNewPdSer, ref strSQLUpdateStr, ref pAPara);
							this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

							bool bllIsNewRefPdX3 = false;
							dtrRefPdX3 = this.pmGetRefPdX3(dtrTem1PdSer["cRefPdX3"].ToString(), inCorp, inBranch, inRefType, inRefItem, ref bllIsNewRefPdX3);
							dtrRefPdX3["fcRefitem"] = inRefItem;
							dtrRefPdX3["fcPdser"] = dtrRPdSer["fcSkid"].ToString();

							//Update RefPdX3
							strSQLUpdateStr = "";
							DataSetHelper.GenUpdateSQLString(dtrRefPdX3, "FCSKID", bllIsNewRefPdX3, ref strSQLUpdateStr, ref pAPara);
							this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

							//if (!llNewPdSer)
							//{

							//WS.Data.Agents.cDBMSAgent objSQLUtil = new WS.Data.Agents.cDBMSAgent(this.mSaveDBAgent.ConnectionString, this.mSaveDBAgent.DataBaseReside);
							string strPdSerPStep = Component.BizRule.GetPdSerPStep(this.mSaveDBAgent, this.mdbConn, this.mdbTran, dtrRefPdX3["fcPdser"].ToString(), false);
							//dtrRPdSer["fcPStep"] = strPdSerPStep;
							//Update PdSer PStep
							pAPara = new object[] { strPdSerPStep, dtrRPdSer["fcSkid"].ToString() };
							strSQLUpdateStr = "update " + MapTable.Table.PdSer + " set fcPStep = ? where fcSkid = ?";
							this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

							#region "Save xaPdSer"

							pAPara = new object[] { strPdSerPStep, dtrRXaPdSer["cRowID"].ToString() };
							strSQLUpdateStr = "update " + MapTable.Table.xaPdSer + " set cPStep = ? where cRowID = ?";
							this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

							#endregion

							//}

							#endregion
						}
						else
						{
							#region "Save Serial No. ฝั่งขาย/ตัดจ่าย"

							if (dtrTem1PdSer["cPdSer"].ToString() != string.Empty
								&& this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.PdSer, MapTable.Table.PdSer, "select * from " + MapTable.Table.PdSer + " where fcSkid = ?", new object[] { dtrTem1PdSer["cPdSer"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
							{

								dtrRPdSer = this.dtsDataEnv.Tables[MapTable.Table.PdSer].Rows[0];
								bool bllIsNewRefPdX3 = false;
								dtrRefPdX3 = this.pmGetRefPdX3(dtrTem1PdSer["cRefPdX3"].ToString(), inCorp, inBranch, inRefType, inRefItem, ref bllIsNewRefPdX3);
								dtrRefPdX3["fcRefitem"] = inRefItem;
								dtrRefPdX3["fcPdser"] = dtrRPdSer["fcSkid"].ToString();
								dtrRefPdX3["fcCreateAp"] = this.mSaveDBAgent.AppID;

								string strPdSerPStep = BusinessEnum.gc_PDSER_PSTEP_ORDER;
								string strPdSerSStep = BusinessEnum.gc_PDSER_SSTEP_FREE;

								//Update RefPdX3
								strSQLUpdateStr = "";
								DataSetHelper.GenUpdateSQLString(dtrRefPdX3, "FCSKID", bllIsNewRefPdX3, ref strSQLUpdateStr, ref pAPara);
								this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

								//WS.Data.Agents.cDBMSAgent objSQLUtil = new WS.Data.Agents.cDBMSAgent(this.mSaveDBAgent.ConnectionString, this.mSaveDBAgent.DataBaseReside);
								if ((this.xdRefPCrNote).IndexOf(inRefType) > -1)
								{
									strPdSerPStep = Component.BizRule.GetPdSerPStep(this.mSaveDBAgent, this.mdbConn, this.mdbTran, dtrRefPdX3["fcPdser"].ToString(), false);
									dtrRPdSer["fcPStep"] = strPdSerPStep;
								}
								else
								{
									strPdSerSStep = Component.BizRule.GetPdSerSStep(this.mSaveDBAgent, this.mdbConn, this.mdbTran, dtrRefPdX3["fcPdser"].ToString(), false);
									dtrRPdSer["fcSStep"] = strPdSerSStep;
								}

								dtrRPdSer["fcWhouse"] = inWHouse;

								//Update PdSer
								strSQLUpdateStr = "";
								DataSetHelper.GenUpdateSQLString(dtrRPdSer, "FCSKID", false, ref strSQLUpdateStr, ref pAPara);
								this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

								#region "Save xaPdSer"
								if (!this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.xaPdSer, MapTable.Table.xaPdSer, "select * from " + MapTable.Table.xaPdSer + " where cPdSer = ?", new object[] { dtrTem1PdSer["cPdSer"].ToString() }, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
								{
									WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
									strRowID = objConn.RunRowID(MapTable.Table.xaPdSer);
									dtrRXaPdSer = this.dtsDataEnv.Tables[MapTable.Table.xaPdSer].NewRow();
									llNewXaPdSer = true;
									dtrRXaPdSer["cRowID"] = strRowID;
									dtrRXaPdSer["cPdSer"] = dtrRPdSer["fcSkid"].ToString();
									dtrRXaPdSer["cCorp"] = inCorp;
									dtrRXaPdSer["cBranch"] = inBranch;
									dtrRXaPdSer["cProd"] = inProd;
									dtrRXaPdSer["cCreateAp"] = this.mSaveDBAgent2.AppID;
								}
								else
								{
									dtrRXaPdSer = this.dtsDataEnv.Tables[MapTable.Table.xaPdSer].Rows[0];
									llNewXaPdSer = false;
								}

								dtrRXaPdSer["cCode"] = dtrTem1PdSer["cQcPdSer"].ToString();
								dtrRXaPdSer["cCode2"] = dtrTem1PdSer["cQc2PdSer"].ToString();
								dtrRXaPdSer["cWhouse"] = inWHouse;
								dtrRXaPdSer["cWHLoca"] = inWHLoca;
								dtrRXaPdSer["cPStep"] = dtrRPdSer["fcPStep"].ToString();
								dtrRXaPdSer["cSStep"] = dtrRPdSer["fcSStep"].ToString();

								//Update xaPdSer
								strSQLUpdateStr = "";
								DataSetHelper.GenUpdateSQLString(dtrRXaPdSer, "CROWID", llNewXaPdSer, ref strSQLUpdateStr, ref pAPara);
								this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

								#endregion

							}
							else
							{
								//1/7/08 รองรับ Stock ติดลบ
								if (bllCanNegativeStock)
								{

									#region "Save Serial No. ฝั่งขายกรณีติดลบ"

									if (dtrTem1PdSer["cPdSer"].ToString() == string.Empty
										| !this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.PdSer, MapTable.Table.PdSer, "select * from " + MapTable.Table.PdSer + " where fcSkid = ?", new object[] { dtrTem1PdSer["cPdSer"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
									{
										WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
										strRowID = objConn.RunRowID(MapTable.Table.PdSer);
										dtrRPdSer = this.dtsDataEnv.Tables[MapTable.Table.PdSer].NewRow();
										llNewPdSer = true;
										dtrRPdSer["fcSkid"] = strRowID;
										dtrRPdSer["fcCorp"] = inCorp;
										dtrRPdSer["fcBranch"] = inBranch;
										dtrRPdSer["fcProd"] = inProd;
										dtrRPdSer["fcCreateAp"] = this.mSaveDBAgent.AppID;
										dtrRPdSer["fcPStep"] = BusinessEnum.gc_PDSER_PSTEP_FREE;
										dtrRPdSer["fcSStep"] = BusinessEnum.gc_PDSER_SSTEP_ORDER;
									}
									else
									{
										dtrRPdSer = this.dtsDataEnv.Tables[MapTable.Table.PdSer].Rows[0];
										llNewPdSer = false;
									}

									bllUpdWHouse = (this.pmCountRefItem(dtrRPdSer["fcSkid"].ToString()) <= 1);
									string strWHouse = (bllUpdWHouse ? inWHouse : dtrRPdSer["fcWhouse"].ToString());

									#region "Save xaPdSer"
									if (!this.mSaveDBAgent2.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.xaPdSer, MapTable.Table.xaPdSer, "select * from " + MapTable.Table.xaPdSer + " where cPdSer = ?", new object[] { dtrTem1PdSer["cPdSer"].ToString() }, ref strErrorMsg, this.mdbConn2, this.mdbTran2))
									{
										WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
										strRowID = objConn.RunRowID(MapTable.Table.xaPdSer);
										dtrRXaPdSer = this.dtsDataEnv.Tables[MapTable.Table.xaPdSer].NewRow();
										llNewXaPdSer = true;
										dtrRXaPdSer["cRowID"] = strRowID;
										dtrRXaPdSer["cPdSer"] = dtrRPdSer["fcSkid"].ToString();
										dtrRXaPdSer["cCorp"] = inCorp;
										dtrRXaPdSer["cBranch"] = inBranch;
										dtrRXaPdSer["cProd"] = inProd;
										dtrRXaPdSer["cCreateAp"] = this.mSaveDBAgent2.AppID;
									}
									else
									{
										dtrRXaPdSer = this.dtsDataEnv.Tables[MapTable.Table.xaPdSer].Rows[0];
										llNewXaPdSer = false;
									}

									dtrRXaPdSer["cCode"] = dtrTem1PdSer["cQcPdSer"].ToString();
									dtrRXaPdSer["cCode2"] = dtrTem1PdSer["cQc2PdSer"].ToString();
									dtrRXaPdSer["cWhouse"] = strWHouse;
									dtrRXaPdSer["cWHLoca"] = (bllUpdWHouse ? inWHLoca : dtrRXaPdSer["cWHLoca"].ToString());
									dtrRXaPdSer["cLot"] = inLot;
									dtrRXaPdSer["cPStep"] = dtrRPdSer["fcPStep"].ToString();
									dtrRXaPdSer["cSStep"] = dtrRPdSer["fcSStep"].ToString();

									//Update xaPdSer
									strSQLUpdateStr = "";
									DataSetHelper.GenUpdateSQLString(dtrRXaPdSer, "CROWID", llNewXaPdSer, ref strSQLUpdateStr, ref pAPara);
									this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

									#endregion

									dtrRPdSer["fcCode"] = dtrTem1PdSer["cQcPdSer"].ToString();
									dtrRPdSer["fcCode2"] = dtrTem1PdSer["cQc2PdSer"].ToString();
									dtrRPdSer["fcWhouse"] = strWHouse;
									dtrRPdSer["fcLot"] = inLot;
									dtrRPdSer["fclUpdApp"] = this.mSaveDBAgent.AppID;
									dtrRPdSer["fcEAfterR"] = "E";

									//Update PdSer
									strSQLUpdateStr = "";
									DataSetHelper.GenUpdateSQLString(dtrRPdSer, "FCSKID", llNewPdSer, ref strSQLUpdateStr, ref pAPara);
									this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

									bool bllIsNewRefPdX3 = false;
									dtrRefPdX3 = this.pmGetRefPdX3(dtrTem1PdSer["cRefPdX3"].ToString(), inCorp, inBranch, inRefType, inRefItem, ref bllIsNewRefPdX3);
									dtrRefPdX3["fcRefitem"] = inRefItem;
									dtrRefPdX3["fcPdser"] = dtrRPdSer["fcSkid"].ToString();

									//Update RefPdX3
									strSQLUpdateStr = "";
									DataSetHelper.GenUpdateSQLString(dtrRefPdX3, "FCSKID", bllIsNewRefPdX3, ref strSQLUpdateStr, ref pAPara);
									this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

									//if (!llNewPdSer)
									//{

									//WS.Data.Agents.cDBMSAgent objSQLUtil = new WS.Data.Agents.cDBMSAgent(this.mSaveDBAgent.ConnectionString, this.mSaveDBAgent.DataBaseReside);
									string strPdSerPStep = Component.BizRule.GetPdSerPStep(this.mSaveDBAgent, this.mdbConn, this.mdbTran, dtrRefPdX3["fcPdser"].ToString(), false);
									//dtrRPdSer["fcPStep"] = strPdSerPStep;
									//Update PdSer PStep
									pAPara = new object[] { strPdSerPStep, dtrRPdSer["fcSkid"].ToString() };
									strSQLUpdateStr = "update " + MapTable.Table.PdSer + " set fcPStep = ? where fcSkid = ?";
									this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

									#region "Save xaPdSer"

									pAPara = new object[] { strPdSerPStep, dtrRXaPdSer["cRowID"].ToString() };
									strSQLUpdateStr = "update " + MapTable.Table.xaPdSer + " set cPStep = ? where cRowID = ?";
									this.mSaveDBAgent2.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

									#endregion

									//}

									#endregion

								}
								else
								{
									bllResult = false;
									this.mstrErrorMsg = "ไม่พบ Serial number";
									break;
								}
							}

							#endregion
						}
					}
					else // Delete Serial Number
					{
						#region "Delete Serial Number"

						if (dtrTem1PdSer["cRefPdX3"].ToString() != string.Empty
							&& dtrTem1PdSer["cPdSer"].ToString() != string.Empty)
						{
							string strCrPdSer = dtrTem1PdSer["cPdSer"].ToString();

							#region "Count RefPdX3"
							int lnCnt = 0;
							if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.RefPdX3, MapTable.Table.RefPdX3, "select * from " + MapTable.Table.RefPdX3 + " where fcPdSer = ?", new object[] { strCrPdSer }, ref strErrorMsg, this.mdbConn, this.mdbTran))
							{
								foreach (DataRow dtrXRef in this.dtsDataEnv.Tables[MapTable.Table.RefPdX3].Rows)
								{
									if (dtrXRef["fcPdSer"].ToString() == dtrTem1PdSer["cPdSer"].ToString())
									{
										lnCnt++;
										if (dtrTem1PdSer["cRefPdX3"].ToString() == dtrXRef["fcSkid"].ToString())
										{
											pAPara = new object[] { dtrXRef["fcSkid"].ToString() };
											this.mSaveDBAgent.BatchSQLExec("delete from " + MapTable.Table.RefPdX3 + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
											lnCnt--;
										}
									}
								}
							}
							#endregion

							if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.PdSer, MapTable.Table.PdSer, "select * from " + MapTable.Table.PdSer + " where fcSkid = ?", new object[] { strCrPdSer }, ref strErrorMsg, this.mdbConn, this.mdbTran))
							{
								dtrRPdSer = this.dtsDataEnv.Tables[MapTable.Table.PdSer].Rows[0];
								WS.Data.Agents.cDBMSAgent objSQLUtil = new WS.Data.Agents.cDBMSAgent(this.mSaveDBAgent.ConnectionString, this.mSaveDBAgent.DataBaseReside);

								if (inForSaleOrBuy == "P" || (this.xdRefPCrNote).IndexOf(inRefType) > -1)
								{
									#region "Delete กรณีเอกสารซื้อ"
									string strPdSerPStep = Component.BizRule.GetPdSerPStep(this.mSaveDBAgent, this.mdbConn, this.mdbTran, strCrPdSer, false);
									//กณีที่ไม่มีเอกสารไหนอ้างถึงอีก
									if (lnCnt == 0 && strPdSerPStep == BusinessEnum.gc_PDSER_PSTEP_FREE)
									{
										pAPara = new object[] { strCrPdSer };
										this.mSaveDBAgent.BatchSQLExec("delete from " + MapTable.Table.PdSer + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

										this.mSaveDBAgent2.BatchSQLExec("delete from " + MapTable.Table.xaPdSer + " where cPdSer = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

									}
									else
									{
										pAPara = new object[] { strPdSerPStep, strCrPdSer };
										this.mSaveDBAgent.BatchSQLExec("update " + MapTable.Table.PdSer + " set fcPStep = ? where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

										this.mSaveDBAgent2.BatchSQLExec("update " + MapTable.Table.xaPdSer + " set cPStep = ? where cPdSer = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

									}
									#endregion
								}
								else
								{
									#region "Delete กรณีเอกสารขาย"
									string strPdSerSStep = Component.BizRule.GetPdSerSStep(this.mSaveDBAgent, this.mdbConn, this.mdbTran, strCrPdSer, false);
									//กณีที่ไม่มีเอกสารไหนอ้างถึงอีก
									if (lnCnt == 0 && strPdSerSStep == BusinessEnum.gc_PDSER_SSTEP_FREE)
									{
										pAPara = new object[] { strCrPdSer };
										this.mSaveDBAgent.BatchSQLExec("delete from " + MapTable.Table.PdSer + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

										this.mSaveDBAgent2.BatchSQLExec("delete from " + MapTable.Table.xaPdSer + " where cPdSer = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

									}
									else
									{
										pAPara = new object[] { strPdSerSStep, strCrPdSer };
										this.mSaveDBAgent.BatchSQLExec("update " + MapTable.Table.PdSer + " set fcSStep = ? where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

										this.mSaveDBAgent2.BatchSQLExec("update " + MapTable.Table.xaPdSer + " set cSStep = ? where cPdSer = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

									}
									#endregion
								}

								if (this.mstrFrWHouse != string.Empty)
								{
									pAPara = new object[] { this.mstrFrWHouse, strCrPdSer };
									this.mSaveDBAgent.BatchSQLExec("update " + MapTable.Table.PdSer + " set fcWhouse = ? where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

									pAPara = new object[] { this.mstrFrWHouse, this.mstrFrWHLoca, strCrPdSer };
									this.mSaveDBAgent2.BatchSQLExec("update " + MapTable.Table.xaPdSer + " set cWhouse = ? , cWHLoca = ? where cPdSer = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
								}
								else
								{
									if (dtrTem1PdSer["cFrWhouse"].ToString() != string.Empty)
									{
										pAPara = new object[] { dtrTem1PdSer["cFrWhouse"].ToString(), strCrPdSer };
										this.mSaveDBAgent.BatchSQLExec("update " + MapTable.Table.PdSer + " set fcWhouse = ? where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

										this.mSaveDBAgent2.BatchSQLExec("update " + MapTable.Table.xaPdSer + " set cWhouse = ? where cPdSer = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
									}
								}

							}
						}
						#endregion
					}

				}
			}
			return bllResult;
		}

		private bool pmDel1TemRefPdX3(ref string ioErrorMsg)
		{
			DataRow[] dtrTemPdSer = this.dtsDataEnv.Tables[PdSer.xd_Alias_TemPdSer].Select("cParentID = '" + this.mstrParentID + "'");
			if (dtrTemPdSer.Length > 0)
			{
				foreach (DataRow dtrTem1PdSer in dtrTemPdSer)
				{

					//TODO: เรื่องการ Check Step ของ Serial ว่าตัดจ่ายไปแล้วไม่ให้ลบ
					if (dtrTem1PdSer["cQcPdSer"].ToString() != string.Empty)
					{
						dtrTem1PdSer["cIsDelete"] = "Y";
					}

				}
			}
			return true;
		}

		private bool pmDelItemRefPdX3
			(
			string inCorp, string inBranch, string inRefType, string inForSaleOrBuy, string inProd, string inRefItem, string inWHouse, string inWHLoca, string inLot
			)
		{
			string strErrorMsg = "";
			DataRow dtrRPdSer = null;
			string strSQLStr = "select fcSkid , fcPdSer from RefPdX3 where RefPdX3.fcCorp = ? and RefPdX3.fcBranch = ? and RefPdX3.fcRefType = ? and RefPdX3.fcRefItem = ?";

			object[] pAPara = new object[] { inCorp, inBranch, inRefType, inRefItem };
			this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QRefPdX3", MapTable.Table.RefPdX3, strSQLStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
			foreach (DataRow dtrQRefPdX3 in this.dtsDataEnv.Tables["QRefPdX3"].Rows)
			{
				if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "RRefPdX3", MapTable.Table.RefPdX3, "select * from RefPdX3 where fcSkid = ?", new object[] { dtrQRefPdX3["fcSkid"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
				{
					DataRow dtrTemRefPdX3 = this.dtsDataEnv.Tables["RRefPdX3"].Rows[0];
					string strCrPdSer = dtrTemRefPdX3["fcPdSer"].ToString();
					pAPara = new object[] { dtrTemRefPdX3["fcSkid"].ToString() };
					this.mSaveDBAgent.BatchSQLExec("delete from " + MapTable.Table.RefPdX3 + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

					int lnCnt = 0;
					if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "Q2RefPdX3", MapTable.Table.RefPdX3, "select count(*) as nCnt from RefPdX3 where fcPdSer = ?", new object[] { dtrQRefPdX3["fcPdSer"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
					{
						lnCnt = (Convert.IsDBNull(this.dtsDataEnv.Tables["Q2RefPdX3"].Rows[0]["nCnt"]) ? 0 : Convert.ToInt32(this.dtsDataEnv.Tables["Q2RefPdX3"].Rows[0]["nCnt"]));
					}

					if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.PdSer, MapTable.Table.PdSer, "select * from " + MapTable.Table.PdSer + " where fcSkid = ?", new object[] { strCrPdSer }, ref strErrorMsg, this.mdbConn, this.mdbTran))
					{
						dtrRPdSer = this.dtsDataEnv.Tables[MapTable.Table.PdSer].Rows[0];
						WS.Data.Agents.cDBMSAgent objSQLUtil = new WS.Data.Agents.cDBMSAgent(this.mSaveDBAgent.ConnectionString, this.mSaveDBAgent.DataBaseReside);

						if (inForSaleOrBuy == "P" || (this.xdRefPCrNote).IndexOf(inRefType) > -1)
						{
							#region "Delete กรณีเอกสารซื้อ"
							string strPdSerPStep = Component.BizRule.GetPdSerPStep(this.mSaveDBAgent, this.mdbConn, this.mdbTran, strCrPdSer, false);
							//กณีที่ไม่มีเอกสารไหนอ้างถึงอีก
							if (lnCnt == 0 && strPdSerPStep == BusinessEnum.gc_PDSER_PSTEP_FREE)
							{
								pAPara = new object[] { strCrPdSer };
								this.mSaveDBAgent.BatchSQLExec("delete from " + MapTable.Table.PdSer + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

								this.mSaveDBAgent2.BatchSQLExec("delete from " + MapTable.Table.xaPdSer + " where cPdSer = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

							}
							else
							{
								pAPara = new object[] { strPdSerPStep, strCrPdSer };
								this.mSaveDBAgent.BatchSQLExec("update " + MapTable.Table.PdSer + " set fcPStep = ? where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

								this.mSaveDBAgent2.BatchSQLExec("update " + MapTable.Table.xaPdSer + " set cPStep = ? where cPdSer = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

							}
							#endregion
						}
						else
						{
							#region "Delete กรณีเอกสารขาย"
							string strPdSerSStep = Component.BizRule.GetPdSerSStep(this.mSaveDBAgent, this.mdbConn, this.mdbTran, strCrPdSer, false);
							//กณีที่ไม่มีเอกสารไหนอ้างถึงอีก
							if (lnCnt == 0 && strPdSerSStep == BusinessEnum.gc_PDSER_SSTEP_FREE)
							{
								pAPara = new object[] { strCrPdSer };
								this.mSaveDBAgent.BatchSQLExec("delete from " + MapTable.Table.PdSer + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

								this.mSaveDBAgent2.BatchSQLExec("delete from " + MapTable.Table.xaPdSer + " where cPdSer = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

							}
							else
							{
								pAPara = new object[] { strPdSerSStep, strCrPdSer };
								this.mSaveDBAgent.BatchSQLExec("update " + MapTable.Table.PdSer + " set fcSStep = ? where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

								this.mSaveDBAgent2.BatchSQLExec("update " + MapTable.Table.xaPdSer + " set cSStep = ? where cPdSer = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);

							}
							#endregion
						}

						if (inWHouse != string.Empty)
						{
							pAPara = new object[] { inWHouse, strCrPdSer };
							this.mSaveDBAgent.BatchSQLExec("update " + MapTable.Table.PdSer + " set fcWhouse = ? where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

							this.mSaveDBAgent2.BatchSQLExec("update " + MapTable.Table.xaPdSer + " set cWhouse = ? where cPdSer = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
						}

						if (inWHLoca != string.Empty)
						{
							pAPara = new object[] { inWHLoca, strCrPdSer };
							this.mSaveDBAgent2.BatchSQLExec("update " + MapTable.Table.xaPdSer + " set cWHLoca = ? where cPdSer = ?", pAPara, ref strErrorMsg, this.mdbConn2, this.mdbTran2);
						}

					}

				}
			}

			return true;
		}

		private DataRow pmGetRefPdX3(string inRefPdX3, string inCorp, string inBranch, string inRefType, string inRefItem, ref bool ioIsNewRow)
		{
			DataRow dtrRefPdX3 = null;
			string strErrorMsg = "";
			if (inRefPdX3 == string.Empty
				| !this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.RefPdX3, MapTable.Table.RefPdX3, "select * from " + MapTable.Table.RefPdX3 + " where fcSkid = ?", new object[] { inRefPdX3 }, ref strErrorMsg, this.mdbConn, this.mdbTran))
			{
				WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
				string strRowID = objConn.RunRowID(MapTable.Table.RefPdX3);

				dtrRefPdX3 = this.dtsDataEnv.Tables[MapTable.Table.RefPdX3].NewRow();
				dtrRefPdX3["fcSkid"] = strRowID;
				dtrRefPdX3["fcCorp"] = inCorp;
				dtrRefPdX3["fcBranch"] = inBranch;
				dtrRefPdX3["fcRefType"] = inRefType;
				dtrRefPdX3["fcRefitem"] = inRefItem;
				dtrRefPdX3["fcCreateAp"] = this.mSaveDBAgent.AppID;
				dtrRefPdX3["fclUpdApp"] = this.mSaveDBAgent.AppID;
				dtrRefPdX3["fcEAfterR"] = "E";
				ioIsNewRow = true;
			}
			else
			{
				dtrRefPdX3 = this.dtsDataEnv.Tables[MapTable.Table.RefPdX3].Rows[0];
				ioIsNewRow = false;
			}
			return dtrRefPdX3;
		}

		private int pmCountRefItem(string inPdSer)
		{
			string strErrorMsg = "";
			int lnCnt = 0;
			if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "Q2RefPdX3", MapTable.Table.RefPdX3, "select count(*) as nCnt from RefPdX3 where fcPdSer = ?", new object[] { inPdSer }, ref strErrorMsg, this.mdbConn, this.mdbTran))
			{
				lnCnt = (Convert.IsDBNull(this.dtsDataEnv.Tables["Q2RefPdX3"].Rows[0]["nCnt"]) ? 0 : Convert.ToInt32(this.dtsDataEnv.Tables["Q2RefPdX3"].Rows[0]["nCnt"]));
			}
			return lnCnt;
		}

		public static DataTable GetPdSerTable
			(
			WS.Data.Agents.cDBMSAgent inSQLHelper
			, WS.Data.Agents.cDBMSAgent inSQLHelper2
			, string inCorp
			, string inBranch
			, string inProd
			, string inWHouse
			, string inWHLoca
			)
		{

			string strErrorMsg = "";

			DataSet dtsDataEnv = new DataSet();
			DataTable dtbTemPdSer = new DataTable(xd_Alias_QPdSer);
			dtbTemPdSer.Columns.Add("cPdSer", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cCode", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cLot", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cWHLoca", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cWHouse", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cQcWHLoca", System.Type.GetType("System.String"));
			dtbTemPdSer.Columns.Add("cQcWHouse", System.Type.GetType("System.String"));

			WS.Data.Agents.cDBMSAgent pobjSQLUtil = inSQLHelper;
			WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = inSQLHelper2;

			bool bllFoundPdSer = false;
			pobjSQLUtil2.SetPara(new object[] { inCorp, inBranch, inWHouse, inProd, BusinessEnum.gc_PDSER_SSTEP_FREE, BusinessEnum.gc_PDSER_PSTEP_RETURN });
			string strSQLStr = "select * from XAPDSER where CCORP = ? and CBRANCH = ? and CWHOUSE = ? and CPROD = ? and CSSTEP = ? and CPSTEP <> ?";
			bllFoundPdSer = pobjSQLUtil2.SQLExec(ref dtsDataEnv, "QStkUtil_PdSer", "XAPDSER", strSQLStr, ref strErrorMsg);

			if (bllFoundPdSer)
			{
				foreach (DataRow dtrPdSer in dtsDataEnv.Tables["QStkUtil_PdSer"].Rows)
				{
					DataRow dtrNewRow = dtbTemPdSer.NewRow();
					dtrNewRow["cPdSer"] = dtrPdSer["cPdSer"].ToString();
					dtrNewRow["cCode"] = dtrPdSer["cCode"].ToString();
					dtrNewRow["cLot"] = dtrPdSer["cLot"].ToString();
					dtrNewRow["cWHLoca"] = dtrPdSer["cWHLoca"].ToString();
					dtrNewRow["cWHouse"] = dtrPdSer["cWHouse"].ToString();

					pobjSQLUtil2.SetPara(new object[] { dtrPdSer["cWHLoca"].ToString() });
					if (pobjSQLUtil2.SQLExec(ref dtsDataEnv, "QWHLoca", "WHLOCA", "select * from WHLOCA where CROWID = ?", ref strErrorMsg))
					{
						dtrNewRow["cQcWHLoca"] = dtsDataEnv.Tables["QWHLoca"].Rows[0]["cCode"].ToString();
					}

					pobjSQLUtil.SetPara(new object[] { dtrPdSer["cWHouse"].ToString() });
					if (pobjSQLUtil.SQLExec(ref dtsDataEnv, "QWHouse", "WHOUSE", "select * from WHOUSE where FCSKID = ?", ref strErrorMsg))
					{
						dtrNewRow["cQcWHouse"] = dtsDataEnv.Tables["QWHouse"].Rows[0]["fcCode"].ToString();
					}

					dtbTemPdSer.Rows.Add(dtrNewRow);
				}
			}
			return dtbTemPdSer.Copy();
		}


	}
}
