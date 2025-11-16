using ExaminationSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class ExamQuestion : BaseModel
    {
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
