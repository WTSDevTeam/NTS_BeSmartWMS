using System;
using System.Collections.Generic;
using System.Text;

namespace mBudget.Business.Entity
{
    public class BusinessEnum
    {

        public static string gc_KEEPLOG_TYPE_INSERT = "A";
        public static string gc_KEEPLOG_TYPE_UPDATE = "U";
        public static string gc_KEEPLOG_TYPE_DELETE = "D";
        public static string gc_KEEPLOG_TYPE_LOG_IN = "I";
        public static string gc_KEEPLOG_TYPE_LOG_OUT = "O";

    }

    public enum CoorType
    {
        Customer, Supplier
    }

    public enum WareHouseType
    {
        Normal, WIP, OfficeSupply
    }

    public enum WHLocaType
    {
        Normal
    }

    public enum OrganizationType
    {
        Detail, Group
    }

    public enum AppUserType
    {
        User, Group
    }

    public enum AuthenType
    {
        Visible, AskPassword, CanAccess, CanInsert, CanEdit, CanDelete, CanPrint, CanExport, CanApprove
    }

    public enum AppCodeName
    {
        MF2, MF1, MCM, ERP, CRM, SCM
    }

    /// <summary>
    ///Application Module
    /// </summary>
    public enum AppModule
    {
        MRP2, MRP, MCM, PDC, SFC, MM, SE, PE, IC, AP, AR, CQ, FA, GL
    }

    public enum AppObjType
    {
        Bar, Pad, Item
    }

    public enum AppObjMenuType
    {
        None, Input, Report, Process, Config
    }

    public enum KeepLogType
    {
        Insert, Update, Delete, LogIn, LogOut
    }

    public enum KeepLogChagneType
    {
        None, ChgCode, ChgName
    }

}
