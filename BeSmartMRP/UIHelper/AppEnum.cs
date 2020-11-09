using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.UIHelper
{

    public class AppEnum
    {

        public static WsToolBar GetToolBarEnum(string inKey)
        {
            return (WsToolBar)System.Enum.Parse(Type.GetType(SysDef.gc_ToolBar_NameSpace), inKey, true);
        }

        public static AppLocale GetLocaleEnum(string inKey)
        {
            if (inKey != string.Empty)
            {
                return (AppLocale)System.Enum.Parse(Type.GetType(SysDef.gc_Locale_NameSpace), inKey, true);
            }
            else
                return AppLocale.en_US;
        }

    }

    public enum WsToolBar
    {
        Enter, Insert, Update, Delete, Print, Cancel, Search, Close, Undo, Save, Refresh, Filter, Exit, RowInsert, RowDelete, RowMoveUp, RowMoveDown, Export
    }

    public enum FormActiveMode
    {
        Edit, PopUp, Report
    }

    public enum AppFormState
    {
        Edit = 0,
        Insert = 1
    }

    public enum AppLocale
    {
        th_TH, en_US
    }

    public enum DocEditType
    {
        Insert , Edit, Delete, Cancel , Close
    }

    public enum ChangeLogType
    {
        EditCode = 0,
        Cancel = 1,
        Delete = 2
    }

    //public enum PostLevel
    //{
    //    Top, Middle, Operator
    //}

    //public enum BudgetStep
    //{
    //    Prepare, Approve, Post, Revise
    //}

    //public enum ApproveStep
    //{
    //    Wait, Approve, Reject, Post, Revise, Pass
    //}

    //public enum MfgResourceType
    //{
    //    Machine, Tool
    //}

    //public enum DocumentType
    //{
    //    AC, A1, A2, A3, A4, A5, A6, A7, A8, A9, AA,
    //    AB, B1, B2,
    //    B3, P1, P2, P3, P4, P5,
    //    P6, R1, R2, R3, R4, R5,
    //    R6, T1, T2, T3
    //}

}
