using DataAccess.Context;
using DataAccess.Models;
using ExaminationSystem.Repositories;

namespace BuisnessModel.Repositories
{
    internal class StudentAnswerRepository : GeneralRepository<StudentAnswer> 
    {
        public StudentAnswerRepository(QuizSystemContext context) : base(context)
        {
        }
    }
}
