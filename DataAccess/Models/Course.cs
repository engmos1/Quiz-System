using DataAccess.Identity;
using DataAccess.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    public class Course : BaseModel
    {
        public string Name { get; set; }
        public int Hours { get; set; }
        public ICollection<Exam> Exam { get; set; }


        public string InstructorId { get; set; }
        [ForeignKey("InstructorId")]
        public ApplicationUser Instructor { get; set; }

        public ICollection<CourseAssignment> CourseAssignments { get; set; }
    }
}
