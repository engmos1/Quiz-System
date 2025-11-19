using BuisnessModel.Interfaces;
using DataAccess.Context;
using DataAccess.Models;
using ExaminationSystem.Repositories;

namespace BuisnessModel.Repositories
{
    public class QuestionRepository : GeneralRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(QuizSystemContext context) : base(context)
        {
        }

    }
}
