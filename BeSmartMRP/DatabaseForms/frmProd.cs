
#define xd_RUNMODE_DEBUG

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;
using BeSmartMRP.Business.Component;

using DevExpress.XtraGrid.Views.Base;
using DevExpress.Utils;

namespace BeSmartMRP.DatabaseForms
{

	public partial class frmProd : UIHelper.frmBase
	{

		public const string xdCMRem = "Rem";
		public const string xdCMRem2 = "Rm2";
		public const string xdCMRem3 = "Rm3";
		public const string xdCMRem4 = "Rm4";
		public const string xdCMRem5 = "Rm5";
		public const string xdCMRem6 = "Rm6";
		public const string xdCMRem7 = "Rm7";
		public const string xdCMRem8 = "Rm8";
		public const string xdCMRem9 = "Rm9";
		public const string xdCMRem10 = "RmA";

		public static string TASKNAME = "EPROD";

		public static int MAXLENGTH_CODE = 40;
		public static int MAXLENGTH_NAME = 70;
		public static int MAXLENGTH_NAME2 = 70;

		private const int xd_PAGE_BROWSE = 0;
		private const int xd_PAGE_EDIT1 = 1;

		private DataSet dtsDataEnv = new DataSet();
		private string mstrBrowViewAlias = "Brow_Alias";

		private string mstrMasterTable = MapTable.Table.Product;
		private string mstrRefTable = MapTable.Table.Product;
		private int mintSaveBrowViewRowIndex = -1;
		private string mstrSortKey = "CCODE";

		private bool mbllAddNew = false;

		private string mstrMasterRowID = "";
		private string mstrEditRowID = "";
		private string mstrSaveRowID = "";

		private string mstrPdType = "";

		private string mstrMemoS1 = "";
		private string mstrMemoS2 = "";
		private string mstrMemoS3 = "";
		private string mstrMemoS4 = "";
		private string mstrMemoS5 = "";

		private UIHelper.AppFormState mFormEditMode;

		private string mstrOldCode = "";
		private string mstrOldName = "";
		private string mstrOldName2 = "";

		private FormActiveMode mFormActiveMode = FormActiveMode.Edit;
		private bool mbllPopUpResult = true;
		private bool mbllIsShowDialog = false;
		private string mstrSearchStr = "";
		private bool mbllIsFormQuery = false;
		private string mstrTemPdVer = "TemPdVersion";

		WS.Data.Agents.cDBMSAgent mSaveDBAgent = null;
		System.Data.IDbConnection mdbConn = null;
		System.Data.IDbTransaction mdbTran = null;

		WS.Data.Agents.cDBMSAgent mSaveDBAgent2 = null;
		System.Data.IDbConnection mdbConn2 = null;
		System.Data.IDbTransaction mdbTran2 = null;

		private DialogForms.dlgGetAcChart pofrmGetAcChart = null;

		//private frmPdClass pofrmGetPdClass = null;
		//private frmPdCateg pofrmGetPdCateg = null;
		//private frmPdContent pofrmGetPdContent = null;

		private DialogForms.dlgGetPdGrp pofrmGetPdGrp = null;
		private DialogForms.dlgGetCoor pofrmGetSuppl = null;
		private DialogForms.dlgGetUM pofrmGetUM = null;
		private DialogForms.dlgGetProdType pofrmGetPdType = null;

		ArrayList paTxtStdUM = new ArrayList();
		private int mintFocusRow = -1;

		public frmProd()
		{
			InitializeComponent();
			this.pmInitForm();
		}

		public frmProd(FormActiveMode inMode)
		{
			InitializeComponent();

			this.mFormActiveMode = inMode;
			this.pmInitForm();
		}

		private static frmProd mInstanse = null;

		public static frmProd GetInstanse()
		{
			if (mInstanse == null)
			{
				mInstanse = new frmProd();
			}
			return mInstanse;
		}

		private static void pmClearInstanse()
		{
			if (mInstanse != null)
			{
				mInstanse = null;
			}
		}

		public FormActiveMode ActiveMode
		{
			get { return this.mFormActiveMode; }
			set { this.mFormActiveMode = value; }
		}

		public bool PopUpResult
		{
			get { return this.mbllPopUpResult; }
		}

		public bool IsShowDialog
		{
			get { return this.mbllIsShowDialog; }
		}

		private void pmInitForm()
		{
			UIBase.WaitWind("Loading Form...");
			this.pmInitializeComponent();
			this.pmGotoBrowPage();

			this.pmInitGridProp();
			this.gridView1.MoveLast();

			UIBase.SetDefaultChildAppreance(this);

			UIBase.WaitClear();
		}

		private void pmInitializeComponent()
		{
			this.txtCode.Properties.MaxLength = frmProd.MAXLENGTH_CODE;
			this.txtName.Properties.MaxLength = frmProd.MAXLENGTH_NAME;
			this.txtName2.Properties.MaxLength = frmProd.MAXLENGTH_NAME2;

			this.txtQcPdType.Properties.MaxLength = DialogForms.dlgGetProdType.MAXLENGTH_CODE;
			this.txtQnPdType.Properties.MaxLength = DialogForms.dlgGetProdType.MAXLENGTH_NAME;

			//this.txtQcPdClass.Properties.MaxLength = frmPdClass.MAXLENGTH_CODE;
			//this.txtQnPdClass.Properties.MaxLength = frmPdClass.MAXLENGTH_NAME;

			//this.txtQcPdCateg.Properties.MaxLength = frmPdCateg.MAXLENGTH_CODE;
			//this.txtQnPdCateg.Properties.MaxLength = frmPdCateg.MAXLENGTH_NAME;

			//this.txtQcPdContent.Properties.MaxLength = frmPdContent.MAXLENGTH_CODE;
			//this.txtQnPdContent.Properties.MaxLength = frmPdContent.MAXLENGTH_NAME;
			
			this.txtQcAccBCash.Properties.MaxLength = DialogForms.dlgGetAcChart.MAXLENGTH_CODE;
			this.txtQnAccBCash.Properties.MaxLength = DialogForms.dlgGetAcChart.MAXLENGTH_NAME;
			this.txtQcAccBCred.Properties.MaxLength = DialogForms.dlgGetAcChart.MAXLENGTH_CODE;
			this.txtQnAccBCred.Properties.MaxLength = DialogForms.dlgGetAcChart.MAXLENGTH_NAME;
			this.txtQcAccSCash.Properties.MaxLength = DialogForms.dlgGetAcChart.MAXLENGTH_CODE;
			this.txtQnAccSCash.Properties.MaxLength = DialogForms.dlgGetAcChart.MAXLENGTH_NAME;
			this.txtQcAccSCred.Properties.MaxLength = DialogForms.dlgGetAcChart.MAXLENGTH_CODE;
			this.txtQnAccSCred.Properties.MaxLength = DialogForms.dlgGetAcChart.MAXLENGTH_NAME;

			this.txtDiscStr.Properties.MaxLength = 20;

			UIHelper.UIBase.CreateStandardToolbar(this.barMainEdit, this.barMain);
			if (this.mFormActiveMode == FormActiveMode.PopUp
				|| this.mFormActiveMode == FormActiveMode.Report)
			{
				this.ShowInTaskbar = false;
				this.ControlBox = false;
				this.Size = new Size(750, 480);
			}

			this.cmbVatIsOut.Properties.Items.Clear();

			this.cmbVatIsOut.Properties.Items.AddRange(new object[] { 
														UIBase.GetAppUIText(new string[] { "Y = VAT แยกนอก", "Y = Exclude VAT" })
														, UIBase.GetAppUIText(new string[] { "N = VAT รวมใน", "N = Include VAT" }) });

			this.cmbVatIsOut.SelectedIndex = 0;

			this.cmbCtrlStock.Properties.Items.Clear();
			this.cmbCtrlStock.Properties.Items.AddRange(new object[] { 
														UIBase.GetAppUIText(new string[] { "0 = ตามที่บริษัทกำหนด", "0 = As Specify In Company Menu" })
														, UIBase.GetAppUIText(new string[] { "1 = นับยอดสต๊อคและไม่ให้ติดลบ", "1 = Control Stock Not Less Than 0" })
														, UIBase.GetAppUIText(new string[] { "2 = นับยอดสต๊อคแต่ยอดติดลบได้", "2 = Control Stock Buy Less Than 0" })
														, UIBase.GetAppUIText(new string[] { "3 = ยังไม่นับยอดสต๊อค", "3= Not Control Stock" }) });

			this.cmbCtrlStock.SelectedIndex = 0;

			this.cmbIsConsume.Properties.Items.Clear();
			this.cmbIsConsume.Properties.Items.AddRange(new object[] { 
														UIBase.GetAppUIText(new string[] { "Y = ออกรายงานสินค้าคงเหลือ", "Y = Shown in Stock Report" })
														, UIBase.GetAppUIText(new string[] { "N = ไม่ออกรายงานสินค้าคงเหลือ", "N = Don't Shown in Stock Report" }) });

			this.cmbIsConsume.SelectedIndex = 0;

			this.cmbActive.Properties.Items.Clear();
			this.cmbActive.Properties.Items.AddRange(new object[] { "ว่าง = ACTIVE", "I = INACTIVE" });
			this.cmbActive.SelectedIndex = 0;

#region "Add Standard UM"
			this.paTxtStdUM.Add(this.txtQnStdUm1);
			this.paTxtStdUM.Add(this.txtQnStdUm2);
			this.paTxtStdUM.Add(this.txtQnStdUm3);
			this.paTxtStdUM.Add(this.txtQnStdUm4);
			this.paTxtStdUM.Add(this.txtQnStdUm6);
			this.paTxtStdUM.Add(this.txtQnStdUm7);
			this.paTxtStdUM.Add(this.txtQnStdUm8);
			this.paTxtStdUM.Add(this.txtQnStdUm9);
			this.paTxtStdUM.Add(this.txtQnStdUm10);
			this.paTxtStdUM.Add(this.txtQnStdUm12);
			this.paTxtStdUM.Add(this.txtQnStdUm13);
			this.paTxtStdUM.Add(this.txtQnStdUm14);
			this.paTxtStdUM.Add(this.txtQnStdUm15);
			this.paTxtStdUM.Add(this.txtQnStdUm17);
			this.paTxtStdUM.Add(this.txtQnStdUm18);
			this.paTxtStdUM.Add(this.txtQnStdUm19);
			this.paTxtStdUM.Add(this.txtQnStdUm20);
			this.paTxtStdUM.Add(this.txtQnStdUm22);
			this.paTxtStdUM.Add(this.txtQnStdUm23);
			this.paTxtStdUM.Add(this.txtQnStdUm24);
			this.paTxtStdUM.Add(this.txtQnStdUm25);
			this.paTxtStdUM.Add(this.txtQnStdUm26);
			this.paTxtStdUM.Add(this.txtQnStdUm27);
			this.paTxtStdUM.Add(this.txtQnStdUm28);
			this.paTxtStdUM.Add(this.txtQnStdUm29);
			this.paTxtStdUM.Add(this.txtQnPrice1);
			this.paTxtStdUM.Add(this.txtQnPriceA1);
			this.paTxtStdUM.Add(this.txtQnPriceB1);
			this.paTxtStdUM.Add(this.txtQnPriceC1);
			this.paTxtStdUM.Add(this.txtQnPriceD1);
			#endregion

			this.pmSetFormUI();
			UIBase.SetDefaultChildAppreance(this);

			this.pmCreateTem();
			this.pmInitGridProp_TemPdVer();

		}

		private string mstrFormMenuName = "";
		private void pmSetFormUI()
		{

			this.Text = UIBase.GetAppUIText(new string[] { "ฐานข้อมูลสินค้า/วัตถุดิบ", "Product/Rawmat Items" });
			this.mstrFormMenuName = UIBase.GetAppUIText(new string[] { "สินค้า", "Product" });

			this.pgfMainEdit.TabPages[0].Text = UIBase.GetAppUIText(new string[] { "หน้า Browse", "Browse Page" });

			for (int i = 1; i < this.pgfMainEdit.TabPages.Count; i++)
			{
				this.pgfMainEdit.TabPages[i].Text = UIBase.GetAppUIText(new string[] { "หน้าที่ " + i.ToString(), "Detail Page " + i.ToString() });
			}
				
			this.lblCode.Text = UIBase.GetAppUIText(new string[] { "รหัสสินค้า:", "Product Code:" });
			this.lblName.Text = UIBase.GetAppUIText(new string[] { "ชื่อสินค้า (ไทย):", "Name Lang-1:" });
			this.lblSName.Text = UIBase.GetAppUIText(new string[] { "ชื่อย่อภาษาไทย:", "Abbr. Name Lang-1:" });
			this.lblName2.Text = UIBase.GetAppUIText(new string[] { "ชื่อสินค้า (ภาษา2):", "Name Lang-2:" });
			this.lblSName2.Text = UIBase.GetAppUIText(new string[] { "ชื่อย่อภาษา 2:", "Abbr. Name Lang-2:" });
			this.lblPDType.Text = UIBase.GetAppUIText(new string[] { "ชนิดสินค้า:", "Product Type:" });
			this.lblPdGrp.Text = UIBase.GetAppUIText(new string[] { "รหัสกลุ่มสินค้า:", "Product Group:" });
			this.lblCost.Text = UIBase.GetAppUIText(new string[] { "ราคาทุนมาตรฐาน:", "Standard Cost:" });
			this.lblSaleMeth.Text = UIBase.GetAppUIText(new string[] { "ราคาขายเป็นแบบ:", "Price Is:" });
			this.lblStockMeth.Text = UIBase.GetAppUIText(new string[] { "การนับยอดสต๊อค:", "Control Stock:" });
			this.lblSuppl.Text = UIBase.GetAppUIText(new string[] { "รหัสผู้จำหน่าย:", "Supplier Code:" });
			this.lblIsComsume.Text = UIBase.GetAppUIText(new string[] { "ออกรายงานสินค้าคงเหลือ:", "Shown in Stock Report:" });

			this.lblUM.Text = UIBase.GetAppUIText(new string[] { "หน่วยนับมาตรฐาน :", "Base U/M:" });
			this.lblPUM.Text = UIBase.GetAppUIText(new string[] { "หน่วยซื้อส่วนใหญ่ :", "Purchase Unit Measure :" });
			this.lblSUM.Text = UIBase.GetAppUIText(new string[] { "หน่วยขายส่วนใหญ่ :", "Sale Unit Measure :" });
			this.lblStkUM.Text = UIBase.GetAppUIText(new string[] { "หน่วยนับระบบคุม STOCK :", "STOCK Unit Mesure :" });
			this.lblStk_PUM.Text = UIBase.GetAppUIText(new string[] { "หน่วยคุม STOCK ที่ใช้ในการ ซื้อส่วนใหญ่ :", "STOCK Buy Unit :" });
			this.lblStk_SUM.Text = UIBase.GetAppUIText(new string[] { "หน่วยคุม STOCK ที่ใช้ในการ ขายส่วนใหญ่ :", "STOCK Sale Unit :" });

			this.lblBRAcc.Text = UIBase.GetAppUIText(new string[] { "บัญชีซื้อสด :", "Purcahse Acc." });
			this.lblBIAcc.Text = UIBase.GetAppUIText(new string[] { "บัญชีซื้อเชื่อ :", "Credit Purcahse Acc." });
			this.lblSRAcc.Text = UIBase.GetAppUIText(new string[] { "บัญชีขายสด :", "Cash Sale Acc." });
			this.lblSIAcc.Text = UIBase.GetAppUIText(new string[] { "บัญชีขายเชื่อ :", "Credit Sale Acc." });

			this.lblSalePrice.Text = UIBase.GetAppUIText(new string[] { "ราคาขายมาตรฐาน:", "Standard Price:" });
			this.lblMinSPrice.Text = UIBase.GetAppUIText(new string[] { "ราคาต่ำสุด:", "Minimum Price:" });
			this.lblDisc.Text = UIBase.GetAppUIText(new string[] { "ส่วนลดมาตรฐาน:", "Standard Disc.:" });

			this.lblPer1.Text = UIBase.GetAppUIText(new string[] { "ละ", "Each" });
			this.lblPer2.Text = this.lblPer1.Text;
			this.lblPer3.Text = this.lblPer1.Text;
			this.lblPer4.Text = this.lblPer1.Text;
			this.lblPer5.Text = this.lblPer1.Text;
			this.lblPer6.Text = this.lblPer1.Text;
			
			this.lblPerA1.Text = this.lblPer1.Text;
			this.lblPerA2.Text = this.lblPer1.Text;
			this.lblPerA3.Text = this.lblPer1.Text;
			this.lblPerA4.Text = this.lblPer1.Text;
			this.lblPerA5.Text = this.lblPer1.Text;

			this.lblPerB1.Text = this.lblPer1.Text;
			this.lblPerB2.Text = this.lblPer1.Text;
			this.lblPerB3.Text = this.lblPer1.Text;
			this.lblPerB4.Text = this.lblPer1.Text;
			this.lblPerB5.Text = this.lblPer1.Text;

			this.lblPerC1.Text = this.lblPer1.Text;
			this.lblPerC2.Text = this.lblPer1.Text;
			this.lblPerC3.Text = this.lblPer1.Text;
			this.lblPerC4.Text = this.lblPer1.Text;
			this.lblPerC5.Text = this.lblPer1.Text;

			this.lblPerD1.Text = this.lblPer1.Text;
			this.lblPerD2.Text = this.lblPer1.Text;
			this.lblPerD3.Text = this.lblPer1.Text;
			this.lblPerD4.Text = this.lblPer1.Text;
			this.lblPerD5.Text = this.lblPer1.Text;

			this.grbPriceA.Text = UIBase.GetAppUIText(new string[] { "ราคาขาย A", "Grade A" });
			this.grbPriceB.Text = UIBase.GetAppUIText(new string[] { "ราคาขาย B", "Grade B" });
			this.grbPriceC.Text = UIBase.GetAppUIText(new string[] { "ราคาขาย C", "Grade C" });
			this.grbPriceD.Text = UIBase.GetAppUIText(new string[] { "ราคาขาย D", "Grade D" });

			this.lblRemark1.Text = UIBase.GetAppUIText(new string[] { "หมายเหตุ 1:", "Remark 1:" });
			this.lblRemark2.Text = UIBase.GetAppUIText(new string[] { "หมายเหตุ 2:", "Remark 2:" });
			this.lblRemark3.Text = UIBase.GetAppUIText(new string[] { "หมายเหตุ 3:", "Remark 3:" });
			this.lblRemark4.Text = UIBase.GetAppUIText(new string[] { "หมายเหตุ 4:", "Remark 4:" });
			this.lblRemark5.Text = UIBase.GetAppUIText(new string[] { "หมายเหตุ 5:", "Remark 5:" });
			this.lblRemark6.Text = UIBase.GetAppUIText(new string[] { "หมายเหตุ 6:", "Remark 6:" });
			this.lblRemark7.Text = UIBase.GetAppUIText(new string[] { "หมายเหตุ 7:", "Remark 7:" });
			this.lblRemark8.Text = UIBase.GetAppUIText(new string[] { "หมายเหตุ 8:", "Remark 8:" });
			this.lblRemark9.Text = UIBase.GetAppUIText(new string[] { "หมายเหตุ 9:", "Remark 9:" });
			this.lblRemark10.Text = UIBase.GetAppUIText(new string[] { "หมายเหตุ 10:", "Remark 10:" });

		}

		private void pmCreateTem()
		{

			DataTable dtbTemPdVer = new DataTable(this.mstrTemPdVer);

			dtbTemPdVer.Columns.Add("cRowID", System.Type.GetType("System.String"));
			dtbTemPdVer.Columns.Add("cVersion", System.Type.GetType("System.String"));
			dtbTemPdVer.Columns.Add("dDate", System.Type.GetType("System.DateTime"));
			dtbTemPdVer.Columns.Add("nQty", System.Type.GetType("System.Decimal"));
			dtbTemPdVer.Columns.Add("cUm", System.Type.GetType("System.String"));
			dtbTemPdVer.Columns.Add("cQnUm", System.Type.GetType("System.String"));

			dtbTemPdVer.Columns["cRowID"].DefaultValue = "";
			dtbTemPdVer.Columns["cVersion"].DefaultValue = "";
			dtbTemPdVer.Columns["nQty"].DefaultValue = 0;
			dtbTemPdVer.Columns["cUm"].DefaultValue = "";
			dtbTemPdVer.Columns["cQnUm"].DefaultValue = "";

			this.dtsDataEnv.Tables.Add(dtbTemPdVer);

		}

		private void pmSetBrowView()
		{

			string strErrorMsg = "";
			//WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
			WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

			string strFMDBName = App.ConfigurationManager.ConnectionInfo.ERPDBMSName;
			string strEmplRTab = strFMDBName + ".dbo.EMPLR";

			//string strStat = "CSTATUS = case {0}.CSTATUS when 'I' then 'INACTIVE' when '' then 'ACTIVE' end";
			//string strSQLExec = "select {0}.CROWID, {0}.CCODE, {0}.CSNAME, {0}.CNAME, " + strStat + ", {0}.DINACTIVE, {0}.CTYPE, {0}.DCREATE, {0}.DLASTUPD, EM1.FCLOGIN as CLOGIN_ADD, EM2.FCLOGIN as CLOGIN_UPD from {0} ";
			//strSQLExec += " left join {1} EM1 ON EM1.FCSKID = {0}.CCREATEBY ";
			//strSQLExec += " left join {1} EM2 ON EM2.FCSKID = {0}.CLASTUPDBY ";
			//strSQLExec += " where {0}.CCORP = ?";


			//string strSQLExec = "select {0}.FCSKID as CROWID, {0}.FCCODE as CCODE, {0}.FCSNAME as CSNAME , {0}.FCNAME as CNAME, " + strStat + ", {0}.FDINACTIVE as DINACTIVE, {0}.FCTYPE as CTYPE , {0}.FTCREATE as DCREATE, {0}.FTLASTUPD as DLASTUPD, EM1.FCLOGIN as CLOGIN_ADD, EM2.FCLOGIN as CLOGIN_UPD from {0} ";
			//strSQLExec += " left join {1} EM1 ON EM1.FCSKID = {0}.FCCREATEBY ";
			//strSQLExec += " left join {1} EM2 ON EM2.FCSKID = {0}.FCLASTUPDBY ";

			string strStat = "CSTATUS = case {0}.FCSTATUS when 'I' then 'INACTIVE' when '' then 'ACTIVE' end";

			//string strSQLExec = "select {0}.FCSKID as CROWID, {0}.FCCODE as CCODE, {0}.FCSNAME as CSNAME , {0}.FCNAME as CNAME, {0}.FNPRICE as NPRICE, " + strStat + ", {0}.FDINACTIVE as DINACTIVE, {0}.FCTYPE as CTYPE , {0}.FTDATETIME as DCREATE, {0}.FTLASTUPD as DLASTUPD from {0} ";
			string strSQLExec = "select {0}.FCSKID as CROWID, {0}.FCCODE as CCODE, {0}.FCSNAME as CSNAME , {0}.FCNAME as CNAME, {0}.FNPRICE as NPRICE, {0}.FNSTDCOST as STDCOST, " + strStat + ", {0}.FDINACTIVE as DINACTIVE, {0}.FCTYPE as CTYPE , {0}.FTDATETIME as DCREATE, {0}.FTLASTUPD as DLASTUPD ";
			strSQLExec += " , UM.FCNAME as QNUM ";
			strSQLExec += " from {0} ";
			strSQLExec += " left join UM on PROD.FCUM = UM.FCSKID";
			strSQLExec += " where {0}.FCCORP = ?";

			strSQLExec = string.Format(strSQLExec, new string[] { MapTable.Table.Product, "EMPLR" });

			pobjSQLUtil.SetPara(new object[] { App.gcCorp });
			pobjSQLUtil.NotUpperSQLExecString = true;
			pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrBrowViewAlias, "Master_Prod", strSQLExec, ref strErrorMsg);

		}

		private void pmInitGridProp()
		{
			System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
			dvBrow.Sort = "CCODE";

			this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

			for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
			{
				this.gridView1.Columns[intCnt].Visible = false;
				this.gridView1.Columns[intCnt].OptionsColumn.ShowInCustomizationForm = false;
			}

			this.gridView1.Columns["CSTATUS"].OptionsColumn.ShowInCustomizationForm = true;
			this.gridView1.Columns["DINACTIVE"].OptionsColumn.ShowInCustomizationForm = true;
			this.gridView1.Columns["CCODE"].OptionsColumn.ShowInCustomizationForm = true;
			this.gridView1.Columns["CTYPE"].OptionsColumn.ShowInCustomizationForm = true;
			this.gridView1.Columns["CNAME"].OptionsColumn.ShowInCustomizationForm = true;
			this.gridView1.Columns["CSNAME"].OptionsColumn.ShowInCustomizationForm = true;
			this.gridView1.Columns["NPRICE"].OptionsColumn.ShowInCustomizationForm = true;
			this.gridView1.Columns["STDCOST"].OptionsColumn.ShowInCustomizationForm = true;
			this.gridView1.Columns["QNUM"].OptionsColumn.ShowInCustomizationForm = true;

			int i = 0;
			//this.gridView1.Columns["CSTATUS"].VisibleIndex = i++;
			//this.gridView1.Columns["DINACTIVE"].VisibleIndex = i++;
			this.gridView1.Columns["CTYPE"].VisibleIndex = i++;
			this.gridView1.Columns["CCODE"].VisibleIndex = i++;
			this.gridView1.Columns["CNAME"].VisibleIndex = i++;
			this.gridView1.Columns["CSNAME"].VisibleIndex = i++;
			this.gridView1.Columns["STDCOST"].VisibleIndex = i++;
			this.gridView1.Columns["QNUM"].VisibleIndex = i++;

			this.gridView1.Columns["CSTATUS"].Caption = UIBase.GetAppUIText(new string[] { "สถานะ", "Status" });
			this.gridView1.Columns["DINACTIVE"].Caption = UIBase.GetAppUIText(new string[] { "วันที่ InActive", "InActive Date" });
			this.gridView1.Columns["CTYPE"].Caption = " ";
			this.gridView1.Columns["CCODE"].Caption = UIBase.GetAppUIText(new string[] { "รหัสสินค้า", "Product Code" });
			this.gridView1.Columns["CNAME"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อสินค้า", "Product Name" });
			this.gridView1.Columns["CSNAME"].Caption = UIBase.GetAppUIText(new string[] { "ชื่อย่อ", "Abbr. Name" });
			this.gridView1.Columns["NPRICE"].Caption = UIBase.GetAppUIText(new string[] { "ราคาขาย", "Price" });
			this.gridView1.Columns["QNUM"].Caption = UIBase.GetAppUIText(new string[] { "หน่วยนับ", "U/M" });

			this.gridView1.Columns["STDCOST"].Caption = UIBase.GetAppText(App.mLocale_UI, new string[] { "ต้นทุน", "Std.Cost" });
			this.gridView1.Columns["STDCOST"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.gridView1.Columns["STDCOST"].DisplayFormat.FormatString = "#,###,###,##0.0000";


			this.gridView1.Columns["CSTATUS"].Width = 15;
			this.gridView1.Columns["DINACTIVE"].Width = 15;
			this.gridView1.Columns["CTYPE"].Width = 30;
			this.gridView1.Columns["CCODE"].Width = 120;
			this.gridView1.Columns["CSNAME"].Width = 100;
			this.gridView1.Columns["NPRICE"].Width = 20;
			this.gridView1.Columns["STDCOST"].Width = 70;
			this.gridView1.Columns["QNUM"].Width = 70;

			this.pmSetSortKey("CCODE", true);
		}

		private void grdBrowView_Resize(object sender, EventArgs e)
		{
			this.pmCalcColWidth1();
		}

		private void gridView1_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
		{
			this.pmCalcColWidth1();
		}

		private void pmCalcColWidth1()
		{

			int intColWidth = this.gridView1.Columns["CTYPE"].Width
									+ this.gridView1.Columns["CCODE"].Width
									+ this.gridView1.Columns["STDCOST"].Width
									+ this.gridView1.Columns["QNUM"].Width
									+ this.gridView1.Columns["CSNAME"].Width;

			int intNewWidth = this.Width - intColWidth - 60;
			this.gridView1.Columns["CNAME"].Width = (intNewWidth > 70 ? intNewWidth : 70);

		}

		private void pmInitGridProp_TemPdVer()
		{
			System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrTemPdVer].DefaultView;

			this.grdHistory.DataSource = this.dtsDataEnv.Tables[this.mstrTemPdVer];

			for (int intCnt = 0; intCnt < this.gridView2.Columns.Count; intCnt++)
			{
				this.gridView2.Columns[intCnt].Visible = true;
			}

			this.gridView2.Columns["cRowID"].Visible = false;
			this.gridView2.Columns["cUm"].Visible = false;

			this.gridView2.Columns["cVersion"].Caption = "พิมพ์ครั้งที่";
			this.gridView2.Columns["dDate"].Caption = "วัน/เดือน/ปี";
			this.gridView2.Columns["nQty"].Caption = "จำนวนสินค้า";
			this.gridView2.Columns["cQnUm"].Caption = "หน่วยนับ";

			this.gridView2.Columns["cVersion"].Width = 15;
			this.gridView2.Columns["dDate"].Width = 15;
			this.gridView2.Columns["nQty"].Width = 15;
			this.gridView2.Columns["cQnUm"].Width = 30;

			this.gridView2.Columns["cQnUm"].ColumnEdit = this.grcUM;
			this.gridView2.Columns["dDate"].ColumnEdit = this.grcDate;
			this.gridView2.Columns["nQty"].ColumnEdit = this.grcValue;
			//this.gridView2.Columns["cQnProd"].ColumnEdit = this.grcUM;

		}

		private void pmSetSortKey(string inColumn, bool inIsClear)
		{
			if (inIsClear)
			{
				this.gridView1.SortInfo.Clear();
				this.gridView1.SortInfo.Add(this.gridView1.Columns[this.mstrSortKey], DevExpress.Data.ColumnSortOrder.Ascending);
				this.gridView1.RefreshData();
			}

			for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
			{
				this.gridView1.Columns[intCnt].AppearanceCell.BackColor = Color.White;
			}
			this.mstrSortKey = inColumn.ToUpper();
			this.gridView1.Columns[this.mstrSortKey].AppearanceCell.BackColor = Color.WhiteSmoke;
			this.gridView1.FocusedColumn = this.gridView1.Columns[this.mstrSortKey];
		}

		private void pgfMainEdit_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
		{
			this.pmSetToolbarState(pgfMainEdit.SelectedTabPageIndex);

			if (this.mFormActiveMode == FormActiveMode.PopUp
				|| this.mFormActiveMode == FormActiveMode.Report)
			{
				if (pgfMainEdit.SelectedTabPageIndex == xd_PAGE_BROWSE)
				{
					this.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
				}
				else
				{
					this.Location = new Point(Convert.ToInt16((AppUtil.CommonHelper.SysMetric(1) - this.Width) / 2), Convert.ToInt16((AppUtil.CommonHelper.SysMetric(2) - this.Height) / 2));
				}
			}
		
		}

		private void pmSetToolbarState(int inActivePage)
		{
			this.barMainEdit.Items[WsToolBar.Enter.ToString()].Visibility = (this.mFormActiveMode == FormActiveMode.PopUp ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never);
			//this.barMainEdit.Items[WsToolBar.Export.ToString()].Visibility = (this.mFormActiveMode == FormActiveMode.PopUp ? DevExpress.XtraBars.BarItemVisibility.Never : DevExpress.XtraBars.BarItemVisibility.Always);

			if (this.mFormActiveMode == FormActiveMode.Report)
			{
				this.barMainEdit.Items[WsToolBar.Enter.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
				this.barMainEdit.Items[WsToolBar.Insert.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
				this.barMainEdit.Items[WsToolBar.Update.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
				this.barMainEdit.Items[WsToolBar.Delete.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
			}

			this.barMainEdit.Items[WsToolBar.Enter.ToString()].Enabled = (inActivePage == 0 ? true : false);
			this.barMainEdit.Items[WsToolBar.Insert.ToString()].Enabled = (inActivePage == 0 ? true : false);
			this.barMainEdit.Items[WsToolBar.Update.ToString()].Enabled = (inActivePage == 0 ? true : false);
			this.barMainEdit.Items[WsToolBar.Delete.ToString()].Enabled = (inActivePage == 0 ? true : false);
			this.barMainEdit.Items[WsToolBar.Search.ToString()].Enabled = (inActivePage == 0 ? true : false);
			this.barMainEdit.Items[WsToolBar.Refresh.ToString()].Enabled = (inActivePage == 0 ? true : false);
			//this.barMainEdit.Items[WsToolBar.Export.ToString()].Enabled = (inActivePage == 0 ? true : false);

			this.barMainEdit.Items[WsToolBar.Save.ToString()].Enabled = (inActivePage == 0 ? false : true);
			this.barMainEdit.Items[WsToolBar.Undo.ToString()].Enabled = (inActivePage == 0 ? false : true);

            this.barMainEdit.Items[WsToolBar.Print.ToString()].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        
        }

		private void pmGotoBrowPage()
		{
			this.pmBlankFormData();
			this.pmRefreshBrowView();

			this.pmSetPageStatus(false);
			this.pgfMainEdit.TabPages[0].PageEnabled = true;
			this.pgfMainEdit.SelectedTabPageIndex = 0;
			this.grdBrowView.Focus();
		}

		private void pmRefreshBrowView()
		{
			this.mintSaveBrowViewRowIndex = this.gridView1.FocusedRowHandle;
			this.pmSetBrowView();
			this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

			if (this.gridView1.RowCount > this.mintSaveBrowViewRowIndex)
				this.gridView1.FocusedRowHandle = this.mintSaveBrowViewRowIndex;

		}

		private void pmSetPageStatus(bool inIsEnable)
		{
			for (int intCnt = 0; intCnt < this.pgfMainEdit.TabPages.Count; intCnt++)
			{
				this.pgfMainEdit.TabPages[intCnt].PageEnabled = inIsEnable;
			}
		}

		private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			if (e.Item.Tag != null)
			{
				string strMenuName = e.Item.Tag.ToString();
                switch (strMenuName)
                {
                    case "RPT_DESIGN":
                        pmEditReportForm();
                        break;
                    default:
                        #region "Default Menu"
                        WsToolBar oToolButton = AppEnum.GetToolBarEnum(strMenuName);
                        switch (oToolButton)
                        {
                            case WsToolBar.Enter:
                                this.pmEnterForm();
                                break;
                            case WsToolBar.Insert:
                                if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanInsert, App.AppUserName, App.AppUserID))
                                {
                                    this.mFormEditMode = UIHelper.AppFormState.Insert;
                                    this.pmLoadEditPage();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการเพิ่มข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;
                            case WsToolBar.Update:
                                if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanEdit, App.AppUserName, App.AppUserID))
                                {
                                    this.mFormEditMode = UIHelper.AppFormState.Edit;
                                    this.pmLoadEditPage();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการแก้ไขข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;
                            case WsToolBar.Delete:
                                if (App.PermissionManager.CheckPermission(TASKNAME, AuthenType.CanDelete, App.AppUserName, App.AppUserID))
                                {
                                    this.pmDeleteData();
                                }
                                else
                                    MessageBox.Show("ไม่มีสิทธิ์ในการลบข้อมูล !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                break;
                            case WsToolBar.Print:
                                this.pmPrintData();
                                break;
                            case WsToolBar.Search:
                                if (this.gridView1.OptionsView.ShowAutoFilterRow)
                                {
                                    this.gridView1.OptionsView.ShowAutoFilterRow = false;
                                    this.gridView1.ClearColumnsFilter();
                                }
                                else
                                {
                                    this.gridView1.OptionsView.ShowAutoFilterRow = true;
                                }
                                //this.pmSearchData();
                                break;
                            case WsToolBar.Undo:
                                if (MessageBox.Show("ยกเลิกการแก้ไข ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                                    this.pmGotoBrowPage();
                                break;
                            case WsToolBar.Save:
                                this.pmSaveData();
                                break;
                            case WsToolBar.Refresh:
                                this.pmRefreshBrowView();
                                break;
                            case WsToolBar.Export:
                                this.pmExportBrowView();
                                break;
                        }
                        #endregion
                        break;
                }

			}
		}

        private void pmEditReportForm()
        {

            Report.RPT.xrPdBarCode01 report = new Report.RPT.xrPdBarCode01();

            //string[] strADir = new string[] { Application.StartupPath + "\\RPT\\PFORM_WHTTAX_01.REPX" };

            string pstrRPTName = "";

            //string[] strADir = null;
            //string strFormPath = Application.StartupPath + "\\RPT\\FORM_" + this.mstrRefType + "\\";

            //pstrRPTName = Application.StartupPath + "\\RPT\\FORM_PROD\\";
            //pstrRPTName = Application.StartupPath + "\\RPT\\FORM_PROD\\PPROD01.REPX";
            using (Transaction.Common.frmRPTSelect dlg = new Transaction.Common.frmRPTSelect())
            {

                string[] strADir = null;
                string strFormPath = Application.StartupPath + "\\RPT\\FORM_PROD\\";
                if (System.IO.Directory.Exists(strFormPath))
                {
                    strADir = System.IO.Directory.GetFiles(strFormPath);
                    dlg.LoadRPT(strFormPath);
                }

                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    pstrRPTName = strADir[dlg.lstRPT.SelectedIndex];
                    //this.pmPrintRangeData(dlg.BeginCode, dlg.EndCode, strADir[dlg.cmbPForm.SelectedIndex]);

                    if (System.IO.File.Exists(pstrRPTName))
                        report.LoadLayout(pstrRPTName);

                    frmDXReportDesign designForm = new frmDXReportDesign();

                    try
                    {
                        designForm.OpenReport(report);
                        designForm.FileName = pstrRPTName;
                        designForm.ShowDesignerForm(this);
                        //this.pmShowDesignerForm(designForm, this);
                        if (designForm.FileName != pstrRPTName && System.IO.File.Exists(designForm.FileName))
                            System.IO.File.Copy(designForm.FileName, pstrRPTName, true);
                    }
                    finally
                    {
                        designForm.Dispose();
                        report.Dispose();
                    }

                }
            }

        }

		private void pmExportBrowView()
		{
			this.dlgSaveFile.ShowDialog();
			if (this.dlgSaveFile.FileName != "")
			{
				try
				{
					//this.grdBrowView.ExportToXls(this.dlgSaveFile.FileName);
                    this.dtsDataEnv.Tables[this.mstrBrowViewAlias].WriteXmlSchema("d:\\temp\\p1.xsd");
                    this.dtsDataEnv.Tables[this.mstrBrowViewAlias].WriteXml("d:\\temp\\p1.xml");
					MessageBox.Show(this, "Complete", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, "เกิดข้อผิดพลาดระหว่างการ Export กรุณาลองใหม่อีกครั้ง \r\n" + ex.Message, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				finally{}
			}

		}

		private void pmDeleteData()
		{
			string strErrorMsg = "";

			DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
			if (this.gridView1.RowCount == 0 || dtrBrow == null)
				return;

			this.mstrEditRowID = dtrBrow["cRowID"].ToString();

			string strDeleteDesc = "(" + dtrBrow["cCode"].ToString().TrimEnd() + ") " + dtrBrow["cName"].ToString().TrimEnd();
			if (!this.pmCheckHasUsed(this.mstrEditRowID, dtrBrow["cCode"].ToString(), ref strErrorMsg))
			{
				//if (MessageBox.Show("ยืนยันการลบข้อมูลสินค้า : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
				if (MessageBox.Show(UIBase.GetAppUIText(new string[] { "ยืนยันการลบข้อมูล", "Do you want to delete " }) + this.mstrFormMenuName + " : " + strDeleteDesc + " ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
				{
					if (this.pmDeleteRow(this.mstrEditRowID, dtrBrow["cCode"].ToString(), dtrBrow["cName"].ToString(), ref strErrorMsg))
					{
						this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.RemoveAt(this.pmGetRowID(this.mstrEditRowID));
					}
				}
			}
			else
			{
				if (strErrorMsg != "")
					MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			this.mstrEditRowID = "";
		}

		private bool pmCheckHasUsed(string inRowID, string inCode, ref string ioErrorMsg)
		{
			bool bllHasUsed = this.pmHasUsedChild1Corp(inRowID, ref ioErrorMsg);
			if (bllHasUsed)
			{
				ioErrorMsg = UIBase.GetAppUIText(new string[] { "ไม่สามารถลบข้อมูลได้เนื่องจากมีการอ้างอิงถึงใน ", "Can not delete because has refer to " }) + ioErrorMsg;
			}
			return bllHasUsed;
		}

		private bool pmHasUsedChild1Corp(string inRowID, ref string ioErrorMsg)
		{
			bool bllResult = false;
			string strErrorMsg = "";
			string strRefMsg = "";

			WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
			WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

			objSQLHelper.SetPara(new object[] { inRowID });
			if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.RefProd, "select GLRef.fcSkid, GLRef.fcCode, GLRef.fcRefNo from RefProd left join GLREF on GLREF.FCSKID = REFPROD.FCGLREF where RefProd.fcProd = ?", ref strErrorMsg))
			{
				//string strCorpStr = "บริษัท (" + this.mdtrCurrCorp["fcCode"].ToString().TrimEnd() + ") " + this.mdtrCurrCorp["fcName"].ToString().TrimEnd();
				foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
				{
					//strRefMsg += "\r\n     (" + dtrChildTab["fcCode"].ToString().TrimEnd() + ") " + dtrChildTab["fcName"].ToString().TrimEnd();
					strRefMsg += "\r\n     (" + dtrChildTab["fcRefNo"].ToString().TrimEnd() + ") ";
				}
				//ioErrorMsg = strCorpStr + "\r\nเอกสาร" + strRefMsg + "\r\n";
				ioErrorMsg = "Document : " + strRefMsg + "\r\n";
				return true;
			}

			if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.OrderI, "select OrderH.fcSkid, OrderH.fcCode, OrderH.fcRefNo from OrderI left join ORDERH on OrderH.FCSKID = OrderI.fcOrderH where OrderI.fcProd = ?", ref strErrorMsg))
			{
				//string strCorpStr = "บริษัท (" + this.mdtrCurrCorp["fcCode"].ToString().TrimEnd() + ") " + this.mdtrCurrCorp["fcName"].ToString().TrimEnd();
				foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
				{
					//strRefMsg += "\r\n     (" + dtrChildTab["fcCode"].ToString().TrimEnd() + ") " + dtrChildTab["fcName"].ToString().TrimEnd();
					strRefMsg += "\r\n     (" + dtrChildTab["fcRefNo"].ToString().TrimEnd() + ") ";
				}
				//ioErrorMsg = strCorpStr + "\r\nเอกสาร" + strRefMsg + "\r\n";
				ioErrorMsg = "Document : " + strRefMsg + "\r\n";
				return true;
			}

			objSQLHelper2.SetPara(new object[] { inRowID });
			if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.MFBOMHead, "select cCode, cName from " + MapTable.Table.MFBOMHead + " where cMfgProd = ?", ref strErrorMsg))
			{
				foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
				{
					strRefMsg += "\r\n     (" + dtrChildTab["cCode"].ToString().TrimEnd() + ") " + dtrChildTab["cName"].ToString().TrimEnd();
				}
				ioErrorMsg = "BOM : " + strRefMsg + "\r\n";
				return true;
			}

			objSQLHelper2.SetPara(new object[] { inRowID });
			if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QChk", MapTable.Table.MFBOMHead, "select MFBOMHD.cCode, MFBOMHD.cName from MFBOMIT_PD left join MFBOMHD on MFBOMHD.CROWID = MFBOMIT_PD.CBOMHD where MFBOMIT_PD.cProd = ?", ref strErrorMsg))
			{
				foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
				{
					strRefMsg += "\r\n     (" + dtrChildTab["cCode"].ToString().TrimEnd() + ") " + dtrChildTab["cName"].ToString().TrimEnd();
				}
				ioErrorMsg = "BOM : " + strRefMsg + "\r\n";
				return true;
			}

			objSQLHelper2.SetPara(new object[] { inRowID });
			if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QChk", "MFWORDERHD", "select MFWORDERHD.CCODE, MFWORDERHD.CREFNO, MFWORDERHD.DDATE from MFWORDERHD where MFWORDERHD.CMFGPROD = ? ", ref strErrorMsg))
			{
				foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
				{
					strRefMsg += "\r\n     (" + dtrChildTab["cRefNo"].ToString().TrimEnd() + ") " + " Date : " + Convert.ToDateTime(dtrChildTab["dDate"]).ToString("dd/MM/yy");
				}
				ioErrorMsg = "#MO " + strRefMsg + "\r\n";
				return true;
			}

			objSQLHelper2.SetPara(new object[] { inRowID });
			if (objSQLHelper2.SQLExec(ref this.dtsDataEnv, "QChk", "MFWORDERHD", "select MFWORDERHD.CCODE, MFWORDERHD.CREFNO, MFWORDERHD.DDATE from MFWORDERIT_PD left join MFWORDERHD on MFWORDERHD.CROWID = MFWORDERIT_PD.CWORDERH where MFWORDERIT_PD.CPROD = ? ", ref strErrorMsg))
			{
				foreach (DataRow dtrChildTab in this.dtsDataEnv.Tables["QChk"].Rows)
				{
					strRefMsg += "\r\n     (" + dtrChildTab["cRefNo"].ToString().TrimEnd() + ") " + " Date : " + Convert.ToDateTime(dtrChildTab["dDate"]).ToString("dd/MM/yy");
				}
				ioErrorMsg = "#MO " + strRefMsg + "\r\n";
				return true;
			}

			return bllResult;
		}

		private bool pmDeleteRow(string inRowID, string inCode, string inName, ref string ioErrorMsg)
		{
			bool bllIsCommit = false;
			bool bllResult = false;
			this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
			this.mSaveDBAgent.AppID = App.AppID;
			this.mdbConn = this.mSaveDBAgent.GetDBConnection();

			//this.mSaveDBAgent2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
			//this.mSaveDBAgent2.AppID = App.AppID;
			//this.mdbConn2 = this.mSaveDBAgent2.GetDBConnection();

			try
			{

				this.mdbConn.Open();
				this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

				//this.mdbConn2.Open();
				//this.mdbTran2 = this.mdbConn2.BeginTransaction(IsolationLevel.ReadUncommitted);

				string strErrorMsg = "";

				////Delete Child Table
				//Business.Entity.QMasterProd QRefChild = new QMasterProd(App.ConnectionString, App.DatabaseReside);
				//QRefChild.SetSaveDBAgent(this.mSaveDBAgent, this.mdbConn, this.mdbTran);
				//QRefChild.SetSaveDBAgent2(this.mSaveDBAgent2, this.mdbConn2, this.mdbTran2);
				//QRefChild.DeleteChildTable(inCode);

				object[] pAPara = new object[1] { inRowID };
				this.mSaveDBAgent.BatchSQLExec("delete from ProdX4 where fcProd = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
				this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrRefTable + " where fcSkid = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

				//string strRefRowID = "";
				//if (this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "QMaster_Prod", MapTable.Table.MasterProd, "select * from " + MapTable.Table.MasterProd + " where cCorp = ? and cCode = ?", new object[2] { App.gcCorp, inCode.TrimEnd() }, ref strErrorMsg, this.mdbConn, this.mdbTran))
				//{
				//    strRefRowID = this.dtsDataEnv.Tables["QMaster_Prod"].Rows[0]["cRowID"].ToString();

				//    object[] pAPara = new object[1] { strRefRowID };

				//    this.mSaveDBAgent.BatchSQLExec("delete from PdVersion where cProd = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
				//    this.mSaveDBAgent.BatchSQLExec("delete from " + MapTable.Table.MasterProdX1 + " where cProd = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

				//    this.mSaveDBAgent.BatchSQLExec("delete from " + this.mstrRefTable + " where cRowID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
				
				//}

				this.mdbTran.Commit();
				//this.mdbTran2.Commit();
				bllIsCommit = true;

				WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
				KeepLogAgent.KeepLog(objSQLHelper, KeepLogType.Delete, TASKNAME, inCode, inName, App.FMAppUserID, App.AppUserName);

				bllResult = true;
			}
			catch (Exception ex)
			{
				ioErrorMsg = ex.Message;
				bllResult = false;

				if (!bllIsCommit)
				{
					this.mdbTran.Rollback();
					//this.mdbTran2.Rollback();
				}
				App.WriteEventsLog(ex);
			}
			finally
			{
				this.mdbConn.Close();
				//this.mdbConn2.Close();
			}
			return bllResult;
		}

		private void pmEnterForm()
		{
			if ((this.mFormActiveMode == FormActiveMode.PopUp || this.mFormActiveMode == FormActiveMode.Report)
				&& this.pgfMainEdit.SelectedTabPageIndex == xd_PAGE_BROWSE)
			{
				this.mbllPopUpResult = true;
				this.Hide();
			}
		}

		private int pmGetRowID(string inTag)
		{
			for (int intCnt = 0; intCnt < this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Count; intCnt++)
			{
				if (this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows[intCnt]["cRowID"].ToString().TrimEnd() == inTag)
					return intCnt;
			}
			return -1;
		}

		private void pmSaveData()
		{
			string strErrorMsg = "";
			if (this.pmValidBeforeSave(ref strErrorMsg))
			{
				UIBase.WaitWind("กำลังบันทึกข้อมูล...");
				this.pmUpdateRecord();
				//dlg.WaitWind(WsWinUtil.GetStringfromCulture(this.WsLocale, new string[] { "บันทึกเรียบร้อย", "Save Complete" }), 500);
				UIBase.WaitWind("บันทึกเรียบร้อย");
				if (this.mFormEditMode == UIHelper.AppFormState.Edit)
					this.pmGotoBrowPage();
				else
					this.pmInsertLoop();

				UIBase.WaitClear();
			}
			if (strErrorMsg != string.Empty)
			{
				MessageBox.Show(strErrorMsg, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private bool pmValidBeforeSave(ref string ioErrorMsg)
		{
			bool bllResult = true;
			if (this.txtCode.Text.TrimEnd() == string.Empty)
			{
				string strMsg = UIBase.GetAppUIText(new string[] {
					"ยังไม่ได้ระบุ " + this.mstrFormMenuName + " Code ต้องการให้เครื่อง Running ให้หรือไม่ ?"
					,this.mstrFormMenuName + " Code is not define ! " + "Do you want to Auto Running Code ?"});

				if (MessageBox.Show(strMsg, "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					this.pmRunCode();
				}
			}

			if (this.txtCode.Text.TrimEnd() == string.Empty)
			{
				ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุรหัส" + this.mstrFormMenuName, this.mstrFormMenuName + " Code is not define ! " });
				this.pgfMainEdit.SelectedTabPageIndex = 1;
				this.txtCode.Focus();
				return false;
			}
			else if (this.txtName.Text.Trim() == string.Empty)
			{
				ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุชื่อ" + this.mstrFormMenuName, this.mstrFormMenuName + " Name is not define ! " });
				this.pgfMainEdit.SelectedTabPageIndex = 1;
				this.txtName.Focus();
				return false;
			}
			else if (this.txtSName.Text.Trim() == string.Empty)
			{
				this.pgfMainEdit.SelectedTabPageIndex = 1;
				ioErrorMsg = UIBase.GetAppUIText(new string[] { "ยังไม่ได้ระบุชื่อย่อ" + this.mstrFormMenuName, this.mstrFormMenuName + " Abbr. Name is not define ! " });
				this.txtSName.Focus();
				return false;
			}
			else if (this.txtQcPdGrp.Text.Trim() == string.Empty)
			{
				this.pgfMainEdit.SelectedTabPageIndex = 1;
				ioErrorMsg = UIBase.GetAppUIText(new string[] { "กรุณาระบุกลุ่มสินค้า", "Product Group is not define !" });
				this.txtQcPdGrp.Focus();
				return false;
			}
			else if (this.txtQcPdType.Text.Trim() == string.Empty)
			{
				this.pgfMainEdit.SelectedTabPageIndex = 1;
				ioErrorMsg = UIBase.GetAppUIText(new string[] { "กรุณาระบุชนิดสินค้า", "Product Type is not define !" });
				this.txtQcPdType.Focus();
				return false;
			}
			else if (this.txtQcUM.Text.Trim() == string.Empty)
			{
				this.pgfMainEdit.SelectedTabPageIndex = 2;
				ioErrorMsg = UIBase.GetAppUIText(new string[] { "กรุณาระบุหน่วยนับมาตรฐาน", "Base Unit is not define !" });
				this.txtQcUM.Focus();
				return false;
			}
			else if (this.txtQcUM1.Text.Trim() == string.Empty)
			{
				this.pgfMainEdit.SelectedTabPageIndex = 2;
				//ioErrorMsg = "กรุณาระบุหน่วยซื้อส่วนใหญ่";
				ioErrorMsg = UIBase.GetAppUIText(new string[] { "กรุณาระบุหน่วยซื้อส่วนใหญ่", "Purchase Unit is not define !" });
				this.txtQcUM1.Focus();
				return false;
			}
			else if (this.txtQcUM2.Text.Trim() == string.Empty)
			{
				this.pgfMainEdit.SelectedTabPageIndex = 2;
				//ioErrorMsg = "กรุณาระบุหน่วยขายส่วนใหญ่";
				ioErrorMsg = UIBase.GetAppUIText(new string[] { "กรุณาระบุหน่วยขายส่วนใหญ่", "Sale Unit is not define !" });
				this.txtQcUM2.Focus();
				return false;
			}
			else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldCode.TrimEnd() != this.txtCode.Text.TrimEnd())
			   && !this.pmIsValidateCode(new object[] { App.gcCorp, this.txtCode.Text.TrimEnd() }))
			{
				//ioErrorMsg = "รหัสสินค้าซ้ำ";
				ioErrorMsg = UIBase.GetAppUIText(new string[] { "รหัสสินค้าซ้ำ", "Duplicate Code !" });
				this.txtCode.Focus();
				return false;
			}
			else if ((this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName.TrimEnd() != this.txtName.Text.TrimEnd())
			   && !this.pmIsValidateName(new object[] { App.gcCorp, this.txtName.Text.TrimEnd() }))
			{
				//ioErrorMsg = "ชื่อสินค้าซ้ำ";
				ioErrorMsg = UIBase.GetAppUIText(new string[] { "ชื่อสินค้าซ้ำ", "Duplicate Name !" });
				this.txtName.Focus();
				return false;
			}
			else if (this.txtName2.Text.TrimEnd() != string.Empty
				&& (this.mFormEditMode == UIHelper.AppFormState.Insert || this.mstrOldName2.TrimEnd() != this.txtName2.Text.TrimEnd())
				&& !this.pmIsValidateName2(new object[] { App.gcCorp, this.txtName2.Text.TrimEnd() }))
			{
				//ioErrorMsg = "ชื่อสินค้าภาษา 2 ซ้ำ";
				ioErrorMsg = UIBase.GetAppUIText(new string[] { "ชื่อสินค้าภาษา 2 ซ้ำ", "Duplicate Name-2 !" });
				this.txtName2.Focus();
				return false;
			}
			else
				bllResult = true;

			return bllResult;
		}

		private bool pmRunCode()
		{
			//this.txtCode.Text = this.mobjTabRefer.RunCode(new object[] { App.ActiveCorp.RowID, this.mstrBranchID }, this.txtCode.MaxLength);

			string strErrorMsg = "";
			string strLastRunCode = "";
			int intCodeLen = 0;
			int intRunCode = 1;
			int inMaxLength = this.txtCode.Properties.MaxLength;
			WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
			objSQLHelper.SetPara(new object[] { App.gcCorp });
			if (objSQLHelper.SQLExec(ref this.dtsDataEnv, "QRunCode", this.mstrRefTable, "select cCode from " + this.mstrMasterTable + " where fcCorp = ? and fcCode < ':' order by fcCode desc", ref strErrorMsg))
			{
				strLastRunCode = this.dtsDataEnv.Tables["QRunCode"].Rows[0]["FCCODE"].ToString().Trim();
				try
				{
					intRunCode = Convert.ToInt32(strLastRunCode) + 1;
				}
				catch (Exception ex)
				{
					strErrorMsg = ex.Message.ToString();
					intRunCode++;
				}
			}
			intCodeLen = (strLastRunCode.Length > 0 ? strLastRunCode.Length : inMaxLength);
			this.txtCode.Text = intRunCode.ToString(StringHelper.Replicate("0", intCodeLen));

			return true;
		}

		private bool pmIsValidateCode(object[] inPrefixPara)
		{
			bool bllResult = true;
			string strErrorMsg = "";
			WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
			if (objSQLHelper.SetPara(inPrefixPara)
				&& objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select fcCode from " + this.mstrMasterTable + " where fcCorp = ? and fcCode = ?", ref strErrorMsg))
			{
				bllResult = false;
			}
			return bllResult;
		}

		private bool pmIsValidateName(object[] inPrefixPara)
		{
			bool bllResult = true;
			string strErrorMsg = "";
			WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
			if (objSQLHelper.SetPara(inPrefixPara)
				&& objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select fcCode from " + this.mstrMasterTable + " where fcCorp = ? and fcName = ?", ref strErrorMsg))
			{
				bllResult = false;
			}
			return bllResult;
		}

		private bool pmIsValidateName2(object[] inPrefixPara)
		{
			bool bllResult = true;
			string strErrorMsg = "";
			WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
			if (objSQLHelper.SetPara(inPrefixPara)
				&& objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select fcCode from " + this.mstrMasterTable + " where fcCorp = ? and fcName2 = ?", ref strErrorMsg))
			{
				bllResult = false;
			}
			return bllResult;
		}

		private void pmUpdateRecord()
		{
			string strErrorMsg = "";
			bool bllIsNewRow = false;
			bool bllIsCommit = false;

			WS.Data.Agents.cDBMSAgent objSQLHelper = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

			DataRow dtrSaveInfo = this.dtsDataEnv.Tables[this.mstrRefTable].NewRow();
			if (this.mFormEditMode == UIHelper.AppFormState.Insert
				|| (objSQLHelper.SetPara(new object[] { this.mstrEditRowID })
				&& !objSQLHelper.SQLExec(ref this.dtsDataEnv, "QChkRow", this.mstrRefTable, "select fcSkid from " + this.mstrRefTable + " where fcSkid = ?", ref strErrorMsg)))
			{
				bllIsNewRow = true;
				if (this.mstrEditRowID == string.Empty)
				{
					WS.Data.Agents.cConn objConn = WS.Data.Agents.cConn.GetInStance();
					this.mstrEditRowID = objConn.RunRowID(this.mstrRefTable);
				}
				//dtrSaveInfo["fcCreateBy"] = App.FMAppUserID;
			}

			this.mstrSaveRowID = this.mstrEditRowID;

			string gcTemStr01 = BizRule.SetMemData(this.txtRemark1.Text.Trim(), xdCMRem);
			gcTemStr01 += BizRule.SetMemData(this.txtRemark2.Text.Trim(), xdCMRem2);
			gcTemStr01 += BizRule.SetMemData(this.txtRemark3.Text.Trim(), xdCMRem3);
			gcTemStr01 += BizRule.SetMemData(this.txtRemark4.Text.Trim(), xdCMRem4);
			gcTemStr01 += BizRule.SetMemData(this.txtRemark5.Text.Trim(), xdCMRem5);
			gcTemStr01 += BizRule.SetMemData(this.txtRemark6.Text.Trim(), xdCMRem6);
			gcTemStr01 += BizRule.SetMemData(this.txtRemark7.Text.Trim(), xdCMRem7);
			gcTemStr01 += BizRule.SetMemData(this.txtRemark8.Text.Trim(), xdCMRem8);
			gcTemStr01 += BizRule.SetMemData(this.txtRemark9.Text.Trim(), xdCMRem9);
			gcTemStr01 += BizRule.SetMemData(this.txtRemark10.Text.Trim(), xdCMRem10);

			dtrSaveInfo["ftLastUpd"] = objSQLHelper.GetDBServerDateTime();

			dtrSaveInfo["fcSkid"] = this.mstrEditRowID;
			dtrSaveInfo["fcCorp"] = App.gcCorp;
			dtrSaveInfo["fcType"] = this.txtQcPdType.Text.TrimEnd();
			dtrSaveInfo["fcCode"] = this.txtCode.Text.TrimEnd();
			dtrSaveInfo["fcName"] = this.txtName.Text.TrimEnd();
			dtrSaveInfo["fcSName"] = this.txtSName.Text.TrimEnd();
			dtrSaveInfo["fcFChr"] = AppUtil.StringHelper.GetFChr(this.txtName.Text.TrimEnd());
			dtrSaveInfo["fcName2"] = this.txtName2.Text.TrimEnd();
			dtrSaveInfo["fcSName2"] = this.txtSName2.Text.TrimEnd();
			dtrSaveInfo["fcPdGrp"] = this.txtQcPdGrp.Tag.ToString();
			dtrSaveInfo["fcSupp"] = this.txtQcSuppl.Tag.ToString();
			dtrSaveInfo["fcVatIsOut"] = (this.cmbVatIsOut.SelectedIndex == 0 ? "Y" : "N");
			dtrSaveInfo["fnIsConsum"] = (this.cmbIsConsume.SelectedIndex == 0 ? 1 : 0);
			dtrSaveInfo["fcDiscStr"] = this.txtDiscStr.Text.TrimEnd();

            dtrSaveInfo["fnAgeLong"] = Convert.ToDecimal(this.txtAgeLong.Value);

			dtrSaveInfo["fcStatus"] = (this.cmbActive.SelectedIndex == 0 ? " " : "I");
			if (this.cmbActive.SelectedIndex == 0)
			{
				dtrSaveInfo["fdInactive"] = Convert.DBNull;
			}
			else
			{
				dtrSaveInfo["fdInactive"] = this.txtDInActive.DateTime.Date;
			}

			//dtrSaveInfo["fcPdClass"] = this.txtQcPdClass.Tag.ToString();
			//dtrSaveInfo["fcPdCateg"] = this.txtQcPdCateg.Tag.ToString();
			//dtrSaveInfo["fcPdContent"] = this.txtQcPdContent.Tag.ToString();

			string strCtrlStoc = "0";
			switch (this.cmbCtrlStock.SelectedIndex)
			{
				case 0:
					strCtrlStoc = "0";
					break;
				case 1:
					strCtrlStoc = "1";
					break;
				case 2:
					strCtrlStoc = "2";
					break;
				case 3:
					strCtrlStoc = "3";
					break;
			}
			dtrSaveInfo["fcCtrlStoc"] = strCtrlStoc;
			dtrSaveInfo["fnStdCost"] = Convert.ToDecimal(this.txtStdCost.Value);
			dtrSaveInfo["fnPrice"] = Convert.ToDecimal(this.txtPrice1.Value);

			dtrSaveInfo["fcUM"] = this.txtQcUM.Tag.ToString();
			dtrSaveInfo["fcUM1"] = this.txtQcUM1.Tag.ToString();
			dtrSaveInfo["fcUM2"] = this.txtQcUM2.Tag.ToString();
			dtrSaveInfo["fcStUm"] = this.txtQcStUm.Tag.ToString();
			dtrSaveInfo["fcStUm1"] = this.txtQcStUm1.Tag.ToString();
			dtrSaveInfo["fcStUm2"] = this.txtQcStUm2.Tag.ToString();

			if (this.txtQcUM1.Tag.ToString() != this.txtQcUM.Tag.ToString())
			{
				dtrSaveInfo["fnUmQty1"] = Convert.ToDecimal(this.txtUm1Qty.Value);
			}
			else
			{
				dtrSaveInfo["fnUmQty1"] = 1;
			}

			if (this.txtQcUM2.Tag.ToString() != this.txtQcUM.Tag.ToString())
			{
				dtrSaveInfo["fnUmQty2"] = Convert.ToDecimal(this.txtUm2Qty.Value);
			}
			else
			{
				dtrSaveInfo["fnUmQty2"] = 1;
			}

			if (this.txtQcStUm1.Tag.ToString() != this.txtQcStUm.Tag.ToString())
			{
				dtrSaveInfo["fnStUmQty1"] = Convert.ToDecimal(this.txtStUm1Qty.Value);
			}
			else
			{
				dtrSaveInfo["fnStUmQty1"] = 1;
			}

			if (this.txtQcStUm2.Tag.ToString() != this.txtQcStUm.Tag.ToString())
			{
				dtrSaveInfo["fnStUmQty2"] = Convert.ToDecimal(this.txtStUm2Qty.Value);
			}
			else
			{
				dtrSaveInfo["fnStUmQty2"] = 1;
			}

			dtrSaveInfo["fcAccBCash"] = this.txtQcAccBCash.Tag.ToString();
			dtrSaveInfo["fcAccBCred"] = this.txtQcAccBCred.Tag.ToString();
			dtrSaveInfo["fcAccSCash"] = this.txtQcAccSCash.Tag.ToString();
			dtrSaveInfo["fcAccSCred"] = this.txtQcAccSCred.Tag.ToString();
			//dtrSaveInfo["fcAccSCost"] = this.txtQcAccSCost.Tag.ToString();
			//dtrSaveInfo["fcAccItem"] = this.txtQcAccItem.Tag.ToString();            

			dtrSaveInfo["fmPicName"] = this.lblImgFileName.Text;


			string gcTemStr02, gcTemStr03, gcTemStr04, gcTemStr05, gcTemStr06;
			int intVarCharLen = 500;
			gcTemStr02 = (gcTemStr01.Length > 0 ? StringHelper.SubStr(gcTemStr01, 1, intVarCharLen) : DBEnum.NullString);
			gcTemStr03 = (gcTemStr01.Length > intVarCharLen ? StringHelper.SubStr(gcTemStr01, intVarCharLen + 1, intVarCharLen) : DBEnum.NullString);
			gcTemStr04 = (gcTemStr01.Length > intVarCharLen * 2 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 2 + 1, intVarCharLen) : DBEnum.NullString);
			gcTemStr05 = (gcTemStr01.Length > intVarCharLen * 3 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 3 + 1, intVarCharLen) : DBEnum.NullString);
			gcTemStr06 = (gcTemStr01.Length > intVarCharLen * 4 ? StringHelper.SubStr(gcTemStr01, intVarCharLen * 4 + 1, intVarCharLen) : DBEnum.NullString);

			//dtrSaveInfo["fcMemData"] = gcTemStr02;
			//dtrSaveInfo["fcMemData2"] = gcTemStr03;
			//dtrSaveInfo["fcMemData3"] = gcTemStr04;
			//dtrSaveInfo["fcMemData4"] = gcTemStr05;
			//dtrSaveInfo["fcMemData5"] = gcTemStr06;

			//dtrSaveInfo["fcLastUpdBy"] = App.FMAppUserID;
			//dtrSaveInfo["fdLastUpd"] = objSQLHelper.GetDBServerDateTime(); ;

			this.mSaveDBAgent = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
			this.mSaveDBAgent.AppID = App.AppID;
			this.mdbConn = this.mSaveDBAgent.GetDBConnection();

			//this.mSaveDBAgent2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
			//this.mSaveDBAgent2.AppID = App.AppID;
			//this.mdbConn2 = this.mSaveDBAgent2.GetDBConnection();

			try
			{

				this.mdbConn.Open();
				this.mdbTran = this.mdbConn.BeginTransaction(IsolationLevel.ReadUncommitted);

				//this.mdbConn2.Open();
				//this.mdbTran2 = this.mdbConn2.BeginTransaction(IsolationLevel.ReadUncommitted);

				string strSQLUpdateStr = "";
				object[] pAPara = null;
				cDBMSAgent.GenUpdateSQLString(dtrSaveInfo, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);

				this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

				this.mstrMemoS1 = BizRule.SetMemData(this.txtRemark1.Text.Trim(), xdCMRem) + BizRule.SetMemData(this.txtRemark2.Text.Trim(), xdCMRem2);
				this.mstrMemoS2 = BizRule.SetMemData(this.txtRemark3.Text.Trim(), xdCMRem3) + BizRule.SetMemData(this.txtRemark4.Text.Trim(), xdCMRem4);
				this.mstrMemoS3 = BizRule.SetMemData(this.txtRemark5.Text.Trim(), xdCMRem5) + BizRule.SetMemData(this.txtRemark6.Text.Trim(), xdCMRem6);
				this.mstrMemoS4 = BizRule.SetMemData(this.txtRemark7.Text.Trim(), xdCMRem7) + BizRule.SetMemData(this.txtRemark8.Text.Trim(), xdCMRem8);
				this.mstrMemoS5 = BizRule.SetMemData(this.txtRemark9.Text.Trim(), xdCMRem9) + BizRule.SetMemData(this.txtRemark10.Text.Trim(), xdCMRem10);

				this.pmSaveProdX4(this.mstrEditRowID);

				//this.pmUpdatePdVersion(ref strErrorMsg);

				this.mdbTran.Commit();
				bllIsCommit = true;

				WS.Data.Agents.cDBMSAgent objSQLHelper2 = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);

				if (this.mFormEditMode == UIHelper.AppFormState.Insert)
				{
					KeepLogAgent.KeepLog(objSQLHelper2, KeepLogType.Insert, TASKNAME, this.txtCode.Text, this.txtName.Text, App.FMAppUserID, App.AppUserName);
				}
				else if (this.mFormEditMode == UIHelper.AppFormState.Edit)
				{
					if (this.mstrOldCode == this.txtCode.Text && this.mstrOldName == this.txtName.Text)
					{
						KeepLogAgent.KeepLog(objSQLHelper2, KeepLogType.Update, TASKNAME, this.txtCode.Text, this.txtName.Text, App.FMAppUserID, App.AppUserName);
					}
					else 
					{
						KeepLogAgent.KeepLogChgValue(objSQLHelper2, KeepLogType.Update, TASKNAME, this.txtCode.Text, this.txtName.Text, App.FMAppUserID, App.AppUserName, this.mstrOldCode, this.mstrOldName);
					}
				}

			}
			catch (Exception ex)
			{
				if (!bllIsCommit)
				{
					this.mdbTran.Rollback();
				}
				App.WriteEventsLog(ex);
#if xd_RUNMODE_DEBUG
				MessageBox.Show("Message : " + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
#endif
			}

			finally
			{
				this.mdbConn.Close();
			}

		}

		private bool pmSaveProdX4(string inProd)
		{
			string strErrorMsg = "";
			bool bllIsNewRow = false;
			string strRowID = "";
			object[] pAPara = null;

			DataRow dtrRProdX1 = null;
			if (this.mstrMemoS1.Trim() + this.mstrMemoS2.Trim() + this.mstrMemoS3.Trim() + this.mstrMemoS4.Trim() + this.mstrMemoS5.Trim() != string.Empty)
			{
				if (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "PRODX4", "PRODX4", "select * from PRODX4 where fcProd = ?", new object[1] { inProd }, ref strErrorMsg, this.mdbConn, this.mdbTran))
				{

					bllIsNewRow = true;

					strRowID = App.mRunRowID("PRODX4");
					dtrRProdX1 = this.dtsDataEnv.Tables["PRODX4"].NewRow();
					dtrRProdX1["fcSkid"] = strRowID;
					//dtrRProdX1["cCreateBy"] = App.FMAppUserID;
				}
				else
				{
					bllIsNewRow = false;
					strRowID = this.dtsDataEnv.Tables["PRODX4"].Rows[0]["fcSkid"].ToString();
					dtrRProdX1 = this.dtsDataEnv.Tables["PRODX4"].Rows[0];
				}

				dtrRProdX1["fcCorp"] = App.gcCorp;
				dtrRProdX1["fcProd"] = inProd;
				dtrRProdX1["fmMemData"] = this.mstrMemoS1;
				dtrRProdX1["fmMemData2"] = this.mstrMemoS2;
				dtrRProdX1["fmMemData3"] = this.mstrMemoS3;
				dtrRProdX1["fmMemData4"] = this.mstrMemoS4;
				dtrRProdX1["fmMemData5"] = this.mstrMemoS5;
				//dtrRProdX1["cLastUpdBy"] = App.FMAppUserID;
				//dtrRProdX1["dLastUpd"] = this.mSaveDBAgent.GetDBServerDateTime();

				string strSQLUpdateStr = "";
				cDBMSAgent.GenUpdateSQLString(dtrRProdX1, "FCSKID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
				this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
			}
			else
			{

				pAPara = new object[] { inProd };
				this.mSaveDBAgent.BatchSQLExec("delete from PRODX4 where FCPROD = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

			}
			return true;

		}
		
		private bool pmUpdateProdX1(ref string ioErrorMsg)
		{

			string strErrorMsg = "";
			bool bllIsNewRow = false;
			string strRowID = "";
			object[] pAPara = null;

			DataRow dtrRProdX1 = null;
			if (this.mstrMemoS1.Trim() + this.mstrMemoS2.Trim() + this.mstrMemoS3.Trim() + this.mstrMemoS4.Trim() + this.mstrMemoS5.Trim() != string.Empty)
			{
				if (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, MapTable.Table.MasterProdX1, MapTable.Table.MasterProdX1, "select * from " + MapTable.Table.MasterProdX1 + " where cProd = ?", new object[1] { this.mstrSaveRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran))
				{

					bllIsNewRow = true;

					strRowID = App.mRunRowID(MapTable.Table.MasterProdX1);
					dtrRProdX1 = this.dtsDataEnv.Tables[MapTable.Table.MasterProdX1].NewRow();
					dtrRProdX1["cRowID"] = strRowID;
					dtrRProdX1["cCreateBy"] = App.FMAppUserID;
				}
				else
				{
					bllIsNewRow = false;
					strRowID = this.dtsDataEnv.Tables[MapTable.Table.MasterProdX1].Rows[0]["cRowID"].ToString();
					dtrRProdX1 = this.dtsDataEnv.Tables[MapTable.Table.MasterProdX1].Rows[0];
				}

				dtrRProdX1["cCorp"] = App.gcCorp;
				dtrRProdX1["cProd"] = this.mstrSaveRowID;
				dtrRProdX1["cMemData"] = this.mstrMemoS1;
				dtrRProdX1["cMemData2"] = this.mstrMemoS2;
				dtrRProdX1["cMemData3"] = this.mstrMemoS3;
				dtrRProdX1["cMemData4"] = this.mstrMemoS4;
				dtrRProdX1["cMemData5"] = this.mstrMemoS5;
				dtrRProdX1["cLastUpdBy"] = App.FMAppUserID;
				dtrRProdX1["dLastUpd"] = this.mSaveDBAgent.GetDBServerDateTime();

				string strSQLUpdateStr = "";
				cDBMSAgent.GenUpdateSQLString(dtrRProdX1, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
				this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
			}
			else
			{

				pAPara = new object[] { this.mstrSaveRowID };
				this.mSaveDBAgent.BatchSQLExec("delete from "+MapTable.Table.MasterProdX1+" where CPROD = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);

			}
			return true;
		}

		private bool pmUpdatePdVersion(ref string ioErrorMsg)
		{
			bool bllResult = false;
			string strRowID = "";
			DataRow dtrRPdVersion = null;
			object[] pAPara = null;
			string strErrorMsg = "";

			foreach (DataRow dtrTemPdVer in this.dtsDataEnv.Tables[this.mstrTemPdVer].Rows)
			{
				bool bllIsNewRow = false;
				if (dtrTemPdVer["cVersion"].ToString().TrimEnd() != string.Empty)
				{
					if ((dtrTemPdVer["cRowID"].ToString().TrimEnd() == string.Empty)
						& (!this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "RPdVersion", "PdVersion", "select * from PdVersion where cRowID = ?", new object[1] { dtrTemPdVer["cRowID"].ToString() }, ref strErrorMsg, this.mdbConn, this.mdbTran)))
					{
						strRowID = App.mRunRowID("PdVersion");
						bllIsNewRow = true;
					}
					else
					{
						bllIsNewRow = false;
						strRowID = dtrTemPdVer["cRowID"].ToString();
						dtrRPdVersion = this.dtsDataEnv.Tables["RPdVersion"].Rows[0];
					}

					dtrTemPdVer["cRowID"] = strRowID;

					DataRow dtrPdVersion = null;

					this.pmReplRecordPdVervsion(bllIsNewRow, dtrTemPdVer, ref dtrPdVersion, strRowID);

					string strSQLUpdateStr = "";
					cDBMSAgent.GenUpdateSQLString(dtrPdVersion, "CROWID", bllIsNewRow, ref strSQLUpdateStr, ref pAPara);
					this.mSaveDBAgent.BatchSQLExec(strSQLUpdateStr, pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
				}
				else
				{
					//Delete PdVersion
					if (dtrTemPdVer["cRowID"].ToString().TrimEnd() != string.Empty)
					{
						pAPara = new object[] { dtrTemPdVer["cRowID"].ToString() };
						this.mSaveDBAgent.BatchSQLExec("delete from PdVersion where CROWID = ?", pAPara, ref strErrorMsg, this.mdbConn, this.mdbTran);
					}
				}
			}
			return bllResult;
		}

		private bool pmReplRecordPdVervsion(bool inState, DataRow inSource, ref DataRow ioReplRow, string inRowID)
		{
			bool bllIsNewRec = inState;
			string strErrorMsg = "";
			DataRow dtrPdVersion;
			if (bllIsNewRec)
			{
				this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "PDVERSION", "PDVERSION", "select * from PdVersion where 0=1", null, ref strErrorMsg, this.mdbConn, this.mdbTran);
				dtrPdVersion = this.dtsDataEnv.Tables["PDVERSION"].NewRow();
				dtrPdVersion["cRowID"] = inRowID;
				dtrPdVersion["cCreateBy"] = App.FMAppUserID;
			}
			else
			{
				this.mSaveDBAgent.BatchSQLExec(ref this.dtsDataEnv, "PDVERSION", "PDVERSION", "select * from PdVersion where cRowID = ?", new object[1] { inRowID }, ref strErrorMsg, this.mdbConn, this.mdbTran);
				dtrPdVersion = this.dtsDataEnv.Tables["PDVERSION"].Rows[0];
			}

			dtrPdVersion["cCorp"] = App.gcCorp;
			dtrPdVersion["cProd"] = this.mstrSaveRowID;
			dtrPdVersion["cVersion"] = inSource["cVersion"].ToString().TrimEnd();
			dtrPdVersion["dDate"] = (Convert.IsDBNull(inSource["dDate"]) ? Convert.DBNull : Convert.ToDateTime(inSource["dDate"]).Date);
			dtrPdVersion["nQty"] = Convert.ToDecimal(inSource["nQty"]);
			dtrPdVersion["cUm"] = inSource["cUm"].ToString();
			//dtrPdVersion["cSeq"] = StringHelper.ConvertToBase64(Convert.ToInt32(inSource["nRecNo"]), 2);
			dtrPdVersion["cLastUpdBy"] = App.FMAppUserID;
			dtrPdVersion["dLastUpd"] = this.mSaveDBAgent.GetDBServerDateTime();

			ioReplRow = dtrPdVersion;
			return true;
		}

		private void pmLoadEditPage()
		{
			this.pmSetPageStatus(true);
			this.pgfMainEdit.TabPages[0].PageEnabled = false;
			this.pgfMainEdit.SelectedTabPageIndex = 1;
			this.pmBlankFormData();
			this.txtCode.Focus();
			if (this.mFormEditMode == UIHelper.AppFormState.Edit)
			{
				this.pmLoadFormData();
			}

		}

		private void pmBlankFormData()
		{

			string strErrorMsg = "";
			//WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
			WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

			pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where 0=1", ref strErrorMsg);

			this.mbllAddNew = true;

			this.mstrEditRowID = "";
			this.mstrSaveRowID = "";
			this.mstrMasterRowID = "";

			this.mstrMemoS1 = "";
			this.mstrMemoS2 = "";
			this.mstrMemoS3 = "";
			this.mstrMemoS4 = "";
			this.mstrMemoS5 = "";

			//Page Edit 1
			this.txtCode.Text = "";
			this.txtName.Text = "";
			this.txtSName.Text = "";
			this.txtName2.Text = "";
			this.txtSName2.Text = "";
			this.txtStdCost.Value = 0;
            this.txtAgeLong.Value = 0;

			this.txtQcPdType.Tag = ""; this.txtQcPdType.Text = ""; this.txtQnPdType.Text = "";
			this.txtQcPdGrp.Tag = ""; this.txtQcPdGrp.Text = ""; this.txtQnPdGrp.Text = "";
			//Default Product Group
			//if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdGrp", MapTable.Table.MasterPdGrp, "select * from " + MapTable.Table.MasterPdGrp + " order by cCode", ref strErrorMsg))
			pobjSQLUtil.SetPara(new object[] { App.gcCorp });
			if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdGrp", MapTable.Table.MasterPdGrp, "select * from " + MapTable.Table.ProdGroup + " where fcCorp = ? and order by fcCode", ref strErrorMsg))
			{
				this.txtQcPdGrp.Tag = this.dtsDataEnv.Tables["QPdGrp"].Rows[0]["fcSkid"].ToString();
				this.txtQcPdGrp.Text = this.dtsDataEnv.Tables["QPdGrp"].Rows[0]["fcCode"].ToString().TrimEnd();
				this.txtQnPdGrp.Text = this.dtsDataEnv.Tables["QPdGrp"].Rows[0]["fcName"].ToString().TrimEnd();
			}

			this.cmbActive.SelectedIndex = 0;
			this.txtDInActive.EditValue = null;

			this.txtQcPdClass.Tag = "";
			this.txtQcPdClass.Text = "";
			this.txtQnPdClass.Text = "";

			this.txtQcPdCateg.Tag = "";
			this.txtQcPdCateg.Text = "";
			this.txtQnPdCateg.Text = "";

			this.txtQcPdContent.Tag = "";
			this.txtQcPdContent.Text = "";
			this.txtQnPdContent.Text = "";

			this.txtDiscStr.Text = "";

			this.cmbVatIsOut.SelectedIndex = 0;
			this.cmbCtrlStock.SelectedIndex = 0;
			this.cmbIsConsume.SelectedIndex = 0;

			this.txtQcSuppl.Tag = ""; this.txtQcSuppl.Text = ""; this.txtQnSuppl.Text = "";

			//Page Edit 2
			this.txtQcUM.Enabled = true;
			this.txtQcStUm.Enabled = true;
			this.txtQnUM.Enabled = true;
			this.txtQnStUm.Enabled = true;

			this.txtQcUM.Tag = "";  this.txtQcUM.Text = ""; this.txtQnUM.Text = "";
			this.txtQcUM1.Tag = ""; this.txtQcUM1.Text = ""; this.txtQnUM1.Text = "";
			this.txtQcUM2.Tag = ""; this.txtQcUM2.Text = ""; this.txtQnUM2.Text = "";
			this.txtQcStUm.Tag = ""; this.txtQcStUm.Text = ""; this.txtQnStUm.Text = "";
			this.txtQcStUm1.Tag = ""; this.txtQcStUm1.Text = ""; this.txtQnStUm1.Text = "";
			this.txtQcStUm2.Tag = ""; this.txtQcStUm2.Text = ""; this.txtQnStUm2.Text = "";

			this.txtUm1Qty.Value = 1; this.txtUm2Qty.Value = 1;
			this.txtStUm1Qty.Value = 1; this.txtStUm2Qty.Value = 1;

			this.txtQcAccBCash.Tag = ""; this.txtQcAccBCash.Text = ""; this.txtQnAccBCash.Text = "";
			this.txtQcAccBCred.Tag = ""; this.txtQcAccBCred.Text = ""; this.txtQnAccBCred.Text = "";
			this.txtQcAccSCash.Tag = ""; this.txtQcAccSCash.Text = ""; this.txtQnAccSCash.Text = "";
			this.txtQcAccSCred.Tag = ""; this.txtQcAccSCred.Text = ""; this.txtQnAccSCred.Text = "";
			//this.txtQcAccSCost.Tag = ""; this.txtQcAccSCost.Text = ""; this.txtQnAccSCost.Text = "";
			//this.txtQcAccItem.Tag = ""; this.txtQcAccItem.Text = ""; this.txtQnAccItem.Text = "";

			//Page Edit 3
			this.txtQnPrice1.Tag = ""; this.txtQnPrice1.Text = "";
			this.txtQnPrice2.Tag = ""; this.txtQnPrice2.Text = "";
			this.txtQnPrice3.Tag = ""; this.txtQnPrice3.Text = "";
			this.txtQnPrice4.Tag = ""; this.txtQnPrice4.Text = "";
			this.txtQnPrice5.Tag = ""; this.txtQnPrice5.Text = "";

			this.txtPrice1.Value = 1;
			this.txtPrice2.Value = 1;
			this.txtPrice3.Value = 1;
			this.txtPrice4.Value = 1;
			this.txtPrice5.Value = 1;

			this.txtMinPrice.Value = 0;

			//Page Edit 4
			this.txtQnPriceA1.Tag = ""; this.txtQnPriceA1.Text = "";
			this.txtQnPriceA2.Tag = ""; this.txtQnPriceA2.Text = "";
			this.txtQnPriceA3.Tag = ""; this.txtQnPriceA3.Text = "";
			this.txtQnPriceA4.Tag = ""; this.txtQnPriceA4.Text = "";
			this.txtQnPriceA5.Tag = ""; this.txtQnPriceA5.Text = "";

			this.txtQnPriceB1.Tag = ""; this.txtQnPriceB1.Text = "";
			this.txtQnPriceB2.Tag = ""; this.txtQnPriceB2.Text = "";
			this.txtQnPriceB3.Tag = ""; this.txtQnPriceB3.Text = "";
			this.txtQnPriceB4.Tag = ""; this.txtQnPriceB4.Text = "";
			this.txtQnPriceB5.Tag = ""; this.txtQnPriceB5.Text = "";

			//Page Edit 4
			this.txtQnPriceC1.Tag = ""; this.txtQnPriceC1.Text = "";
			this.txtQnPriceC2.Tag = ""; this.txtQnPriceC2.Text = "";
			this.txtQnPriceC3.Tag = ""; this.txtQnPriceC3.Text = "";
			this.txtQnPriceC4.Tag = ""; this.txtQnPriceC4.Text = "";
			this.txtQnPriceC5.Tag = ""; this.txtQnPriceC5.Text = "";

			this.txtQnPriceD1.Tag = ""; this.txtQnPriceD1.Text = "";
			this.txtQnPriceD2.Tag = ""; this.txtQnPriceD2.Text = "";
			this.txtQnPriceD3.Tag = ""; this.txtQnPriceD3.Text = "";
			this.txtQnPriceD4.Tag = ""; this.txtQnPriceD4.Text = "";
			this.txtQnPriceD5.Tag = ""; this.txtQnPriceD5.Text = "";

			//Page Edit 6
			this.txtRemark1.Text = "";
			this.txtRemark2.Text = "";
			this.txtRemark3.Text = "";
			this.txtRemark4.Text = "";
			this.txtRemark5.Text = "";
			this.txtRemark6.Text = "";
			this.txtRemark7.Text = "";
			this.txtRemark8.Text = "";
			this.txtRemark9.Text = "";
			this.txtRemark10.Text = "";

			//Page Edit 7
			this.lblImgFileName.Text = "";
			this.pcbPdImage.Image = null;

			this.txtCAuthor.Text = "";
			this.dtsDataEnv.Tables[this.mstrTemPdVer].Rows.Clear();

			this.pmLoadOldVar();

		}

		private void pmLoadFormData()
		{
			DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
			if (dtrBrow != null)
			{

				this.mstrMasterRowID = dtrBrow["cRowID"].ToString();
				this.mstrEditRowID = dtrBrow["cRowID"].ToString();
				string strErrorMsg = "";
				WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
				WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

				pobjSQLUtil2.SetPara(new object[] { this.mstrMasterRowID });
				if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, this.mstrMasterTable, this.mstrMasterTable, "select * from " + this.mstrMasterTable + " where fcSkid = ?", ref strErrorMsg))
				{

					DataRow dtrLoadForm = this.dtsDataEnv.Tables[this.mstrMasterTable].Rows[0];

					//DataRow dtrLoadForm2 = null;
					//pobjSQLUtil.SetPara(new object[] { App.gcCorp, dtrLoadForm["fcCode"].ToString().TrimEnd() });
					//if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, this.mstrRefTable, this.mstrRefTable, "select * from " + this.mstrRefTable + " where cCorp = ? and cCode = ? ", ref strErrorMsg))
					//{
					//    this.mbllAddNew = false;
					//    dtrLoadForm2 = this.dtsDataEnv.Tables[this.mstrRefTable].Rows[0];
					//    this.mstrEditRowID = dtrLoadForm2["cRowID"].ToString();
					//}
					//else
					//{
					//    this.mbllAddNew = true;
					//    dtrLoadForm2 = this.dtsDataEnv.Tables[this.mstrRefTable].NewRow();
					//    this.dtsDataEnv.Tables[this.mstrRefTable].Rows.Add(dtrLoadForm2);
					//}

					this.txtCode.Text = dtrLoadForm["fcCode"].ToString().TrimEnd();
					this.txtName.Text = dtrLoadForm["fcName"].ToString().TrimEnd();
					this.txtSName.Text = dtrLoadForm["fcSName"].ToString().TrimEnd();
					this.txtName2.Text = dtrLoadForm["fcName2"].ToString().TrimEnd();
					this.txtSName2.Text = dtrLoadForm["fcSName2"].ToString().TrimEnd();

					this.cmbVatIsOut.SelectedIndex = (dtrLoadForm["fcVatIsOut"].ToString() == "Y" ? 0 : 1);
					this.cmbIsConsume.SelectedIndex = (Convert.ToInt32(dtrLoadForm["fnIsConsum"]) == 0 ? 1 : 0);
					this.txtStdCost.Value = Convert.ToDecimal(dtrLoadForm["fnStdCost"]);
					this.txtDiscStr.Text = dtrLoadForm["fcDiscStr"].ToString().Trim();
					this.txtPrice1.Value = Convert.ToDecimal(dtrLoadForm["fnPrice"]);

                    this.txtAgeLong.Value = Convert.ToDecimal(dtrLoadForm["fnAgeLong"]);

					this.cmbActive.SelectedIndex = (dtrLoadForm["fcStatus"].ToString() == "I" ? 1 : 0);
					if (!Convert.IsDBNull(dtrLoadForm["fdInActive"]))
					{
						this.txtDInActive.DateTime = Convert.ToDateTime(dtrLoadForm["fdInActive"]);
					}

					int intCtrlStoc = 0;
					string strCtrlStoc = dtrLoadForm["fcCtrlStoc"].ToString();
					switch (strCtrlStoc)
					{
						case "0":
							intCtrlStoc = 0;
							break;
						case "1":
							intCtrlStoc = 1;
							break;
						case "2":
							intCtrlStoc = 2;
							break;
						case "3":
							intCtrlStoc = 3;
							break;
					}
					this.cmbCtrlStock.SelectedIndex = intCtrlStoc;

					this.txtQcPdType.Tag = dtrLoadForm["fcType"].ToString();
					pobjSQLUtil2.SetPara(new object[] { this.txtQcPdType.Tag.ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPdType", "PDTYPE", "select * from PRODTYPE where FCCODE = ?", ref strErrorMsg))
					{
						DataRow dtrPdType = this.dtsDataEnv.Tables["QPdType"].Rows[0];
						this.txtQcPdType.Text = dtrPdType["fcCode"].ToString().TrimEnd();
						this.txtQnPdType.Text = dtrPdType["fcName"].ToString().TrimEnd();
					}

					this.txtQcPdGrp.Tag = dtrLoadForm["fcPdGrp"].ToString();
					pobjSQLUtil2.SetPara(new object[] { this.txtQcPdGrp.Tag.ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QPdGrp", this.mstrMasterTable, "select * from " + MapTable.Table.ProdGroup + " where fcSkid = ?", ref strErrorMsg))
					{
						DataRow dtrPdGrp = this.dtsDataEnv.Tables["QPdGrp"].Rows[0];
						this.txtQcPdGrp.Text = dtrPdGrp["fcCode"].ToString().TrimEnd();
						this.txtQnPdGrp.Text = dtrPdGrp["fcName"].ToString().TrimEnd();
					}

					//if (!this.mbllAddNew)
					//{

					//    this.txtQcPdClass.Tag = dtrLoadForm2["cPdClass"].ToString();
					//    pobjSQLUtil.SetPara(new object[] { this.txtQcPdClass.Tag.ToString() });
					//    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdClass", this.mstrMasterTable, "select * from " + MapTable.Table.PdClass + " where cRowID = ?", ref strErrorMsg))
					//    {
					//        DataRow dtrVal01 = this.dtsDataEnv.Tables["QPdClass"].Rows[0];
					//        this.txtQcPdClass.Text = dtrVal01["cCode"].ToString().TrimEnd();
					//        this.txtQnPdClass.Text = dtrVal01["cName"].ToString().TrimEnd();
					//    }

					//    this.txtQcPdCateg.Tag = dtrLoadForm2["cPdCateg"].ToString();
					//    pobjSQLUtil.SetPara(new object[] { this.txtQcPdCateg.Tag.ToString() });
					//    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdCateg", this.mstrMasterTable, "select * from " + MapTable.Table.PdCateg + " where cRowID = ?", ref strErrorMsg))
					//    {
					//        DataRow dtrVal01 = this.dtsDataEnv.Tables["QPdCateg"].Rows[0];
					//        this.txtQcPdCateg.Text = dtrVal01["cCode"].ToString().TrimEnd();
					//        this.txtQnPdCateg.Text = dtrVal01["cName"].ToString().TrimEnd();
					//    }

					//    this.txtQcPdContent.Tag = dtrLoadForm2["cPdContent"].ToString();
					//    pobjSQLUtil.SetPara(new object[] { this.txtQcPdContent.Tag.ToString() });
					//    if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdContent", this.mstrMasterTable, "select * from " + MapTable.Table.PdContent + " where cRowID = ?", ref strErrorMsg))
					//    {
					//        DataRow dtrVal01 = this.dtsDataEnv.Tables["QPdContent"].Rows[0];
					//        this.txtQcPdContent.Text = dtrVal01["cCode"].ToString().TrimEnd();
					//        this.txtQnPdContent.Text = dtrVal01["cName"].ToString().TrimEnd();
					//    }

					//    this.txtCAuthor.Text = dtrLoadForm2["cAuthor"].ToString().TrimEnd();

					//}

					this.txtQcSuppl.Tag = dtrLoadForm["fcSupp"].ToString();
					pobjSQLUtil2.SetPara(new object[] { this.txtQcSuppl.Tag.ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QSuppl", MapTable.Table.Coor, "select * from COOR where fcSkid = ?", ref strErrorMsg))
					{
						DataRow dtrSuppl = this.dtsDataEnv.Tables["QSuppl"].Rows[0];
						this.txtQcSuppl.Text = dtrSuppl["fcCode"].ToString().TrimEnd();
						this.txtQnSuppl.Text = dtrSuppl["fcName"].ToString().TrimEnd();
					}

					this.lblImgFileName.Text = dtrLoadForm["fmPicName"].ToString();
					if (this.lblImgFileName.Text.TrimEnd() != string.Empty && this.lblImgFileName.Text != "...")
					{
						if (System.IO.File.Exists(this.lblImgFileName.Text))
						{
							this.pcbPdImage.Image = System.Drawing.Image.FromFile(this.lblImgFileName.Text);
						}
						else
						{
							MessageBox.Show(this, "ไม่พบไฟลฺ์รูปภาพ " + this.lblImgFileName.Text + " !", "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
					}


					#region "Load AcChart"
					this.txtQcAccBCash.Tag = dtrLoadForm["fcAccBCash"].ToString();
					pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcAccBCash"].ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QAcChart", this.mstrMasterTable, "select * from AcChart where fcSkid = ?", ref strErrorMsg))
					{
						DataRow dtrAcChart = this.dtsDataEnv.Tables["QAcChart"].Rows[0];
						this.txtQcAccBCash.Text = dtrAcChart["fcCode"].ToString().TrimEnd();
						this.txtQnAccBCash.Text = dtrAcChart["fcName"].ToString().TrimEnd();
					}

					this.txtQcAccBCred.Tag = dtrLoadForm["fcAccBCred"].ToString();
					pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcAccBCred"].ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QAcChart", this.mstrMasterTable, "select * from AcChart where fcSkid = ?", ref strErrorMsg))
					{
						DataRow dtrAcChart = this.dtsDataEnv.Tables["QAcChart"].Rows[0];
						this.txtQcAccBCred.Text = dtrAcChart["fcCode"].ToString().TrimEnd();
						this.txtQnAccBCred.Text = dtrAcChart["fcName"].ToString().TrimEnd();
					}

					this.txtQcAccSCash.Tag = dtrLoadForm["fcAccSCash"].ToString();
					pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcAccSCash"].ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QAcChart", this.mstrMasterTable, "select * from AcChart where fcSkid = ?", ref strErrorMsg))
					{
						DataRow dtrAcChart = this.dtsDataEnv.Tables["QAcChart"].Rows[0];
						this.txtQcAccSCash.Text = dtrAcChart["fcCode"].ToString().TrimEnd();
						this.txtQnAccSCash.Text = dtrAcChart["fcName"].ToString().TrimEnd();
					}

					this.txtQcAccSCred.Tag = dtrLoadForm["fcAccSCred"].ToString();
					pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcAccSCred"].ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QAcChart", this.mstrMasterTable, "select * from AcChart where fcSkid = ?", ref strErrorMsg))
					{
						DataRow dtrAcChart = this.dtsDataEnv.Tables["QAcChart"].Rows[0];
						this.txtQcAccSCred.Text = dtrAcChart["fcCode"].ToString().TrimEnd();
						this.txtQnAccSCred.Text = dtrAcChart["fcName"].ToString().TrimEnd();
					}
					#endregion

					#region "Load UM"

					bool bllHasUsed = this.pmHasUsedChild1Corp(this.mstrEditRowID, ref strErrorMsg);
					if (bllHasUsed)
					{
						//ioErrorMsg = UIBase.GetAppUIText(new string[] { "ไม่สามารถแก้ไขหน่วยนับมาตรฐานได้เนื่องจากมีการอ้างอิงถึงใน ", "Can not delete because has refer to " }) + ioErrorMsg;
						this.txtQcUM.Enabled = false;
						this.txtQcStUm.Enabled = false;
						this.txtQnUM.Enabled = false;
						this.txtQnStUm.Enabled = false;
					}

					this.txtUm1Qty.Value = Convert.ToDecimal(dtrLoadForm["fnUmQty1"]);
					this.txtUm2Qty.Value = Convert.ToDecimal(dtrLoadForm["fnUmQty2"]);
					this.txtStUm1Qty.Value = Convert.ToDecimal(dtrLoadForm["fnStUmQty1"]);
					this.txtStUm2Qty.Value = Convert.ToDecimal(dtrLoadForm["fnStUmQty2"]);

					this.txtQcUM.Tag = dtrLoadForm["fcUM"].ToString();
					pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcUM"].ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QUM", this.mstrMasterTable, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", ref strErrorMsg))
					{
						DataRow dtrUM = this.dtsDataEnv.Tables["QUM"].Rows[0];
						this.txtQcUM.Text = dtrUM["fcCode"].ToString().TrimEnd();
						this.txtQnUM.Text = dtrUM["fcName"].ToString().TrimEnd();

						this.pmSetStdQnUM(this.txtQcUM.Tag.ToString(), this.txtQnUM.Text);

					}

					this.txtQcUM1.Tag = dtrLoadForm["fcUM1"].ToString();
					pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcUM1"].ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QUM", this.mstrMasterTable, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", ref strErrorMsg))
					{
						DataRow dtrUM = this.dtsDataEnv.Tables["QUM"].Rows[0];
						this.txtQcUM1.Text = dtrUM["fcCode"].ToString().TrimEnd();
						this.txtQnUM1.Text = dtrUM["fcName"].ToString().TrimEnd();
					}

					this.txtQcUM2.Tag = dtrLoadForm["fcUM2"].ToString();
					pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcUM2"].ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QUM", this.mstrMasterTable, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", ref strErrorMsg))
					{
						DataRow dtrUM = this.dtsDataEnv.Tables["QUM"].Rows[0];
						this.txtQcUM2.Text = dtrUM["fcCode"].ToString().TrimEnd();
						this.txtQnUM2.Text = dtrUM["fcName"].ToString().TrimEnd();
					}

					this.txtQcStUm.Tag = dtrLoadForm["fcStUm"].ToString();
					pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcStUm"].ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QUM", this.mstrMasterTable, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", ref strErrorMsg))
					{
						DataRow dtrUM = this.dtsDataEnv.Tables["QUM"].Rows[0];
						this.txtQcStUm.Text = dtrUM["fcCode"].ToString().TrimEnd();
						this.txtQnStUm.Text = dtrUM["fcName"].ToString().TrimEnd();
					}

					this.txtQcStUm1.Tag = dtrLoadForm["fcStUm1"].ToString();
					pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcStUm1"].ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QUM", this.mstrMasterTable, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", ref strErrorMsg))
					{
						DataRow dtrUM = this.dtsDataEnv.Tables["QUM"].Rows[0];
						this.txtQcStUm1.Text = dtrUM["fcCode"].ToString().TrimEnd();
						this.txtQnStUm1.Text = dtrUM["fcName"].ToString().TrimEnd();
					}

					this.txtQcStUm2.Tag = dtrLoadForm["fcStUm2"].ToString();
					pobjSQLUtil2.SetPara(new object[] { dtrLoadForm["fcStUm2"].ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QUM", this.mstrMasterTable, "select * from " + MapTable.Table.UOM + " where fcSkid = ?", ref strErrorMsg))
					{
						DataRow dtrUM = this.dtsDataEnv.Tables["QUM"].Rows[0];
						this.txtQcStUm2.Text = dtrUM["fcCode"].ToString().TrimEnd();
						this.txtQnStUm2.Text = dtrUM["fcName"].ToString().TrimEnd();
					}

					#endregion

					this.pmLoadRemark();
					//this.pmLoadPdVersion();
				}
				this.pmLoadOldVar();
			}
		}

		private void pmLoadOldVar()
		{
			this.mstrOldCode = this.txtCode.Text;
			this.mstrOldName = this.txtName.Text;
			this.mstrOldName2 = this.txtName2.Text;
		}

		private void pmInsertLoop()
		{
			if (this.mFormActiveMode != FormActiveMode.PopUp)
				this.pmLoadEditPage();
			else
				this.pmGotoBrowPage();
		}

		private void pmLoadRemark()
		{

			string strErrorMsg = "";
			WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

			//pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
			pobjSQLUtil.SetPara(new object[] { this.mstrMasterRowID });
			if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProdX4", "PRODX4", "select * from ProdX4 where fcProd = ?", ref strErrorMsg))
			{
				DataRow dtrProdX4 = this.dtsDataEnv.Tables["QProdX4"].Rows[0];

				string gcTemStr01 = (Convert.IsDBNull(dtrProdX4["fmMemData"]) ? "" : dtrProdX4["fmMemData"].ToString().TrimEnd());
				string gcTemStr02 = (Convert.IsDBNull(dtrProdX4["fmMemData2"]) ? "" : dtrProdX4["fmMemData2"].ToString().TrimEnd());
				string gcTemStr03 = (Convert.IsDBNull(dtrProdX4["fmMemData3"]) ? "" : dtrProdX4["fmMemData3"].ToString().TrimEnd());
				string gcTemStr04 = (Convert.IsDBNull(dtrProdX4["fmMemData4"]) ? "" : dtrProdX4["fmMemData4"].ToString().TrimEnd());
				string gcTemStr05 = (Convert.IsDBNull(dtrProdX4["fmMemData5"]) ? "" : dtrProdX4["fmMemData5"].ToString().TrimEnd());

				this.txtRemark1.Text = BizRule.GetMemData(gcTemStr01, xdCMRem);
				this.txtRemark2.Text = BizRule.GetMemData(gcTemStr01, xdCMRem2);
				this.txtRemark3.Text = BizRule.GetMemData(gcTemStr02, xdCMRem3);
				this.txtRemark4.Text = BizRule.GetMemData(gcTemStr02, xdCMRem4);
				this.txtRemark5.Text = BizRule.GetMemData(gcTemStr03, xdCMRem5);
				this.txtRemark6.Text = BizRule.GetMemData(gcTemStr03, xdCMRem6);
				this.txtRemark7.Text = BizRule.GetMemData(gcTemStr04, xdCMRem7);
				this.txtRemark8.Text = BizRule.GetMemData(gcTemStr04, xdCMRem8);
				this.txtRemark9.Text = BizRule.GetMemData(gcTemStr05, xdCMRem9);
				this.txtRemark10.Text = BizRule.GetMemData(gcTemStr05, xdCMRem10);

				//ให้ Link กับ Remark9
				this.txtCAuthor.Text = this.txtRemark9.Text;

			}

		}

		private void pmLoadPdVersion()
		{
			string strErrorMsg = "";
			WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
			WS.Data.Agents.cDBMSAgent pobjSQLUtil2 = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);

			pobjSQLUtil.SetPara(new object[] { this.mstrEditRowID });
			if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPdVer", "PDVERSION", "select * from PdVersion where cProd = ?", ref strErrorMsg))
			{
				foreach (DataRow dtrPdVer in this.dtsDataEnv.Tables["QPdVer"].Rows)
				{
					DataRow dtrNewRow = this.dtsDataEnv.Tables[this.mstrTemPdVer].NewRow();

					dtrNewRow["cRowID"] = dtrPdVer["cRowID"].ToString();
					dtrNewRow["cVersion"] = dtrPdVer["cVersion"].ToString().TrimEnd();
					dtrNewRow["dDate"] = Convert.ToDateTime(dtrPdVer["dDate"]);
					dtrNewRow["nQty"] = Convert.ToDecimal(dtrPdVer["nQty"]);
					dtrNewRow["cUm"] = dtrPdVer["cUm"].ToString();

					pobjSQLUtil2.SetPara(new object[] { dtrNewRow["cUm"].ToString() });
					if (pobjSQLUtil2.SQLExec(ref this.dtsDataEnv, "QUM", "UM", "select * from UM where fcSkid = ?", ref strErrorMsg))
					{
						dtrNewRow["cQnUm"] = this.dtsDataEnv.Tables["QUM"].Rows[0]["fcName"].ToString().TrimEnd();
					}

					this.dtsDataEnv.Tables[this.mstrTemPdVer].Rows.Add(dtrNewRow);
				}
			}

		}

		private void frmProd_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Alt | e.Control | e.Shift)
				return;

			switch (e.KeyCode)
			{
				case Keys.Enter:
					this.pmEnterForm();
					break;
				case Keys.PageUp:
					if (this.pgfMainEdit.SelectedTabPageIndex > xd_PAGE_BROWSE)
						this.pgfMainEdit.SelectedTabPageIndex = (this.pgfMainEdit.SelectedTabPageIndex - 1 <= xd_PAGE_BROWSE ? this.pgfMainEdit.TabPages.Count - 1 : this.pgfMainEdit.SelectedTabPageIndex - 1);

					//this.pgfMainEdit.SelectedTabPage
					//do while this.pgfMainEdit.TabPages[
					break;
				case Keys.PageDown:
					if (this.pgfMainEdit.SelectedTabPageIndex > xd_PAGE_BROWSE)
						this.pgfMainEdit.SelectedTabPageIndex = (this.pgfMainEdit.SelectedTabPageIndex + 1 > this.pgfMainEdit.TabPages.Count - 1 ? xd_PAGE_EDIT1 : this.pgfMainEdit.SelectedTabPageIndex + 1);
					break;
				case Keys.Escape:
					if (this.pgfMainEdit.SelectedTabPageIndex != xd_PAGE_BROWSE)
					{
						if (MessageBox.Show("ยกเลิกการแก้ไข ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
						{
							this.pmGotoBrowPage();
						}
					}
					else
					{
						this.mbllPopUpResult = false;
						if (this.mFormActiveMode == FormActiveMode.Edit)
						{
							this.Close();
						}
						else
							this.Hide();
					}
					break;
			}
		
		}

		private void gridView1_EndSorting(object sender, EventArgs e)
		{
			this.pmSetSortKey(this.gridView1.SortedColumns[0].FieldName, false);
		}

		private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
		{
			DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
			if (dtrBrow != null)
			{
				//this.txtFooter.Text = "สร้างโดย LOGIN : " + dtrBrow["CLOGIN_ADD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dCreate"]).ToString("dd/MM/yy hh:mm:ss");
				//this.txtFooter.Text += "\r\nแก้ไขล่าสุดโดย LOGIN : " + dtrBrow["CLOGIN_UPD"].ToString().TrimEnd() + " วันที่ : " + Convert.ToDateTime(dtrBrow["dLastUpd"]).ToString("dd/MM/yy hh:mm:ss");
				this.txtFooter.Text = "วันที่สร้าง : " + Convert.ToDateTime(dtrBrow["dCreate"]).ToString("dd/MM/yy hh:mm:ss");
				this.txtFooter.Text += "\r\nแก้ไขล่าสุดวันที่ : " + Convert.ToDateTime(dtrBrow["dLastUpd"]).ToString("dd/MM/yy hh:mm:ss");
			}
			else 
			{
				this.txtFooter.Text = "";
			}
		}

		private void gridView1_DoubleClick(object sender, EventArgs e)
		{
			this.pmEnterForm();
		}

		private void pmInitPopUpDialog(string inDialogName)
		{
			switch (inDialogName.TrimEnd().ToUpper())
			{
				case "ACCHART":
					if (this.pofrmGetAcChart == null)
					{
						this.pofrmGetAcChart = new DialogForms.dlgGetAcChart();
						this.pofrmGetAcChart.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetAcChart.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
					}
					break;
				case "PDTYPE":
					if (this.pofrmGetPdType == null)
					{
						this.pofrmGetPdType = new DialogForms.dlgGetProdType();
						this.pofrmGetPdType.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPdType.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
					}
					break;
				case "PDGRP":
					if (this.pofrmGetPdGrp == null)
					{
						//this.pofrmGetPdGrp = new DatabaseForms.frmPdGrp(FormActiveMode.PopUp);
						this.pofrmGetPdGrp = new DialogForms.dlgGetPdGrp();
						this.pofrmGetPdGrp.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetPdGrp.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
					}
					break;
				case "SUPPL":
					if (this.pofrmGetSuppl == null)
					{
						this.pofrmGetSuppl = new DialogForms.dlgGetCoor(CoorType.Supplier);
						this.pofrmGetSuppl.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetSuppl.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
					}
					break;
				case "UM":
					if (this.pofrmGetUM == null)
					{
						//this.pofrmGetUM = new DatabaseForms.frmUM(FormActiveMode.PopUp);
						this.pofrmGetUM = new DialogForms.dlgGetUM();
						this.pofrmGetUM.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.pofrmGetUM.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
					}
					break;
			}
		}

		private void txtPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
			DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
			this.pmPopUpButtonClick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
		}

		private void txtPopUp_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Alt | e.Control | e.Shift)
				return;

			switch (e.KeyCode)
			{
				case Keys.F3:
					DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
					this.pmPopUpButtonClick(txtPopUp.Name.ToUpper(), txtPopUp.Text);
					break;
			}

		}

		private void pmPopUpButtonClick(string inTextbox, string inPara1)
		{
			string strTxtPrefix = "";
			switch (inTextbox)
			{
				case "TXTQCACCBCASH":
				case "TXTQNACCBCASH":
				case "TXTQCACCBCRED":
				case "TXTQNACCBCRED":
				case "TXTQCACCSCASH":
				case "TXTQNACCSCASH":
				case "TXTQCACCSCRED":
				case "TXTQNACCSCRED":
				case "TXTQCACCSCOST":
				case "TXTQNACCSCOST":
				case "TXTQCACCITEM":
				case "TXTQNACCITEM":
					strTxtPrefix = StringHelper.Left(inTextbox.ToUpper(), 5);
					this.pmInitPopUpDialog("ACCHART");
					this.pofrmGetAcChart.ValidateField(inPara1, (strTxtPrefix == "TXTQC" ? "FCCODE" : "FCNAME"), true);
					if (this.pofrmGetAcChart.PopUpResult)
					{
						this.pmRetrievePopUpVal(inTextbox);
					}
					break;
				case "TXTQCPDTYPE":
				case "TXTQNPDTYPE":
					strTxtPrefix = StringHelper.Left(inTextbox.ToUpper(), 5);
					this.pmInitPopUpDialog("PDTYPE");
					this.pofrmGetPdType.ValidateField(inPara1, (strTxtPrefix == "TXTQC" ? "FCCODE" : "FCNAME"), true);
					if (this.pofrmGetPdType.PopUpResult)
					{
						this.pmRetrievePopUpVal(inTextbox);
					}
					break;
				case "TXTQCPDGRP":
				case "TXTQNPDGRP":
					strTxtPrefix = StringHelper.Left(inTextbox.ToUpper(), 5);
					this.pmInitPopUpDialog("PDGRP");
					//this.pofrmGetPdGrp.ValidateField(inPara1, (strTxtPrefix == "TXTQC" ? "CCODE" : "CNAME"), true);
					this.pofrmGetPdGrp.ValidateField(inPara1, (strTxtPrefix == "TXTQC" ? "FCCODE" : "FCNAME"), true);
					if (this.pofrmGetPdGrp.PopUpResult)
					{
						this.pmRetrievePopUpVal(inTextbox);
					}
					break;
				case "TXTQCSUPPL":
				case "TXTQNSUPPL":
					strTxtPrefix = StringHelper.Left(inTextbox.ToUpper(), 5);
					this.pmInitPopUpDialog("SUPPL");
					this.pofrmGetSuppl.ValidateField(inPara1, (strTxtPrefix == "TXTQC" ? "FCCODE" : "FCNAME"), true);
					if (this.pofrmGetSuppl.PopUpResult)
					{
						this.pmRetrievePopUpVal(inTextbox);
					}
					break;
				case "TXTQCUM":
				case "TXTQNUM":
				case "TXTQCUM1":
				case "TXTQNUM1":
				case "TXTQCUM2":
				case "TXTQNUM2":
				case "TXTQCSTUM":
				case "TXTQNSTUM":
				case "TXTQCSTUM1":
				case "TXTQNSTUM1":
				case "TXTQCSTUM2":
				case "TXTQNSTUM2":

				case "TXTQNPRICE1":
				case "TXTQNPRICE2":
				case "TXTQNPRICE3":
				case "TXTQNPRICE4":
				case "TXTQNPRICE5":
				case "TXTQNPRICEA1":
				case "TXTQNPRICEA2":
				case "TXTQNPRICEA3":
				case "TXTQNPRICEA4":
				case "TXTQNPRICEA5":
				case "TXTQNPRICEB1":
				case "TXTQNPRICEB2":
				case "TXTQNPRICEB3":
				case "TXTQNPRICEB4":
				case "TXTQNPRICEB5":
				case "TXTQNPRICEC1":
				case "TXTQNPRICEC2":
				case "TXTQNPRICEC3":
				case "TXTQNPRICEC4":
				case "TXTQNPRICEC5":
				case "TXTQNPRICED1":
				case "TXTQNPRICED2":
				case "TXTQNPRICED3":
				case "TXTQNPRICED4":
				case "TXTQNPRICED5": 
					
					strTxtPrefix = StringHelper.Left(inTextbox.ToUpper(), 5);
					this.pmInitPopUpDialog("UM");
					//this.pofrmGetUM.ValidateField(inPara1, (strTxtPrefix == "TXTQC" ? "CCODE" : "CNAME"), true);
					this.pofrmGetUM.ValidateField(inPara1, (strTxtPrefix == "TXTQC" ? "FCCODE" : "FCNAME"), true);
					if (this.pofrmGetUM.PopUpResult)
					{
						this.pmRetrievePopUpVal(inTextbox);
					}
					break;
			}
		}

		private string pmRetrievePopUpVal(string inPopupForm)
		{
			string strRetValue = "";
			string strErrorMsg = "";
			WS.Data.Agents.cDBMSAgent pobjSQLUtil = null;

			switch (inPopupForm.TrimEnd().ToUpper())
			{
				case "TXTQCACCBCASH":
				case "TXTQNACCBCASH":
					if (this.pofrmGetAcChart != null)
					{
						DataRow dtrAcChart = this.pofrmGetAcChart.RetrieveValue();

						if (dtrAcChart != null)
						{
							this.txtQcAccBCash.Tag = dtrAcChart["fcSkid"].ToString();
							this.txtQcAccBCash.Text = dtrAcChart["fcCode"].ToString().TrimEnd();
							this.txtQnAccBCash.Text = dtrAcChart["fcName"].ToString().TrimEnd();
						}
						else
						{
							this.txtQcAccBCash.Tag = "";
							this.txtQcAccBCash.Text = "";
							this.txtQnAccBCash.Text = "";
						}
					}
					break;

				case "TXTQCACCBCRED":
				case "TXTQNACCBCRED":
					if (this.pofrmGetAcChart != null)
					{
						DataRow dtrAcChart = this.pofrmGetAcChart.RetrieveValue();

						if (dtrAcChart != null)
						{
							this.txtQcAccBCred.Tag = dtrAcChart["fcSkid"].ToString();
							this.txtQcAccBCred.Text = dtrAcChart["fcCode"].ToString().TrimEnd();
							this.txtQnAccBCred.Text = dtrAcChart["fcName"].ToString().TrimEnd();
						}
						else
						{
							this.txtQcAccBCred.Tag = "";
							this.txtQcAccBCred.Text = "";
							this.txtQnAccBCred.Text = "";
						}
					}
					break;

				case "TXTQCACCSCASH":
				case "TXTQNACCSCASH":
					if (this.pofrmGetAcChart != null)
					{
						DataRow dtrAcChart = this.pofrmGetAcChart.RetrieveValue();

						if (dtrAcChart != null)
						{
							this.txtQcAccSCash.Tag = dtrAcChart["fcSkid"].ToString();
							this.txtQcAccSCash.Text = dtrAcChart["fcCode"].ToString().TrimEnd();
							this.txtQnAccSCash.Text = dtrAcChart["fcName"].ToString().TrimEnd();
						}
						else
						{
							this.txtQcAccSCash.Tag = "";
							this.txtQcAccSCash.Text = "";
							this.txtQnAccSCash.Text = "";
						}
					}
					break;

				case "TXTQCACCSCRED":
				case "TXTQNACCSCRED":
					if (this.pofrmGetAcChart != null)
					{
						DataRow dtrAcChart = this.pofrmGetAcChart.RetrieveValue();

						if (dtrAcChart != null)
						{
							this.txtQcAccSCred.Tag = dtrAcChart["fcSkid"].ToString();
							this.txtQcAccSCred.Text = dtrAcChart["fcCode"].ToString().TrimEnd();
							this.txtQnAccSCred.Text = dtrAcChart["fcName"].ToString().TrimEnd();
						}
						else
						{
							this.txtQcAccSCred.Tag = "";
							this.txtQcAccSCred.Text = "";
							this.txtQnAccSCred.Text = "";
						}
					}
					break;

				case "TXTQCPDTYPE":
				case "TXTQNPDTYPE":
					if (this.pofrmGetPdType != null)
					{
						DataRow dtrPdType = this.pofrmGetPdType.RetrieveValue();

						if (dtrPdType != null)
						{
							this.txtQcPdType.Tag = dtrPdType["fcCode"].ToString();
							this.txtQcPdType.Text = dtrPdType["fcCode"].ToString().TrimEnd();
							this.txtQnPdType.Text = dtrPdType["fcName"].ToString().TrimEnd();
						}
						else
						{
							this.txtQcPdType.Tag = "";
							this.txtQcPdType.Text = "";
							this.txtQnPdType.Text = "";
						}
					}
					break;

				case "TXTQCPDGRP":
				case "TXTQNPDGRP":
					if (this.pofrmGetPdGrp != null)
					{
						DataRow dtrPdGrp = this.pofrmGetPdGrp.RetrieveValue();

						if (dtrPdGrp != null)
						{
							//this.txtQcPdGrp.Tag = dtrPdGrp["cRowID"].ToString();
							//this.txtQcPdGrp.Text = dtrPdGrp["cCode"].ToString().TrimEnd();
							//this.txtQnPdGrp.Text = dtrPdGrp["cName"].ToString().TrimEnd();
							this.txtQcPdGrp.Tag = dtrPdGrp["fcSkid"].ToString();
							this.txtQcPdGrp.Text = dtrPdGrp["fcCode"].ToString().TrimEnd();
							this.txtQnPdGrp.Text = dtrPdGrp["fcName"].ToString().TrimEnd();
						}
						else
						{
							this.txtQcPdGrp.Tag = "";
							this.txtQcPdGrp.Text = "";
							this.txtQnPdGrp.Text = "";
						}
					}
					break;

				case "TXTQCSUPPL":
				case "TXTQNSUPPL":
					if (this.pofrmGetSuppl != null)
					{
						DataRow dtrSuppl = this.pofrmGetSuppl.RetrieveValue();

						if (dtrSuppl != null)
						{
							this.txtQcSuppl.Tag = dtrSuppl["fcSkid"].ToString();
							this.txtQcSuppl.Text = dtrSuppl["fcCode"].ToString().TrimEnd();
							this.txtQnSuppl.Text = dtrSuppl["fcName"].ToString().TrimEnd();
						}
						else
						{
							this.txtQcSuppl.Tag = "";
							this.txtQcSuppl.Text = "";
							this.txtQnSuppl.Text = "";
						}
					}
					break;

				case "CQNUM":
					if (this.pofrmGetUM != null)
					{
						DataRow dtrUM = this.pofrmGetUM.RetrieveValue();
						DataRow dtrTemPdVer = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

						if (dtrUM != null)
						{
							dtrTemPdVer["cUm"] = dtrUM["fcSkid"].ToString();
							dtrTemPdVer["cQnUm"] = dtrUM["fcName"].ToString().TrimEnd();
							strRetValue = dtrUM["fcName"].ToString().TrimEnd();
						}
						else
						{
							dtrTemPdVer["cUm"] = "";
							dtrTemPdVer["cQnUm"] = "";
							strRetValue = "";
						}
						this.gridView2.UpdateCurrentRow();
					}
					break;

				case "TXTQCUM":
				case "TXTQNUM":
				case "TXTQCUM1":
				case "TXTQNUM1":
				case "TXTQCUM2":
				case "TXTQNUM2":
				case "TXTQCSTUM":
				case "TXTQNSTUM":
				case "TXTQCSTUM1":
				case "TXTQNSTUM1":
				case "TXTQCSTUM2":
				case "TXTQNSTUM2":
				case "TXTQNPRICE1":
				case "TXTQNPRICE2":
				case "TXTQNPRICE3":
				case "TXTQNPRICE4":
				case "TXTQNPRICE5":
				case "TXTQNPRICEA1":
				case "TXTQNPRICEA2":
				case "TXTQNPRICEA3":
				case "TXTQNPRICEA4":
				case "TXTQNPRICEA5":
				case "TXTQNPRICEB1":
				case "TXTQNPRICEB2":
				case "TXTQNPRICEB3":
				case "TXTQNPRICEB4":
				case "TXTQNPRICEB5":
				case "TXTQNPRICEC1":
				case "TXTQNPRICEC2":
				case "TXTQNPRICEC3":
				case "TXTQNPRICEC4":
				case "TXTQNPRICEC5":
				case "TXTQNPRICED1":
				case "TXTQNPRICED2":
				case "TXTQNPRICED3":
				case "TXTQNPRICED4":
				case "TXTQNPRICED5":
					if (this.pofrmGetUM != null)
					{
						DataRow dtrUM = this.pofrmGetUM.RetrieveValue();

						if (dtrUM != null)
						{
							//string strUM = dtrUM["cRowID"].ToString();
							//string strQcUm = dtrUM["cCode"].ToString().TrimEnd();
							//string strQnUm = dtrUM["cName"].ToString().TrimEnd();

							string strUM = dtrUM["fcSkid"].ToString();
							string strQcUm = dtrUM["fcCode"].ToString().TrimEnd();
							string strQnUm = dtrUM["fcName"].ToString().TrimEnd();

							switch (inPopupForm.TrimEnd().ToUpper())
							{

								#region "Page Edit2"
								case "TXTQCUM":
								case "TXTQNUM":
									this.txtQcUM.Tag = strUM;
									this.txtQcUM.Text = strQcUm;
									this.txtQnUM.Text = strQnUm;

									this.pmSetDefaUM();
									this.pmSetStdQnUM(strUM, strQnUm);

									break;
								case "TXTQCUM1":
								case "TXTQNUM1":
									this.txtQcUM1.Tag = strUM;
									this.txtQcUM1.Text = strQcUm;
									this.txtQnUM1.Text = strQnUm;
									break;
								case "TXTQCUM2":
								case "TXTQNUM2":
									this.txtQcUM2.Tag = strUM;
									this.txtQcUM2.Text = strQcUm;
									this.txtQnUM2.Text = strQnUm;
									break;
								case "TXTQCSTUM":
								case "TXTQNSTUM":
									this.txtQcStUm.Tag = strUM;
									this.txtQcStUm.Text = strQcUm;
									this.txtQnStUm.Text = strQnUm;
									break;
								case "TXTQCSTUM1":
								case "TXTQNSTUM1":
									this.txtQcStUm1.Tag = strUM;
									this.txtQcStUm1.Text = strQcUm;
									this.txtQnStUm1.Text = strQnUm;
									break;
								case "TXTQCSTUM2":
								case "TXTQNSTUM2":
									this.txtQcStUm2.Tag = strUM;
									this.txtQcStUm2.Text = strQcUm;
									this.txtQnStUm2.Text = strQnUm;
									break;
								#endregion

								#region "Std Price"
								case "TXTQNPRICE2":
									this.txtQnPrice2.Tag = strUM;
									this.txtQnPrice2.Text = strQnUm;
									break;
								case "TXTQNPRICE3":
									this.txtQnPrice3.Tag = strUM;
									this.txtQnPrice3.Text = strQnUm;
									break;
								case "TXTQNPRICE4":
									this.txtQnPrice4.Tag = strUM;
									this.txtQnPrice4.Text = strQnUm;
									break;
								case "TXTQNPRICE5":
									this.txtQnPrice5.Tag = strUM;
									this.txtQnPrice5.Text = strQnUm;
									break;
								#endregion

								#region "Price A"
								case "TXTQNPRICEA2":
									this.txtQnPriceA2.Tag = strUM;
									this.txtQnPriceA2.Text = strQnUm;
									break;
								case "TXTQNPRICEA3":
									this.txtQnPriceA3.Tag = strUM;
									this.txtQnPriceA3.Text = strQnUm;
									break;
								case "TXTQNPRICEA4":
									this.txtQnPriceA4.Tag = strUM;
									this.txtQnPriceA4.Text = strQnUm;
									break;
								case "TXTQNPRICEA5":
									this.txtQnPriceA5.Tag = strUM;
									this.txtQnPriceA5.Text = strQnUm;
									break;
								#endregion

								#region "Price B"

								case "TXTQNPRICEB2":
									this.txtQnPriceB2.Tag = strUM;
									this.txtQnPriceB2.Text = strQnUm;
									break;
								case "TXTQNPRICEB3":
									this.txtQnPriceB3.Tag = strUM;
									this.txtQnPriceB3.Text = strQnUm;
									break;
								case "TXTQNPRICEB4":
									this.txtQnPriceB4.Tag = strUM;
									this.txtQnPriceB4.Text = strQnUm;
									break;
								case "TXTQNPRICEB5":
									this.txtQnPriceB5.Tag = strUM;
									this.txtQnPriceB5.Text = strQnUm;
									break;
								#endregion

								#region "Price C"
								case "TXTQNPRICEC2":
									this.txtQnPriceC2.Tag = strUM;
									this.txtQnPriceC2.Text = strQnUm;
									break;
								case "TXTQNPRICEC3":
									this.txtQnPriceC3.Tag = strUM;
									this.txtQnPriceC3.Text = strQnUm;
									break;
								case "TXTQNPRICEC4":
									this.txtQnPriceC4.Tag = strUM;
									this.txtQnPriceC4.Text = strQnUm;
									break;
								case "TXTQNPRICEC5":
									this.txtQnPriceC5.Tag = strUM;
									this.txtQnPriceC5.Text = strQnUm;
									break;
								#endregion

								#region "Price D"
								case "TXTQNPRICED2":
									this.txtQnPriceD2.Tag = strUM;
									this.txtQnPriceD2.Text = strQnUm;
									break;
								case "TXTQNPRICED3":
									this.txtQnPriceD3.Tag = strUM;
									this.txtQnPriceD3.Text = strQnUm;
									break;
								case "TXTQNPRICED4":
									this.txtQnPriceD4.Tag = strUM;
									this.txtQnPriceD4.Text = strQnUm;
									break;
								case "TXTQNPRICED5":
									this.txtQnPriceD5.Tag = strUM;
									this.txtQnPriceD5.Text = strQnUm;
									break;
								#endregion

							}

						}
						else
						{


							switch (inPopupForm.TrimEnd().ToUpper())
							{

								#region "Page Edit2"
								case "TXTQCUM":
								case "TXTQNUM":
									this.txtQcUM.Tag = "";
									this.txtQcUM.Text = "";
									this.txtQnUM.Text = "";
									break;
								case "TXTQCUM1":
								case "TXTQNUM1":
									this.txtQcUM1.Tag = "";
									this.txtQcUM1.Text = "";
									this.txtQnUM1.Text = "";
									break;
								case "TXTQCUM2":
								case "TXTQNUM2":
									this.txtQcUM2.Tag = "";
									this.txtQcUM2.Text = "";
									this.txtQnUM2.Text = "";
									break;
								case "TXTQCSTUM":
								case "TXTQNSTUM":
									this.txtQcStUm.Tag = "";
									this.txtQcStUm.Text = "";
									this.txtQnStUm.Text = "";
									break;
								case "TXTQCSTUM1":
								case "TXTQNSTUM1":
									this.txtQcStUm1.Tag = "";
									this.txtQcStUm1.Text = "";
									this.txtQnStUm1.Text = "";
									break;
								case "TXTQCSTUM2":
								case "TXTQNSTUM2":
									this.txtQcStUm2.Tag = "";
									this.txtQcStUm2.Text = "";
									this.txtQnStUm2.Text = "";
									break;
								#endregion

								#region "Std Price"
								case "TXTQNPRICE2":
									this.txtQnPrice2.Tag = "";
									this.txtQnPrice2.Text = "";
									break;
								case "TXTQNPRICE3":
									this.txtQnPrice3.Tag = "";
									this.txtQnPrice3.Text = "";
									break;
								case "TXTQNPRICE4":
									this.txtQnPrice4.Tag = "";
									this.txtQnPrice4.Text = "";
									break;
								case "TXTQNPRICE5":
									this.txtQnPrice5.Tag = "";
									this.txtQnPrice5.Text = "";
									break;
								#endregion

								#region "Price A"
								case "TXTQNPRICEA2":
									this.txtQnPriceA2.Tag = "";
									this.txtQnPriceA2.Text = "";
									break;
								case "TXTQNPRICEA3":
									this.txtQnPriceA3.Tag = "";
									this.txtQnPriceA3.Text = "";
									break;
								case "TXTQNPRICEA4":
									this.txtQnPriceA4.Tag = "";
									this.txtQnPriceA4.Text = "";
									break;
								case "TXTQNPRICEA5":
									this.txtQnPriceA5.Tag = "";
									this.txtQnPriceA5.Text = "";
									break;
								#endregion

								#region "Price B"

								case "TXTQNPRICEB2":
									this.txtQnPriceB2.Tag = "";
									this.txtQnPriceB2.Text = "";
									break;
								case "TXTQNPRICEB3":
									this.txtQnPriceB3.Tag = "";
									this.txtQnPriceB3.Text = "";
									break;
								case "TXTQNPRICEB4":
									this.txtQnPriceB4.Tag = "";
									this.txtQnPriceB4.Text = "";
									break;
								case "TXTQNPRICEB5":
									this.txtQnPriceB5.Tag = "";
									this.txtQnPriceB5.Text = "";
									break;
								#endregion

								#region "Price C"
								case "TXTQNPRICEC2":
									this.txtQnPriceC2.Tag = "";
									this.txtQnPriceC2.Text = "";
									break;
								case "TXTQNPRICEC3":
									this.txtQnPriceC3.Tag = "";
									this.txtQnPriceC3.Text = "";
									break;
								case "TXTQNPRICEC4":
									this.txtQnPriceC4.Tag = "";
									this.txtQnPriceC4.Text = "";
									break;
								case "TXTQNPRICEC5":
									this.txtQnPriceC5.Tag = "";
									this.txtQnPriceC5.Text = "";
									break;
								#endregion

								#region "Price D"
								case "TXTQNPRICED2":
									this.txtQnPriceD2.Tag = "";
									this.txtQnPriceD2.Text = "";
									break;
								case "TXTQNPRICED3":
									this.txtQnPriceD3.Tag = "";
									this.txtQnPriceD3.Text = "";
									break;
								case "TXTQNPRICED4":
									this.txtQnPriceD4.Tag = "";
									this.txtQnPriceD4.Text = "";
									break;
								case "TXTQNPRICED5":
									this.txtQnPriceD5.Tag = "";
									this.txtQnPriceD5.Text = "";
									break;
								#endregion

							}

						}
					}
					break;
			}
			return strRetValue;
		}

		private void pmSetDefaUM()
		{
			if (this.txtQcUM1.Text.Trim() == string.Empty)
			{
				this.txtUm1Qty.Value = 1;
				this.txtQcUM1.Tag = this.txtQcUM.Tag.ToString();
				this.txtQcUM1.Text = this.txtQcUM.Text;
				this.txtQnUM1.Text = this.txtQnUM.Text;
			}

			if (this.txtQcUM2.Text.Trim() == string.Empty)
			{
				this.txtUm2Qty.Value = 1;
				this.txtQcUM2.Tag = this.txtQcUM.Tag.ToString();
				this.txtQcUM2.Text = this.txtQcUM.Text;
				this.txtQnUM2.Text = this.txtQnUM.Text;
			}

			if (this.txtQcStUm.Text.Trim() == string.Empty)
			{
				this.txtQcStUm.Tag = this.txtQcUM.Tag.ToString();
				this.txtQcStUm.Text = this.txtQcUM.Text;
				this.txtQnStUm.Text = this.txtQnUM.Text;
			}

			if (this.txtQcStUm1.Text.Trim() == string.Empty)
			{
				this.txtStUm1Qty.Value = 1;
				this.txtQcStUm1.Tag = this.txtQcUM.Tag.ToString();
				this.txtQcStUm1.Text = this.txtQcUM.Text;
				this.txtQnStUm1.Text = this.txtQnUM.Text;
			}

			if (this.txtQcStUm2.Text.Trim() == string.Empty)
			{
				this.txtStUm2Qty.Value = 1;
				this.txtQcStUm2.Tag = this.txtQcUM.Tag.ToString();
				this.txtQcStUm2.Text = this.txtQcUM.Text;
				this.txtQnStUm2.Text = this.txtQnUM.Text;
			}

		}

		private void pmSetStdQnUM(string inUM, string inQnUmStd)
		{
			for (int intCnt = 0; intCnt < this.paTxtStdUM.Count; intCnt++)
			{
				DevExpress.XtraEditors.ButtonEdit oTxtQnUm = (DevExpress.XtraEditors.ButtonEdit)this.paTxtStdUM[intCnt];
				oTxtQnUm.Tag = inUM;
				oTxtQnUm.Text = inQnUmStd;
			}
		}

		private void txtQcAccBCash_Validating(object sender, CancelEventArgs e)
		{
			DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
			string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCACCBCASH" ? "FCCODE" : "FCNAME");
			if (txtPopUp.Text == "")
			{
				this.txtQcAccBCash.Tag = "";
				this.txtQcAccBCash.Text = "";
				this.txtQnAccBCash.Text = "";
			}
			else
			{
				this.pmInitPopUpDialog("ACCHART");
				e.Cancel = !this.pofrmGetAcChart.ValidateField(txtPopUp.Text, strOrderBy, false);
				if (this.pofrmGetAcChart.PopUpResult)
				{
					this.pmRetrievePopUpVal(txtPopUp.Name);
				}
				else
				{
					txtPopUp.Text = txtPopUp.OldEditValue.ToString();
				}
			}

		}

		private void txtQcAccBCred_Validating(object sender, CancelEventArgs e)
		{
			DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
			string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCACCBCRED" ? "FCCODE" : "FCNAME");
			if (txtPopUp.Text == "")
			{
				this.txtQcAccBCred.Tag = "";
				this.txtQcAccBCred.Text = "";
				this.txtQnAccBCred.Text = "";
			}
			else
			{
				this.pmInitPopUpDialog("ACCHART");
				e.Cancel = !this.pofrmGetAcChart.ValidateField(txtPopUp.Text, strOrderBy, false);
				if (this.pofrmGetAcChart.PopUpResult)
				{
					this.pmRetrievePopUpVal(txtPopUp.Name);
				}
				else
				{
					txtPopUp.Text = txtPopUp.OldEditValue.ToString();
				}
			}

		}

		private void txtQcAccSCash_Validating(object sender, CancelEventArgs e)
		{
			DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
			string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCACCSCASH" ? "FCCODE" : "FCNAME");
			if (txtPopUp.Text == "")
			{
				this.txtQcAccSCash.Tag = "";
				this.txtQcAccSCash.Text = "";
				this.txtQnAccSCash.Text = "";
			}
			else
			{
				this.pmInitPopUpDialog("ACCHART");
				e.Cancel = !this.pofrmGetAcChart.ValidateField(txtPopUp.Text, strOrderBy, false);
				if (this.pofrmGetAcChart.PopUpResult)
				{
					this.pmRetrievePopUpVal(txtPopUp.Name);
				}
				else
				{
					txtPopUp.Text = txtPopUp.OldEditValue.ToString();
				}
			}

		}

		private void txtQcAccSCred_Validating(object sender, CancelEventArgs e)
		{
			DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
			string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCACCSCRED" ? "FCCODE" : "FCNAME");
			if (txtPopUp.Text == "")
			{
				this.txtQcAccSCred.Tag = "";
				this.txtQcAccSCred.Text = "";
				this.txtQnAccSCred.Text = "";
			}
			else
			{
				this.pmInitPopUpDialog("ACCHART");
				e.Cancel = !this.pofrmGetAcChart.ValidateField(txtPopUp.Text, strOrderBy, false);
				if (this.pofrmGetAcChart.PopUpResult)
				{
					this.pmRetrievePopUpVal(txtPopUp.Name);
				}
				else
				{
					txtPopUp.Text = txtPopUp.OldEditValue.ToString();
				}
			}

		}

		private void txtQcPdGrp_Validating(object sender, CancelEventArgs e)
		{
			DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
			//string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCPDGRP" ? "CCODE" : "CNAME");
			string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCPDGRP" ? "FCCODE" : "FCNAME");
			if (txtPopUp.Text == "")
			{
				this.txtQcPdGrp.Tag = "";
				this.txtQcPdGrp.Text = "";
				this.txtQnPdGrp.Text = "";
			}
			else
			{
				this.pmInitPopUpDialog("PDGRP");
				e.Cancel = !this.pofrmGetPdGrp.ValidateField(txtPopUp.Text, strOrderBy, false);
				if (this.pofrmGetPdGrp.PopUpResult)
				{
					this.pmRetrievePopUpVal(txtPopUp.Name);
				}
				else
				{
					txtPopUp.Text = txtPopUp.OldEditValue.ToString();
				}
			}

		}

		private void txtQcSuppl_Validating(object sender, CancelEventArgs e)
		{
			DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
			string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCSUPPL" ? "FCCODE" : "FCNAME");
			if (txtPopUp.Text == "")
			{
				this.txtQcSuppl.Tag = "";
				this.txtQcSuppl.Text = "";
				this.txtQnSuppl.Text = "";
			}
			else
			{
				this.pmInitPopUpDialog("SUPPL");
				e.Cancel = !this.pofrmGetSuppl.ValidateField(txtPopUp.Text, strOrderBy, false);
				if (this.pofrmGetSuppl.PopUpResult)
				{
					this.pmRetrievePopUpVal(txtPopUp.Name);
				}
				else
				{
					txtPopUp.Text = txtPopUp.OldEditValue.ToString();
				}
			}
		}

		private void txtQcUM_Validating(object sender, CancelEventArgs e)
		{

			DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
			string strTxtPrefix = StringHelper.Left(txtPopUp.Name.ToUpper(), 5);
			//string strOrderBy = (strTxtPrefix == "TXTQC" ? "CCODE" : "CNAME");
			string strOrderBy = (strTxtPrefix == "TXTQC" ? "FCCODE" : "FCNAME");
			if (txtPopUp.Text == "")
			{
				txtPopUp.Tag = "";
				switch (txtPopUp.Name.ToUpper())
				{
					case "TXTQCUM":
					case "TXTQNUM":
						this.txtQcUM.Tag = "";
						this.txtQcUM.Text = "";
						this.txtQnUM.Text = "";
						break;
					case "TXTQCUM1":
					case "TXTQNUM1":
						this.txtQcUM1.Tag = "";
						this.txtQcUM1.Text = "";
						this.txtQnUM1.Text = "";
						break;
					case "TXTQCUM2":
					case "TXTQNUM2":
						this.txtQcUM2.Tag = "";
						this.txtQcUM2.Text = "";
						this.txtQnUM2.Text = "";
						break;
					case "TXTQCSTUM":
					case "TXTQNSTUM":
						this.txtQcStUm.Tag = "";
						this.txtQcStUm.Text = "";
						this.txtQnStUm.Text = "";
						break;
					case "TXTQCSTUM1":
					case "TXTQNSTUM1":
						this.txtQcStUm1.Tag = "";
						this.txtQcStUm1.Text = "";
						this.txtQnStUm1.Text = "";
						break;
					case "TXTQCSTUM2":
					case "TXTQNSTUM2":
						this.txtQcStUm2.Tag = "";
						this.txtQcStUm2.Text = "";
						this.txtQnStUm2.Text = "";
						break;
				}
			}
			else
			{
				this.pmInitPopUpDialog("UM");
				e.Cancel = !this.pofrmGetUM.ValidateField(txtPopUp.Text, strOrderBy, false);
				if (this.pofrmGetUM.PopUpResult)
				{
					this.pmRetrievePopUpVal(txtPopUp.Name);
				}
				else
				{
					txtPopUp.Text = txtPopUp.OldEditValue.ToString();
				}
			}

		}

		private void txtQcPdType_Validating(object sender, CancelEventArgs e)
		{
			DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
			string strOrderBy = (txtPopUp.Name.ToUpper() == "TXTQCPDTYPE" ? "FCCODE" : "FCNAME");
			if (txtPopUp.Text == "")
			{
				this.txtQcPdType.Tag = "";
				this.txtQcPdType.Text = "";
				this.txtQnPdType.Text = "";
			}
			else
			{
				this.pmInitPopUpDialog("PDTYPE");
				e.Cancel = !this.pofrmGetPdType.ValidateField(txtPopUp.Text, strOrderBy, false);
				if (this.pofrmGetPdType.PopUpResult)
				{
					this.pmRetrievePopUpVal(txtPopUp.Name);
				}
				else
				{
					txtPopUp.Text = txtPopUp.OldEditValue.ToString();
				}
			}
		}

		private void pmLoadImage()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = "ระบุรูปภาพสินค้า";
			dlg.Filter = "Image Files (JPEG, GIF, BMP, etc.)|"
				+ "*.jpg;*.jpeg;*.gif;*.bmp;"
				+ "*.tif;*.tiff;*.png|"
				+ "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|"
				+ "GIF files (*.gif)|*.gif|"
				+ "BMP files (*.bmp)|*.bmp|"
				+ "TIFF files (*.tif;*.tiff)|*.tif;*.tiff|"
				+ "PNG files (*.png)|*.png";

			dlg.InitialDirectory = Environment.CurrentDirectory;
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				this.pmClearImage();
				this.lblImgFileName.Text = dlg.FileName;
				this.pcbPdImage.Image = System.Drawing.Image.FromFile(dlg.FileName);
				this.pcbPdImage.Invalidate();
			}
			dlg.Dispose();
		}

		private void pmClearImage()
		{
			this.lblImgFileName.Text = "";
			this.pcbPdImage.Image = null;
		}

		private void btnLoadImg_Click(object sender, EventArgs e)
		{
			this.pmLoadImage();
		}

		private void btnClearImg_Click(object sender, EventArgs e)
		{
			this.pmClearImage();
		}


		public bool ValidateField(string inSearchStr, string inOrderBy, bool inForcePopUp)
		{
			bool bllIsVField = true;
			this.mbllPopUpResult = false;

			this.gridView1.OptionsView.ShowAutoFilterRow = true;
			this.gridView1.OptionsView.ShowAutoFilterRow = false;
			this.gridView1.OptionsView.ShowAutoFilterRow = true;

			inOrderBy = inOrderBy.ToUpper();
			if (inOrderBy.ToUpper() == "CCODE")
				inSearchStr = inSearchStr.PadRight(MAXLENGTH_CODE);
			else
				inSearchStr = inSearchStr.PadRight(MAXLENGTH_NAME);

			if (this.mstrSortKey != inOrderBy)
			{
				this.mstrSortKey = inOrderBy;
				this.pmSetSortKey(this.mstrSortKey, true);
			}

			if (this.mstrSearchStr != inSearchStr
				|| this.mstrPdType.Trim() != ""
				|| this.mbllIsFormQuery == false)
			{
				string strSearch = inSearchStr.TrimEnd();
				if (strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_STAR ||
					strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_2SLASH)
				{
					strSearch = "";
				}


				this.mstrPdType = "";

				this.mstrSearchStr = "%" + strSearch + "%";
				this.pmRefreshBrowView();
			}

			int intSeekVal = this.gridView1.LocateByValue(0, this.gridView1.Columns[this.mstrSortKey], inSearchStr);
			bllIsVField = (intSeekVal < 0);

			this.mbllIsShowDialog = false;
			if (inForcePopUp || bllIsVField)
			{
				//this.gridView1.StartIncrementalSearch(inSearchStr.TrimEnd());
				//this.ShowDialog();
				//this.mbllIsShowDialog = true;

				this.gridView1.FocusedColumn = this.gridView1.Columns[this.mstrSortKey];

				this.gridView1.MoveLast();
				this.gridView1.StartIncrementalSearch(inSearchStr.TrimEnd());
				if (this.gridView1.FocusedRowHandle >= this.gridView1.RowCount - 1)
				{
					string strSeekNear = BizRule.GetNearString(this.dtsDataEnv.Tables[this.mstrBrowViewAlias], inSearchStr.Trim(), inOrderBy);
					strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QEMJobInfo.Field.Code ? MAXLENGTH_CODE : MAXLENGTH_NAME);
					this.gridView1.StartIncrementalSearch(strSeekNear);
				}

				this.mintFocusRow = this.gridView1.FocusedRowHandle;

				this.ShowDialog();
				this.mbllIsShowDialog = true;

			}
			else
			{
				this.gridView1.FocusedRowHandle = intSeekVal;
				this.mbllPopUpResult = true;
			}
			return this.mbllPopUpResult;
		}

		public bool ValidateField(string inPdType, string inSearchStr, string inOrderBy, bool inForcePopUp)
		{
			bool bllIsVField = true;
			this.mbllPopUpResult = false;

			this.gridView1.OptionsView.ShowAutoFilterRow = true;
			this.gridView1.OptionsView.ShowAutoFilterRow = false;
			this.gridView1.OptionsView.ShowAutoFilterRow = true;

			inOrderBy = inOrderBy.ToUpper();
			if (inOrderBy.ToUpper() == "CCODE")
				inSearchStr = inSearchStr.PadRight(MAXLENGTH_CODE);
			else
				inSearchStr = inSearchStr.PadRight(MAXLENGTH_NAME);

			//if (inOrderBy.ToUpper() == "FCCODE")
			//    inSearchStr = inSearchStr.TrimEnd();
			//else
			//    inSearchStr = inSearchStr.TrimEnd();

			if (this.mstrSortKey != inOrderBy)
			{
				this.mstrSortKey = inOrderBy;
				this.pmSetSortKey(this.mstrSortKey, true);
			}

			if (this.mstrSearchStr != inSearchStr
				|| inPdType != this.mstrPdType
				|| this.mbllIsFormQuery == false)
			{
				string strSearch = inSearchStr.TrimEnd();
				if (strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_STAR ||
					strSearch == SysDef.gc_DEFAULT_VALDATEKEY_PREFIX_2SLASH)
				{
					strSearch = "";
				}

				//this.mstrSearchStr = "%" + strSearch + "%";
				this.mstrSearchStr = strSearch;
				this.mstrPdType = inPdType;
				this.pmRefreshBrowView();
			}

			int intSeekVal = this.gridView1.LocateByValue(0, this.gridView1.Columns[this.mstrSortKey], inSearchStr);
			bllIsVField = (intSeekVal < 0);

			this.mbllIsShowDialog = false;
			if (inForcePopUp || bllIsVField)
			{
				//this.gridView1.StartIncrementalSearch(inSearchStr.TrimEnd());
				//this.ShowDialog();
				//this.mbllIsShowDialog = true;

				this.gridView1.FocusedColumn = this.gridView1.Columns[this.mstrSortKey];

				this.gridView1.MoveLast();
				this.gridView1.StartIncrementalSearch(inSearchStr.TrimEnd());
				if (this.gridView1.FocusedRowHandle >= this.gridView1.RowCount - 1)
				{
					string strSeekNear = BizRule.GetNearString(this.dtsDataEnv.Tables[this.mstrBrowViewAlias], inSearchStr.Trim(), inOrderBy);
					strSeekNear = strSeekNear.PadRight(inOrderBy.ToUpper() == QEMJobInfo.Field.Code ? MAXLENGTH_CODE : MAXLENGTH_NAME);
					this.gridView1.StartIncrementalSearch(strSeekNear);
				}

				this.mintFocusRow = this.gridView1.FocusedRowHandle;

				this.ShowDialog();
				this.mbllIsShowDialog = true;

			}
			else
			{
				this.gridView1.FocusedRowHandle = intSeekVal;
				this.mbllPopUpResult = true;
			}
			return this.mbllPopUpResult;
		}

		public DataRow RetrieveValue()
		{

			DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
			if (dtrBrow == null) return null;

			string strErrorMsg = "";
			WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ERPConnectionString, App.DatabaseReside);
			pobjSQLUtil.SetPara(new object[1] { dtrBrow["CROWID"].ToString() });
			if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QVFLD_Table", this.mstrRefTable, "select * from " + this.mstrRefTable + " where fcSkid = ? ", ref strErrorMsg))
			{
				return this.dtsDataEnv.Tables["QVFLD_Table"].Rows[0];
			}
			return null;
		}

		private void cmbActive_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pmSetDActive();
		}

		private void pmSetDActive()
		{
			this.txtDInActive.Enabled = (this.cmbActive.SelectedIndex == 1);
		}

		private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
		{
			if (e.Value == null)
				return;

			ColumnView view = this.grdHistory.MainView as ColumnView;

			string strCol = gridView2.FocusedColumn.FieldName;
			DataRow dtrTemPdVer = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

			switch (strCol.ToUpper())
			{
				case "CQNUM":

					string strValue = e.Value.ToString();
					this.pmInitPopUpDialog("UM");

					e.Valid = !this.pofrmGetUM.ValidateField(strValue, "FCNAME", false);

					if (this.pofrmGetUM.PopUpResult)
					{
						string strQnUm = this.pmRetrievePopUpVal(strCol.ToUpper());
						e.Value = strQnUm;
						e.Valid = true;
					}
					else
					{
						e.Value = "";
						dtrTemPdVer["cUM"] = "";
						//txtPopUp.Text = txtPopUp.OldEditValue.ToString();
					}

					break;
			}

		}

		private void grcUM_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{

			DataRow dtrTemPdVer = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);

			if (dtrTemPdVer == null)
			{
				dtrTemPdVer = this.dtsDataEnv.Tables[this.mstrTemPdVer].NewRow();
				this.dtsDataEnv.Tables[this.mstrTemPdVer].Rows.Add(dtrTemPdVer);
			}
			this.pmInitPopUpDialog("UM");
			this.pofrmGetUM.ValidateField("", "FCNAME", true);
			if (this.pofrmGetUM.PopUpResult)
			{
				//this.gridView2.UpdateCurrentRow();
				this.pmRetrievePopUpVal("CQNUM");
			}
		}

		private void grcUM_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Alt | e.Control | e.Shift)
				return;

			switch (e.KeyCode)
			{
				case Keys.F3:
					this.pmInitPopUpDialog("UM");
					this.pofrmGetUM.ValidateField("", "FCNAME", true);
					if (this.pofrmGetUM.PopUpResult)
					{
						//this.gridView2.UpdateCurrentRow();
						DataRow dtrTemPdVer = this.gridView2.GetDataRow(this.gridView2.FocusedRowHandle);
						if (dtrTemPdVer == null)
						{
							dtrTemPdVer = this.dtsDataEnv.Tables[this.mstrTemPdVer].NewRow();
							this.dtsDataEnv.Tables[this.mstrTemPdVer].Rows.Add(dtrTemPdVer);
						}
						this.pmRetrievePopUpVal("CQNUM");
					}
					break;
			}
		}

		private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
		{
			int rowIndex = e.RowHandle;
			if (rowIndex >= 0)
			{
				rowIndex++;
				e.Info.DisplayText = rowIndex.ToString();
			}
		}

        private void pmPrintData()
        {

            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow == null)
                return;

            //this.mstrEditRowID = dtrBrow["fcSkid"].ToString();

            string strCode = dtrBrow["cCode"].ToString().TrimEnd();
            using (Transaction.Common.dlgPRange dlg = new Transaction.Common.dlgPRange())
            {

                dlg.BeginCode = strCode;
                dlg.EndCode = strCode;

                string[] strADir = null;
                string strFormPath = Application.StartupPath + "\\RPT\\FORM_PROD\\";
                if (System.IO.Directory.Exists(strFormPath))
                {
                    strADir = System.IO.Directory.GetFiles(strFormPath);
                    dlg.LoadRPT(strFormPath);
                }

                dlg.pnlPrnQty.Visible = true;
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.OK)
                {
                    //App.mSave_hWnd = App.GetForegroundWindow();
                    int intNum2Prn = (int)dlg.txtPNum.Value;
                    string strRPTFileName = "";
                    this.pmPrintRangeData(dlg.BeginCode, dlg.EndCode, strADir[dlg.cmbPForm.SelectedIndex], intNum2Prn);
                }
            }
        }

        private void pmPrintRangeData(string inBegCode, string inEndCode, string inRPTFileName, int inNum2Prn)
        {
            string strErrorMsg = "";
            if (inNum2Prn <=0) inNum2Prn = 1;
            
            Report.RPT.dtsPBarCode dtsPrintPreview = new Report.RPT.dtsPBarCode();

            //dlgWaitWind dlg = new dlgWaitWind();
            //dlg.WaitWind("กำลังเตรียมข้อมูลเพื่อการพิมพ์");

            WS.Data.Agents.cDBMSAgent pobjSQLUtil = new WS.Data.Agents.cDBMSAgent(App.ConnectionString, App.DatabaseReside);
            //object[] pAPara = new object[3] { App.ActiveCorp.CorpID, inBegCode, inEndCode};

            //Report.Agents.PrintField oPrintField = new Report.Agents.PrintField(pobjSQLUtil, pobjSQLUtil2, App.ActiveCorp);

            string strSQLStr = "";
            strSQLStr = "select * from " + this.mstrRefTable + " where fcCorp = ? and fcCode between ? and ? order by fcCode";
            pobjSQLUtil.SetPara(new object[] { App.ActiveCorp.RowID, inBegCode, inEndCode });

            if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QPrn", this.mstrRefTable, strSQLStr, ref strErrorMsg))
            {
                foreach (DataRow dtrPFormH in this.dtsDataEnv.Tables["QPrn"].Rows)
                {
                    for (int i = 0; i < inNum2Prn; i++)
                    {

                        #region "Print 1 Row"
                        DataRow dtrLastRow = dtsPrintPreview.DTSBARCODE.NewRow();
                        //จำนวน
                        dtrLastRow["Barcode"] = dtrPFormH["fcCode"].ToString().Trim();
                        //มูลค่า
                        pobjSQLUtil.SetPara(new object[] { dtrPFormH["FCSKID"].ToString() });
                        if (pobjSQLUtil.SQLExec(ref this.dtsDataEnv, "QProd", "PROD", "select * from PROD where FCSKID = ?", ref strErrorMsg))
                        {
                            DataRow dtrProd = this.dtsDataEnv.Tables["QProd"].Rows[0];
                            dtrLastRow["BarCode"] = dtrProd["FCCODE"].ToString().TrimEnd();
                            dtrLastRow["QnProd"] = dtrProd["FCNAME"].ToString().TrimEnd();
                            dtrLastRow["QnProd"] = dtrProd["FCNAME"].ToString().TrimEnd();
                            dtrLastRow["Cost"] = Convert.ToDecimal(dtrProd["fnStdCost"]);
                            dtrLastRow["Price_Sale1"] = Convert.ToDecimal(dtrProd["fnPrice"]);
                        }

                        dtsPrintPreview.DTSBARCODE.Rows.Add(dtrLastRow);
                        #endregion

                    }

                }

                pmPreviewReport(dtsPrintPreview, inRPTFileName);
            }
        }


        //ArrayList pACrPara = new ArrayList();
        private void pmPreviewReport(DataSet inData, string inRPTFileName)
        {
            Report.RPT.xrPdBarCode01 oprn = new Report.RPT.xrPdBarCode01();

            string strRPTFileName1 = inRPTFileName;
            if (System.IO.File.Exists(strRPTFileName1))
                oprn.LoadLayout(strRPTFileName1);

            oprn.DataSource = inData;
            oprn.CreateDocument();
            oprn.ShowPrintMarginsWarning = false;
            frmDXPreviewReport oPreview = new frmDXPreviewReport();

            oPreview.printControl1.PrintingSystem = oprn.PrintingSystem;

            oPreview.Show();
            
            ////oprn.LoadLayout(
            //oprn.DataSource = inData;
            //oprn.CreateDocument();
            //frmDXPreviewReport oPreview = new frmDXPreviewReport();

            //oPreview.printControl1.PrintingSystem = oprn.PrintingSystem;

            //oPreview.Show();

            //DevExpress.XtraReports.for
            //ReportDocument rptPreviewReport = new ReportDocument();
            //if (!System.IO.File.Exists(inRPTFileName))
            //{
            //    MessageBox.Show("File not found " + inRPTFileName, "Application Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //rptPreviewReport.Load(inRPTFileName);
            //rptPreviewReport.SetDataSource(inData);

            //App.PreviewReport(this, false, rptPreviewReport);
        }

	}
}
