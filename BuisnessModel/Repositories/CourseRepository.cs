using DataAccess.Context;
using ExaminationSystem.Models;
using BuisnessModel.Interfaces;


namespace ExaminationSystem.Repositories
{
    public class CourseRepository : GeneralRepository<Course>, ICourseRepository
    {
        public CourseRepository(QuizSystemContext context) : base(context)
        {
          
        }
    }
}
