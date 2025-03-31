using System.Security.Claims;
using Business.Abstract;
using Business.ViewModels.TeacherDashboard;
using Common.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;
        private readonly IDashboardRepository dashboardRepository;
        public DashboardService(AppDbContext context, IDashboardRepository dashboardRepository)
        {
            _context = context;
        }

        public async Task<DashboardIndexVM> GetDashboardDataAsync(string userId, string email)
        {
            var user = await _context.Users.FindAsync(userId);
            var lessons = await _context.Lessons
                .Include(l => l.Student)
                .Include(l => l.Teacher)
                .ToListAsync();

            var userType = await GetUserTypeAsync(userId);
            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.DateTime)
                .ToListAsync();

            decimal balance = CalculateBalance(transactions);

            return new DashboardIndexVM
            {
                Balance = balance,
                UserType = userType,
                Lessons = lessons,
                Email = email
            };
        }

        public async Task<BalanceIndexVM> GetBalanceDataAsync(string userId)
        {
            var userType = await GetUserTypeAsync(userId);
            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.DateTime)
                .ToListAsync();

            decimal balance = CalculateBalance(transactions);

            return new BalanceIndexVM
            {
                TransactionList = transactions,
                Balance = balance,
                UserType = userType
            };
        }

        public async Task<bool> AddLessonAsync(string userId, LessonCreateVM model)
        {
            var teacher = await _context.Users.OfType<UserTeacher>().FirstOrDefaultAsync(u => u.Id == userId);
            if (teacher == null) return false;

            if(model.LessonDate <= DateTime.UtcNow) return false;

            var lesson = new Lesson
            {
                Name = model.Name,
                Price = model.Price,
                LessonDate = DateTime.SpecifyKind(model.LessonDate, DateTimeKind.Utc),
                Teacher = teacher,
                IsTaken = false
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Lesson>> GetAvailableLessonsAsync()
        {
            return await _context.Lessons
                .Include(l => l.Teacher)
                .Include(l => l.Student)
                .ToListAsync();
        }

        public async Task<List<Lesson>> GetUsersLessonsAsync(string userId)
        {
            return await _context.Lessons
                .Include(l => l.Teacher)
                .Include(l => l.Student)
                .Where(l => l.Teacher.Id == userId || l.Student.Id == userId)
                .ToListAsync();
        }




        public async Task<bool> BuyLessonAsync(string userId, int lessonId)
        {
            var student = await _context.Users.OfType<UserStudent>().FirstOrDefaultAsync(u => u.Id == userId);
            if (student == null) return false;

            var lesson = await _context.Lessons.Include(l => l.Teacher).FirstOrDefaultAsync(l => l.Id == lessonId);
            if (lesson == null || lesson.IsTaken || lesson.Price > student.Balance) return false;

            lesson.IsTaken = true;
            lesson.Student = student;
            student.Balance -= lesson.Price;
            student.Transactions.Add(new Transaction { UserId = student.Id, Amount = lesson.Price, TransactionType = Common.Constants.TransactionType.Withdrawal });
            lesson.Teacher.Balance += lesson.Price;
            lesson.Teacher.Transactions.Add(new Transaction { UserId = student.Id, Amount = lesson.Price, TransactionType = Common.Constants.TransactionType.Income });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GetUserTypeAsync(string userId)
        {
            var discriminator = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => EF.Property<string>(u, "Discriminator"))
                .FirstOrDefaultAsync();

            return discriminator switch
            {
                "UserTeacher" => "Teacher",
                "UserStudent" => "Student",
                _ => ""
            };
        }

        private decimal CalculateBalance(List<Transaction> transactions)
        {
            decimal balance = 0;
            foreach (var transaction in transactions)
            {
                balance += transaction.TransactionType == Common.Constants.TransactionType.Income ? transaction.Amount : -transaction.Amount;
            }
            return balance;
        }

       
    }
}