using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgGetXLS2 : UIHelper.frmBase
    {

        public dlgGetXLS2()
        {
            InitializeComponent();
        }

        private string mstrPath = "";

        private void txtDir_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            dlgSaveFile1.ShowDialog(this);
            if (dlgSaveFile1.SelectedPath != string.Empty)
            {
                this.txtDir.Text = dlgSaveFile1.SelectedPath;
                string[] strADir = null;
                string strFormPath = dlgSaveFile1.SelectedPath;
                if (System.IO.Directory.Exists(strFormPath))
                {
                    this.lsbOption.Items.Clear();
                    //strADir = System.IO.Directory.GetFiles(strFormPath,"*.XML");

                    this.LoadRPT(strFormPath);
                }

            }

        }

        private void LoadRPT(string inPath)
        {
            int intLen1 = (inPath).Length;
            this.lsbOption.Items.Clear();
            string[] strADir = System.IO.Directory.GetFiles(inPath, "*.XML");
            for (int i = 0; i < strADir.Length; i++)
            {
                //string strFormName = strADir[i].Substring(intLen1, strADir[i].Length - intLen1);
                string strFormName = strADir[i].Replace(inPath + "\\", "");
                this.lsbOption.Items.Add(strFormName);
            }
            if (this.lsbOption.Items.Count > 0)
            {
                this.lsbOption.SelectedIndex = 0;
            }
        }

        private void lsbOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            pmLoadXML(lsbOption.SelectedItem.ToString());
        }

        public DataSet dtsLoadXML = new DataSet();

        public string Load_DocNo = "";
        public string Load_XMLFileName = "";
        public string Load_XSDFileName = "";

        private void pmLoadXML(string inFileName)
        {
            string strFileName = inFileName.ToUpper().Replace(".XML", "");
            string strFlieXML = this.txtDir.Text + "\\" + strFileName;

            try
            {
                dtsLoadXML.Clear();
                dtsLoadXML.ReadXmlSchema(strFlieXML + ".XSD");
                dtsLoadXML.ReadXml(strFlieXML + ".XML");

                this.Load_XMLFileName = strFlieXML + ".XML";
                this.Load_XSDFileName = strFlieXML + ".XSD";

                this.Load_DocNo = dtsLoadXML.Tables["ScanHeader"].Rows[0]["DocNumber"].ToString().TrimEnd();
                this.grdBrowView.DataSource = dtsLoadXML.Tables["ScanHeader"];
                this.grdBrowView2.DataSource = dtsLoadXML.Tables["ScanItem"];

            }
            catch (Exception ex)
            {
                MessageBox.Show("พบข้อผิดพลาดในการอ่านข้อมูลไฟล์ XML\r\n" + ex.Message);
            }
            finally
            { }
        }

    }
}
