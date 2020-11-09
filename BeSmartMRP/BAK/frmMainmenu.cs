
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using mBudget.DialogForms;
using mBudget.Business.Entity;
using mBudget.UIHelper;
using mBudget.Business.Agents;

namespace mBudget
{
    public partial class frmMainmenu : Form
    {

        public frmMainmenu()
        {
            InitializeComponent();
        }

        private void barMainEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Tag != null)
            {
                this.pmActivateMenu(e.Item.Tag.ToString());
            }
        }

		private void pmActivateMenu(string inMenuName)
		{
            switch (inMenuName.ToUpper())
            {
                case "START":
                    frmStartPage ofrmStartPage = frmStartPage.GetInstanse();
                    ofrmStartPage.MdiParent = this;
                    ofrmStartPage.Show();
                    ofrmStartPage.Activate();
                    break;
                case "EXIT":
                    this.Close();
                    break;
                case "SETCONFIG":
                    App.SetAppConfig();
                    break;

                case "SETRIGHT":
                    Permission.frmSetPermission ofrmSetPermission = new Permission.frmSetPermission();
                    ofrmSetPermission.MdiParent = this;
                    ofrmSetPermission.Show();
                    ofrmSetPermission.Activate();
                    break;

                case "EBGBOOK":
                    DatabaseForms.frmWsBook ofrmWsBook = mBudget.DatabaseForms.frmWsBook.GetInstanse();
                    ofrmWsBook.MdiParent = this;
                    ofrmWsBook.Show();
                    ofrmWsBook.Activate();
                    break;

                case "EBUDTYPE":
                    DatabaseForms.frmBudType ofrmRetVal = mBudget.DatabaseForms.frmBudType.GetInstanse();
                    ofrmRetVal.MdiParent = this;
                    ofrmRetVal.Show();
                    ofrmRetVal.Activate();
                    break;

                case "EBUDCH":
                    DatabaseForms.frmBudChart ofrmBudChart = mBudget.DatabaseForms.frmBudChart.GetInstanse();
                    ofrmBudChart.MdiParent = this;
                    ofrmBudChart.Show();
                    ofrmBudChart.Activate();
                    break;

                case "EBGALLOCATE":
                    DatabaseForms.frmBudAllocate ofrmBudAllocate = mBudget.DatabaseForms.frmBudAllocate.GetInstanse();

                    if (ofrmBudAllocate != null)
                    {
                        ofrmBudAllocate.MdiParent = this;
                        ofrmBudAllocate.Show();
                        ofrmBudAllocate.Activate();
                    }
                    break;

                case "EDEPT":
                    DatabaseForms.frmDept ofrmDept = mBudget.DatabaseForms.frmDept.GetInstanse();
                    ofrmDept.MdiParent = this;
                    ofrmDept.Show();
                    ofrmDept.Activate();
                    break;
                case "ESECT":
                    DatabaseForms.frmSect ofrmSect = mBudget.DatabaseForms.frmSect.GetInstanse();
                    ofrmSect.MdiParent = this;
                    ofrmSect.Show();
                    ofrmSect.Activate();
                    break;

            } 
        }

        protected override void OnClosing(CancelEventArgs ce)
        {
            if (this.pmClosingApp())
            {
                base.OnClosing(ce);
                Application.Exit();
            }
            else
                ce.Cancel = true;
        }

        private bool pmClosingApp()
        {
            return (MessageBox.Show(this, "ต้องการที่จะออกจากการใช้งานโปรแกรม ?", "Application confirm message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes);
        }

        private void pmGetCorp()
        {
            string strModule = "";

            dlgGetCorp ofrmGetCorp = new dlgGetCorp();
            ofrmGetCorp.ShowDialog();
            if (ofrmGetCorp.DialogResult == DialogResult.OK)
            {
                App.SetActiveCorp(ofrmGetCorp.RetrieveValue());
                //this.MainStatusBar.Panels["CORPNAME"].Text = App.ActiveCorp.Name;
                //this.Text = App.ActiveCorp.Name.TrimEnd() + " " + strModule + " for MS SQL Server Version date [" + App.AppVersion + "]";
            }
            else
            {
                this.pmGetLogin();
            }
        }

        private void pmGetLogin()
        {
            using (frmLogin dlg = new frmLogin())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.pmGetCorp();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void frmMainmenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.MdiChildren.Length > 0 | e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    //this.Close();
                    this.pmGetCorp();
                    break;
            }
        }

        private void frmMainmenu_Load(object sender, EventArgs e)
        {
            frmStartPage ofrmStartPage = frmStartPage.GetInstanse();
            ofrmStartPage.MdiParent = this;
            ofrmStartPage.Show();

            dlgGetCorp ofrmGetCorp = new dlgGetCorp();
            ofrmGetCorp.ShowDialog();
            if (ofrmGetCorp.DialogResult == DialogResult.OK)
            {
                App.SetActiveCorp(ofrmGetCorp.RetrieveValue());
                //this.MainStatusBar.Panels["CORPNAME"].Text = App.ActiveCorp.Name;
                //this.Text = App.ActiveCorp.Name.TrimEnd() + " " + strModule + " for MS SQL Server Version date [" + App.AppVersion + "]";
            }
            else
            {
                Application.Exit();
            }
        }

    }
}
