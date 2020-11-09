using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgFindPdSer : UIHelper.frmBase
    {
        public dlgFindPdSer()
        {
            InitializeComponent();
        }


		public dlgFindPdSer(string inBranch, string inWHouse, string inProd)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.mstrBranchID = inBranch;
			this.mstrProdID = inProd;
			this.mstrWHouse = inWHouse;
		}

		private DataSet dtsDataEnv = null;

		private dlgSelPdSerial2 podlgGetPdSer = null;

		private string mstrParentID = "";

		private string mstrBranchID = "";
		private string mstrProdID = "";
		private string mstrLot = "";
		private string mstrWHLoca = "";
		private string mstrWHouse = "";

		private string mstrPdSer = "";

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

		public string PdSerID
		{
			get { return this.txtPdSer.Tag.ToString(); }
			set { this.txtPdSer.Tag = value; }
		}

		public string PdSerCode
		{
			get { return this.txtPdSer.Text; }
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{

			if (this.pmValidPdSer())
			{
				this.DialogResult = DialogResult.OK;
			}
			else
			{
			}

		}

		public void BindData(DataSet inDataEnv, int inParent)
		{

			this.mstrParentID = inParent.ToString("00000");
			this.dtsDataEnv = inDataEnv;

			//this.pmBindData();
		}

		private bool pmValidPdSer()
		{
			return true;
		}

		private void pmInitPopUpDialog(string inDialogName)
		{
			switch (inDialogName.TrimEnd().ToUpper())
			{
				case "PDSER":
					if (this.podlgGetPdSer == null)
					{
						this.podlgGetPdSer = new dlgSelPdSerial2(this.mstrBranchID, this.mstrWHouse, this.mstrProdID);
						this.podlgGetPdSer.Location = new Point(AppUtil.CommonHelper.SysMetric(1) - this.podlgGetPdSer.Width - 10, AppUtil.CommonHelper.SysMetric(9) + 5);
					}
					break;
			}
		}

		private void pmRetrievePopUpVal(string inPopupForm)
		{

			switch (inPopupForm.TrimEnd().ToUpper())
			{
				case "PDSER":
					DataRow dtrPdSer = this.podlgGetPdSer.RetrieveValue();
					if (dtrPdSer != null)
					{

						this.mstrLot = dtrPdSer["cLot"].ToString();
						this.mstrWHLoca = dtrPdSer["cWHLoca"].ToString();
						this.mstrWHouse = dtrPdSer["cWHouse"].ToString();

						//this.txtPdSer.Tag = dtrPdSer["cRowID"].ToString();
						this.txtPdSer.Tag = dtrPdSer["cPdSer"].ToString();
						this.txtPdSer.Text = dtrPdSer["cCode"].ToString().TrimEnd();
					}
					else
					{
						this.mstrLot = "";
						this.mstrWHLoca = "";
						this.mstrWHouse = "";

						this.txtPdSer.Tag = "";
						this.txtPdSer.Text = "";
					}
					break;
			}
		}

		private void txtPopup_ButtonClick(object sender, System.EventArgs e)
		{
			//WealthTech.Windows.Forms.WsEditBox txtPopup = (WealthTech.Windows.Forms.WsEditBox)sender;
			DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
			this.pmPopUpButtonClick(txtPopup.Name.ToUpper());
		}

		private void txtPopup_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.Alt | e.Control | e.Shift)
				return;

			switch (e.KeyCode)
			{
				case Keys.F3:
					//WealthTech.Windows.Forms.WsEditBox txtPopup = (WealthTech.Windows.Forms.WsEditBox)sender;
					DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
					this.pmPopUpButtonClick(txtPopup.Name.ToUpper());
					break;
			}
		}

		private void pmPopUpButtonClick(string inTextbox)
		{
			string strOrderBy = "";
			switch (inTextbox)
			{
				case "TXTPDSER":
					this.pmInitPopUpDialog("PDSER");
					this.podlgGetPdSer.ValidateField("", true);
					if (this.podlgGetPdSer.PopUpResult)
						this.pmRetrievePopUpVal("PDSER");
					break;
			}
		}

		private void txtPdSer_WsOnValidating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string strErrorMsg = "";

			//WealthTech.Windows.Forms.WsEditBox txtPopup = (WealthTech.Windows.Forms.WsEditBox)sender;
			DevExpress.XtraEditors.ButtonEdit txtPopup = (DevExpress.XtraEditors.ButtonEdit)sender;
			if (txtPopup.Text == "")
			{
				txtPopup.Tag = "";
				this.mstrLot = "";
				this.mstrWHLoca = "";
				this.mstrWHouse = "";
			}
			else
			{
				this.pmInitPopUpDialog("PDSER");
				e.Cancel = !this.podlgGetPdSer.ValidateField(txtPopup.Text, false);
				if (this.podlgGetPdSer.PopUpResult)
				{
					this.pmRetrievePopUpVal("PDSER");
				}
			}
		}

		private void txtPopUp_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
			DevExpress.XtraEditors.ButtonEdit txtPopUp = (DevExpress.XtraEditors.ButtonEdit)sender;
			this.pmPopUpButtonClick(txtPopUp.Name.ToUpper());
		}

	}
}
