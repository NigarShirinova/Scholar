using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using Data.Repositories.Base;

namespace Data.Repositories.Abstract
{
    public interface IDashboardRepository 
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<List<Lesson>> GetLessonsAsync();
        Task<List<Transaction>> GetTransactionsByUserIdAsync(string userId);
        Task CreateLessonAsync(Lesson lesson);
        Task<Lesson> GetLessonByIdAsync(int lessonId);
    }
}
