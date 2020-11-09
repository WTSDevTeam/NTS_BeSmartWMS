using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Business.Entity
{

    public abstract class BaseInfo : System.MarshalByRefObject
    {

        #region Constructor
        public BaseInfo() { }
        #endregion

        #region Private Member
        private string mstrRowID = "";
        private string mstrCreateApp = "";
        private string mstrUpdateApp = "";

        private DateTime mdttCreateDateTime = DateTime.Now;
        private DateTime mdttLastAccessDateTime = DateTime.Now;
        private DateTime mdttLastModifyDateTime = DateTime.Now;
        #endregion

        #region Public Member
        /// <summary>
        /// ระบุ Object RowID.
        /// </summary>
        public string RowID
        {
            get { return this.mstrRowID; }
            set { this.mstrRowID = value; }
        }

        /// <summary>
        /// ระบุ App ที่สร้าง
        /// </summary>
        public string CreateApp
        {
            get { return this.mstrCreateApp; }
            set { this.mstrCreateApp = value; }
        }

        /// <summary>
        /// ระบุ App ที่แก้ไขล่าสุด
        /// </summary>
        public string UpdateApp
        {
            get { return this.mstrUpdateApp; }
            set { this.mstrUpdateApp = value; }
        }

        /// <summary>
        /// Created Date time.
        /// </summary>
        public DateTime CreateDateTime
        {
            get { return this.mdttCreateDateTime; }
            set { this.mdttCreateDateTime = Convert.ToDateTime(value); }
        }

        /// <summary>
        /// Last Modify Date time.
        /// </summary>
        public DateTime LastModifyDateTime
        {
            get { return this.mdttLastModifyDateTime; }
            set { this.mdttLastModifyDateTime = Convert.ToDateTime(value); }
        }

        /// <summary>
        /// Last Modify Date time.
        /// </summary>
        public DateTime LastAccessDateTime
        {
            get { return this.mdttLastAccessDateTime; }
            set { this.mdttLastAccessDateTime = Convert.ToDateTime(value); }
        }
        #endregion

    }

    public class ShareEntity
    {
        public ShareEntity() { }

        private string mstrTag = "";
        private string mstrCode = "";
        private string mstrName = "";
        private string mstrName2 = "";
        private string mstrSName = "";
        private string mstrSName2 = "";

        private string mstrKey1 = "";
        private string mstrKey2 = "";
        private string mstrKey3 = "";
        private string mstrKey4 = "";

        public string Tag
        {
            get { return this.mstrTag; }
            set { this.mstrTag = value; }
        }

        public string Code
        {
            get { return this.mstrCode; }
            set { this.mstrCode = value; }
        }

        public string Name
        {
            get { return this.mstrName; }
            set { this.mstrName = value; }
        }

        public string Name2
        {
            get { return this.mstrName2; }
            set { this.mstrName2 = value; }
        }

        public string ShortName
        {
            get { return this.mstrSName; }
            set { this.mstrSName = value; }
        }

        public string ShortName2
        {
            get { return this.mstrSName2; }
            set { this.mstrSName2 = value; }
        }

        public string Key1
        {
            get { return this.mstrKey1; }
            set { this.mstrKey1 = value; }
        }

        public string Key2
        {
            get { return this.mstrKey2; }
            set { this.mstrKey2 = value; }
        }

        public string Key3
        {
            get { return this.mstrKey3; }
            set { this.mstrKey3 = value; }
        }

        public string Key4
        {
            get { return this.mstrKey4; }
            set { this.mstrKey4 = value; }
        }

    }

}
