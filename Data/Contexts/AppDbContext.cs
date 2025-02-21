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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .ToTable("AspNetUsers") 
            .HasDiscriminator<string>("Discriminator") 
            .HasValue<User>("User")
            .HasValue<UserStudent>("UserStudent")
            .HasValue<UserTeacher>("UserTeacher");
        }

    }
}
