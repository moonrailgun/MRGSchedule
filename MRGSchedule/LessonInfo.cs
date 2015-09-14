using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRGSchedule
{
    [Serializable]
    class LessonInfo
    {
        public string lessonName = "";
        public int lessonStartWeek = 0;
        public int lessonOverWeek = 0;
        public string lessonTeacher = "";
        public string lessonSite = "";
        public LessonType type = LessonType.正常;
        public List<string> lessonRemarks = new List<string>();
    }

    enum LessonType
    {
        正常 = 0,
        单周 = 1,
        双周 = 2
    }
}
