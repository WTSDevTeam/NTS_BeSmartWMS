using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BeSmartMRP.DatabaseForms.Common
{
    public partial class dlgGetOPTime : UIHelper.frmBase
    {
        public dlgGetOPTime()
        {
            InitializeComponent();
            BeSmartMRP.UIHelper.UIBase.SetDefaultChildAppreance(this);
        }

        public void SetTime(string inKey, decimal inHour, decimal inMin, decimal inSec)
        {
            switch (inKey.ToUpper())
            { 
                case "QUEUE":
                    this.txtQueue_Hr.Value = inHour;
                    this.txtQueue_Min.Value = inMin;
                    this.txtQueue_Sec.Value = inSec;
                    break;
                case "SETUP":
                    this.txtSetUp_Hr.Value = inHour;
                    this.txtSetUp_Min.Value = inMin;
                    this.txtSetUp_Sec.Value = inSec;
                    break;
                case "PROCESS":
                    this.txtProcess_Hr.Value = inHour;
                    this.txtProcess_Min.Value = inMin;
                    this.txtProcess_Sec.Value = inSec;
                    break;
                case "TEAR":
                    this.txtTear_Hr.Value = inHour;
                    this.txtTear_Min.Value = inMin;
                    this.txtTear_Sec.Value = inSec;
                    break;
                case "WAIT":
                    this.txtWait_Hr.Value = inHour;
                    this.txtWait_Min.Value = inMin;
                    this.txtWait_Sec.Value = inSec;
                    break;
                case "MOVE":
                    this.txtMove_Hr.Value = inHour;
                    this.txtMove_Min.Value = inMin;
                    this.txtMove_Sec.Value = inSec;
                    break;
            }

        }

        public void GetTime(string inKey, ref decimal inHour, ref decimal inMin, ref decimal inSec)
        {
            switch (inKey.ToUpper())
            {
                case "QUEUE":
                    inHour = this.txtQueue_Hr.Value;
                    inMin = this.txtQueue_Min.Value;
                    inSec = this.txtQueue_Sec.Value;
                    break;
                case "SETUP":
                    inHour = this.txtSetUp_Hr.Value;
                    inMin = this.txtSetUp_Min.Value;
                    inSec = this.txtSetUp_Sec.Value;
                    break;
                case "PROCESS":
                    inHour = this.txtProcess_Hr.Value;
                    inMin = this.txtProcess_Min.Value;
                    inSec = this.txtProcess_Sec.Value;
                    break;
                case "TEAR":
                    inHour = this.txtTear_Hr.Value;
                    inMin = this.txtTear_Min.Value;
                    inSec = this.txtTear_Sec.Value;
                    break;
                case "WAIT":
                    inHour = this.txtWait_Hr.Value;
                    inMin = this.txtWait_Min.Value;
                    inSec = this.txtWait_Sec.Value;
                    break;
                case "MOVE":
                    inHour = this.txtMove_Hr.Value;
                    inMin = this.txtMove_Min.Value;
                    inSec = this.txtMove_Sec.Value;
                    break;
            }

        }

    
    }
}
