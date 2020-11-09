using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BeSmartMRP.UIHelper
{

    public interface IfrmDBBase
    {
        //IfrmDBBase(FormActiveMode inMode);
        bool ValidateField(string inSearchStr, string inOrderBy, bool inForcePopUp);

        bool PopUpResult
        {
            get;
        }
        
        DataRow RetrieveValue();

    }

}
