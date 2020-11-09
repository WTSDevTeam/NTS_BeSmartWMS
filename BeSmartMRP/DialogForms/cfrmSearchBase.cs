using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BeSmartMRP.DialogForms
{
    public partial class cfrmSearchBase : UIHelper.frmBase
    {
        public cfrmSearchBase()
        {
            InitializeComponent();

            this.pmInitForm();
        }

        protected internal DataSet dtsDataEnv = new DataSet();
        protected internal string mstrBrowViewAlias = "Brow_Alias";
        protected internal int mintSaveBrowViewRowIndex = -1;

        protected internal string mstrSearchVal = "";
        protected internal string mstrSearchKey = "";

        private System.Collections.ArrayList palSearchKey = new System.Collections.ArrayList();

        private void pmInitForm()
        {
            this.OnInitComponent();
        }

        protected virtual void OnInitComponent()
        {
            this.txtSearchVal.Text = "";
            if (this.cmbSearchBy.Items.Count > 0)
            {
                this.cmbSearchBy.SelectedIndex = 0;
            }
        }

        protected void AddSearchKey(string inText, string inKeyValue)
        {
            this.cmbSearchBy.Items.Add(inText);
            this.palSearchKey.Add(inKeyValue);
        }

        protected virtual void OnSearch(string inSearchVal, string inKey)
        {
            string strMessage = "";
            if (this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Count == 0)
            {
                strMessage = "ไม่พบข้อมูล                ";
                MessageBox.Show(strMessage, "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int intFoundRow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].Rows.Count;
                string strFoundStr = intFoundRow.ToString("###,###,###");
                strMessage = "พบข้อมูลทั้งสิ้น " + strFoundStr + " รายการ";
            }
            
            this.statusStrip1.Items[0].Text = strMessage;
            //this.statusStrip1.Text = strMessage;
        }

        protected virtual void OnSelected()
        { }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            this.OnSearch(this.txtSearchVal.Text, this.palSearchKey[this.cmbSearchBy.SelectedIndex].ToString());
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            this.OnSelected();
            this.DialogResult = DialogResult.OK;
        }

        private void grdSearch_DoubleClick(object sender, System.EventArgs e)
        {
            this.OnSelected();
            this.DialogResult = DialogResult.OK;
        }
    
    }
}