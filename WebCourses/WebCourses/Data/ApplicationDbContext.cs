using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebCourses.Models;
using WebCourses.ViewModels;

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

        public virtual DbSet<UserTestResult> UserTestResults { get; set; }

        public virtual DbSet<OpenQuestionAnswer> OpenQuestionAnswers { get; set; }

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
          
            modelBuilder.Entity<UserTestResult>()
                .HasOne<User>(utr => utr.User)
                .WithMany(u => u.UserTestResults)
                .HasForeignKey(utr => utr.UserId);

            modelBuilder.Entity<UserTestResult>()
                .HasOne<Test>(utr => utr.Test)
                .WithMany(t => t.UserTestResults)
                .HasForeignKey(utr => utr.TestId);

            modelBuilder.Entity<OpenQuestionAnswer>()
                .HasOne<User>(oqa => oqa.User)
                .WithMany(u => u.OpenQuestionAnswers)
                .HasForeignKey(oqa => oqa.UserId);

            modelBuilder.Entity<OpenQuestionAnswer>()
                .HasOne<Question>(oqa => oqa.Question)
                .WithMany(q => q.OpenQuestionAnswers)
                .HasForeignKey(oqa => oqa.QuestionId);
        }

        public DbSet<AnswerViewModel> AnswerViewModel { get; set; }

        public DbSet<QuestionViewModel> QuestionViewModel { get; set; }
    }
}
