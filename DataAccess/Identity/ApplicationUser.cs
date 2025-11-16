using DataAccess.Models;
using ExaminationSystem.Models;
using Microsoft.AspNetCore.Identity;
namespace DataAccess.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public ICollection<Course> CoursesTaught { get; set; }

        public ICollection<CourseAssignment> CourseAssignments { get; set; }

        public ICollection<Question> CreatedQuestions { get; set; }

        public ICollection<Exam> CreatedExams { get; set; }

    }
}
