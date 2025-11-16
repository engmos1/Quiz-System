using DataAccess.Context;
using DataAccess.Models;
using ExaminationSystem.Repositories;

namespace BuisnessModel.Repositories
{
    public class StudentExamRepository : GeneralRepository<StudentExam>
    {
        public StudentExamRepository(QuizSystemContext context) : base(context)
        {
        }
    }
}
