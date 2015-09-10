using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MRGSchedule
{
    class Regex
    {
        public static string GetMiddleContent(string originStr, string startStr, string endStr, bool includeEdge = false)
        {
            string returnStr = "";
            try
            {
                int startIndex = originStr.IndexOf(startStr);
                returnStr = originStr.Substring(startIndex + startStr.Length);
                int endIndex = returnStr.IndexOf(endStr);
                Console.WriteLine(endIndex + "|" + returnStr.Length);
                returnStr = returnStr.Substring(0, endIndex);
                if (includeEdge)
                {
                    returnStr = startStr + returnStr + endStr;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            return returnStr;
        }
    }
}
