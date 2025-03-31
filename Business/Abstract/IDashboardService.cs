using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ViewModels.TeacherDashboard;
using Common.Entities;

namespace Business.Abstract
{
    public interface IDashboardService
    {
        Task<DashboardIndexVM> GetDashboardDataAsync(string userId, string email);
        Task<BalanceIndexVM> GetBalanceDataAsync(string userId);
        Task<bool> AddLessonAsync(string userId, LessonCreateVM model);
        Task<string> GetUserTypeAsync(string userId);
        Task<List<Lesson>> GetAvailableLessonsAsync();
        Task<bool> BuyLessonAsync(string userId, int lessonId);
        Task<List<Lesson>> GetUsersLessonsAsync(string userId);
    }

}
