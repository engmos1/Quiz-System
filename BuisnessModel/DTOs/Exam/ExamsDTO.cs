using DataAccess.Models.Enums;
using ExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.DTOs.Exam
{
    public class ExamsDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string CourseName { get; set; }

        public ExamType Type { get; set; }

        public int NumberOfQuestions { get; set; }

        public string CreatedByInstructorName { get; set; }
    }
}
