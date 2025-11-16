using DataAccess.Identity;
using DataAccess.Models;
using ExaminationSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class QuizSystemContext : IdentityDbContext<ApplicationUser>
    {
        public QuizSystemContext(DbContextOptions<QuizSystemContext> options) : base(options)
        {
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<StudentExam> StudentExams { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configurations can be added here
            modelBuilder.Entity<Exam>()
        .HasOne(e => e.Course)
        .WithMany(c => c.Exam)
        .HasForeignKey(e => e.CourseId)
        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Course>()
               .HasOne(c => c.Instructor)
               .WithMany(u => u.CoursesTaught)
               .HasForeignKey(c => c.InstructorId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CourseAssignment>()
                .HasOne(ca => ca.Student)
                .WithMany(u => u.CourseAssignments)
                .HasForeignKey(ca => ca.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
