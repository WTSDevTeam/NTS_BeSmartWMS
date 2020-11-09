using System;
using System.Collections.Generic;
using System.Text;

namespace mBudget.Business.Agents
{
    public class SecurityAgent
    {

        public bool CheckPermission(Business.Entity.AuthenType inChkType, string inAppUser, string inTaskName)
        {
            return true;
            //return this.pmCheckPermission(inChkType, inAppUser, inTaskName);
        }
    }
}
