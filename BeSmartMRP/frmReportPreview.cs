using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BeSmartMRP
{
    public partial class frmReportPreview : UIHelper.frmBase
    {
        public frmReportPreview()
        {
            InitializeComponent();
        }


        private System.Windows.Forms.Form mParentForm = null;

        public void ClearReportParent()
        {
            if (this.mParentForm != null)
                this.mParentForm = null;
        }

        public void InitReport(object sender)
        {
            this.mParentForm = (System.Windows.Forms.Form)sender;
            //this.crReportViewer.ReportSource = inReport;
        }

        public void InitReport(object sender, bool inPrint)
        {
            this.mParentForm = (System.Windows.Forms.Form)sender;
        }

        private void frmReportPreview_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
                this.mParentForm.Activate();
                //App.SetForegroundWindow(App.mSave_hWnd);
            }
        }
    
    
    
    }
}