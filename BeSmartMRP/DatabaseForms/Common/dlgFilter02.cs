
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.DatabaseForms;
using BeSmartMRP.UIHelper;

namespace BeSmartMRP.DatabaseForms.Common
{
    public partial class dlgFilter02 : UIHelper.frmBase
    {

        public dlgFilter02()
        {
            InitializeComponent();
            this.pmInitForm();
        }

        private string mstrRefType = "";

        public void SetTitle(string inTitle, string inTaskName)
        {
            this.lblTitle.Text = inTitle;
            this.lblTaskName.Text = "TASKNAME : " + inTaskName;
        }

        public string RefTypeCode
        {
            get { return this.mstrRefType; }
        }

        public string RefTypeName
        {
            get { return this.txtQnRefType.Text; }
        }
        
        private void pmInitForm()
        {
            this.cmbQcRefType.SelectedIndex = 0;
        }

        private void cmbQcRefType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.cmbQcRefType.SelectedIndex)
            {
                case 0:
                    this.txtQnRefType.Text = "เอกสารการปันส่วนงบประมาณ";
                    this.mstrRefType = SysDef.gc_REFTYPE_BUDALLOC;
                    break;
            }
        }

    }
}