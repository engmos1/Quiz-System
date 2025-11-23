using BuisnessModel.Interfaces;
using DataAccess.Context;
using DataAccess.Identity;
using DataAccess.Models;
using ExaminationSystem.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
namespace BuisnessModel.Repositories
{
    public class CourseAssignmentRepository : GeneralRepository<CourseAssignment>, ICourseAssignmentRepository
    {
        QuizSystemContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CourseAssignmentRepository(QuizSystemContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        public CourseAssignment GetByCourseAndStudent(int courseId, string studentId)
        {
            var assignment = _context.CourseAssignments
                .FirstOrDefault(ca => ca.CourseId == courseId && ca.StudentId == studentId);
            return assignment;
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> ValidateCourseAssignment(int courseId, string studentId)
        {
            if (courseId <= 0)
            {
                return (false, "Invalid course ID.");
            }

            var course = _context.Courses.Find(courseId);
            if (course == null)
            {
                return (false, "Course not found.");
            }

            if (string.IsNullOrWhiteSpace(studentId))
            {
                return (false, "Invalid student ID.");
            }

            var student = _userManager.FindByIdAsync(studentId);
            if (student == null)
                return (false, "User not found.");

            bool isStudent = await _userManager.IsInRoleAsync(await student, "Student");
            if (!isStudent)
                return (false, "User is not a student.");


            return (true, string.Empty);
        }


    }
}
