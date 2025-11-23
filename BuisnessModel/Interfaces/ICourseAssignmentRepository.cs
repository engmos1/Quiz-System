
using DataAccess.Models;

namespace BuisnessModel.Interfaces
{
    public interface ICourseAssignmentRepository : IGeneralRepository<CourseAssignment>
    {
        CourseAssignment GetByCourseAndStudent(int courseId, string studentId);

       Task<(bool IsSuccess, string ErrorMessage)> ValidateCourseAssignment(int courseId, string studentId);
    }
}
