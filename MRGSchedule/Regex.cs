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
        /// 获取两端文本中间的文本列表(所有符合条件的)
        /// </summary>
        public static List<string> GetMiddleContentList(string originStr, string startStr, string endStr, bool includeEdge = false)
        {
            List<string> returnStrs = new List<string>();
            try
            {
                string str = originStr;
                while (true)
                {
                    if (str.Contains(startStr) && str.Contains(endStr))
                    {
                        //只有同时存在才进行
                        string tempStr = GetMiddleContent(str, startStr, endStr, includeEdge);
                        returnStrs.Add(tempStr);//添加到列表
                        int remainStartIndex = str.IndexOf(tempStr) + tempStr.Length;//剩下字符串的起始索引为匹配到的文本索引+文本长度
                        str = str.Substring(remainStartIndex);
                    }
                    else
                    {
                        break;//跳出循环
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return returnStrs;
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
            string regexStr = @">(?<text>[\s\S]*)</";
            string returnStr = "";
            try
            {
                Match mat = System.Text.RegularExpressions.Regex.Match(originStr, regexStr);
                returnStr = mat.Groups["text"].Value;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            return returnStr;
        }
    }
}
