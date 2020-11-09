using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BeSmartMRP.UIHelper;

namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgPRange : UIHelper.frmBase
    {
        public dlgPRange()
        {
            InitializeComponent();

            this.lblTitle.Text = UIBase.GetAppUIText(new string[] { "ระบุช่วงในการพิมพ์ข้อมูล", "Select Document to print" });
            this.lblCode.Text = UIBase.GetAppUIText(new string[] { "ตั้งแต่ :", "From :" });
            this.lblToCode.Text = UIBase.GetAppUIText(new string[] { "ถึง :", "To :" });

            UIBase.SetDefaultChildAppreance(this);
        }

        public void LoadRPT(string inPath)
        {
            int intLen1 = (inPath).Length;
            this.cmbPForm.Properties.Items.Clear();
            string[] strADir = System.IO.Directory.GetFiles(inPath);
            for (int i = 0; i < strADir.Length; i++)
            {
                string strFormName = strADir[i].Substring(intLen1, strADir[i].Length - intLen1);
                this.cmbPForm.Properties.Items.Add(strFormName);
            }
            if (this.cmbPForm.Properties.Items.Count > 0)
            {
                this.cmbPForm.SelectedIndex = 0;
            }
        }

        //public bool ShowPrintForm
        //{
        //    get { return this.pnlWhatForm.Visible; }
        //    set { this.pnlWhatForm.Visible = value; }
        //}

        //public int SelectedFormIndex
        //{
        //    get { return this.cmbForm.SelectedIndex; }
        //    set
        //    {
        //        int intIndex = Convert.ToInt32(value);
        //        if (intIndex < this.cmbForm.Items.Count)
        //            this.cmbForm.SelectedIndex = intIndex;
        //    }
        //}

        //public bool IsShowGroup
        //{
        //    get { return this.chkIsGroup.Visible; }
        //    set { this.chkIsGroup.Visible = value; }
        //}

        //public bool IsPrintGroup
        //{
        //    get { return this.chkIsGroup.Checked; }
        //    set { this.chkIsGroup.Checked = value; }
        //}

        public void InsertForm(string inText, object inValue)
        {
            //this.pmInsertForm(inText, inValue);
        }

        //private void pmInsertForm(string inText, object inValue)
        //{
        //    this.cmbForm.Items.Add(inText, inValue);
        //}

        public string BeginCode
        {
            get { return this.txtBegCode.Text.TrimEnd(); }
            set { this.txtBegCode.Text = value; }
        }

        public string EndCode
        {
            get { return this.txtEndCode.Text.TrimEnd(); }
            set { this.txtEndCode.Text = value; }
        }

    }
}
