using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace MRGSchedule
{
    struct Lesson
    {
        public WeekDay weekNum;//星期几
        public int classNum;//第几大节
        public LessonInfo lesson;
    }
    enum WeekDay
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }

    class Schedule
    {
        private List<Lesson> lessonList = new List<Lesson>();

        public void ImportSchedule()
        {
            string filePath = @"C:\Users\Chen\Downloads\schedule.doc";
            StreamReader temp = new StreamReader(filePath);
            StreamReader sr = new StreamReader(temp.BaseStream, Encoding.Default);//GBK编码获取数据
            string str = sr.ReadToEnd();
            str = str.Replace("\n","");

            //辽工大课程表匹配文本
            //string regexStr = @"^<p class=MsoNormal[\w\W]*<span[\w\W]*>(?<stuInfo>[\w\W]+)</span>";
            string regexStr = @"<p class=MsoNormal[^>]*>[^<]*<b><span[^>*]>(?<stuInfo>[^<]+)";
            Match mat = Regex.Match(str, regexStr);
            string a = mat.Groups["stuInfo"].Value;
        }

        /// <summary>
        /// 添加课程到列表
        /// </summary>
        private bool AddLesson(Lesson lesson)
        {
            foreach (Lesson l in lessonList)
            {
                if (l.classNum == lesson.classNum && l.weekNum == lesson.weekNum)
                {
                    return false;
                }
            }

            lessonList.Add(lesson);
            return true;
        }
    }
}
