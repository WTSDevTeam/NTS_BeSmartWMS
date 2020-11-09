using System;
using System.Collections.Generic;
using System.Text;

namespace BeSmartMRP.Report.Agents
{
    public class N2Alpha
    {
        private static string[] stNumber = new string[10] { "", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" };
        private static string[] stBase = new string[6] { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน" };
        private static string stText = null;
        private static long minLastBase = -1;

        public N2Alpha()
        {
        }

        public static string ConvNumberToText(double argNumber)
        {
            stText = null;
            long real = (long)argNumber;
            double fraction = argNumber - (double)real;
            Convt(real, 0);
            if (fraction == 0.0)
                return stText + "บาทถ้วน";
            string temp = stText;
            stText = null;
            fraction = fraction + 0.005;
            string strfr = fraction.ToString();
            strfr = strfr.Remove(0, 2);
            strfr = strfr.Remove(2, strfr.Length - 2);
            long fr = Convert.ToInt64(strfr);

            Convt(fr, 0);
            return temp + "บาท" + stText + "สตางค์";
        }

        private static string Convt(long argNumber, int CurrentBase)
        {
            if (argNumber < 10 && argNumber >= 0)
            {
                if (argNumber % 10 == 1 && CurrentBase % 6 == 1)
                    stText += stBase[CurrentBase % 6];
                else if (argNumber % 10 == 2 && CurrentBase % 6 == 1)
                    stText += "ยี่" + stBase[CurrentBase % 6];
                else
                    stText += stNumber[argNumber % 10] + stBase[CurrentBase % 6];
            }
            else if (argNumber >= 10)
                Convt((argNumber / 10), CurrentBase + 1);

            if (minLastBase != 0 && argNumber % 10 == 1 && CurrentBase % 6 == 0 && argNumber > 10)
                stText += "เอ็ด";

            else if (argNumber > 10)
            {
                if (argNumber % 10 == 1 && CurrentBase % 6 == 1)
                    stText += stBase[CurrentBase % 6];
                else if (argNumber % 10 == 2 && CurrentBase % 6 == 1)
                    stText += "ยี่" + stBase[CurrentBase % 6];
                else if (argNumber % 10 != 0)
                    stText += stNumber[argNumber % 10] + stBase[CurrentBase % 6];

            }
            if (CurrentBase % 6 == 0 && CurrentBase != 0)
            {
                for (int i = 1; i <= (int)(CurrentBase / 6); i++)
                    stText += "ล้าน";
                return stText;
            }
            minLastBase = argNumber % 10;
            return stText;
        }

    }
}
