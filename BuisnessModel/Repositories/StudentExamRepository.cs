using BuisnessModel.Interfaces;
using DataAccess.Context;
using DataAccess.Models;
using ExaminationSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BuisnessModel.Repositories
{
    public class StudentExamRepository : GeneralRepository<StudentExam>, IStudentExamRepository
    {
        QuizSystemContext QuizSystemContext;
        public StudentExamRepository(QuizSystemContext context) : base(context)
        {
            QuizSystemContext = context;
        }

        public async Task<bool> IsExist(Expression<Func<StudentExam, bool>> predicate)
        {
            return await QuizSystemContext.StudentExams.AnyAsync(predicate);
        }

        public async Task<StudentExam> GetWithDetailsAsync(int id)
        {
            return await QuizSystemContext.StudentExams
                .Include(se => se.Exam)
                .Include(se => se.Student)
                .Include(se => se.StudentAnswers)
                .FirstOrDefaultAsync(se => se.ID == id);
        }

        public async Task<IEnumerable<StudentExam>> GetByStudentIdAsync(string studentId)
        {
            return await QuizSystemContext.StudentExams
                .Include(se => se.Exam)
                .Where(se => se.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentExam>> GetByExamIdAsync(int examId)
        {
            return await QuizSystemContext.StudentExams
                .Include(se => se.Student)
                .Where(se => se.ExamId == examId)
                .ToListAsync();
        }

        public async Task<bool> AssignStudentToExamAsync(string studentId, int examId)
        {
            var entity = new StudentExam
            {
                StudentId = studentId,
                ExamId = examId,
                AssignedAt = DateTime.UtcNow,
                IsCompleted = false
            };

            await QuizSystemContext.StudentExams.AddAsync(entity);
            return await QuizSystemContext.SaveChangesAsync() > 0;
        }
    }
}
