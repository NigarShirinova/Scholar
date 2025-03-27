using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;

namespace Business.ViewModels.TeacherDashboard
{
    public class LessonVM
    {
        public List<Lesson> Lessons { get; set; }
        public string UserType { get; set; }
        public string UserId { get; set; }
    }
}
