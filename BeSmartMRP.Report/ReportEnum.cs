using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeSmartMRP.Report
{
    public class ReportEnum
    {
        public ReportEnum() { }

        public string[] aThaiSMonthName = new string[] { "ม.ค.", "ก.พ.", "มี.ค.", "เม.ษ.", "พ.ค.", "มิ.ย.", "ก.ค.", "ส.ค.", "ก.ย.", "ต.ค.", "พ.ย.", "ธ.ค." };
        public string[] aEngSMonthName = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

    }

    public enum PrintFieldType
    {
        GLRefField, RefProdField, OrderHField, OrderIField
    }

}
