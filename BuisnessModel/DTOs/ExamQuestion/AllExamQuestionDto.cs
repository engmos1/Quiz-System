using BuisnessModel.DTOs.Question;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessModel.DTOs.ExamQuestion
{
    public class AllExamQuestionDto
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }

        public QuestionsDto Question { get; set; }


    }
}
