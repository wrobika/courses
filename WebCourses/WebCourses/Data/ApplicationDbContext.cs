using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebCourses.Models;

namespace WebCourses.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<CourseUser> CourseUsers { get; set; }

        public virtual DbSet<Test> Tests { get; set; }

        public virtual DbSet<Question> Questions { get; set; }

        public virtual DbSet<Answer> Answers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CourseUser>()
                .HasKey(cu => new { cu.CourseId, cu.UserId });

            modelBuilder.Entity<CourseUser>()
                .HasOne<Course>(cu => cu.Course)
                .WithMany(c => c.CourseUsers)
                .HasForeignKey(cu => cu.CourseId);

            modelBuilder.Entity<CourseUser>()
                .HasOne<User>(cu => cu.User)
                .WithMany(s => s.CourseUsers)
                .HasForeignKey(cu => cu.UserId);

            modelBuilder.Entity<Course>()
                .HasOne<User>(c => c.User)
                .WithMany(u => u.Courses)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Test>()
                .HasOne<Course>(t => t.Course)
                .WithMany(c => c.Tests)
                .HasForeignKey(t => t.CourseId);

            modelBuilder.Entity<Question>()
                .HasOne<Test>(t => t.Test)
                .WithMany(q => q.Questions)
                .HasForeignKey(t => t.TestId);

            modelBuilder.Entity<Answer>()
                .HasOne<Question>(q => q.Question)
                .WithMany(a => a.Answers)
                .HasForeignKey(q => q.QuestionId);
        }
    }
}
