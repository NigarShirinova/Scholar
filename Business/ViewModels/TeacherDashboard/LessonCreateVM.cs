using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Business.ViewModels.TeacherDashboard
{
    public class LessonCreateVM
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime LessonDate { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
