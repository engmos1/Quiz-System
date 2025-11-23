using BuisnessModel.Interfaces;
using DataAccess.Context;
using DataAccess.Models;
using ExaminationSystem.Repositories;

namespace BuisnessModel.Repositories
{
    public class ExamQuestionRepository : GeneralRepository<ExamQuestion>, IExamQuestionRepository
    {
        QuizSystemContext _context;

        public ExamQuestionRepository(QuizSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExamQuestion>> GetQuestionsByExamId(int examId , CancellationToken cancellationToken=default)
        {
            var questions = _context.ExamQuestions
                .Where(eq => eq.ExamId == examId)
                .ToList();
            return questions;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> ValidateExamQuestion(int ExamId, int QuestionId)
        {
            if (ExamId <= 0)
            {
                return await Task.FromResult((false, "Invalid exam ID."));
            }
            var exam = await _context.Exams.FindAsync(ExamId);
            if (exam == null)
            {
                return await Task.FromResult((false, "Exam not found."));
            }
            if (QuestionId <= 0)
            {
                return await Task.FromResult((false, "Invalid question ID."));
            }
            var question = await _context.Questions.FindAsync(QuestionId);
            if (question == null)
            {
                return await Task.FromResult((false, "Question not found."));
            }
            return await Task.FromResult((true, string.Empty));
        }

    }
}
