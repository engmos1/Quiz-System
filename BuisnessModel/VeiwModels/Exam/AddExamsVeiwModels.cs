using DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.VeiwModels.Exam
{
    public class AddExamsVeiwModels
    {
        public string Name { get; set; }

        public string CourseName { get; set; }

        public ExamType Type { get; set; }

        public int NumberOfQuestions { get; set; }

        public string CreatedByInstructorName { get; set; }
    }
}
