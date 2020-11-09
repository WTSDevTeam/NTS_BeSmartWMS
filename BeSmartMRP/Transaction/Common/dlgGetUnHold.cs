using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WS.Data;
using WS.Data.Agents;
using AppUtil;

using BeSmartMRP.Business.Entity;
using BeSmartMRP.UIHelper;
using BeSmartMRP.Business.Agents;
namespace BeSmartMRP.Transaction.Common
{
    public partial class dlgGetUnHold : UIHelper.frmBase
    {
        public dlgGetUnHold()
        {
            InitializeComponent();
        }

        private DataSet dtsDataEnv = new DataSet();
        private string mstrBrowViewAlias="QBrow_Hold";
        private string mstrEditRowID = "";

        public void SetBrowView(DataSet inDataEnv)
        {
            this.dtsDataEnv = inDataEnv;
            this.pmInitGridProp();

        }

        public string EditRowID
        {
            get { return this.mstrEditRowID; }
        }

        private void pmInitGridProp()
        {

            System.Data.DataView dvBrow = this.dtsDataEnv.Tables[this.mstrBrowViewAlias].DefaultView;
            this.grdBrowView.DataSource = this.dtsDataEnv.Tables[this.mstrBrowViewAlias];

            for (int intCnt = 0; intCnt < this.gridView1.Columns.Count; intCnt++)
            {
                this.gridView1.Columns[intCnt].Visible = false;
            }

            int i = 0;
            //this.gridView1.Columns["FCSTAT"].VisibleIndex = i++;
            //this.gridView1.Columns["FCISHOLD"].VisibleIndex = i++;
            //this.gridView1.Columns["FCATSTEP"].VisibleIndex = i++;
            //this.gridView1.Columns["FCSTEP"].VisibleIndex = i++;
            //FTDATETIME
            this.gridView1.Columns["FTDATETIME"].VisibleIndex = i++;
            this.gridView1.Columns["FCCODE"].VisibleIndex = i++;
            this.gridView1.Columns["FCREFNO"].VisibleIndex = i++;
            this.gridView1.Columns["FDDATE"].VisibleIndex = i++;
            this.gridView1.Columns["CQNCOOR"].VisibleIndex = i++;
            //this.gridView1.Columns["NAMT"].VisibleIndex = i++;
            //this.gridView1.Columns["FCDEBTCODE"].VisibleIndex = i++;
            //this.gridView1.Columns["CREFTO"].VisibleIndex = i++;

            this.gridView1.Columns["FTDATETIME"].Caption = "วัน/เวลา";
            this.gridView1.Columns["FCISHOLD"].Caption = "H";
            this.gridView1.Columns["FCSTAT"].Caption = "C";
            this.gridView1.Columns["FCATSTEP"].Caption = "P";
            this.gridView1.Columns["FCSTEP"].Caption = "";
            this.gridView1.Columns["FCCODE"].Caption = "เลขที่ภายใน";
            this.gridView1.Columns["FCREFNO"].Caption = "เลขที่อ้างอิง";
            this.gridView1.Columns["FDDATE"].Caption = "วันที่";
            this.gridView1.Columns["CQNCOOR"].Caption = "ชื่อย่อลูกค้า";
            this.gridView1.Columns["FCSIGN"].Caption = "สกุลเงิน";
            this.gridView1.Columns["NAMT"].Caption = "มูลค่า";
            this.gridView1.Columns["CREFTO"].Caption = "";
            this.gridView1.Columns["FCDEBTCODE"].Caption = "เลขที่ใบกำกับ";

            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.Columns["FCSTAT"].Width = 30;
            this.gridView1.Columns["FCISHOLD"].Width = 30;

            //this.gridView1.Columns["FCSTAT"].AppearanceHeader.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //this.gridView1.Columns["FCSTAT"].AppearanceCell.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            this.gridView1.Columns["FCATSTEP"].Width = 30;
            //this.gridView1.Columns["FCATSTEP"].AppearanceHeader.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //this.gridView1.Columns["FCATSTEP"].AppearanceCell.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            this.gridView1.Columns["FCSTEP"].Width = 30;
            //this.gridView1.Columns["FCSTEP"].AppearanceHeader.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            //this.gridView1.Columns["FCSTEP"].AppearanceCell.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            this.gridView1.Columns["FCCODE"].Width = 80;
            this.gridView1.Columns["FCREFNO"].Width = 80;
            this.gridView1.Columns["FTDATETIME"].Width = 100;
            this.gridView1.Columns["FDDATE"].Width = 80;
            this.gridView1.Columns["NAMT"].Width = 100;
            this.gridView1.Columns["CREFTO"].Width = 10;

            this.gridView1.Columns["NAMT"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["NAMT"].DisplayFormat.FormatString = "#,###,###.00";

            this.gridView1.Columns["FDDATE"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["FDDATE"].DisplayFormat.FormatString = "dd/MM/yy";

            this.gridView1.Columns["FTDATETIME"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridView1.Columns["FTDATETIME"].DisplayFormat.FormatString = "dd/MM/yy HH:mm:ss";

            this.pmCalcColWidth();
        }

        private void pmCalcColWidth()
        {

            int intColWidth = this.gridView1.Columns["FCSTAT"].Width
                //this.gridView1.Columns["FCATSTEP"].Width
                // this.gridView1.Columns["FCSTEP"].Width
                                    + this.gridView1.Columns["FCCODE"].Width
                                    + this.gridView1.Columns["FCREFNO"].Width
                                    + this.gridView1.Columns["FDDATE"].Width
                                    + this.gridView1.Columns["FCDEBTCODE"].Width
                                    + this.gridView1.Columns["NAMT"].Width
                                    + this.gridView1.Columns["CREFTO"].Width;

            //this.gridView1.Columns["FCDEBTCODE"].VisibleIndex = i++;

            int intNewWidth = this.grdBrowView.Width - intColWidth - 35;
            this.gridView1.Columns["CQNCOOR"].Width = (intNewWidth > 0 ? intNewWidth : 70);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DataRow dtrBrow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dtrBrow != null)
            {

                this.mstrEditRowID = dtrBrow["fcSkid"].ToString();
                this.DialogResult = DialogResult.OK;
            }
        }

    }
}
