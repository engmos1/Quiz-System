using DataAccess.Context;
using DataAccess.Models;
using ExaminationSystem.Repositories;

namespace BuisnessModel.Repositories
{
    internal class QuestionRepository : GeneralRepository<Question>
    {
        public QuestionRepository(QuizSystemContext context) : base(context)
        {
        }

    }
}
