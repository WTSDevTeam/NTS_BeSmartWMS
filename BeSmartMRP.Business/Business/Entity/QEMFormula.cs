using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BeSmartMRP.Business.Entity.Info;
using BeSmartMRP.Business.Agents;

namespace BeSmartMRP.Business.Entity
{
    public class QEMFormula
    {
        public QEMFormula()
		{
			QFormulaCollection x = new QFormulaCollection();
			FormulaInfo y = x.NewRow();
			y.PdStruct = "";
			x.Add(y);
		}
    }
}

namespace BeSmartMRP.Business.Entity.Info
{

    public class QFormulaCollection : System.Collections.CollectionBase
    {

        // Methods
        public QFormulaCollection() { }

        public FormulaInfo NewRow()
        {
            return new FormulaInfo();
        }

        public void Add(FormulaInfo inList)
        {
            this.List.Add(inList);
        }

        //		public void Add(FormulaInfo inTable)
        //		{
        //			this.List.Add(inTable);
        //		} 

        public void Remove(int inTableIndex)
        {
            this.List.RemoveAt(inTableIndex);
        }

        public void Remove(FormulaInfo inTable)
        {
            this.List.Remove(inTable);
        }

        //		public FormulaInfo this[string inTableName]
        //		{
        //			get
        //			{
        //				if (this.parlTableIndex.IndexOf(inTableName) > -1)
        //				{
        //					return (FormulaInfo)this.List[this.parlTableIndex.IndexOf(inTableName)];
        //				}
        //				return null;
        //			}
        //		}

        public FormulaInfo this[int inTableindex]
        {
            get { return (FormulaInfo)this.List[inTableindex]; }
        }

    }

    public class FormulaInfo
    {
        public string PdStruct = "";
        public string RefPdType = "";
        public string PdOrFM = "";
        public decimal Qty = 0;
        public string UM = "";
        public decimal UMQty = 1;
        public string RootFM = "";
        public string ParentFM = "";
        public string ParentQcFM = "";
    }


}
