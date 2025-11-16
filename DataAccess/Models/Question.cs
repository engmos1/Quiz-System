using DataAccess.Identity;
using DataAccess.Models.Enums;
using ExaminationSystem.Models;

namespace DataAccess.Models
{

    public class Question : BaseModel
    {
        public string Text { get; set; }
        public QuestionLevel Level { get; set; }
        public bool IsReusable { get; set; }

        public string CreatedByInstructorId { get; set; }
        public ApplicationUser CreatedByInstructor { get; set; }

        public ICollection<Choice> Choices { get; set; }
        public ICollection<ExamQuestion> ExamQuestions { get; set; }
    }
}
