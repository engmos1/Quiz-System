using DataAccess.Context;
using DataAccess.Models;
using ExaminationSystem.Repositories;
namespace BuisnessModel.Repositories
{
    internal class CourseAssignmentRepository : GeneralRepository<CourseAssignment>
    {
        public CourseAssignmentRepository(QuizSystemContext context) : base(context)
        {
        }
    
    }
}
