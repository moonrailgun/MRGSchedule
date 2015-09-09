using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool AddLesson(Lesson lesson)
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
