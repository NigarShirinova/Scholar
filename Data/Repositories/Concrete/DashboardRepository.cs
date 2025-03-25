using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Concrete
{
    public class DashboardRepository : BaseRepository<Lesson>, IDashboardRepository
    {
        private readonly AppDbContext _context;
        public DashboardRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<List<Lesson>> GetLessonsAsync()
        {
            return await _context.Lessons.Include(l => l.Student).Include(l => l.Teacher).ToListAsync();
        }

        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(string userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.DateTime)
                .ToListAsync();
        }

        public async Task CreateLessonAsync(Lesson lesson)
        {
            await CreateAsync(lesson); 
        }

        public async Task<Lesson> GetLessonByIdAsync(int lessonId)
        {
            return await GetAsync(lessonId); 
        }
    }

}
