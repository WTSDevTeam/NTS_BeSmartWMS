using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeSmartMRP.UIHelper
{

    public interface IfrmStockBase
    {
        //TODO: เรื่อง Location
        decimal GetStockBalance(string inProd, string inWHouse, string inLot);
        void QueryTemLot(string inProd, string inWHouse);
        bool ValidateQty(bool inSetError, ref string ioErrorMsg);

    }
}
