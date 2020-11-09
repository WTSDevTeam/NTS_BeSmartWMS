using System;

using WS.Data;
using AppUtil;

namespace BeSmartMRP.Business.Entity
{
    public class cEntityBase
    {

		protected string mstrConnectionString = "";
		protected DBMSType mDataBaseReside = DBMSType.MSSQLServer;
		protected string mstrAlias = "EntityObject";
		protected string mstrBrowViewAlias = "Brow_EntityObject";

		public cEntityBase()
		{
			this.pmInitComponent();
		}
		/// <summary>
		///ฐานข้อมูล Application Object
		/// </summary>
		/// <param name="inAlias">ระบุชื่อ Alias ที่จะใช้อ้างอิงใน DataSet</param>
		public cEntityBase(string inAlias)
		{
			this.mstrAlias = inAlias;
			this.mstrBrowViewAlias = this.mstrAlias;
			this.pmInitComponent();
		}
		/// <summary>
		///ฐานข้อมูล Application Object
		/// </summary>
		/// <param name="inConnectionString">ระบุ Connection String ในการ Access Database</param>
		/// <param name="inDataBaseReside">ระบุ Database Type</param>
		public cEntityBase(string inConnectionString, DBMSType inDataBaseReside)
		{
			this.mstrConnectionString = inConnectionString;
			this.mDataBaseReside = inDataBaseReside;
			this.pmInitComponent();
		}
		/// <summary>
		///ฐานข้อมูล Application Object
		/// </summary>
		/// <param name="inAlias">ระบุชื่อ Alias ที่จะใช้อ้างอิงใน DataSet</param>
		/// <param name="inConnectionString">ระบุ Connection String ในการ Access Database</param>
		/// <param name="inDataBaseReside">ระบุ Database Type</param>
		public cEntityBase(string inAlias, string inConnectionString, DBMSType inDataBaseReside)
		{
			this.mstrAlias = inAlias;
			this.mstrBrowViewAlias = this.mstrAlias;
			this.mstrConnectionString = inConnectionString;
			this.mDataBaseReside = inDataBaseReside;
			this.pmInitComponent();
		}

		protected virtual void pmInitComponent() { }

		/// <summary>
		/// Connection String สำหรับการเชื่อมต่อกับ DBMS
		/// </summary>
		public string ConnectionString
		{
			get { return this.mstrConnectionString; }
			set { this.mstrConnectionString = (string)value; }
		}

		/// <summary>
		/// DataBase Reside
		/// </summary>
		public DBMSType DataBaseReside
		{
			get { return this.mDataBaseReside; }
			set { this.mDataBaseReside = value; }
		}

		public string Alias
		{
			get { return this.mstrAlias; }
			set { this.mstrAlias = value; }
		}

		public string BrowserAlias
		{
			get { return this.mstrBrowViewAlias; }
			set { this.mstrBrowViewAlias = value; }
		}

		private string mstrAppID = "";
		public string AppID
		{
			get { return this.mstrAppID; }
			set { this.mstrAppID = value; }
		}

	}
}
