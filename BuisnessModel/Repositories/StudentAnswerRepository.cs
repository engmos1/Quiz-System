using BuisnessModel.Interfaces;
using DataAccess.Context;
using DataAccess.Models;
using ExaminationSystem.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BuisnessModel.Repositories
{
    public class StudentAnswerRepository : GeneralRepository<StudentAnswer>, IStudentAnswerRepository
    {
        private readonly QuizSystemContext _context;

        public StudentAnswerRepository(QuizSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentAnswer>> GetByStudentExamIdAsync(int studentExamId)
        {
            return await _context.StudentAnswers
                .Include(sa => sa.Question)
                .Include(sa => sa.Choice)
                .Where(sa => sa.StudentExamId == studentExamId)
                .ToListAsync();
        }

        public async Task<StudentAnswer> GetWithDetailsAsync(int id)
        {
            var answer = await _context.StudentAnswers
                .Include(sa => sa.Question)
                .Include(sa => sa.Choice)
                .Include(sa => sa.StudentExam)
                .FirstOrDefaultAsync(sa => sa.ID == id);
            return answer;
        }

        public async Task<bool> SaveStudentAnswerAsync(StudentAnswer answer)
        {
            await _context.StudentAnswers.AddAsync(answer);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
