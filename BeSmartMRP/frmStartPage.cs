
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BeSmartMRP.DialogForms;

namespace BeSmartMRP
{
    public partial class frmStartPage : UIHelper.frmBase
    {
        public frmStartPage()
        {
            InitializeComponent();
        }

        private static frmStartPage mInstanse = null;

        public static frmStartPage GetInstanse()
        {
            if (mInstanse == null)
            {
                mInstanse = new frmStartPage();
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

        private void frmStartPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt | e.Control | e.Shift)
                return;

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    //this.Close();
                    this.pmGetCorp();
                    break;
            }
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
            using (DialogForms.frmLogin dlg = new DialogForms.frmLogin())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //App.ofrmMainMenu.statusStrip1.Items[2].Text = "LOGIN : " + App.AppUserName.TrimEnd();
                    this.pmGetCorp();
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void lblLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Windows.Forms.LinkLabel oSender = sender as System.Windows.Forms.LinkLabel;
            this.pmActivateMenu1(oSender.Tag.ToString().ToUpper(), new System.Drawing.Point(oSender.Parent.Left + oSender.Left + 70, oSender.Parent.Top + oSender.Top + 70));
        }

        private void pcbIcon_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.PictureBox oSender = sender as System.Windows.Forms.PictureBox;
            this.pmActivateMenu1(oSender.Tag.ToString().ToUpper(), new System.Drawing.Point(oSender.Parent.Left + oSender.Left + 70, oSender.Parent.Top + oSender.Top + 70));
        }


        public void pmActivateMenu1(string inMenuName, System.Drawing.Point p)
        {
            switch (inMenuName.ToUpper())
            {
                case "MENU1":
                    this.popupMenu1.ShowPopup(p);
                    break;
                case "MENU2":
                    this.popupMenu2.ShowPopup(p);
                    break;
                case "MENU3":
                    this.popupMenu3.ShowPopup(p);
                    break;
            }
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
                case "SETCONFIG":
                    App.SetAppConfig();
                    break;
                case "SETRIGHT":
                    //Permission.frmSetPermission ofrmSetPermission = new Permission.frmSetPermission();
                    //ofrmSetPermission.MdiParent = this.MdiParent;
                    //ofrmSetPermission.Show();
                    //ofrmSetPermission.Activate();
                    break;
                case "EDEPT":
                    //DatabaseForms.frmDept ofrmDept = BeSmartMRP.DatabaseForms.frmDept.GetInstanse();
                    //ofrmDept.MdiParent = this.MdiParent;
                    //ofrmDept.Show();
                    //ofrmDept.Activate();
                    break;
                case "ESECT":
                    //DatabaseForms.frmSect ofrmSect = BeSmartMRP.DatabaseForms.frmSect.GetInstanse();
                    //ofrmSect.MdiParent = this.MdiParent;
                    //ofrmSect.Show();
                    //ofrmSect.Activate();
                    break;

            }
        }

    }
}