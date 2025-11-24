using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.DTOs.StudentAnswer
{
    public class StudentAnswerAddDto
    {
        public int StudentExamId { get; set; }
        public int QuestionId { get; set; }
        public int? ChoiceId { get; set; }
    }
}
