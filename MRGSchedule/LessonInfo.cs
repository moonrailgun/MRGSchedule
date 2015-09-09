using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRGSchedule
{
    class LessonInfo
    {
        public string lessonName = "";
        public int lessonStartWeek = 0;
        public int lessonOverWeek = 0;
        public string lessonTeacher = "";
        public string lessonSite = "";
        public List<string> lessonRemarks = new List<string>();
    }
}
