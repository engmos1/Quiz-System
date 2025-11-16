using DataAccess.Models.Enums;

namespace BuisnessModel.VeiwModels.Exam
{
    public class ExamsVeiwModels
    {
        public string Name { get; set; }

        public string CourseName { get; set; }

        public ExamType Type { get; set; }

        public int NumberOfQuestions { get; set; }

        public string CreatedByInstructorName { get; set; }
    }
}
