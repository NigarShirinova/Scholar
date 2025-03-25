using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<UserStudent> UserStudents { get; set; }
        public DbSet<UserTeacher> UserTeachers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .ToTable("AspNetUsers") 
            .HasDiscriminator<string>("Discriminator") 
            .HasValue<User>("User")
            .HasValue<UserStudent>("UserStudent")
            .HasValue<UserTeacher>("UserTeacher");

            modelBuilder.Entity<User>()
                .Property(u => u.Balance)
                .HasColumnType("decimal(18,2)"); 

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");
        }

    }
}
