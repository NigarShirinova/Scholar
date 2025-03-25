using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Lesson : BaseEntity
    {
        public UserStudent? Student { get; set; }
        public UserTeacher Teacher { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public DateTime LessonDate { get; set; }
 
        public Boolean IsTaken { get; set; }
    }
}
