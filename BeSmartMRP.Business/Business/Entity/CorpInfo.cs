﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{

    public class CorpInfo : BaseInfo
    {
        public CorpInfo() { }

        /// <summary>
        /// รหัสบริษัท
        /// </summary>
        public string Code = "";

        /// <summary>
        /// ชื่อบริษัท
        /// </summary>
        public string Name = "";

        /// <summary>
        /// ชื่อบริษัทภาษา 2
        /// </summary>
        public string Name2 = "";

        /// <summary>
        /// วันที่เริ่มใช้ระบบ
        /// </summary>
        public DateTime StartAppDate = DateTime.MinValue;

        /// <summary>
        /// ที่อยู่บริษัทบรรทัดที่ 1
        /// </summary>
        public string Address1 = "";

        /// <summary>
        /// ที่อยู่บริษัทบรรทัดที่ 2
        /// </summary>
        public string Address2 = "";

        /// <summary>
        /// ที่อยู่บริษัทภาษา 2 บรรทัดที่ 1
        /// </summary>
        public string Address12 = "";

        /// <summary>
        /// ที่อยู่บริษัทภาษา 2 บรรทัดที่ 2
        /// </summary>
        public string Address22 = "";

        /// <summary>
        /// เบอร์โทรศัพท์
        /// </summary>
        public string TelNo = "";

        /// <summary>
        /// เบอร์ Fax
        /// </summary>
        public string FaxNo = "";

        /// <summary>
        /// เลขประจำตัวผู้เสียภาษี
        /// </summary>
        public string TaxID = "";

        /// <summary>
        /// Summary description for XRIC03.
        /// </summary>
        public string DefaultSectID = "";

        /// <summary>
        /// Summary description for XRIC03.
        /// </summary>
        public string DefaultDeptID = "";

        /// <summary>
        /// Summary description for XRIC03.
        /// </summary>
        public string DefaultProjectID = "";

        /// <summary>
        /// Summary description for XRIC03.
        /// </summary>
        public string DefaultJobID = "";
        public string DefaultCurrencyID = "";
        public decimal DefaultExchangeRate = 1;

        public string SaleVATType = "";
        public string SaleVATIsOut = "";

        public string VATType = "";
        public decimal VATRate = 0;

        public string SCtrlStock = "";
        public string SStCtrlStock = "2";

        public string AmtFormatString = "";
        public string QtyFormatString = "";
        public string PriceFormatString = "";
        public string MMQtyFormatString = "";

        public int RoundAmtAt = 2;
        public int RoundQtyAt = 2;
        public int RoundPriceAt = 2;

        public string ShowFormulaCompo = "1";

        public string CorpChar = "";

        public string CostMethod_Goods = "A";
        public string CostMethod_Rawmat = "A";

    }
}
