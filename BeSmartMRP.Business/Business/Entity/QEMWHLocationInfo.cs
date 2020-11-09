﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeSmartMRP.Business.Entity
{
    public class QEMWHLocationInfo
    {
        public static string TableName = MapTable.Table.EMWHouseLocation;

        public struct Field
        {

            public static string CorpID = "FCCORP";
            public static string Code = "FCCODE";
            public static string Name = "FCNAME";
            public static string Name2 = "FCNAME2";
            public static string Type = "FCTYPE";
            public static string Status = "FCSTAT";

        }

    }
}