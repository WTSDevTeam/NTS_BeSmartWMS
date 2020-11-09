using System;

namespace BeSmartMRP.Business.Entity.Info
{

    public class ConnectionInfo
    {
        public ConnectionInfo() { }

        private string mstrServerName = "";
        private string mstrDBMSName = "";
        private string mstrERPServer = "";
        private string mstrERPDBMS = "";

        public string ServerName
        {
            get { return this.mstrServerName; }
            set { this.mstrServerName = value; }
        }

        public string DBMSName
        {
            get { return this.mstrDBMSName; }
            set { this.mstrDBMSName = value; }
        }

        public string ERPServerName
        {
            get { return this.mstrERPServer; }
            set { this.mstrERPServer = value; }
        }

        public string ERPDBMSName
        {
            get { return this.mstrERPDBMS; }
            set { this.mstrERPDBMS = value; }
        }

    }

}
