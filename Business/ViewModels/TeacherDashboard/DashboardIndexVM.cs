using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;

namespace Business.ViewModels.TeacherDashboard
{
    public class DashboardIndexVM
    {
        public decimal Balance { get; set; }
        public string UserType { get; set; }
        public List<Lesson> Lessons { get; set; }
        public string Email { get; set; }
    }
}
