using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraBars.Docking;
using DevExpress.XtraReports.UserDesigner;

namespace BeSmartMRP
{
    //public partial class frmDXReportDesign : DevExpress.XtraReports.UserDesigner.XRDesignFormEx
    public partial class frmDXReportDesign : DevExpress.XtraReports.UserDesigner.XRDesignRibbonFormEx
    {
        public frmDXReportDesign()
        {
            InitializeComponent();

            this.DesignPanel.SetCommandVisibility(ReportCommand.NewReport, CommandVisibility.None);
            this.DesignPanel.SetCommandVisibility(ReportCommand.NewReportWizard, CommandVisibility.None);
            this.DesignPanel.SetCommandVisibility(ReportCommand.VerbReportWizard, CommandVisibility.None);


            // Access the Group and Sort panel.
            GroupAndSortDockPanel groupSort =
        (GroupAndSortDockPanel)this.DesignDockManager[DesignDockPanelType.GroupAndSort];
            groupSort.Visibility = DockVisibility.AutoHide;

            // Access the Script Errors panel.
            ErrorListDockPanel errorList =
        (ErrorListDockPanel)this.DesignDockManager[DesignDockPanelType.ErrorList];
            errorList.Visibility = DockVisibility.AutoHide;

            // Access the Field List.
            FieldListDockPanel fieldList =
        (FieldListDockPanel)this.DesignDockManager[DesignDockPanelType.FieldList];
            fieldList.ShowNodeToolTips = false;
            fieldList.ShowParametersNode = false;

            // Access the Report Explorer.
            ReportExplorerDockPanel reportExplorer =
        (ReportExplorerDockPanel)this.DesignDockManager[DesignDockPanelType.ReportExplorer];
            reportExplorer.CollapseAll();

            // Access the Property Grid.
            PropertyGridDockPanel propertyGrid =
        (PropertyGridDockPanel)this.DesignDockManager[DesignDockPanelType.PropertyGrid];
            propertyGrid.ShowCategories = false;
            propertyGrid.ShowDescription = false;

        }


        protected override void SaveLayout() { }
        protected override void RestoreLayout() { }

        public void ShowDesignerForm(Form inParent)
        {
            pmShowDesignerForm(inParent);
        }

        private void pmShowDesignerForm(Form inParent)
        {
            this.SetDesktopBounds(inParent.Left, inParent.Top, inParent.Width, inParent.Height);
            this.MinimumSize = inParent.MinimumSize;
            inParent.Visible = false;
            this.ShowDialog();
            inParent.SetDesktopBounds(this.Left, this.Top, this.Width, this.Height);
            inParent.Visible = true;
        }


    }
}
