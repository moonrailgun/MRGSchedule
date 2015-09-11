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
        /// <summary>
        /// 获取两端文本中间的文本
        /// </summary>
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

        /// <summary>
        /// 根据正则表达式获取所有匹配的数据
        /// </summary>
        public static List<string> GetMatchedGroup(string originStr, string pattern)
        {
            List<string> returnStrs = new List<string>();
            try
            {
                MatchCollection mats = System.Text.RegularExpressions.Regex.Matches(originStr, pattern);
                foreach (Match mat in mats)
                {
                    returnStrs.Add(mat.Value.ToString());
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            return returnStrs;
        }

        /// <summary>
        /// 获取标签页中间的数据
        /// </summary>
        public static string GetInnerHtml(string originStr)
        {
            string regexStr = @">([^<]*)</";
            string returnStr = "";
            try
            {
                Match mat = System.Text.RegularExpressions.Regex.Match(originStr, regexStr);
                returnStr = mat.Groups[0].Value.ToString();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            return returnStr;
        }
    }
}
