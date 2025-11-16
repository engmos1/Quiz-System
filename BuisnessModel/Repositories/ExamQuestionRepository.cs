using DataAccess.Context;
using DataAccess.Models;
using ExaminationSystem.Repositories;

namespace BuisnessModel.Repositories
{
    internal class ExamQuestionRepository : GeneralRepository<ExamQuestion>
    {
        public ExamQuestionRepository(QuizSystemContext context) : base(context)
        {
        }
    }
}
