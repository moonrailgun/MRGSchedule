﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MRGSchedule
{
    class Schedule
    {
        private List<Lesson> lessonList = new List<Lesson>();


        /// <summary>
        /// 导入课程数据（辽工大）
        /// </summary>
        public void ImportSchedule()
        {
            string filePath = @"C:\Users\Chen\Downloads\schedule.doc";
            StreamReader temp = new StreamReader(filePath);
            StreamReader sr = new StreamReader(temp.BaseStream, Encoding.Default);//GBK编码获取数据
            string str = sr.ReadToEnd();
            str = str.Replace("\n", "");

            //辽工大课程表匹配文本
            try
            {
                lessonList.Clear();//清空已有数据

                //获取用户信息
                string body = Regex.GetMiddleContent(str, "<body", @"</body>", true);
                List<string> tableList = Regex.GetMiddleContentList(body, "<tr", "</tr>", true);
                tableList.RemoveAt(0);//删除第一个(星期数据)

                int classNum = 0;
                foreach (string tableData in tableList)
                {
                    classNum++;
                    List<string> tempList = Regex.GetMiddleContentList(tableData, "<span", "</span>", true);
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        tempList[i] = Regex.GetInnerHtml(tempList[i]).Trim();//获取标签页中内容
                    }

                    int weekNum = 0;
                    foreach (string data in tempList)
                    {
                        weekNum++;
                        if (!string.IsNullOrEmpty(data))
                        {
                            //如果数据不为空（有课）添加数据
                            Lesson lesson = new Lesson();
                            lesson.weekNum = (WeekDay)weekNum;
                            lesson.classNum = classNum;

                            //课程信息获取
                            LessonInfo lessonInfo = new LessonInfo();
                            string[] infoData = data.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                            string[] weekInfoData = infoData[2].Split('-');
                            if (weekInfoData[1].EndsWith("单") || weekInfoData[1].EndsWith("双"))
                            {
                                weekInfoData[1] = System.Text.RegularExpressions.Regex.Match(weekInfoData[1], @"\d*").Value;
                                string type = System.Text.RegularExpressions.Regex.Match(weekInfoData[1], @"[^\d]*").Value;
                                if (type == "单")
                                { lessonInfo.type = LessonType.单周; }
                                else if (type == "双")
                                { lessonInfo.type = LessonType.双周; }
                            }
                            lessonInfo.lessonName = infoData[0];
                            lessonInfo.lessonTeacher = infoData[1];
                            lessonInfo.lessonStartWeek = Convert.ToInt32(weekInfoData[0]);
                            lessonInfo.lessonOverWeek = Convert.ToInt32(weekInfoData[1]);
                            lessonInfo.lessonSite = infoData[3];

                            lesson.lessonInfo = lessonInfo;
                            AddLesson(lesson);//添加到课程表
                        }
                    }
                }

                PrintSchedule();
            }
            catch (Exception ex)
            {
                Console.WriteLine("导入数据失败:" + ex.ToString());
            }
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

        /// <summary>
        /// 打印课表到控制台
        /// </summary>
        public void PrintSchedule()
        {
            Console.WriteLine("当前课程表共有{0}节课程：\n星期\t大节\t课程名\t时间", this.lessonList.Count);
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    foreach (Lesson lesson in lessonList)
                    {
                        if (lesson.weekNum == (WeekDay)i && lesson.classNum == j)
                        {
                            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t", lesson.weekNum.ToString(), "第" + lesson.classNum + "大节", lesson.lessonInfo.lessonName, lesson.lessonInfo.lessonStartWeek + "-" + lesson.lessonInfo.lessonOverWeek);
                        }
                    }
                }
            }

        }
    }

    struct Lesson
    {
        public WeekDay weekNum;//星期几
        public int classNum;//第几大节
        public LessonInfo lessonInfo;
    }
    enum WeekDay
    {
        星期日 = 0,
        星期一 = 1,
        星期二 = 2,
        星期三 = 3,
        星期四 = 4,
        星期五 = 5,
        星期六 = 6
    }

}
