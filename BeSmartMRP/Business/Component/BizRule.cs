
using System;
using System.Collections.Generic;
using System.Text;

using AppUtil;

namespace mBudget.Business.Component
{

    class BizRule
    {

        public static string GetMemData(string inMemData, string inFieldCode)
        {
            int tStart = StringHelper.At(Convert.ToChar(127).ToString() + inFieldCode, inMemData) + inFieldCode.Length + 1;
            int tLen = StringHelper.At(inFieldCode + Convert.ToChar(127).ToString(), inMemData) - tStart;
            return (tLen > 0 ? StringHelper.SubStr(inMemData, tStart, tLen) : "");
        }

        public static string SetMemData(string inSetData, string inFieldCode)
        {
            return (inSetData.Length > 0 ? StringHelper.Chr(127) + inFieldCode + inSetData + inFieldCode + StringHelper.Chr(127) : "");
        }

    
    }

}
