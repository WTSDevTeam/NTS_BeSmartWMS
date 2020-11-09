using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;

namespace BeSmartMRP
{
    public partial class frmDXPreviewReport : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmDXPreviewReport()
        {
            InitializeComponent();
        }

        public void PrintData(DataSet inData, string inRPTFileName)
        {
            //XtraReport1 report = new XtraReport1();

            Report.RPT.xrPSTK_Template01 oprn = new Report.RPT.xrPSTK_Template01();

            string strRPTFileName1 = inRPTFileName;
            if (System.IO.File.Exists(strRPTFileName1))
                oprn.LoadLayout(strRPTFileName1);

            oprn.DataSource = inData;
            oprn.CreateDocument();
            oprn.ShowPrintMarginsWarning = false;
            //frmDXPreviewReport oPreview = new frmDXPreviewReport();

            //oPreview.printControl1.PrintingSystem = oprn.PrintingSystem;

            using (ReportPrintTool printTool = new ReportPrintTool(oprn))
            {
                // Invoke the Print dialog.
                //printTool.PrintDialog();
                try
                {
                    // Send the report to the default printer.
                    printTool.PrintingSystem.ShowPrintStatusDialog = false;
                    printTool.PrintingSystem.ShowMarginsWarning = false;
                    printTool.Print();
                }
                catch (Exception ex)
                { }
                finally { }
                // Send the report to the specified printer.
                //printTool.Print("myPrinter");
            }
        }

    }
}
