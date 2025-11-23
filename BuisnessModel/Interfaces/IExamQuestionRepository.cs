using DataAccess.Models;
using ExaminationSystem.Models;


namespace BuisnessModel.Interfaces
{
    public interface IExamQuestionRepository : IGeneralRepository<ExamQuestion>
    {
        Task<IEnumerable<ExamQuestion>> GetQuestionsByExamId(int examId, CancellationToken cancellationToken = default);
        Task<(bool IsSuccess, string ErrorMessage)> ValidateExamQuestion(int ExamId, int QuestionId);
    }
}
